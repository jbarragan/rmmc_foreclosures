using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class SaleBasicClaimEvents : BaseEvents
    {
        public DateTimeObject basic_claim_date = new DateTimeObject();
        public DateTimeObject sale_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as sale_basic_claim_events_loan_id",
             "(select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 10700 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_ord_req ) as sale_basic_claim_events_basic_claim_date", 
             "(select top 1 mfia1.fcl_item_date from ms_fcl_item_amts_1 mfia1 where mfia1.item_id = 1600 and mfia1.loan_id = loan_id_column order by mfia1.fcl_item_date ) as sale_basic_claim_events_sale_date",
            };

        public SaleBasicClaimEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.loan.loan_type != "VA") return;
            if (this.sale_date.isNull) return;
            if (this.sale_date.date <= this.basic_claim_date.date && this.basic_claim_date.isNull == false) return;
            if (sale_date.date > DateTime.Today.AddDays(-20)) return;

            if (this.basic_claim_date.isNull)
            {
                Event e = new Event();
                e.loan = this.loan;
                if (sale_date.date > DateTime.Today.AddDays(-30))
                {
                    e.type = Event.WARNING;
                    e.name = "No Basic Claim Date Warning";
                }
                else
                {
                    e.name = "No Basic Claim Date Alert";
                    e.type = Event.ALERT;
                }
                e.description = "No Basic Claim Date with Sale Date " + this.sale_date.ToShortDateString();
                this.events.Add(e);
            }

            if (this.basic_claim_date.isNull == false && this.sale_date.date < this.basic_claim_date.date.AddDays(-20))
            {
                Event e = new Event();
                e.loan = this.loan;
                if (sale_date.date > this.basic_claim_date.date.AddDays(-30))
                {
                    e.type = Event.WARNING;
                    e.name = "Basic Claim Date Warning";
                }
                else
                {
                    e.name = "Basic Claim Date Alert";
                    e.type = Event.ALERT;
                }
                e.description = "Basic Claim Date on " + this.basic_claim_date.ToShortDateString() + " with Sale Date " + this.sale_date.ToShortDateString();
                this.events.Add(e);
            }
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.basic_claim_date = DbModel.readDBDateObject(reader, pos++);
            this.sale_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@sale_basic_claim_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@sale_basic_claim_events_basic_claim_date", this.basic_claim_date.toDBInsert());
            cmd.Parameters.AddWithValue("@sale_basic_claim_events_sale_date", this.sale_date.toDBInsert());
        }
    }
}