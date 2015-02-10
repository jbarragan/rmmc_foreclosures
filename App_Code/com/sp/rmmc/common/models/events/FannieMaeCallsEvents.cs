using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class FannieMaeCallsEvents : BaseEvents
    {
        public DateTimeObject last_bc_call_memo_date = new DateTimeObject();
        public DateTimeObject last_bc_promised_to_pay_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as fannie_mae_calls_events_loan_id",
             "(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'BC%' order by memo_id desc ) as fannie_mae_calls_events_last_bc_call_memo_date",
             "(select top 1 memo.memo_notify_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc = 'BC-Promised to Pay' order by memo_notify_dt desc ) as fannie_mae_calls_events_last_bc_promised_to_pay_date",
            };

        public FannieMaeCallsEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.loan.loan_type != "CNV") return;
            if (does_require_fannie_mae_call() == false) return;
            Event e = new Event();
            e.loan = this.loan;
            e.type = Event.WARNING;
            e.name = "FANNIE MAE loan last call Warning";
            e.description = "Last call " + this.last_bc_call_memo_date.ToString() +
                ". Last promised to pay date " + this.last_bc_promised_to_pay_date.ToString() + ". New call is required.";
            this.events.Add(e);
        }

        public bool does_require_fannie_mae_call()
        {
            if (last_bc_promised_to_pay_date.isNull == false &&
                last_bc_promised_to_pay_date.date >= DateTime.Today) return false;
            TimeSpan ts = DateTime.Today.AddDays(1) - this.last_bc_call_memo_date.date;
            if (ts.Days >= 3) return true;
            if (ts.Days == 0) return false;
            for (int i = 3 - ts.Days; i >= 1; i--)
            {
                DateTime date_to_call = DateTime.Today.AddDays(i);
                if (date_to_call.DayOfWeek != DayOfWeek.Saturday && date_to_call.DayOfWeek != DayOfWeek.Sunday) return false;
            }
            return true;
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {   
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.last_bc_call_memo_date = DbModel.readDBDateObject(reader, pos++);
            this.last_bc_promised_to_pay_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@fannie_mae_calls_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@fannie_mae_calls_events_last_bc_call_memo_date", this.last_bc_call_memo_date.toDBInsert());
            cmd.Parameters.AddWithValue("@fannie_mae_calls_events_last_bc_promised_to_pay_date", this.last_bc_promised_to_pay_date.toDBInsert());
        }
    }
}
