using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class SaleTransferCustodyEvents : BaseEvents
    {
        public DateTimeObject transfer_of_custody_date = new DateTimeObject();
        public DateTimeObject sale_date = new DateTimeObject();
        public DateTimeObject reo_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as sale_transfer_custody_events_loan_id",
             "(select top 1 fcl_ud.fcl_ud_item_date from ms_fcl_ud_item_amts_1 fcl_ud where fcl_ud.loan_id = loan_id_column and fcl_ud.fcl_ud_item_desc like '%QUISITION PAY%' order by fcl_ud_item_date desc) as sale_transfer_custody_events_transfer_of_custody_date", 
             "(select top 1 mfia1.fcl_item_date from ms_fcl_item_amts_1 mfia1 where mfia1.item_id = 1600 and mfia1.loan_id = loan_id_column order by mfia1.fcl_item_date ) as sale_transfer_custody_events_sale_date",
             "(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'FC-REO%' order by memo_create_dt desc ) as sale_transfer_custody_events_reo_date"
            };

        public SaleTransferCustodyEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.loan.loan_type != "VA") return;
            if (this.reo_date.isNull == false && this.reo_date.date > this.sale_date.date) return;
            if (this.sale_date.isNull) return;
            if (this.sale_date.date <= this.transfer_of_custody_date.date && this.transfer_of_custody_date.isNull == false) return;
            if (sale_date.date > DateTime.Today.AddDays(-7)) return;

            if (this.transfer_of_custody_date.isNull)
            {
                Event e = new Event();
                e.loan = this.loan;
                if (sale_date.date > DateTime.Today.AddDays(-15))
                {
                    e.type = Event.WARNING;
                    e.name = "No Transfer of Custody Date Warning";
                }
                else
                {
                    e.name = "No Transfer of Custody Date Alert";
                    e.type = Event.ALERT;
                }
                e.description = "No Transfer of Custody Date with Sale Date " + this.sale_date.ToShortDateString();
                this.events.Add(e);
            }

            if (this.transfer_of_custody_date.isNull == false && this.sale_date.date < this.transfer_of_custody_date.date.AddDays(-7))
            {
                Event e = new Event();
                e.loan = this.loan;
                if (sale_date.date > this.transfer_of_custody_date.date.AddDays(-7))
                {
                    e.type = Event.WARNING;
                    e.name = "Transfer of Custody Date Warning";
                }
                else
                {
                    e.name = "Transfer of Custody Date Alert";
                    e.type = Event.ALERT;
                }
                e.description = "Transfer of Custody Date on " + this.transfer_of_custody_date.ToShortDateString() + " with Sale Date " + this.sale_date.ToShortDateString();
                this.events.Add(e);
            }
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.transfer_of_custody_date = DbModel.readDBDateObject(reader, pos++);
            this.sale_date = DbModel.readDBDateObject(reader, pos++);
            this.reo_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@sale_transfer_custody_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@sale_transfer_custody_events_transfer_of_custody_date", this.transfer_of_custody_date.toDBInsert());
            cmd.Parameters.AddWithValue("@sale_transfer_custody_events_sale_date", this.sale_date.toDBInsert());
            cmd.Parameters.AddWithValue("@sale_transfer_custody_events_reo_date", this.sale_date.toDBInsert());
        }
    }
}