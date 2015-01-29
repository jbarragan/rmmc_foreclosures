using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class XdemandOccupancyLetterEvents : BaseEvents
    {
        public DateTimeObject nl_current_occupancy_letter_date = new DateTimeObject();
        public DateTimeObject demand_letter_due_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as xdemand_occupancy_letter_events_loan_id",
             "(select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = loan_id_column and memo.memo_type_id = 138 order by memo_id desc) as xdemand_occupancy_letter_events_nl_current_occupancy_letter_date",
             "(select top 1 misc.misc_dt_value from ms_loan_misc_fields misc where misc_field_id = 10020 and misc.loan_id = loan_id_column order by misc.misc_dt_value ) as xdemand_occupancy_letter_events_demand_letter_due_date"
            };

        public XdemandOccupancyLetterEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.loan.loan_type != "FHA") return;
            if (this.demand_letter_due_date.isNull) return;
            String description = "";
            if (this.nl_current_occupancy_letter_date.isNull) 
                description = "No Current occupancy letter for removed demand with demand date: " + this.demand_letter_due_date.ToShortDateString();
            if (this.nl_current_occupancy_letter_date.date < this.demand_letter_due_date.date.AddDays(-35))
                description = "No Current occupancy letter for removed demand with demand date: " + this.demand_letter_due_date.ToShortDateString() + 
                    " and with last current ocuppancy letter date: " + this.nl_current_occupancy_letter_date.ToShortDateString() ;

            if( this.nl_current_occupancy_letter_date.isNull || this.nl_current_occupancy_letter_date.date < this.demand_letter_due_date.date.AddDays(-35))
            {
                Event e = new Event();
                e.loan = this.loan;
                e.type = Event.WARNING;
                e.name = "Removed Demand Current Occupancy Letter Warning";
                e.description = description;
                this.events.Add(e);
            }
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.nl_current_occupancy_letter_date = DbModel.readDBDateObject(reader, pos++);
            this.demand_letter_due_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }        
    }
}