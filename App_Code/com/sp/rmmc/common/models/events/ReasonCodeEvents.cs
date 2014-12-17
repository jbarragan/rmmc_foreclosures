using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.foreclosures.models;

namespace com.sp.rmmc.common.models.events
{
    public class ReasonCodeEvents : BaseEvents
    {
        public String reason_code = "";
        public String last_memo_reason_code = "";

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as reason_code_events_loan_id",
             "(select top 1 ms_credit_information.default_reason_code from ms_credit_information where ms_credit_information.loan_id = loan_id_column order by ms_credit_information.default_reason_code ) as reason_code_events_reason_code",
             "(select top 1 ms_memo_categories.memo_category_desc from ms_loan_memo memo, ms_memo_categories, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_category_id = ms_memo_categories.memo_category_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_categories.memo_category_desc like '0%' and (ms_memo_types.memo_type_desc = 'BC-Promised to Pay' or ms_memo_types.memo_type_desc = 'BC-First Right Party Contact' or ms_memo_categories.memo_category_desc like '031%')  order by memo_create_dt desc ) as reason_code_events_last_memo_reason_code"
            };
        
        public ReasonCodeEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.last_memo_reason_code.StartsWith(this.reason_code) == false)
            {
                Event e = new Event();
                e.loan = this.loan;
                e.type = Event.ALERT;
                e.name = "Different Reason Codes";
                e.description = "Current Reason Code (" + this.reason_code + ") is diffent that last Memo Reason Code (" + this.last_memo_reason_code + ")";
                this.events.Add(e);
            }
        }

        public void read(DbDataReader reader, int pos, MsLoan loan)
        {   
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.reason_code = DbModel.readDBString(reader, pos++);
            this.last_memo_reason_code = DbModel.readDBString(reader, pos++);
            this.load_events();
        }
    }
}