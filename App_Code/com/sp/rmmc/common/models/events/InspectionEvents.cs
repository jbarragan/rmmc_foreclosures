
using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class InspectionEvents : BaseEvents
    {
        public DateTimeObject last_ordered_date = new DateTimeObject();
        public DateTimeObject last_received_notify_date = new DateTimeObject();
        public String last_received_description = "";

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as inspection_events_loan_id",
             "(select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = loan_id_column and lower(memo.memo_subject) like '%property inspection report%' order by memo.memo_create_dt desc ) as inspection_events_last_ordered_date", 
             "(select top 1 memo.memo_notify_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'PI-%' order by memo.memo_notify_dt desc ) as inspection_events_last_received_notify_date",
             "(select top 1 ms_memo_types.memo_type_desc from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'PI-%' order by memo.memo_notify_dt desc ) as inspection_events_last_received_description"
            };

        public InspectionEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.last_ordered_date.isNull)
            {
                Event e = new Event();
                e.loan = this.loan;
                e.type = Event.ALERT;
                e.name = "No Inspection Ordered";
                e.description = "No Inspection Ordered";
                this.events.Add(e);
            }
            else if (this.last_received_notify_date.isNull == false && 
                     this.last_received_notify_date.date > this.last_ordered_date.date &&
                     this.last_received_notify_date.date < DateTime.Now.AddDays(-25))
            {
                Event e = new Event();
                e.loan = this.loan;
                e.type = Event.ALERT;
                e.name = "Inspection Expired";
                e.description = "Inspection Expired, last received on " + this.last_received_notify_date.ToString() + "; last ordered on " + this.last_ordered_date.ToShortDateString();
                this.events.Add(e);
            }
            else if (this.last_received_notify_date.isNull == false &&
                     this.last_received_notify_date.date > this.last_ordered_date.date &&
                     this.last_received_notify_date.date < DateTime.Now.AddDays(-20) && 
                      ( this.last_received_description != "PI-Initial Vacant Inspection" &&
                        this.last_received_description != "PI-Ongoing Int./Ext. Vacant Inspection" &&
                        this.last_received_description != "PI-No Inspection Ordered"
                      )
                    )
            {
                Event e = new Event();
                e.loan = this.loan;
                e.type = Event.ALERT;
                e.name = "Inspection Expired";
                e.description = "Inspection Expired, last received on " + this.last_received_notify_date.ToString() + "; last ordered on " + this.last_ordered_date.ToShortDateString();
                this.events.Add(e);
            }
            else if (this.last_received_notify_date.isNull || this.last_received_notify_date.date < this.last_ordered_date.date.AddDays(-1))
            {
                if (this.last_ordered_date.date.AddDays(19) < DateTime.Today && this.last_ordered_date.date.AddDays(26) > DateTime.Today)
                {
                    Event e = new Event();
                    e.loan = this.loan;
                    e.type = Event.WARNING;
                    e.name = "No Inspection Received Warning";
                    e.description = "No Inspection Received in the last 19 days, last ordered on " + this.last_ordered_date.ToShortDateString();
                    this.events.Add(e);
                }
                else if (this.last_ordered_date.date.AddDays(26) <= DateTime.Today)
                {
                    Event e = new Event();
                    e.loan = this.loan;
                    e.type = Event.ALERT;
                    e.name = "No Inspection Received Alert";
                    e.description = "No Inspection Received in the last 26 days, last ordered on " + this.last_ordered_date.ToShortDateString();
                    this.events.Add(e);
                }
            }
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.last_ordered_date = DbModel.readDBDateObject(reader, pos++);
            this.last_received_notify_date = DbModel.readDBDateObject(reader, pos++);
            this.last_received_description = DbModel.readDBString(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@inspection_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@inspection_events_last_ordered_date", this.last_ordered_date.toDBInsert());
            cmd.Parameters.AddWithValue("@inspection_events_last_received_notify_date", this.last_received_notify_date.toDBInsert());
            cmd.Parameters.AddWithValue("@inspection_events_last_received_description", this.last_received_description);
        }

    }
}