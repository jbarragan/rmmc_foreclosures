using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class DemandOccupancyEvents : BaseEvents
    {
        public DateTimeObject demand_letter_due_date = new DateTimeObject();
        public DateTimeObject occupancy_fcl_hud_notice_of_occ_sent_date = new DateTimeObject();
        public DateTimeObject occupancy_prop_insp_date = new DateTimeObject();
        public DateTimeObject last_pi_memo_date = new DateTimeObject();
        public String va_occupancy_code = "";
        public String occupancy_code = "";

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as demand_occupancy_events_loan_id",
             "(select top 1 misc.misc_dt_value from ms_loan_misc_fields misc where misc_field_id = 10020 and misc.loan_id = loan_id_column order by misc.misc_dt_value ) as demand_occupancy_events_demand_letter_due_date",
             "(select top 1 ms_loan_occupancy.fcl_hud_notice_of_occ_sent from ms_loan_occupancy where ms_loan_occupancy.loan_id = loan_id_column order by ms_loan_occupancy.fcl_hud_notice_of_occ_sent desc) as demand_occupancy_events_occupancy_fcl_hud_notice_of_occ_sent_date",
             "(select top 1 ms_loan_occupancy.prop_insp_date from ms_loan_occupancy where ms_loan_occupancy.loan_id = loan_id_column order by ms_loan_occupancy.prop_insp_date desc) as demand_occupancy_events_occupancy_prop_insp_date",
             "(select top 1 memo.memo_notify_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and (ms_memo_types.memo_type_desc like 'PI-%') order by memo_notify_dt desc ) as demand_occupancy_events_last_pi_memo_date",
             "(select top 1 ms_loan_occupancy.va_occ_cd from ms_loan_occupancy where ms_loan_occupancy.loan_id = loan_id_column order by ms_loan_occupancy.prop_insp_date desc, ms_loan_occupancy.fcl_hud_notice_of_occ_sent desc) as demand_occupancy_events_va_occupancy_code",
             "(select top 1 ms_occupancy_types.occ_description from ms_loan_occupancy, ms_occupancy_types where ms_occupancy_types.fha_occ_code = ms_loan_occupancy.fha_occ_code and ms_loan_occupancy.loan_id = loan_id_column order by ms_loan_occupancy.fcl_hud_notice_of_occ_sent desc, ms_loan_occupancy.prop_insp_date desc  ) as demand_occupancy_events_occupancy_code"
            };

        public DemandOccupancyEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.loan.loan_type != "FHA") return;
            if (this.demand_letter_due_date.isNull) return;
            String description = "";
            TimeSpan ts = this.occupancy_date().date - this.last_pi_memo_date.date;
            if (this.occupancy_date().isNull) description = "Occupancy date not set for occupancy " + this.getOccupancyDescriptions();
            else if (this.occupancy_date().isNull) 
                description = "No Last PI Memo set for occupancy " + this.getOccupancyDescriptions() + " with occupancy date: " + this.occupancy_date().ToShortDateString();
            else if (Math.Abs(ts.Days) > 1)
                description = "Occupancy date not updated for occupancy " + this.getOccupancyDescriptions() + 
                    " with occupancy date: " + this.occupancy_date().ToShortDateString() +
                    " and last PI memo date: " + this.last_pi_memo_date.ToShortDateString();

            if (Math.Abs(ts.Days) > 1 || occupancy_date().isNull || this.last_pi_memo_date.isNull){
                Event e = new Event();
                e.loan = this.loan;
                e.type = Event.ALERT;
                e.name = "Demand Occupancy Alert";
                e.description = description;
                this.events.Add(e);
            }
        }

        public string va_occupancy_code_description()
        {
            if (this.va_occupancy_code == "0") return "Abandonment";
            else if (this.va_occupancy_code == "1") return "Original Veteran";
            else if (this.va_occupancy_code == "2") return "Transferree";
            else if (this.va_occupancy_code == "4") return "Vacant";
            else if (this.va_occupancy_code == "8") return "Tenant";
            return "";
        }

        public string getOccupancyDescriptions()
        {
            if (this.loan.loan_type == "VA")
                return this.va_occupancy_code_description();
            else
                return this.occupancy_code;
        }

        public DateTimeObject occupancy_date()
        {
            if (this.loan.loan_type == "FHA")
                return this.occupancy_fcl_hud_notice_of_occ_sent_date;
            else
                return this.occupancy_prop_insp_date;
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.demand_letter_due_date = DbModel.readDBDateObject(reader, pos++);
            this.occupancy_fcl_hud_notice_of_occ_sent_date = DbModel.readDBDateObject(reader, pos++);
            this.occupancy_prop_insp_date = DbModel.readDBDateObject(reader, pos++);
            this.last_pi_memo_date = DbModel.readDBDateObject(reader, pos++);
            this.va_occupancy_code = DbModel.readDBString(reader, pos++);
            this.occupancy_code = DbModel.readDBString(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@demand_occupancy_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@demand_occupancy_events_demand_letter_due_date", this.demand_letter_due_date.toDBInsert());
            cmd.Parameters.AddWithValue("@demand_occupancy_events_occupancy_fcl_hud_notice_of_occ_sent_date", this.occupancy_fcl_hud_notice_of_occ_sent_date.toDBInsert());
            cmd.Parameters.AddWithValue("@demand_occupancy_events_occupancy_prop_insp_date", this.occupancy_prop_insp_date.toDBInsert());
            cmd.Parameters.AddWithValue("@demand_occupancy_events_last_pi_memo_date", this.last_pi_memo_date.toDBInsert());
            cmd.Parameters.AddWithValue("@demand_occupancy_events_va_occupancy_code", this.va_occupancy_code);
            cmd.Parameters.AddWithValue("@demand_occupancy_events_occupancy_code", this.occupancy_code);
        }
    }
}