
using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class SaleFannieMaeClaim571Events : BaseEvents
    {
        public DateTimeObject fannie_mae_claim_571_date = new DateTimeObject();
        public DateTimeObject sale_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as sale_fannie_mae_claim_571_events_loan_id",
             "(select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 10300 and ms_fcl.loan_id = loan_id_column and ms_fcl.fcl_item_desc like '%571%' order by ms_fcl.fcl_item_ord_req ) as sale_fannie_mae_claim_571_events_fannie_mae_claim_571_date", 
             "(select top 1 mfia1.fcl_item_date from ms_fcl_item_amts_1 mfia1 where mfia1.item_id = 1600 and mfia1.loan_id = loan_id_column order by mfia1.fcl_item_date ) as sale_fannie_mae_claim_571_events_sale_date",
            };

        public SaleFannieMaeClaim571Events()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            int alert_days = 30;
            int warning_days = 20;
            string date_name = "Fannie Mae Claim Date";
            if (this.loan.loan_type != "CNV") return;
            if (this.sale_date.isNull) return;
            if (this.sale_date.date <= this.fannie_mae_claim_571_date.date && this.fannie_mae_claim_571_date.isNull == false) return;
            if (sale_date.date > this.loan.event_todays_date.AddDays(-1 * warning_days)) return;

            if (this.fannie_mae_claim_571_date.isNull)
            {
                Event e = new Event();
                e.loan = this.loan;
                if (sale_date.date > this.loan.event_todays_date.AddDays(-1 * alert_days))
                {
                    e.type = Event.WARNING;
                    e.name = "No " + date_name + " Warning";
                }
                else
                {
                    e.name = "No " + date_name + " Alert";
                    e.type = Event.ALERT;
                }
                e.description = "No " + date_name + " with Sale Date " + this.sale_date.ToShortDateString();
                this.events.Add(e);
            }

            if (this.fannie_mae_claim_571_date.isNull == false && this.sale_date.date < this.fannie_mae_claim_571_date.date.AddDays(-1 * alert_days))
            {
                Event e = new Event();
                e.loan = this.loan;
                if (sale_date.date > this.fannie_mae_claim_571_date.date.AddDays(-1 * alert_days))
                {
                    e.type = Event.WARNING;
                    e.name = date_name + " Warning";
                }
                else
                {
                    e.name = date_name + " Alert";
                    e.type = Event.ALERT;
                }
                e.description = date_name + " on " + this.fannie_mae_claim_571_date.ToShortDateString() + " with Sale Date " + this.sale_date.ToShortDateString();
                this.events.Add(e);
            }
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.fannie_mae_claim_571_date = DbModel.readDBDateObject(reader, pos++);
            this.sale_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@sale_fannie_mae_claim_571_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@sale_fannie_mae_claim_571_events_fannie_mae_claim_571_date", this.fannie_mae_claim_571_date.toDBInsert());
            cmd.Parameters.AddWithValue("@sale_fannie_mae_claim_571_events_sale_date", this.sale_date.toDBInsert());
        }
    }
}