using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Common;

using com.sp.rmmc.common.models;

namespace com.sp.rmmc.foreclosures.models
{
    public class LMInProcess : DbModel, com.sp.rmmc.common.tools.ICSVExport
    {
        public int id = 0;
        public decimal loan_id = 0M;

        public String stop_code_description = "";
        public String mortgage_status = "";
        public DateTimeObject mortgage_status_date = new DateTimeObject();

        public MsLoan loan = new MsLoan();


        private string base_query = "" +
"select base.loan_id, " +
"(select top 1 msc.stop_description from ms_loan_stop_code mlsc join ms_stop_code msc on mlsc.stop_code_id = msc.stop_code_id where mlsc.loan_id = base.loan_id and msc.stop_description like '8 TR%'  order by msc.stop_description ) as stop_code_description, " +
"(select top 1 mms.mortgage_status_desc from ms_credit_information mci join ms_mortgage_status mms on mms.mortgage_status_code = mci.mortgage_status where mci.loan_id = base.loan_id order by mms.mortgage_status_desc ) as mortgage_status, " +
"(select top 1 mci.mortgage_status_date from ms_credit_information mci where mci.loan_id = base.loan_id order by mci.mortgage_status_date desc ) as mortgage_status_date, " +
MsLoan.columns("base.loan_id") +
"from  ( " +
"(select loan_id from ms_loan_stop_code mlsc join ms_stop_code msc on mlsc.stop_code_id = msc.stop_code_id where msc.stop_description like '8 TR%L PAYMENTS') " +
"UNION " +
"(select mci.loan_id from ms_credit_information  mci join ms_mortgage_status mms on mms.mortgage_status_code = mci.mortgage_status where mms.mortgage_status_desc in ('Trial Payment Plan', 'FHA-HAMP Trial Modification Plan') ) " +
") as base " +
"where " + MsLoan.MS_LOAN_PRIN_BAL + " != 0 ";

        public List<LMInProcess> all()
        {
            string query = base_query + " order by loan_id";
            return get_list(query);
        }

        protected List<LMInProcess> get_list(String query)
        {
            List<LMInProcess> list = new List<LMInProcess>();
            SybaseModel model = new SybaseModel();

            DbDataReader reader = model.getReader(query);
            while (reader.Read())
            {
                LMInProcess loan = new LMInProcess();
                loan = read(reader);
                list.Add(loan);
            }
            reader.Close();
            model.close_connection();
            return list;
        }

        private LMInProcess read(DbDataReader reader)
        {
            LMInProcess loan = new LMInProcess();
            int pos = 0;
            loan.loan_id = readDBDecimal(reader, pos++);
            loan.stop_code_description = readDBString(reader, pos++);
            loan.mortgage_status = readDBString(reader, pos++);
            loan.mortgage_status_date = readDBDateObject(reader, pos++);

            if (loan.loan_id > 0)
            {
                loan.loan = MsLoan.read(reader, pos);
            }
            return loan;
        }

        public String to_csv()
        {
            return this.loan_id.ToString() + "," + this.loan.loan_type + ",\"" + this.loan.loan_name + "\",\"" +
                this.loan.prin_bal.ToString("C") + "\",\"" +
                this.mortgage_status + "\",\"" +
                this.mortgage_status_date.ToShortDateString() + "\",\"" +
                this.stop_code_description + "\"" +
                "";
        }
    }
}