using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class SaleClaimPartBEvents : BaseEvents
    {
        public DateTimeObject claim_part_b_date = new DateTimeObject();
        public DateTimeObject title_approval_date = new DateTimeObject();
        public DateTimeObject reo_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as sale_claim_part_b_events_loan_id",
             "(select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11900 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_recvd_expd ) as sale_claim_part_b_events_claim_part_b_date",
             "(select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11600 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_ord_req ) as sale_claim_part_b_events_title_approval_date",
             "(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'FC-REO%' order by memo_create_dt desc ) as sale_claim_part_b_events_reo_date"
            };

        public SaleClaimPartBEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.loan.loan_type != "FHA") return;

            if (this.reo_date.isNull == false && this.reo_date.date > this.title_approval_date.date) return;
            if (this.title_approval_date.isNull) return;
            if (this.title_approval_date.date <= this.claim_part_b_date.date && this.claim_part_b_date.isNull == false) return;
            if (title_approval_date.date > DateTime.Today.AddDays(-7)) return;

            if (this.claim_part_b_date.isNull)
            {
                Event e = new Event();
                e.loan = this.loan;
                if (title_approval_date.date > DateTime.Today.AddDays(-15))
                {
                    e.type = Event.WARNING;
                    e.name = "No Claim Part B Warning";
                }
                else
                {
                    e.name = "No Claim Part B Alert";
                    e.type = Event.ALERT;
                }
                e.description = "No Claim Part B Date with Title Approval Date " + this.title_approval_date.ToShortDateString();
                this.events.Add(e);
            }

            if (this.claim_part_b_date.isNull == false && this.title_approval_date.date < this.claim_part_b_date.date.AddDays(-7))
            {
                Event e = new Event();
                e.loan = this.loan;
                if (title_approval_date.date > this.claim_part_b_date.date.AddDays(-7))
                {
                    e.type = Event.WARNING;
                    e.name = "Claim Part B Date Warning";
                }
                else
                {
                    e.name = "Claim Part B Date Alert";
                    e.type = Event.ALERT;
                }
                e.description = "Claim Part B Date on " + this.claim_part_b_date.ToShortDateString() + " with Title Approval Date " + this.title_approval_date.ToShortDateString();
                this.events.Add(e);
            }
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.claim_part_b_date = DbModel.readDBDateObject(reader, pos++);
            this.title_approval_date = DbModel.readDBDateObject(reader, pos++);
            this.reo_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@sale_claim_part_b_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@sale_claim_part_b_events_claim_part_b_date", this.claim_part_b_date.toDBInsert());
            cmd.Parameters.AddWithValue("@sale_claim_part_b_events_title_approval_date", this.title_approval_date.toDBInsert());
            cmd.Parameters.AddWithValue("@sale_claim_part_b_events_reo_date", this.reo_date.toDBInsert());
        }
    }
}