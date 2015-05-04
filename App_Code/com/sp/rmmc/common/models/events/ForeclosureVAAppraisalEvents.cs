using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class ForeclosureVAAppraisalEvents : BaseEvents
    {
        public DateTimeObject appraisal_ordered_date = new DateTimeObject();
        public DateTimeObject appraisal_received_date = new DateTimeObject();
        public DateTimeObject nov_ordered_date = new DateTimeObject();
        public DateTimeObject nov_received_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as foreclosure_va_appraisal_events_loan_id",
             "(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1100 and     ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date )  as foreclosure_va_appraisal_events_appraisal_ordered_date",
             "(select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1100 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date )  as foreclosure_va_appraisal_events_appraisal_received_date",
             "(select top 1 fcl_ud.fcl_item_ord_req from ms_fcl_ud_item_amts_2 fcl_ud where fcl_ud.loan_id = loan_id_column and fcl_ud.fcl_item_name like 'NOV%' order by fcl_item_ord_req desc) as foreclosure_va_appraisal_events_nov_ordered_date",
             "(select top 1 fcl_ud.fcl_item_recvd_expd from ms_fcl_ud_item_amts_2 fcl_ud where fcl_ud.loan_id = loan_id_column and fcl_ud.fcl_item_name like 'NOV%' order by fcl_item_recvd_expd desc) as foreclosure_va_appraisal_events_nov_received_date"
            };

        public ForeclosureVAAppraisalEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.loan_id == 156899M)
                this.loan_id = 156899M;
            if (loan.loan_type != "VA") return;

            if (this.appraisal_ordered_date.isNull == false &&
                this.appraisal_ordered_date.date < this.loan.event_todays_date.AddDays(-15) &&
                this.nov_ordered_date.isNull == true)
            {
                Event e = new Event();
                e.loan = this.loan;
                e.type = Event.WARNING;
                e.name = "No VA Appraisal NOV Warning";
                e.description = e.name  + " for VA Appraisal Received on: " + this.appraisal_received_date.ToShortDateString();
                this.events.Add(e);
            }

            if (this.appraisal_ordered_date.isNull == false &&
                this.appraisal_ordered_date.date < this.loan.event_todays_date.AddDays(-7) &&
                this.appraisal_received_date.isNull == true)
            {
                Event e = new Event();
                e.loan = this.loan;
                e.type = Event.WARNING;
                e.name = "No VA Appraisal Received Warning";
                e.description = e.name + " for VA Appraisal Received on: " + this.appraisal_received_date.ToShortDateString();
                this.events.Add(e);
            }

            if (this.nov_received_date.isNull == false &&
                this.nov_received_date.date <= this.loan.event_todays_date.AddDays(10) &&
                this.nov_received_date.date >= this.loan.event_todays_date)
            {
                Event e = new Event();
                e.loan = this.loan;
                e.type = Event.WARNING;
                e.name = "VA Appraisal NOV Expiring Warning";
                e.description = "VA Appraisal NOV Expiring with received/expected date on: " + this.nov_received_date.ToShortDateString();
                this.events.Add(e);
            }

        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.appraisal_ordered_date = DbModel.readDBDateObject(reader, pos++);
            this.appraisal_received_date = DbModel.readDBDateObject(reader, pos++);
            this.nov_ordered_date = DbModel.readDBDateObject(reader, pos++);
            this.nov_received_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@foreclosure_va_appraisal_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@foreclosure_va_appraisal_events_appraisal_ordered_date", this.appraisal_ordered_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_va_appraisal_events_appraisal_received_date", this.appraisal_received_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_va_appraisal_events_nov_ordered_date", this.nov_ordered_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_va_appraisal_events_nov_received_date", this.nov_received_date.toDBInsert());
        }
    }
}