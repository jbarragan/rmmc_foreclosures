using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class ForeclosureMortgageStatusEvents : BaseEvents
    {
        public String mortgage_status = "";
        public DateTimeObject posting_date = new DateTimeObject();
        public DateTimeObject papers_to_attorney_date = new DateTimeObject();
        public String fcl_type = "";
        public DateTimeObject demand_letter_due_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as foreclosure_mortgage_status_events_loan_id",
             "(select top 1 ms_credit_information.mortgage_status from ms_credit_information where ms_credit_information.loan_id = loan_id_column order by ms_credit_information.mortgage_status ) as foreclosure_mortgage_status_events_mortgage_status",
             "(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1200 and ms_fcl.loan_id = loan_id_column  order by ms_fcl.fcl_item_date ) as foreclosure_mortgage_status_events_posting_date",
             "(select top 1 ms_fcl_info.fcl_type from ms_fcl_info where ms_fcl_info.loan_id = loan_id_column order by ms_fcl_info.fcl_type) as foreclosure_mortgage_status_events_fcl_type",
             "(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1000 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_date ) as foreclosure_mortgage_status_events_papers_to_attorney_date",
             "(select top 1 misc.misc_dt_value from ms_loan_misc_fields misc where misc_field_id = 10020 and misc.loan_id = loan_id_column order by misc.misc_dt_value ) as foreclosure_mortgage_status_events_demand_letter_due_date"
            };

        public ForeclosureMortgageStatusEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.loan.loan_type != "FHA") return;
            if (fcl_type != "A" && this.demand_letter_due_date.isNull == false)
            {
                if (this.mortgage_status != "42")
                {
                    Event e = new Event();
                    e.loan = this.loan;
                    e.type = Event.WARNING;
                    e.name = "Demand Mortgage Status not 42 Warning";
                    e.description = "Mortgage Status, " + this.mortgage_status + ", not equal to 42 expected for Demand Date: " + this.demand_letter_due_date.ToShortDateString();
                    this.events.Add(e);
                }
            }

            if (fcl_type != "A") return;
            if (this.posting_date.isNull && this.papers_to_attorney_date.isNull == false)
            {
                if (this.mortgage_status != "42")
                {
                    Event e = new Event();
                    e.loan = this.loan;
                    e.type = Event.WARNING;
                    e.name = "Mortgage Status with Papers to Attorney Date Warning";
                    e.description = "Mortgage Status, " + this.mortgage_status + ", not equal to 42 expected for Foreclosure with Papers to Attorney Date: " + this.papers_to_attorney_date.ToShortDateString();
                    this.events.Add(e);
                }
            }
            if (this.posting_date.isNull == false)
            {
                if (this.mortgage_status != "68")
                {
                    Event e = new Event();
                    e.loan = this.loan;
                    e.type = Event.WARNING;
                    e.name = "Mortgage Status with Posting Date Warning";
                    e.description = "Mortgage Status, " + this.mortgage_status + ", not equal to 68 expected for Foreclosure with Posting Date: " + this.posting_date.ToShortDateString();
                    this.events.Add(e);
                }
            }
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.mortgage_status = DbModel.readDBString(reader, pos++);
            this.posting_date = DbModel.readDBDateObject(reader, pos++);
            this.fcl_type = DbModel.readDBString(reader, pos++);
            this.papers_to_attorney_date = DbModel.readDBDateObject(reader, pos++);
            this.demand_letter_due_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@foreclosure_mortgage_status_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@foreclosure_mortgage_status_events_mortgage_status", this.mortgage_status);
            cmd.Parameters.AddWithValue("@foreclosure_mortgage_status_events_posting_date", this.posting_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_mortgage_status_events_fcl_type", this.fcl_type);
            cmd.Parameters.AddWithValue("@foreclosure_mortgage_status_events_papers_to_attorney_date", this.papers_to_attorney_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_mortgage_status_events_demand_letter_due_date", this.demand_letter_due_date.toDBInsert());
        }
    }
}