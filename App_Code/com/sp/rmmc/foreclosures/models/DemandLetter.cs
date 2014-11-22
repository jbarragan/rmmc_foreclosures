using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Common;

using com.sp.rmmc.common.models;

namespace com.sp.rmmc.foreclosures.models
{
    public class DemandLetter : ForeclosuresCommonModel
    {
        public int id = 0;
        public decimal loan_id = 0M;
        public DateTimeObject demand_letter_due_date = new DateTimeObject();
        public String fcl_type = "";
        public DateTimeObject last_reg_paid_dt = new DateTimeObject();
        public decimal last_payments_balance = 0M;
        public DateTimeObject papers_to_attorney_date = new DateTimeObject();
        public DateTimeObject posting_date = new DateTimeObject();
        public string chapter_filed = "";
        public DateTimeObject bankruptcy_filed_date = new DateTimeObject();
        public DateTimeObject bankruptcy_discharge_date = new DateTimeObject();
        public DateTimeObject bankruptcy_case_dismissed_date = new DateTimeObject();
        public DateTimeObject bankruptcy_closed_date = new DateTimeObject();
        public DateTimeObject nl_curent_occupancy_letter_date = new DateTimeObject();
        public string mortgage_status = "";
        public DateTimeObject mortgage_status_date = new DateTimeObject();
        public DateTimeObject last_inspection_memo_date = new DateTimeObject();
        public DateTimeObject last_notify_inspection_memo_date = new DateTimeObject();
        public string last_inspection_memo_description = "";
        public DateTimeObject memo_with_inspection_report_delinquent_date = new DateTimeObject();
        public String default_reason_code = "";
        public DateTimeObject last_memo_default_reason_code_change_date = new DateTimeObject();
        public String last_memo_default_reason_code = "";
        public String occupancy_code = "";
        public String va_occupancy_code = "";
        public DateTimeObject occupancy_fcl_hud_notice_of_occ_sent_date = new DateTimeObject();
        public DateTimeObject occupancy_prop_insp_date = new DateTimeObject();
        public DateTimeObject last_pi_memo_date = new DateTimeObject();
        public DateTimeObject last_nl_demand_memo_date = new DateTimeObject();
        public DateTimeObject correspondence_on_nl_demand_memo_date = new DateTimeObject();
        public String correspondence_on_nl_demand_memo_subject = "";
        public DateTimeObject last_bc_call_memo_date = new DateTimeObject();
        public DateTimeObject last_bc_promised_to_pay_date = new DateTimeObject();

        public String foreclosure_officers = "";
        public String loan_type = "";
        public MsLoan loan = new MsLoan();
        public ReviewChecklist review_checklist = new ReviewChecklist();

        public static List<DemandLetter> in_pre_demand_letter_mode_list = new List<DemandLetter>();
        public static List<DemandLetter> expired_demand_letter_list = new List<DemandLetter>();
        public static List<DemandLetter> remove_demand_letter_by_loss_mitigation_list = new List<DemandLetter>();
        public static List<DemandLetter> occupacy_letter_for_fha_mode_list = new List<DemandLetter>();
        public static List<DemandLetter> status_code_for_fha_list = new List<DemandLetter>();

        

        private static string base_query = "" +
"select base.loan_id, " +
"base.misc_dt_value as demand_letter_due_date, " +
"(select top 1 misc.misc_field_value from ms_loan_misc_fields misc where misc_field_id = 10023 and misc.loan_id = base.loan_id order by misc.misc_field_value ) as foreclosure_officers, " +
"(select top 1 ms_fcl_info.fcl_type from ms_fcl_info where ms_fcl_info.loan_id = base.loan_id order by ms_fcl_info.fcl_type) as fcl_type, " +
"(select top 1 hist.paid_dt from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'REG' order by paid_dt desc) as last_reg_paid_dt, " +
"(select sum(hist.pmt_amt) from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd in ('REG', 'REGR') and hist.paid_dt >=  dateadd(dd, -30, base.misc_dt_value)) as last_payments_balance, " +
"(select top 1 ms_loan_info.loan_type from ms_loan_information ms_loan_info where ms_loan_info.loan_id = base.loan_id order by ms_loan_info.loan_type ) as loan_type, " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1000 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as papers_to_attorney_date, " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1200 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as posting_date, " +
"(select top 1 ms_credit_information.chapter_filed from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.chapter_filed ) as chapter_filed, " +
"(select top 1 ms_credit_information.bankruptcy_filed_date from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.bankruptcy_filed_date ) as bankruptcy_filed_date, " +
"(select top 1 ms_bankruptcy_info.discharge_date from ms_bankruptcy_info where ms_bankruptcy_info.loan_id = base.loan_id order by ms_bankruptcy_info.discharge_date ) as bankruptcy_discharge_date, " +
"(select top 1 ms_bankruptcy_info.case_dismissed_date from ms_bankruptcy_info where ms_bankruptcy_info.loan_id = base.loan_id order by ms_bankruptcy_info.case_dismissed_date ) as bankruptcy_case_dismissed_date, " +
"(select top 1 ms_bankruptcy_info.closed_date from ms_bankruptcy_info where ms_bankruptcy_info.loan_id = base.loan_id order by ms_bankruptcy_info.closed_date ) as bankruptcy_closed_date, " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id = 138 order by memo_id desc) as nl_curent_occupancy_letter_date, " +
"(select top 1 ms_credit_information.mortgage_status from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.mortgage_status ) as mortgage_status, " +
"(select top 1 date(substr(uad_audit_data_af_chg, 7, 4) + '/' + substr(uad_audit_data_af_chg, 1, 2) + '/' + substr(uad_audit_data_af_chg, 4, 2)) as mortgage_status_date from ms_user_audit_history_data ms_audit_history where ms_audit_history.loan_id = base.loan_id and ms_audit_history.uad_audit_field_name = 'mortgage_status_date' and ms_audit_history.uad_audit_field_name is not null and length(uad_audit_data_af_chg) >= 10 and isdate( mortgage_status_date ) = 1 order by 1 desc ) as mortgage_status_date, " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'PI-%' order by memo_id desc ) as last_inspection_memo_date, " +
"(select top 1 memo.memo_notify_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'PI-%' order by memo.memo_notify_dt desc ) as last_notify_inspection_memo_date, " +
"(select top 1 ms_memo_types.memo_type_desc from ms_loan_memo memo, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'PI-%' order by memo.memo_notify_dt desc ) as last_inspection_memo_description, " +
"(select top 1 memo.memo_notify_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and lower(memo.memo_subject) like '%property inspection report%' order by memo.memo_notify_dt desc ) as memo_with_inspection_report_delinquent_date, " +
//"(select top 1 ms_memo_types.memo_type_desc from ms_loan_memo memo, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'PI-%' order by memo_id desc ) as last_inspection_memo_description, " +
//"(select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and lower(memo.memo_subject) like '%property inspection report%' order by memo_id desc ) as memo_with_inspection_report_delinquent_date, " +
"(select top 1 ms_credit_information.default_reason_code from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.default_reason_code ) as main_default_reason_code, " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_categories, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_category_id = ms_memo_categories.memo_category_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_categories.memo_category_desc like '0%' and (ms_memo_types.memo_type_desc = 'BC-Promised to Pay' or ms_memo_types.memo_type_desc = 'BC-First Right Party Contact' or ms_memo_categories.memo_category_desc like '031%')  order by memo_create_dt desc ) as last_memo_default_reason_code_change_date, " +
"(select top 1 ms_memo_categories.memo_category_desc from ms_loan_memo memo, ms_memo_categories, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_category_id = ms_memo_categories.memo_category_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_categories.memo_category_desc like '0%' and (ms_memo_types.memo_type_desc = 'BC-Promised to Pay' or ms_memo_types.memo_type_desc = 'BC-First Right Party Contact' or ms_memo_categories.memo_category_desc like '031%')  order by memo_create_dt desc ) as last_memo_default_reason_code, " +
"(select top 1 ms_occupancy_types.occ_description from ms_loan_occupancy, ms_occupancy_types where ms_occupancy_types.fha_occ_code = ms_loan_occupancy.fha_occ_code and ms_loan_occupancy.loan_id = base.loan_id order by ms_loan_occupancy.fcl_hud_notice_of_occ_sent desc, ms_loan_occupancy.prop_insp_date desc  ) as occupancy_code, " +
"(select top 1 ms_loan_occupancy.va_occ_cd from ms_loan_occupancy where ms_loan_occupancy.loan_id = base.loan_id order by ms_loan_occupancy.prop_insp_date desc, ms_loan_occupancy.fcl_hud_notice_of_occ_sent desc) as va_occupancy_code, " +
"(select top 1 ms_loan_occupancy.fcl_hud_notice_of_occ_sent from ms_loan_occupancy where ms_loan_occupancy.loan_id = base.loan_id order by ms_loan_occupancy.fcl_hud_notice_of_occ_sent desc) as occupancy_fcl_hud_notice_of_occ_sent_date, " +
"(select top 1 ms_loan_occupancy.prop_insp_date from ms_loan_occupancy where ms_loan_occupancy.loan_id = base.loan_id order by ms_loan_occupancy.prop_insp_date desc) as occupancy_prop_insp_date, " +
"(select top 1 memo.memo_notify_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_type_id = ms_memo_types.memo_type_id and (ms_memo_types.memo_type_desc like 'PI-%') order by memo_notify_dt desc ) as last_pi_memo_date, " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_type_id = ms_memo_types.memo_type_id and (ms_memo_types.memo_type_desc like 'NL-Demand') order by memo_create_dt desc ) as last_nl_demand_memo_date, " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_subject like '%Demand%' and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'Correspondence' and date(memo.memo_create_dt) = date(last_nl_demand_memo_date) order by memo_create_dt desc ) as correspondence_on_nl_demand_memo_date, " +
"(select top 1 memo.memo_subject from ms_loan_memo memo, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_subject like '%Demand%' and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'Correspondence' and date(memo.memo_create_dt) = date(last_nl_demand_memo_date) order by memo_create_dt desc ) as correspondence_on_nl_demand_memo_subject, " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'BC%' order by memo_id desc ) as last_bc_call_memo_date, " +
"(select top 1 memo.memo_notify_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc = 'BC-Promised to Pay' order by memo_notify_dt desc ) as last_bc_promised_to_pay_date, " +


MsLoan.columns("base.loan_id") +  ", " +
ReviewChecklist.columns("base.loan_id") +
"from ms_loan_misc_fields as base " +
"where base.misc_field_id = 10020 ";

        public static List<DemandLetter> in_pre_demand_letter_mode()
        {
            if (in_pre_demand_letter_mode_list.Count == 0)
                in_pre_demand_letter_mode_list = assign_in_pre_demand_and_remove_from_lm(DemandLetter.in_pre_demand_letter_mode_conditions());
            return in_pre_demand_letter_mode_list;
        }

        public static List<DemandLetter> occupacy_letter_for_fha_mode()
        {
            return occupacy_letter_for_fha_mode_list = assign_in_pre_demand_and_remove_from_lm(DemandLetter.occupacy_letter_for_fha_mode_conditions());
        }

        public static List<DemandLetter> status_code_for_fha_mode()
        {
            return assign_in_pre_demand_and_remove_from_lm(DemandLetter.status_code_for_fha_mode_conditions());
        }

        public static List<DemandLetter> remove_from_demand_letter_by_loss_mitigation()
        {
            return remove_demand_letter_by_loss_mitigation_list;
        }

        public static string in_pre_demand_letter_mode_conditions()
        {
            string query = " and ( demand_letter_due_date >= getdate() ) \n" +
                            " and ( fcl_type <> 'A' or fcl_type is null ) \n" +
                            " and ( bankruptcy_filed_date is null or bankruptcy_discharge_date is not null or bankruptcy_case_dismissed_date is not null or bankruptcy_closed_date is not null ) \n" +
                            " and ( last_reg_paid_dt < dateadd(dd, -33, demand_letter_due_date) or last_reg_paid_dt is null or last_payments_balance <= 0)  ";
            return query;
        }

        public static string occupacy_letter_for_fha_mode_conditions()
        {
            string query = " and ( ms_loan_type = 'FHA' ) \n" +
                            " and ( nl_curent_occupancy_letter_date is NULL ) \n" +
                            " and ( demand_letter_due_date >= getdate() ) \n" +
                            " and ( fcl_type <> 'A' or fcl_type is null ) \n" +
                            " and ( bankruptcy_filed_date is null or bankruptcy_discharge_date is not null or bankruptcy_case_dismissed_date is not null or bankruptcy_closed_date is not null ) \n" +
                            " and ( last_reg_paid_dt < dateadd(dd, -33, demand_letter_due_date) or last_reg_paid_dt is null or last_payments_balance <= 0)  ";
            return query;
        }

        public static string status_code_for_fha_mode_conditions()
        {
            string query = " and ( ms_loan_type = 'FHA' ) \n" +
                            " and ( mortgage_status <> '20' ) \n" +
                            " and ( demand_letter_due_date >= getdate() ) \n" +
                            " and ( fcl_type <> 'A' or fcl_type is null ) \n" +
                            " and ( bankruptcy_filed_date is null or bankruptcy_discharge_date is not null or bankruptcy_case_dismissed_date is not null or bankruptcy_closed_date is not null ) \n" +
                            " and ( last_reg_paid_dt < dateadd(dd, -33, demand_letter_due_date) or last_reg_paid_dt is null or last_payments_balance <= 0)  ";
            return query;
        }

        public static List<DemandLetter> remove_lm(List<DemandLetter> list)
        {
            List<DemandLetter> result_list = new List<DemandLetter>();
            List<LMReception> demand_letters_lm_receptions = LMReception.get_demand_letters_lm_receptions(list);
            List<decimal> list_lm_reception_loan_ids = new List<decimal>();
            decimal[] array_lm_reception_loan_ids;
            foreach (LMReception lm_reception in demand_letters_lm_receptions)
                list_lm_reception_loan_ids.Add(lm_reception.loan_id);
            array_lm_reception_loan_ids = list_lm_reception_loan_ids.ToArray();
            foreach (DemandLetter demand_letter in list)
                if (Array.IndexOf(array_lm_reception_loan_ids, demand_letter.loan_id) < 0)
                    result_list.Add(demand_letter);

            return result_list;
        }

        private static List<DemandLetter> assign_in_pre_demand_and_remove_from_lm(string conditions)
        {
            List<DemandLetter> result_list = new List<DemandLetter>();
            string query = base_query + conditions;
            query = query + " order by foreclosure_officers, loan_type, last_reg_paid_dt asc";
            List<DemandLetter> tmp_in_pre_demand_letter_mode = get_list(query);

            List<LMReception> demand_letters_lm_receptions = LMReception.get_demand_letters_lm_receptions(tmp_in_pre_demand_letter_mode);
            List<decimal> list_lm_reception_loan_ids = new List<decimal>();
            decimal[] array_lm_reception_loan_ids;
            foreach (LMReception lm_reception in demand_letters_lm_receptions)
                list_lm_reception_loan_ids.Add(lm_reception.loan_id);
            array_lm_reception_loan_ids = list_lm_reception_loan_ids.ToArray();
            foreach (DemandLetter demand_letter in tmp_in_pre_demand_letter_mode)
                if (Array.IndexOf(array_lm_reception_loan_ids, demand_letter.loan_id) < 0)
                    result_list.Add(demand_letter);
                else
                    remove_demand_letter_by_loss_mitigation_list.Add(demand_letter);

            return result_list;
        }


        public static string in_demand_letter_expired_mode_conditions()
        {
            string query = " and ( demand_letter_due_date < dateadd(dd, 5,getdate()) ) " +
                            " and ( fcl_type <> 'A' or fcl_type is null ) " +
                            " and ( bankruptcy_filed_date is null or bankruptcy_discharge_date is not null or bankruptcy_case_dismissed_date is not null or bankruptcy_closed_date is not null ) " +
                            " and ( last_reg_paid_dt < dateadd(dd, -38, demand_letter_due_date) or last_reg_paid_dt is null or last_payments_balance <= 0 ) " +
                            " and ( ms_loan_prin_bal > 0 ) ";
            return query;
        }

        public static List<DemandLetter> in_demand_letter_expired_mode()
        {
            if (expired_demand_letter_list.Count == 0)
                expired_demand_letter_list = assign_in_pre_demand_and_remove_from_lm(DemandLetter.in_demand_letter_expired_mode_conditions());
            return expired_demand_letter_list;
        }

        public static string inspections_expired_conditions()
        {
            string query = " and ( fcl_type <> 'A' or fcl_type is null ) " +
                           " and ( bankruptcy_filed_date is null or bankruptcy_discharge_date is not null or bankruptcy_case_dismissed_date is not null or bankruptcy_closed_date is not null ) " +
                           " and ( last_reg_paid_dt < dateadd(dd, -38, demand_letter_due_date) or last_reg_paid_dt is null or last_payments_balance <= 0 ) " +
                           " and ( ms_loan_prin_bal > 0 ) " +
                           " and ( memo_with_inspection_report_delinquent_date is null or memo_with_inspection_report_delinquent_date < last_notify_inspection_memo_date ) " +
                           " and (  ( last_inspection_memo_description in ('PI-Initial Vacant Inspection', 'PI-Ongoing Int./Ext. Vacant Inspection', 'PI-No Inspection Ordered')  and  last_notify_inspection_memo_date < dateadd(dd, -25, getdate()) ) " +
                           "     or ( last_notify_inspection_memo_date is null ) " +
                           "     or ( last_inspection_memo_description not in ('PI-Initial Vacant Inspection', 'PI-Ongoing Int./Ext. Vacant Inspection', 'PI-No Inspection Ordered')  and  last_notify_inspection_memo_date < dateadd(dd, -20, getdate()) ) ) ";            
            return query;
        }

        public static List<DemandLetter> inspections_expired_mode()
        {
            string query = base_query + inspections_expired_conditions();
            query = query + " order by loan_type, demand_letter_due_date asc";
            return remove_lm(get_list(query));
        }

        public static string fannie_mae_notification_conditions()
        {
            string query = " and ( fcl_type <> 'A' or fcl_type is null ) " +
                           " and ( bankruptcy_filed_date is null or bankruptcy_discharge_date is not null or bankruptcy_case_dismissed_date is not null or bankruptcy_closed_date is not null ) " +
                           " and ( last_reg_paid_dt < dateadd(dd, -38, demand_letter_due_date) or last_reg_paid_dt is null or last_payments_balance <= 0 ) " +
                           " and ( ms_loan_prin_bal > 0 ) " +
                           " and ( loan_type = 'CONV' or loan_type = 'CNV') " +
                           " and ( getdate() > last_bc_promised_to_pay_date  or last_bc_promised_to_pay_date is null ) ";
            return query;
        }

        public static List<DemandLetter> fannie_mae_notification_mode()
        {
            string query = base_query + fannie_mae_notification_conditions();
            query = query + " order by last_bc_call_memo_date asc";
            return remove_lm(get_list(query));
        }

        public static string remove_demand_letter_by_last_reg_payment_conditions()
        {
            string query = " and ( demand_letter_due_date > dateadd(dd, -30, getdate()) ) " +
                            " and ( last_reg_paid_dt >= dateadd(dd, -33, demand_letter_due_date) and last_payments_balance > 0 ) ";
            return query;
        }

        public static List<DemandLetter> remove_demand_letter_by_last_reg_payment()
        {
            string query = base_query + remove_demand_letter_by_last_reg_payment_conditions();
            query = query + " order by loan_type, demand_letter_due_date asc";
            return get_list(query);
        }

        public static string remove_demand_letter_by_foreclosure_active_conditions()
        {
            string query =  " and ( demand_letter_due_date > getdate() ) " +
                            " and ( fcl_type = 'A' ) ";
            return query;
        }

        public static List<DemandLetter> remove_demand_letter_by_foreclosure_active()
        {
            string query = base_query + remove_demand_letter_by_foreclosure_active_conditions();
            query = query + " order by loan_type, demand_letter_due_date asc";
            return get_list(query);
        }

        public static string remove_from_demand_letter_by_bankruptcy_conditions()
        {
            string query = " and ( demand_letter_due_date > getdate() ) " +
                           " and ( bankruptcy_filed_date is not null and bankruptcy_discharge_date is null and bankruptcy_case_dismissed_date is null and bankruptcy_closed_date is null ) ";
            return query;
        }

        public static List<DemandLetter> remove_from_demand_letter_by_bankruptcy()
        {
            string query = base_query + remove_from_demand_letter_by_bankruptcy_conditions();
            query = query + " order by loan_type, demand_letter_due_date asc";
            return get_list(query);
        }

        public string in_pre_demand_letter_tr_class()
        {
            string s = "";
            if (DateTime.Today.AddDays(5) > demand_letter_due_date.date)
                s = "error";
            else if (DateTime.Today.AddDays(10) > demand_letter_due_date.date)
                s = "warning";
            return s;
        }

        public string last_inspection_since()
        {
            if (this.last_notify_inspection_memo_date.isNull)
                return "NA";
            else
            {
                TimeSpan timespan = (DateTime.Now - this.last_notify_inspection_memo_date.date);
                return timespan.Days.ToString();
            }
        }

        public string months_demand_date_after_delinquent()
        {
            if( this.loan.due_date_next_payment.isNull || this.demand_letter_due_date.isNull )
                return "NA";
            else
            {
                TimeSpan timespan = (this.demand_letter_due_date.date - this.loan.due_date_next_payment.date);
                return ((timespan.Days)/30).ToString();
            }
        }

        public string months_demand_date_after_delinquent_color()
        {
            if( this.loan.due_date_next_payment.isNull || this.demand_letter_due_date.isNull )
                return "text-error";
            else
            {
                TimeSpan timespan = (this.demand_letter_due_date.date - this.loan.due_date_next_payment.date);
                int max_month_wait = 3;
                if (((timespan.Days) / 30) > max_month_wait) return "text-error";
                else return "text-success";
            }
        }

        public string default_reason_code_status_color()
        {
            if( this.last_memo_default_reason_code.StartsWith(this.default_reason_code) ) return "text-success";
            return "text-error";
        }

        public DateTimeObject occupancy_date()
        {
            if (this.loan.loan_type == "FHA")
            {
                return this.occupancy_fcl_hud_notice_of_occ_sent_date;
            }
            else
            {
                return this.occupancy_prop_insp_date;
            }
        }

        public string occupancy_code_status_color()
        {   
            TimeSpan ts = occupancy_date().date - this.last_pi_memo_date.date;
            if (Math.Abs(ts.Days) > 1 || occupancy_date().isNull || this.last_pi_memo_date.isNull) return "text-error";
            return "text-success";
        }

        public string letters_sent_status_color()
        {
            if (this.last_nl_demand_memo_date.isNull || this.correspondence_on_nl_demand_memo_date.isNull) return "text-error";
            return "text-success";
        }

        public bool does_require_fannie_mae_call(){
            TimeSpan ts = DateTime.Today.AddDays(1) - this.last_bc_call_memo_date.date;
            if (ts.Days >= 3) return true;
            if (ts.Days == 0) return false;
            for (int i = 3 - ts.Days; i >= 1; i--)
            {
                DateTime date_to_call = DateTime.Today.AddDays(i);
                if (date_to_call.DayOfWeek != DayOfWeek.Saturday && date_to_call.DayOfWeek != DayOfWeek.Sunday) return false;
            }
            return true;
        }

        public string all_occupancy_codes_descriptions()
        {
            if (this.loan_type == "VA")
            {
                return this.va_occupancy_code_description();
            }
            else
            {
                return this.occupancy_code;
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

        private static List<DemandLetter> get_list(String query)
        {
            List<DemandLetter> list = new List<DemandLetter>();
            DbDataReader reader = getReader(query);
            while (reader.Read())
            {
                DemandLetter demand_letter = new DemandLetter();
                demand_letter = read(reader);
                list.Add(demand_letter);
            }
            reader.Close();
            close_connection();
            return list;
        }

        public static DemandLetter get(decimal loan_id)
        {
            string query = base_query + " and loan_id = " + loan_id.ToString();
            DemandLetter demand_letter = new DemandLetter();
            DbDataReader reader = getReader(query);
            if (reader.Read())
                demand_letter = read(reader);
            reader.Close();
            return demand_letter;
        }

        private static DemandLetter read(DbDataReader reader)
        {
            DemandLetter demand_letter = new DemandLetter();
            int pos = 0;
            demand_letter.loan_id = readDBDecimal(reader, pos++);
            demand_letter.demand_letter_due_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.foreclosure_officers = readDBString(reader, pos++);
            demand_letter.fcl_type = readDBString(reader, pos++);
            demand_letter.last_reg_paid_dt = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.last_payments_balance = readDBDecimal(reader, pos++);
            demand_letter.loan_type = readDBString(reader, pos++);
            demand_letter.papers_to_attorney_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.posting_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.chapter_filed = readDBString(reader, pos++);
            demand_letter.bankruptcy_filed_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.bankruptcy_discharge_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.bankruptcy_case_dismissed_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.bankruptcy_closed_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.nl_curent_occupancy_letter_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.mortgage_status = readDBString(reader, pos++);
            demand_letter.mortgage_status_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.last_inspection_memo_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.last_notify_inspection_memo_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.last_inspection_memo_description = readDBString(reader, pos++);
            demand_letter.memo_with_inspection_report_delinquent_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.default_reason_code = readDBString(reader, pos++);
            demand_letter.last_memo_default_reason_code_change_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.last_memo_default_reason_code = readDBString(reader, pos++);
            demand_letter.occupancy_code = readDBString(reader, pos++);
            demand_letter.va_occupancy_code = readDBString(reader, pos++);
            demand_letter.occupancy_fcl_hud_notice_of_occ_sent_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.occupancy_prop_insp_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.last_pi_memo_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.last_nl_demand_memo_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.correspondence_on_nl_demand_memo_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.correspondence_on_nl_demand_memo_subject = readDBString(reader, pos++);
            demand_letter.last_bc_call_memo_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.last_bc_promised_to_pay_date = readDBDateObject(reader, pos++, DateTime.Now);

            if (demand_letter.loan_id > 0)
            {
                demand_letter.loan = MsLoan.read(reader, pos);
                demand_letter.review_checklist = ReviewChecklist.read(reader, pos + MsLoan.COLUMNS);
            }
            return demand_letter;
        }
    }
}