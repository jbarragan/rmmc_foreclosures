using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class ForeclosurePapersToAttorneyEvents : BaseEvents
    {
        public DateTimeObject papers_to_attorney_date = new DateTimeObject();
        public DateTimeObject received_from_attorney_date = new DateTimeObject();
        public string received_from_attorney_desc = "";

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as foreclosure_papers_to_attorney_events_loan_id",
             "(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1000 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as foreclosure_papers_to_attorney_events_papers_to_attorney_date",
             "(select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1000 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as foreclosure_papers_to_attorney_events_received_from_attorney_date",
             "(select top 1 ms_fcl.fcl_item_desc from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1000 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as foreclosure_papers_to_attorney_events_received_from_attorney_desc"
            };

        public ForeclosurePapersToAttorneyEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.papers_to_attorney_date.isNull) return;
            if (this.papers_to_attorney_date.date >= loan.event_todays_date.AddDays(-3)) return;
            if (this.received_from_attorney_date.isNull == false && 
                this.received_from_attorney_date.date > this.papers_to_attorney_date.date &&
                this.received_from_attorney_desc != "") return;

            String event_text = "Papers Not Received From Attorney Alert";
            Event e = new Event();
            e.loan = this.loan;
            e.type = Event.ALERT;
            e.name = event_text;
            e.description = event_text + " with Papers to Attorney Date: " + this.papers_to_attorney_date.ToShortDateString();
            this.events.Add(e);
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.papers_to_attorney_date = DbModel.readDBDateObject(reader, pos++);
            this.received_from_attorney_date = DbModel.readDBDateObject(reader, pos++);
            this.received_from_attorney_desc = DbModel.readDBString(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@foreclosure_papers_to_attorney_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@foreclosure_papers_to_attorney_events_papers_to_attorney_date", this.papers_to_attorney_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_papers_to_attorney_events_received_from_attorney_date", this.received_from_attorney_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_papers_to_attorney_events_received_from_attorney_desc", this.received_from_attorney_desc);
        }
    }
}