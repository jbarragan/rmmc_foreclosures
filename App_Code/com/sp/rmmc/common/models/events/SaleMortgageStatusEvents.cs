using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class SaleMortgageStatusEvents : BaseEvents
    {
        public String mortgage_status = "";
        public DateTimeObject conveyance_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as sale_mortgage_status_events_loan_id",
             "(select top 1 ms_credit_information.mortgage_status from ms_credit_information where ms_credit_information.loan_id = loan_id_column order by ms_credit_information.mortgage_status ) as sale_mortgage_status_events_mortgage_status",
             "(select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11200 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_ord_req ) as sale_mortgage_status_events_conveyance_date"
            };

        public SaleMortgageStatusEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (conveyance_date.isNull) return;
            if (this.loan.loan_type != "FHA") return;
            if (this.mortgage_status != "46")
            {
                Event e = new Event();
                e.loan = this.loan;
                e.type = Event.WARNING;
                e.name = "Mortgage Status with Conveyance Date Warning";
                e.description = "Mortgage Status, " + this.mortgage_status + ", not equal to 46 expected for Foreclosure Sale with Conveyance Date: " + this.conveyance_date.ToShortDateString();
                this.events.Add(e);
            }
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.mortgage_status = DbModel.readDBString(reader, pos++);
            this.conveyance_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }        
    }
}