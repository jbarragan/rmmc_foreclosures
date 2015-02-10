using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class ForeclosureRfsGnmaForeclosureStatusEvents : BaseEvents
    {
        public String foreclosure_status = "";
        public String fcl_type = "";

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as foreclosure_rfs_foreclosure_status_events_loan_id",
             "(select top 1 ms_credit_information.foreclosure_status from ms_credit_information where ms_credit_information.loan_id = loan_id_column order by ms_credit_information.foreclosure_status ) as foreclosure_rfs_foreclosure_status_events_foreclosure_status",
             "(select top 1 ms_fcl_info.fcl_type from ms_fcl_info where ms_fcl_info.loan_id = base.loan_id order by ms_fcl_info.fcl_type) as foreclosure_rfs_foreclosure_status_events_fcl_type"
            };

        public ForeclosureRfsGnmaForeclosureStatusEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (fcl_type != "A") return;
            if (this.loan.isGNMA() != true) return;
            if( this.foreclosure_status != "1" ){
                Event e = new Event();
                e.loan = this.loan;
                e.type = Event.ALERT;
                e.name = "Foreclosure Status Alert";
                e.description = "Foreclosure Status, " + this.foreclosure_status + ", not equal to 'Foreclosure Proceeding Started' expected for Foreclosure process";
                this.events.Add(e);
            }
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.foreclosure_status = DbModel.readDBString(reader, pos++);
            this.fcl_type = DbModel.readDBString(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@foreclosure_rfs_foreclosure_status_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@foreclosure_rfs_foreclosure_status_events_foreclosure_status", this.foreclosure_status);
            cmd.Parameters.AddWithValue("@foreclosure_rfs_foreclosure_status_events_fcl_type", this.fcl_type);
        }
    }
}