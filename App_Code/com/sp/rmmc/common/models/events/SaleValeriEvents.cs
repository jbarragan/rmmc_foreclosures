using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class SaleValeriEvents : BaseEvents
    {
        public DateTimeObject default_action_date = new DateTimeObject();
        public DateTimeObject default_end_dt = new DateTimeObject();
        public DateTimeObject posting_date = new DateTimeObject();
        public DateTimeObject fc_va_scheduled_memo_created_date = new DateTimeObject();
        
        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as sale_valeri_events_loan_id",
             "(select top 1 default_action_date from ms_default_actions mda where mda.loan_id = loan_id_column and mda.default_action_code = '71' order by mda.default_action_date desc ) as sale_valeri_events_default_action_date",
             "(select top 1 default_end_dt from ms_default_actions mda where mda.loan_id = loan_id_column and mda.default_action_code = '71' order by mda.default_end_dt desc ) as sale_valeri_events_default_end_dt",
             "(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1200 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_date ) as sale_valeri_events_posting_date",
             "(select top 1 memo.memo_create_dt  from ms_loan_memo memo join ms_memo_types on memo.memo_type_id = ms_memo_types.memo_type_id where memo.loan_id = loan_id_column and ms_memo_types.memo_type_desc like 'FC-VA SCHEDULED%' order by memo.memo_create_dt desc ) as sale_valeri_events_fc_va_scheduled_memo_created_date"
            };

        public SaleValeriEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.loan.loan_type != "VA") return;
            if (this.posting_date.isNull) return;
            if (this.default_action_date.isNull) return;
            if (this.default_end_dt.isNull) return;
            if (this.fc_va_scheduled_memo_created_date.isNull == false) return;
            Event e = new Event();
            e.loan = this.loan;
            e.type = Event.WARNING;
            if (this.loan.event_todays_date.AddDays(-2) > this.default_action_date.date ) e.type = Event.ALERT;
            e.name = "VALERI Scheduled Reporting Event";
            e.description = "Not VALERI Scheduled Reporting for action date: " + this.default_action_date.ToString() + 
                " and action end date: " + this.default_end_dt.ToString();
            this.events.Add(e);
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.default_action_date = DbModel.readDBDateObject(reader, pos++);
            this.default_end_dt = DbModel.readDBDateObject(reader, pos++);
            this.posting_date = DbModel.readDBDateObject(reader, pos++);
            this.fc_va_scheduled_memo_created_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@sale_valeri_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@sale_valeri_events_default_action_date", this.default_action_date.toDBInsert());
            cmd.Parameters.AddWithValue("@sale_valeri_events_default_end_dt", this.default_end_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@sale_valeri_events_posting_date", this.posting_date.toDBInsert());
            cmd.Parameters.AddWithValue("@sale_valeri_events_fc_va_scheduled_memo_created_date", this.fc_va_scheduled_memo_created_date.toDBInsert());
        }
    }
}