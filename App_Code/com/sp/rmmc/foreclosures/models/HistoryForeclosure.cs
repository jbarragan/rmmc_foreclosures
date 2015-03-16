using System;
using System.Collections.Generic;
using System.Web;
using System.Data.Common;

using com.sp.rmmc.common.models;
using com.sp.rmmc.common.models.events;
namespace com.sp.rmmc.foreclosures.models
{
    public class HistoryForeclosure : BaseForeclosure
    {
        private string connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["foreclosure"].ConnectionString;
    
        public int history_id = 0;
        protected override string default_columns
        {
            get
            {
                return String.Join(", ", (new CurrentForeclosure()).default_columns_list().ToArray());
            }
        }

        protected override string demands_query
        {
            get
            {
                return
            "( " +
            " select * " +
            " from history_loan_details  " +
            " where demand_letter_due_date is not null " +
            " and (foreclosure_stopped_memo_date is null " +
            "     or demand_letter_due_date >= foreclosure_stopped_memo_date ) " +
            " and history_id = " + this.history_id + " " +
            ") ";
            }
        }

        protected override string foreclosures_query
        {
            get
            {
                return
            "( " +
            " select * " +
            " from history_loan_details  " +
            " where demand_letter_due_date is not null " +
            " and (foreclosure_stopped_memo_date is null " +
            "     or demand_letter_due_date < foreclosure_stopped_memo_date ) " +
            " and history_id = " + this.history_id + " " +
            ") ";
            }
        }

        protected override string deed_in_lieu_query
        {
            get
            {
                return
            "( " +
            " select * " +
            " from history_loan_details  " +
            " where fcl_deed_in_lieu = 'Y' and history_id = " + this.history_id + " " +
            ") ";
            }
        }

        protected override string foreclosed_query
        {
            get
            {
                return
            "( " +
            " select * " +
            " from history_loan_details  " +
            " where sale_date is not null and history_id = " + this.history_id + " " +
            " and sale_date > '2014-01-01' " +
            ") ";
            }
        }

        protected override string all_types_query
        {
            get
            {
                return
            "( " +
            " select * " +
            " from history_loan_details  " +
            " where demand_letter_due_date is not null and history_id = " + this.history_id + " " +
            ") " +
            "UNION " +
            "( " +
            " select * " +
            " from history_loan_details  " +
            " where sale_date is not null and history_id = " + this.history_id + " " +
            " and sale_date > '2014-01-01' " +
            ") " +
            "UNION " +
            "( " +
            " select * " +
            " from history_loan_details  " +
            " where fcl_type = 'A' and history_id = " + this.history_id + " " +
            ") " +
            "UNION " +
            "( " +
            " select * " +
            " from history_loan_details  " +
            " where fcl_deed_in_lieu = 'Y' and history_id = " + this.history_id + " " +
            ") " +
            " ";
            }
        }

        public HistoryForeclosure(int history_id)
        {
            this.history_id = history_id;
        }

        protected override List<BaseForeclosure> get_loan_list(String query)
        {
            List<BaseForeclosure> list = new List<BaseForeclosure>();
            MsSqlModel model = new MsSqlModel();
            model.setConnectionString(this.connection_string);
            DbDataReader reader = model.getReader(query);
            while (reader.Read())
            {
                BaseForeclosure f = new BaseForeclosure();
                f.type = this.type;
                f.read_loan(reader);
                list.Add(f);
            }
            reader.Close();
            model.close_connection();
            return list;
        }

        protected override string getEventsColumns()
        {
            string s = "";
            foreach (BaseEvents be in this.getEventsTypes())
            {
                s += ", \n" + be.columns_name_query();
            }
            if (this.review_checklist != null) s += ", \n" + ReviewChecklist.columns_name_query();
            return s;
        }
    }
}
