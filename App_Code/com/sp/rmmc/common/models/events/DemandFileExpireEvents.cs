using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class DemandFileExpireEvents : BaseEvents
    {
        public DateTimeObject demand_letter_due_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as demand_file_expire_events_loan_id",
             "(select top 1 misc.misc_dt_value from ms_loan_misc_fields misc where misc_field_id = 10020 and misc.loan_id = loan_id_column order by misc.misc_dt_value ) as demand_file_expire_events_demand_letter_due_date"
            };

        public DemandFileExpireEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.demand_letter_due_date.isNull) return;
            if (this.demand_letter_due_date.date.AddDays(-3) > DateTime.Today ) return;
            if (this.demand_letter_due_date.date > DateTime.Today)
            {
                Event e = new Event();
                e.loan = this.loan;
                e.type = Event.ALERT;
                e.name = "Demand File Expired Alert";
                e.description = "Demand File Expired for Demand Due Date: " + this.demand_letter_due_date.ToShortDateString();
                this.events.Add(e);
            }
            else
            {
                Event e = new Event();
                e.loan = this.loan;
                e.type = Event.WARNING;
                e.name = "Demand File Expiring Warning";
                e.description = "Demand File Expiring for Demand Due Date: " + this.demand_letter_due_date.ToShortDateString();
                this.events.Add(e);
            }
        }

        public void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.demand_letter_due_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }        
    }
}