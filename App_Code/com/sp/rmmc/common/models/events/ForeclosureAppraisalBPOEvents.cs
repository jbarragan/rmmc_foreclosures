using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class ForeclosureAppraisalBPOEvents : BaseEvents
    {
        public DateTimeObject appraisal_ordered_date = new DateTimeObject();
        public DateTimeObject appraisal_received_date = new DateTimeObject();
        public String fcl_type = "";
        public DateTimeObject papers_to_attorney_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as foreclosure_appraisal_bpo_events_loan_id",
             "(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1100 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date )  as foreclosure_appraisal_bpo_events_appraisal_ordered_date",
             "(select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1100 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date )  as foreclosure_appraisal_bpo_events_appraisal_received_date",
             "(select top 1 ms_fcl_info.fcl_type from ms_fcl_info where ms_fcl_info.loan_id = base.loan_id order by ms_fcl_info.fcl_type) as foreclosure_appraisal_bpo_events_fcl_type",
             "(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1000 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as foreclosure_appraisal_bpo_events_papers_to_attorney_date"
            };

        public ForeclosureAppraisalBPOEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (fcl_type != "A") return;
            if (loan.loan_type == "FHA") return;
            if (this.papers_to_attorney_date.isNull) return;

            String event_text = "BPO Report";
            if (loan.loan_type == "VA") event_text = "Appraisal Report";

            if (this.appraisal_ordered_date.isNull)
            {
                Event e = new Event();
                e.loan = this.loan;
                e.type = Event.ALERT;
                e.name = event_text + " not ordered with Papers to Attorney Date Warning";
                e.description = event_text + " not ordered with Papers to Attorney Date: " + this.papers_to_attorney_date.ToShortDateString();
                this.events.Add(e);
            }

            if (this.appraisal_ordered_date.isNull == false && 
                this.appraisal_received_date.isNull && 
                this.appraisal_ordered_date.date.AddDays(-3) > DateTime.Today )
            {
                Event e = new Event();
                e.loan = this.loan;
                if( this.appraisal_ordered_date.date.AddDays(-7) > DateTime.Today ){
                    e.type = Event.ALERT;
                    e.name = event_text + " not completed Alert";
                }else{
                    e.type = Event.WARNING;
                    e.name = event_text + " not completed Warning";
                }
                e.description = event_text + " not completed. Ordered on Date: " + this.appraisal_ordered_date.ToShortDateString();
                this.events.Add(e);
            }
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.appraisal_ordered_date = DbModel.readDBDateObject(reader, pos++);
            this.appraisal_received_date = DbModel.readDBDateObject(reader, pos++);
            this.fcl_type = DbModel.readDBString(reader, pos++);
            this.papers_to_attorney_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@foreclosure_appraisal_bpo_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@foreclosure_appraisal_bpo_events_appraisal_ordered_date", this.appraisal_ordered_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_appraisal_bpo_events_appraisal_received_date", this.appraisal_received_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_appraisal_bpo_events_fcl_type", this.fcl_type);
            cmd.Parameters.AddWithValue("@foreclosure_appraisal_bpo_events_papers_to_attorney_date", this.papers_to_attorney_date.toDBInsert());
        }
    }
}