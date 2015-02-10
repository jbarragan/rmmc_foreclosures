using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class XdemandMortgageStatusEvents : BaseEvents
    {
        public String mortgage_status = "";

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as xdemand_mortgage_status_events_loan_id",
             "(select top 1 ms_credit_information.mortgage_status from ms_credit_information where ms_credit_information.loan_id = loan_id_column order by ms_credit_information.mortgage_status ) as xdemand_mortgage_status_events_mortgage_status"
            };

        public XdemandMortgageStatusEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.loan.loan_type != "FHA") return;
            if (this.mortgage_status == "20") return;
            if (this.mortgage_status == "") return;
            if (this.mortgage_status != "20" )
            {
                Event e = new Event();
                e.loan = this.loan;
                e.type = Event.WARNING;
                e.name = "Removed Demand Mortgage Status not 20 Warning";
                e.description = "Mortgage Status, " + this.mortgage_status + ", not equal to 20 expected for Removed Demands";
                this.events.Add(e);
            }
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.mortgage_status = DbModel.readDBString(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@xdemand_mortgage_status_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@xdemand_mortgage_status_events_mortgage_status", this.mortgage_status);
        }
    }
}