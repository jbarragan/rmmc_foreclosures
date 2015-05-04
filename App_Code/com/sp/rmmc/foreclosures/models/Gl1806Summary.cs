using System;
using System.Collections.Generic;
using System.Web;

using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
using com.sp.rmmc.common.models.events;

namespace com.sp.rmmc.foreclosures.models
{

    public class Gl1806Summary : MsSqlModel
    {
        public decimal loan_id = 0M;
        public string name = "";
        public DateTimeObject latest_date = new DateTimeObject();
        public decimal amount_balance = 0M;
        public decimal amount_received = 0M;
        public decimal amount_expended = 0M;
        public string last_description = "";
        public decimal last_amount = 0M;

        private string connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["foreclosure"].ConnectionString;
        private string select_base_query =
            @"select base.*, details.description, details.amount from 
            (
            select [LOAN #] as loan_id ,NAME, max(DATE) as latest_date, sum(amount) as amount_balance,
            sum(case when amount >= 0 THEN amount ELSE 0 END) as amount_received,
            sum(case when amount < 0 THEN abs(amount) ELSE 0 END) as amount_expended
            FROM tbl_gl1806
            group by [LOAN #], NAME
            HAVING NAME IS NOT NULL
            ) base
            join tbl_gl1806 details on base.loan_id = details.[loan #] and details.date = base.latest_date
            where base.latest_date >= Dateadd(Month, Datediff(Month, 0, DATEADD(m, -6, current_timestamp)), 0)
            order by latest_date desc ";

        public Gl1806Summary()
        {
            
        }

        public List<Gl1806Summary> all()
        {
            string query = select_base_query;
            return this.get_list(query);
        }

        private List<Gl1806Summary> get_list(String query)
        {
            List<Gl1806Summary> summaries = new List<Gl1806Summary>();
            this.setConnectionString(this.connection_string);
            DbDataReader reader = getReader(query);
            while (reader.Read()) summaries.Add((new Gl1806Summary()).read(reader, 0));
            reader.Close();
            close_connection();
            return summaries;
        }

        private Gl1806Summary read(DbDataReader reader, int pos)
        {
            Gl1806Summary s = new Gl1806Summary();
            s.loan_id = readDBDoubleDecimal(reader, pos++);
            s.name = readDBString(reader, pos++);
            s.latest_date = readDBDateObject(reader, pos++);
            s.amount_balance = readDBDecimal(reader, pos++);
            s.amount_received = readDBDecimal(reader, pos++);
            s.amount_expended = readDBDecimal(reader, pos++);
            s.last_description = readDBString(reader, pos++);
            s.last_amount = readDBDecimal(reader, pos++);
            return s;
        }
    }
}