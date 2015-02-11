using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class SaleEvictionProcessEvents : BaseEvents
    {
        public string fs_eviction_initiated = "";
        public DateTimeObject eviction_stop_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as sale_eviction_process_events_loan_id",
             "(select top 1 misc.misc_field_value from ms_loan_misc_fields misc where misc_field_id = 10084 and misc.loan_id = base.loan_id order by misc.misc_field_value ) as sale_eviction_process_events_fs_eviction_initiated",
             "(select top 1 misc.misc_dt_value from ms_loan_misc_fields misc where misc_field_id = 10154 and misc.loan_id = base.loan_id order by misc.misc_dt_value ) as sale_eviction_process_events_eviction_stop_date"
            };

        public SaleEvictionProcessEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.fs_eviction_initiated == null || this.fs_eviction_initiated == "") return;
            DateTimeObject eviction_initiated_date = string2date(this.fs_eviction_initiated);
            if (eviction_initiated_date.isNull == false && eviction_stop_date.isNull == false &&
                eviction_initiated_date.date <= eviction_stop_date.date) return;

            Event e = new Event();
            e.loan = this.loan;
            if (eviction_initiated_date.isNull == true ||  eviction_initiated_date.date < DateTime.Today.AddDays(-30))
            {
                e.type = Event.ALERT;
                e.name = "Eviction Process Alert";
            }
            else
            {
                e.name = "Eviction Process Warning";
                e.type = Event.WARNING;
            }
            e.description = "Eviction Process Pending for Eviction Initiated " + this.fs_eviction_initiated;
            this.events.Add(e);
        }

        private DateTimeObject string2date(string s)
        {
            DateTimeObject d = new DateTimeObject();
            try
            {
                d.set_date(DateTime.Parse(s));
            }
            catch (Exception e) { }
            return d;
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.fs_eviction_initiated = DbModel.readDBString(reader, pos++);
            this.eviction_stop_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@sale_eviction_process_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@sale_eviction_process_events_fs_eviction_initiated", this.fs_eviction_initiated);
            cmd.Parameters.AddWithValue("@sale_eviction_process_events_eviction_stop_date", this.eviction_stop_date.toDBInsert());
        }
    }
}