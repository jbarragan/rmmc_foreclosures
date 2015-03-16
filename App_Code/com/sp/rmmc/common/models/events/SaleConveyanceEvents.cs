using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class SaleConveyanceEvents : BaseEvents
    {
        public DateTimeObject possesion_date = new DateTimeObject();
        public DateTimeObject conveyance_date = new DateTimeObject();
        public DateTimeObject reo_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as sale_conveyance_events_loan_id",
             "(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1900 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_date ) as sale_conveyance_events_possesion_date",
             "(select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11200 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_ord_req ) as sale_conveyance_events_conveyance_date",
             "(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'FC-REO%' order by memo_create_dt desc ) as sale_conveyance_events_reo_date"
            };

        public SaleConveyanceEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.loan.loan_type != "FHA") return;
            if (this.possesion_date.isNull) return;
            if (this.reo_date.isNull == false && this.reo_date.date > this.possesion_date.date ) return;
            if (this.possesion_date.date <= this.conveyance_date.date && this.conveyance_date.isNull == false ) return;
            if (possesion_date.date > DateTime.Today.AddDays(-10)) return;

            if (this.conveyance_date.isNull)
            {
                Event e = new Event();
                e.loan = this.loan;
                if (possesion_date.date > DateTime.Today.AddDays(-30))
                {
                    e.type = Event.WARNING;
                    e.name = "No Conveyance Date Warning";
                }
                else
                {
                    e.name = "No Conveyance Date Alert";
                    e.type = Event.ALERT;
                }
                e.description = "No Conveyance Date with Possesion Date " + this.possesion_date.ToShortDateString();
                this.events.Add(e);
            }

            if ( this.conveyance_date.isNull == false && this.possesion_date.date < this.conveyance_date.date.AddDays(-30))
            {
                Event e = new Event();
                e.loan = this.loan;
                if (possesion_date.date > this.conveyance_date.date.AddDays(-10))
                {
                    e.type = Event.WARNING;
                    e.name = "Conveyance Date Warning";
                }
                else
                {
                    e.name = "Conveyance Date Alert";
                    e.type = Event.ALERT;
                }
                e.description = "Conveyance Date on " + this.conveyance_date.ToShortDateString() + " with Possesion Date " + this.possesion_date.ToShortDateString();
                this.events.Add(e);
            }
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.possesion_date = DbModel.readDBDateObject(reader, pos++);
            this.conveyance_date = DbModel.readDBDateObject(reader, pos++);
            this.reo_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@sale_conveyance_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@sale_conveyance_events_possesion_date", this.possesion_date.toDBInsert());
            cmd.Parameters.AddWithValue("@sale_conveyance_events_conveyance_date", this.conveyance_date.toDBInsert());
            cmd.Parameters.AddWithValue("@sale_conveyance_events_reo_date", this.reo_date.toDBInsert());
        }
    }
}