using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class SaleHudAdviceEvents : BaseEvents
    {
        public DateTimeObject hud_advice_date = new DateTimeObject();
        public DateTimeObject conveyance_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as sale_hud_advice_events_loan_id",
             "(select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11200 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_recvd_expd ) as sale_hud_advice_events_hud_advice_date",
             "(select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11200 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_ord_req ) as sale_hud_advice_events_conveyance_date"
            };

        public SaleHudAdviceEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.loan.loan_type != "FHA") return;
            if (this.conveyance_date.isNull) return;
            if (this.conveyance_date.date <= this.hud_advice_date.date && this.hud_advice_date.isNull == false) return;
            if (conveyance_date.date > DateTime.Today.AddDays(-1)) return;

            if (this.hud_advice_date.isNull)
            {
                Event e = new Event();
                e.loan = this.loan;
                if (conveyance_date.date > DateTime.Today.AddDays(-3))
                {
                    e.type = Event.WARNING;
                    e.name = "No Hud Advice Date Warning";
                }
                else
                {
                    e.name = "No Hud Advice Date Alert";
                    e.type = Event.ALERT;
                }
                e.description = "No Hud Advice Date with Conveyance Date " + this.conveyance_date.ToShortDateString();
                this.events.Add(e);
            }

            if (this.hud_advice_date.isNull == false && this.conveyance_date.date < this.hud_advice_date.date.AddDays(-3))
            {
                Event e = new Event();
                e.loan = this.loan;
                if (conveyance_date.date > this.hud_advice_date.date.AddDays(-1))
                {
                    e.type = Event.WARNING;
                    e.name = "Hud Advice Date Warning";
                }
                else
                {
                    e.name = "Hud Advice Date Alert";
                    e.type = Event.ALERT;
                }
                e.description = "Hud Advice Date on " + this.hud_advice_date.ToShortDateString() + " with Conveyance Date " + this.conveyance_date.ToShortDateString();
                this.events.Add(e);
            }
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.hud_advice_date = DbModel.readDBDateObject(reader, pos++);
            this.conveyance_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@sale_hud_advice_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@sale_hud_advice_events_hud_advice_date", this.hud_advice_date.toDBInsert());
            cmd.Parameters.AddWithValue("@sale_hud_advice_events_conveyance_date", this.conveyance_date.toDBInsert());
        }
    }
}