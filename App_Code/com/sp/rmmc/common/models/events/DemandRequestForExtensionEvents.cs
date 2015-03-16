using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class DemandRequestForExtensionEvents : BaseEvents
    {
        public DateTimeObject extension_ordered_date = new DateTimeObject();
        public DateTimeObject foreclosure_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as demand_request_for_extension_events_loan_id",
             "(select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 10800 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_ord_req ) as demand_request_for_extension_events_extension_ordered_date",
             "(select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1200 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_recvd_expd ) as demand_request_for_extension_events_foreclosure_date"
            };

        public DemandRequestForExtensionEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.loan.loan_type != "FHA") return;
            if (this.foreclosure_date.isNull) return;
            if (this.extension_ordered_date.isNull) return;
            if (this.foreclosure_date.date > this.loan.event_todays_date.AddDays(30) ) return;
            if (this.foreclosure_date.date <= this.extension_ordered_date.date) return;
            Event e = new Event();
            e.loan = this.loan;
            e.type = Event.ALERT;
            e.name = "Demand Request for Extension of Time Alert";
            e.description = "Demand Request for Extension of Time required with foreclosure date: " + this.foreclosure_date.ToString() + 
                " and extension ordered to " + this.extension_ordered_date.ToString();
            this.events.Add(e);
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.extension_ordered_date = DbModel.readDBDateObject(reader, pos++);
            this.foreclosure_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@demand_request_for_extension_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@demand_request_for_extension_events_extension_ordered_date", this.extension_ordered_date.toDBInsert());
            cmd.Parameters.AddWithValue("@demand_request_for_extension_events_foreclosure_date", this.foreclosure_date.toDBInsert());
        }
    }
}