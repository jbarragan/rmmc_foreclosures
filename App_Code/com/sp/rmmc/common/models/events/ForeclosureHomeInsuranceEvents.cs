using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class ForeclosureHomeInsuranceEvents : BaseEvents
    {
        public DateTimeObject reo_date = new DateTimeObject();
        public DateTimeObject third_party_sale_date = new DateTimeObject();

        public string carrier = "";
        public string policy_number = "";

        public DateTimeObject disbursement_date = new DateTimeObject();
        public DateTimeObject expiration_date = new DateTimeObject();

        public decimal disbursement_amount = 0M;
        public decimal policy_coverage = 0M;

        public DateTimeObject part_a_filed_date = new DateTimeObject();
        public DateTimeObject cancel_refuse_haz_insurance_date = new DateTimeObject();
        public DateTimeObject va_acquisition_date = new DateTimeObject();
        public DateTimeObject haz_insurance_endorsement_date = new DateTimeObject();
        public DateTimeObject reo_sold_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as foreclosure_home_insurance_events_loan_id",
             "(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'FC-REO%' order by memo_create_dt desc ) as foreclosure_home_insurance_events_reo_date",
             "(select top 1 memo.memo_create_dt  from ms_loan_memo memo join ms_memo_types on memo.memo_type_id = ms_memo_types.memo_type_id where memo.loan_id = loan_id_column and ms_memo_types.memo_type_desc like 'FC-3rd%' order by memo.memo_create_dt desc ) as foreclosure_home_insurance_events_third_party_sale_date",
             "(select top 1 ba.ba_name from ms_loan_ti_disbursements mltd, business_associates ba, ba_types where mltd.ba_id = ba.ba_id  and loan_id = loan_id_column and mltd.ba_type_id = ba_types.ba_type_id and (ba_types.ba_type like 'H_ME' or ba_types.ba_type like 'FIRE') order by ti_disb_id desc) as foreclosure_home_insurance_events_carrier",
             "(select top 1 mltd.tax_account_number from ms_loan_ti_disbursements mltd, ba_types where loan_id = loan_id_column and mltd.ba_type_id = ba_types.ba_type_id and (ba_types.ba_type like 'H_ME' or ba_types.ba_type like 'FIRE') order by ti_disb_id desc) as foreclosure_home_insurance_events_policy_number",
             "(select top 1 mtds.ti_disb_due_dt from ms_loan_ti_disbursements mltd, ms_ti_disbursement_schedule mtds, ba_types where mltd.loan_id = loan_id_column and mltd.loan_id = mtds.loan_id and mtds.ti_disb_id = mltd.ti_disb_id and mltd.ba_type_id = ba_types.ba_type_id and (ba_types.ba_type like 'H_ME' or ba_types.ba_type like 'FIRE') order by mltd.ti_disb_id desc) as foreclosure_home_insurance_events_disbursement_date",
             "(select top 1 mtds.ti_expire_dt from ms_loan_ti_disbursements mltd, ms_ti_disbursement_schedule mtds, ba_types where mltd.loan_id = loan_id_column and mltd.loan_id = mtds.loan_id and mtds.ti_disb_id = mltd.ti_disb_id and mltd.ba_type_id = ba_types.ba_type_id and (ba_types.ba_type like 'H_ME' or ba_types.ba_type like 'FIRE') order by mltd.ti_disb_id desc) as foreclosure_home_insurance_events_expiration_date",
             "(select top 1 mtds.ti_disb_amt from ms_loan_ti_disbursements mltd, ms_ti_disbursement_schedule mtds, ba_types where mltd.loan_id = loan_id_column and mltd.loan_id = mtds.loan_id and mtds.ti_disb_id = mltd.ti_disb_id and mltd.ba_type_id = ba_types.ba_type_id and (ba_types.ba_type like 'H_ME' or ba_types.ba_type like 'FIRE') order by mltd.ti_disb_id desc) as foreclosure_home_insurance_events_disbursement_amount",
             "(select top 1 mltd.policy_coverage_amt from ms_loan_ti_disbursements mltd, ba_types where loan_id = loan_id_column and mltd.ba_type_id = ba_types.ba_type_id and (ba_types.ba_type like 'H_ME' or ba_types.ba_type like 'FIRE') order by ti_disb_id desc) as foreclosure_home_insurance_events_policy_coverage",
             "(select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11200 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_ord_req ) as foreclosure_home_insurance_events_part_a_filed_date",
             "(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 3400 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_date) as foreclosure_home_insurance_events_cancel_refuse_haz_insurance_date",
             "(select top 1 fcl_ud.fcl_ud_item_recvd_expd from ms_fcl_ud_item_amts_1 fcl_ud where fcl_ud.loan_id = loan_id_column and fcl_ud.fcl_ud_item_desc like '%QUISITION PAY%' order by fcl_ud_item_recvd_expd desc) as foreclosure_home_insurance_events_va_acquisition_date",
             "(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 3000 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_date) as foreclosure_home_insurance_events_haz_insurance_endorsement_date",
             "(select top 1 fcl_ud.fcl_ud_item_recvd_expd from ms_fcl_ud_item_amts_1 fcl_ud where fcl_ud.loan_id = loan_id_column and fcl_ud.fcl_ud_item_desc like '%REO SOLD%' order by fcl_ud_item_recvd_expd desc) as foreclosure_home_insurance_events_reo_sold_date"
            };

        public ForeclosureHomeInsuranceEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.third_party_sale_date.isNull == false) return;
            if (this.reo_sold_date.isNull == false) return;
            if (this.cancel_refuse_haz_insurance_date.isNull == false) return;
            if( this.loan.loan_type == "VA" && this.haz_insurance_endorsement_date.isNull == false ) return;
            if (this.expiration_date.isNull) return;
            if (this.expiration_date.date > this.loan.event_todays_date.AddDays(20)) return;

            Event e = new Event();
            e.loan = this.loan;
            if (this.expiration_date.date > this.loan.event_todays_date)
            {
                e.type = Event.WARNING;
                e.name = "Home Insurance Expiring";
                e.description = "Home Insurance is expiring on " + this.expiration_date.ToString();
            }
            else
            {
                e.type = Event.ALERT;
                e.name = "Home Insurance Expired";
                e.description = "Home Insurance expired on " + this.expiration_date.ToString();
            }
            this.events.Add(e);
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.reo_date = DbModel.readDBDateObject(reader, pos++);
            this.third_party_sale_date = DbModel.readDBDateObject(reader, pos++);
            
            this.carrier = DbModel.readDBString(reader, pos++);
            this.policy_number = DbModel.readDBString(reader, pos++);
            
            this.disbursement_date = DbModel.readDBDateObject(reader, pos++);
            this.expiration_date = DbModel.readDBDateObject(reader, pos++);

            this.disbursement_amount = DbModel.readDBDecimal(reader, pos++);
            this.policy_coverage = DbModel.readDBDecimal(reader, pos++);

            this.part_a_filed_date = DbModel.readDBDateObject(reader, pos++);
            this.cancel_refuse_haz_insurance_date = DbModel.readDBDateObject(reader, pos++);
            this.va_acquisition_date = DbModel.readDBDateObject(reader, pos++);
            this.haz_insurance_endorsement_date = DbModel.readDBDateObject(reader, pos++);
            this.reo_sold_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@foreclosure_home_insurance_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@foreclosure_home_insurance_events_reo_date", this.reo_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_home_insurance_events_third_party_sale_date", this.third_party_sale_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_home_insurance_events_carrier", this.carrier);
            cmd.Parameters.AddWithValue("@foreclosure_home_insurance_events_policy_number", this.policy_number);
            cmd.Parameters.AddWithValue("@foreclosure_home_insurance_events_disbursement_date", this.disbursement_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_home_insurance_events_expiration_date", this.expiration_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_home_insurance_events_disbursement_amount", this.disbursement_amount);
            cmd.Parameters.AddWithValue("@foreclosure_home_insurance_events_policy_coverage", this.policy_coverage);
            cmd.Parameters.AddWithValue("@foreclosure_home_insurance_events_part_a_filed_date", this.part_a_filed_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_home_insurance_events_cancel_refuse_haz_insurance_date", this.cancel_refuse_haz_insurance_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_home_insurance_events_va_acquisition_date", this.va_acquisition_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_home_insurance_events_haz_insurance_endorsement_date", this.haz_insurance_endorsement_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_home_insurance_events_reo_sold_date", this.reo_sold_date.toDBInsert());
        }
    }
}