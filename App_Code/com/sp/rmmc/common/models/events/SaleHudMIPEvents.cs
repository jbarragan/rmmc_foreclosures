using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class SaleHudMIPEvents : BaseEvents
    {
        public DateTimeObject reo_date = new DateTimeObject();
        public DateTimeObject third_party_sale_date = new DateTimeObject();
        public DateTimeObject last_pay_off_dt = new DateTimeObject();
        public DateTimeObject mip_cancel_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as sale_hud_mip_events_loan_id",
             "(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'FC-REO%' order by memo_create_dt desc ) as sale_hud_mip_events_reo_date",
             "(select top 1 memo.memo_create_dt  from ms_loan_memo memo join ms_memo_types on memo.memo_type_id = ms_memo_types.memo_type_id where memo.loan_id = loan_id_column and ms_memo_types.memo_type_desc like 'FC-3rd%' order by memo.memo_create_dt desc ) as sale_hud_mip_events_third_party_sale_date",
             "(select top 1 hist.paid_dt from ms_loan_history hist where hist.loan_id = loan_id_column and (hist.trans_desc like 'Third%' or hist.trans_desc like 'Thrd%' or hist.trans_desc like 'Fcl') order by paid_dt desc) as sale_hud_mip_events_last_pay_off_dt ",
             "(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 3600 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_date ) as sale_hud_mip_events_canel_mip_date "
            };

        public SaleHudMIPEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.loan_id == 800089M)
                this.loan_id = 800089M;
            if (this.loan.loan_type != "FHA") return;
            if (this.reo_date.isNull && this.third_party_sale_date.isNull) return;
            if (this.last_pay_off_dt.isNull) return;
            if (mip_cancel_date.isNull == false) return;

            Event e = new Event();
            e.loan = this.loan;
            if (this.loan.event_todays_date > this.last_pay_off_dt.date.AddDays(2)) e.type = Event.ALERT;
            else e.type = Event.WARNING;
            e.name = "HUD MIP Cancellation";
            e.description = "No HUD MIP Cancellation for payoff received on " + this.last_pay_off_dt.ToShortDateString();
            this.events.Add(e);
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.reo_date = DbModel.readDBDateObject(reader, pos++);
            this.third_party_sale_date = DbModel.readDBDateObject(reader, pos++);
            this.last_pay_off_dt = DbModel.readDBDateObject(reader, pos++);
            this.mip_cancel_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@sale_hud_mip_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@sale_hud_mip_events_reo_date", this.reo_date.toDBInsert());
            cmd.Parameters.AddWithValue("@sale_hud_mip_events_third_party_sale_date", this.third_party_sale_date.toDBInsert());
            cmd.Parameters.AddWithValue("@sale_hud_mip_events_last_pay_off_dt", this.last_pay_off_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@sale_hud_mip_events_canel_mip_date", this.mip_cancel_date.toDBInsert());
        }
    }
}