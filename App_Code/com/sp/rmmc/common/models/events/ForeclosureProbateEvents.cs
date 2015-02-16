using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class ForeclosureProbateEvents : BaseEvents
    {
        public DateTimeObject probate_start_date = new DateTimeObject();
        public DateTimeObject probate_end_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as foreclosure_probate_events_loan_id",
             "(select top 1 misc.misc_dt_value from ms_loan_misc_fields misc where misc_field_id = 10157 and misc.loan_id = base.loan_id order by misc.misc_dt_value ) as foreclosure_probate_events_probate_start_date",
             "(select top 1 misc.misc_dt_value from ms_loan_misc_fields misc where misc_field_id = 10158 and misc.loan_id = base.loan_id order by misc.misc_dt_value ) as foreclosure_probate_events_probate_end_date"
            };

        public ForeclosureProbateEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.probate_start_date = DbModel.readDBDateObject(reader, pos++);
            this.probate_end_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@foreclosure_probate_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@foreclosure_probate_events_probate_start_date", this.probate_start_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_probate_events_probate_end_date", this.probate_end_date.toDBInsert());
        }
    }
}