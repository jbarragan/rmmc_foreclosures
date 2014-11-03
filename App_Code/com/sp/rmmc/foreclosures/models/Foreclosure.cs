using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Common;

using com.sp.rmmc.common.models;

namespace com.sp.rmmc.foreclosures.models
{
    public class Foreclosure : ForeclosuresCommonModel
    {
        public int id = 0;
        public decimal loan_id = 0M;
        public DateTimeObject demand_letter_due_date = new DateTimeObject();
        public String fcl_type = "";
        public DateTimeObject last_reg_paid_dt = new DateTimeObject();
        public decimal last_payments_balance = 0M;
        public DateTimeObject papers_to_attorney_date = new DateTimeObject();
        public DateTimeObject posting_date = new DateTimeObject();
        public DateTimeObject sale_date = new DateTimeObject();
        public DateTimeObject sale_date_completed = new DateTimeObject();
        public string chapter_filed = "";
        public DateTimeObject bankruptcy_filed_date = new DateTimeObject();
        public DateTimeObject bankruptcy_discharge_date = new DateTimeObject();
        public DateTimeObject bankruptcy_case_dismissed_date = new DateTimeObject();
        public DateTimeObject bankruptcy_closed_date = new DateTimeObject();
        public String deed_in_lieu = "";
        public DateTimeObject loss_write_off_recvd_date = new DateTimeObject();
        public DateTimeObject fc_foreclosure_referral_date = new DateTimeObject();
        public DateTimeObject fc_payoff_date = new DateTimeObject();
        public DateTimeObject appraisal_ordered_date = new DateTimeObject();
        public DateTimeObject foreclosure_date = new DateTimeObject();

        public String main_default_reason_code = ""; 
        public DateTimeObject last_default_reason_code_change_date = new DateTimeObject();
        public DateTimeObject last_bc_memo_date = new DateTimeObject();
        public String occupancy_code = "";
        public String va_occupancy_code = "";
        public DateTimeObject occupancy_fcl_hud_notice_of_occ_sent_date = new DateTimeObject();
        public DateTimeObject occupancy_prop_insp_date = new DateTimeObject();
        public DateTimeObject last_pi_memo_date = new DateTimeObject();

        public DateTimeObject last_inspection_memo_date = new DateTimeObject();
        public String last_inspection_memo_description = "";
        public DateTimeObject memo_with_inspection_report_delinquent_date = new DateTimeObject();

        public DateTimeObject last_bc_call_memo_date = new DateTimeObject();
        public DateTimeObject last_bc_promised_to_pay_date = new DateTimeObject();

        public decimal ytd_prin_paid_amt = 0M;
        public decimal principal_amt_from_history = 0M;

        public String foreclosure_officers = "";
        public String loan_type = "";
        public MsLoan loan = new MsLoan();

        public PreforeclosureEvent preforeclosure_events = new PreforeclosureEvent();

        public static List<Foreclosure> foreclosure_activated_list = new List<Foreclosure>();
        public static List<Foreclosure> remove_foreclosure_by_loss_mitigation_list = new List<Foreclosure>();


        private static string base_query = "" +
"select base.loan_id, " +
"(select top 1 misc.misc_dt_value from ms_loan_misc_fields misc where misc_field_id = 10020 and misc.loan_id = base.loan_id order by misc.misc_dt_value ) as demand_letter_due_date, " +
"(select top 1 misc.misc_field_value from ms_loan_misc_fields misc where misc_field_id = 10023 and misc.loan_id = base.loan_id order by misc.misc_field_value ) as foreclosure_officers, " +
"base.fcl_type  as fcl_type, " +
"(select top 1 hist.paid_dt from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'REG' order by paid_dt desc) as last_reg_paid_dt, " +
"(select sum(hist.pmt_amt) from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd in ('REG', 'REGR') and hist.paid_dt >=  (select top 1 hist.paid_dt from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'REG' order by paid_dt desc) ) as last_payments_balance, " +
"(select top 1 ms_loan_info.loan_type from ms_loan_information ms_loan_info where ms_loan_info.loan_id = base.loan_id order by ms_loan_info.loan_type ) as loan_type, " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1000 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as papers_to_attorney_date, " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1200 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as posting_date, " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1600 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as sale_date, " +
"(select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1600 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_recvd_expd ) as sale_date_completed, " +
"(select top 1 ms_credit_information.chapter_filed from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.chapter_filed ) as chapter_filed, " +
"(select top 1 ms_credit_information.bankruptcy_filed_date from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.bankruptcy_filed_date ) as bankruptcy_filed_date, " +
"(select top 1 ms_bankruptcy_info.discharge_date from ms_bankruptcy_info where ms_bankruptcy_info.loan_id = base.loan_id order by ms_bankruptcy_info.discharge_date ) as bankruptcy_discharge_date, " +
"(select top 1 ms_bankruptcy_info.case_dismissed_date from ms_bankruptcy_info where ms_bankruptcy_info.loan_id = base.loan_id order by ms_bankruptcy_info.case_dismissed_date ) as bankruptcy_case_dismissed_date, " +
"(select top 1 ms_bankruptcy_info.closed_date from ms_bankruptcy_info where ms_bankruptcy_info.loan_id = base.loan_id order by ms_bankruptcy_info.closed_date ) as bankruptcy_closed_date, " +
"(select top 1 ms_fcl_info.fcl_deed_in_lieu from ms_fcl_info where ms_fcl_info.loan_id = base.loan_id order by ms_fcl_info.fcl_deed_in_lieu ) as deed_in_lieu, " +
"(select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 10600 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_recvd_expd ) as loss_write_off_recvd_date, " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id = 139 order by memo_id desc) as fc_foreclosure_referral_date, " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id = 148 order by memo_id desc) as fc_payoff_date, " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1100 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date )  as appraisal_ordered_date, " +
"(select top 1 ms_credit_information.default_reason_code from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.default_reason_code ) as main_default_reason_code, " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_categories where memo.loan_id = base.loan_id and memo.memo_category_id = ms_memo_categories.memo_category_id and ms_memo_categories.memo_category_desc like '0%'  order by memo_create_dt desc ) as last_default_reason_code_change_date, " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'BC%' order by memo_id desc ) as last_bc_memo_date, " +
"(select top 1 ms_occupancy_types.occ_description from ms_loan_occupancy, ms_occupancy_types where ms_occupancy_types.fha_occ_code = ms_loan_occupancy.fha_occ_code and ms_loan_occupancy.loan_id = base.loan_id order by ms_loan_occupancy.fcl_hud_notice_of_occ_sent desc, ms_loan_occupancy.prop_insp_date desc  ) as occupancy_code, " +
"(select top 1 ms_loan_occupancy.va_occ_cd from ms_loan_occupancy where ms_loan_occupancy.loan_id = base.loan_id order by ms_loan_occupancy.prop_insp_date desc, ms_loan_occupancy.fcl_hud_notice_of_occ_sent desc) as va_occupancy_code, " +
"(select top 1 ms_loan_occupancy.fcl_hud_notice_of_occ_sent from ms_loan_occupancy where ms_loan_occupancy.loan_id = base.loan_id order by ms_loan_occupancy.fcl_hud_notice_of_occ_sent desc) as occupancy_fcl_hud_notice_of_occ_sent_date, " +
"(select top 1 ms_loan_occupancy.prop_insp_date from ms_loan_occupancy where ms_loan_occupancy.loan_id = base.loan_id order by ms_loan_occupancy.prop_insp_date desc) as occupancy_prop_insp_date, " +
"(select top 1 memo.memo_notify_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_type_id = ms_memo_types.memo_type_id and (ms_memo_types.memo_type_desc like 'PI-%') order by memo_notify_dt desc ) as last_pi_memo_date, " +
"(select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1200 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_recvd_expd ) as foreclosure_date, " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'PI-%' order by memo_id desc ) as last_inspection_memo_date, " +
"(select top 1 ms_memo_types.memo_type_desc from ms_loan_memo memo, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'PI-%' order by memo_id desc ) as last_inspection_memo_description, " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and lower(memo.memo_subject) like '%property inspection report%' order by memo_id desc ) as memo_with_inspection_report_delinquent_date, " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'BC%' order by memo_id desc ) as last_bc_call_memo_date, " +
"(select top 1 memo.memo_notify_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc = 'BC-Promised to Pay' order by memo_notify_dt desc ) as last_bc_promised_to_pay_date, " +
"(select top 1 ms_loan_balances.ytd_prin_paid_amt from ms_loan_balances where ms_loan_balances.loan_id = base.loan_id order by ms_loan_balances.ytd_prin_paid_amt) as ytd_prin_paid_amt, " +
"(select top 1 abs(ms_loan_history.prin_amt) from ms_loan_history where ms_loan_history.loan_id = base.loan_id and ms_loan_history.trans_type_cd = 'PIF'  order by ms_loan_history.paid_dt desc) as principal_amt_from_history, " +
MsLoan.columns("base.loan_id") +
"from  ms_fcl_info as base " +
"where base.fcl_type = 'A' ";

        public static List<Foreclosure> recent_foreclosure_activated()
        {
            if (foreclosure_activated_list.Count == 0 && remove_foreclosure_by_loss_mitigation_list.Count == 0)
                assign_foreclosure_and_remove_from_lm();
            return foreclosure_activated_list;
        }

        public static string recent_foreclosure_activated_conditions()
        {
            string query = " and ( last_reg_paid_dt is null or demand_letter_due_date is null or last_reg_paid_dt < dateadd(dd, -30, demand_letter_due_date) or last_payments_balance <= 0   ) " +
                            " and ( bankruptcy_filed_date is null or bankruptcy_discharge_date is not null or bankruptcy_case_dismissed_date is not null or bankruptcy_closed_date is not null ) " +
                            " and ( (sale_date is null) or (demand_letter_due_date > sale_date) ) " +
                            " and ( (sale_date_completed is null) or (demand_letter_due_date > sale_date_completed) ) " +
                            " and ( deed_in_lieu = 'N' or deed_in_lieu is NULL ) " +
                            " and ( fcl_type = 'A' ) ";
            return query;
        }

        public static List<Foreclosure> remove_foreclosure_by_loss_mitigation()
        {
            if (foreclosure_activated_list.Count == 0 && remove_foreclosure_by_loss_mitigation_list.Count == 0)
                assign_foreclosure_and_remove_from_lm();
            return remove_foreclosure_by_loss_mitigation_list;
        }

        public static string inspections_expired_conditions()
        {
            string query = " and ( last_reg_paid_dt is null or demand_letter_due_date is null or last_reg_paid_dt < dateadd(dd, -30, demand_letter_due_date) or last_payments_balance <= 0   ) " +
                            " and ( bankruptcy_filed_date is null or bankruptcy_discharge_date is not null or bankruptcy_case_dismissed_date is not null or bankruptcy_closed_date is not null ) " +
                            " and ( (sale_date is null) or (demand_letter_due_date > sale_date) ) " +
                            " and ( (sale_date_completed is null) or (demand_letter_due_date > sale_date_completed) ) " +
                            " and ( deed_in_lieu = 'N' or deed_in_lieu is NULL ) " +
                            " and ( fcl_type = 'A' ) " +
                           " and ( memo_with_inspection_report_delinquent_date is null or memo_with_inspection_report_delinquent_date < last_inspection_memo_date ) " +
                           " and (  ( last_inspection_memo_description in ('PI-Initial Vacant Inspection', 'PI-Ongoing Int./Ext. Vacant Inspection', 'PI-No Inspection Ordered')  and  last_inspection_memo_date < dateadd(dd, -25, getdate()) ) " +
                           "     or ( last_inspection_memo_date is null ) " +
                           "     or ( last_inspection_memo_description not in ('PI-Initial Vacant Inspection', 'PI-Ongoing Int./Ext. Vacant Inspection', 'PI-No Inspection Ordered')  and  last_inspection_memo_date < dateadd(dd, -20, getdate()) ) ) ";
            return query;
        }

        public static List<Foreclosure> inspections_expired_mode()
        {
            string query = base_query + inspections_expired_conditions();
            query = query + " order by loan_type, foreclosure_date asc";
            return remove_lm(get_list(query));
        }

        public static string fannie_mae_notification_conditions()
        {
            string query = " and ( last_reg_paid_dt is null or demand_letter_due_date is null or last_reg_paid_dt < dateadd(dd, -30, demand_letter_due_date) or last_payments_balance <= 0   ) " +
                            " and ( bankruptcy_filed_date is null or bankruptcy_discharge_date is not null or bankruptcy_case_dismissed_date is not null or bankruptcy_closed_date is not null ) " +
                            " and ( (sale_date is null) or (demand_letter_due_date > sale_date) ) " +
                            " and ( (sale_date_completed is null) or (demand_letter_due_date > sale_date_completed) ) " +
                            " and ( deed_in_lieu = 'N' or deed_in_lieu is NULL ) " +
                            " and ( fcl_type = 'A' ) " +
                           " and ( loan_type = 'CONV' or loan_type = 'CNV') " +
                           " and ( getdate() > last_bc_promised_to_pay_date  or last_bc_promised_to_pay_date is null ) ";
            return query;
        }

        public static List<Foreclosure> fannie_mae_notification_mode()
        {
            string query = base_query + fannie_mae_notification_conditions();
            query = query + " order by last_bc_call_memo_date asc";
            return remove_lm(get_list(query));
        }

        public static List<Foreclosure> remove_lm(List<Foreclosure> list)
        {
            List<Foreclosure> result_list = new List<Foreclosure>();
            List<LMReception> demand_letters_lm_receptions = LMReception.get_foreclosure_lm_receptions(list);
            List<decimal> list_lm_reception_loan_ids = new List<decimal>();
            decimal[] array_lm_reception_loan_ids;
            foreach (LMReception lm_reception in demand_letters_lm_receptions)
                list_lm_reception_loan_ids.Add(lm_reception.loan_id);
            array_lm_reception_loan_ids = list_lm_reception_loan_ids.ToArray();
            foreach (Foreclosure foreclosure in list)
                if (Array.IndexOf(array_lm_reception_loan_ids, foreclosure.loan_id) < 0)
                    result_list.Add(foreclosure);

            return result_list;
        }

        public static void assign_foreclosure_and_remove_from_lm()
        {
            string query = base_query + recent_foreclosure_activated_conditions();
            query = query + " order by loan_type, demand_letter_due_date asc";
            List<Foreclosure> tmp_foreclosure_mode = get_list(query);

            List<LMReception> foreclosure_lm_receptions = LMReception.get_foreclosure_lm_receptions(tmp_foreclosure_mode);
            List<decimal> list_lm_reception_loan_ids = new List<decimal>();
            decimal[] array_lm_reception_loan_ids;
            foreach (LMReception lm_reception in foreclosure_lm_receptions)
                list_lm_reception_loan_ids.Add(lm_reception.loan_id);
            array_lm_reception_loan_ids = list_lm_reception_loan_ids.ToArray();
            foreach (Foreclosure foreclosure in tmp_foreclosure_mode)
                if (Array.IndexOf(array_lm_reception_loan_ids, foreclosure.loan_id) < 0)
                    foreclosure_activated_list.Add(foreclosure);
                else
                    remove_foreclosure_by_loss_mitigation_list.Add(foreclosure);
        }

        public static string remove_from_recent_foreclosure_activated_by_payment_dt_conditions()
        {
            string query = " and ( last_reg_paid_dt >= dateadd(dd, -30, demand_letter_due_date) ) " +
                            " and ( last_payments_balance > 0 ) " +
                            " and ( (sale_date_completed is null) or (demand_letter_due_date > sale_date_completed) ) " +
                            " and ( bankruptcy_filed_date is null or bankruptcy_discharge_date is not null or bankruptcy_case_dismissed_date is not null or bankruptcy_closed_date is not null ) " +
                            " and ( deed_in_lieu = 'N' or deed_in_lieu is NULL ) " +
                            " and ( fcl_type = 'A' ) ";
            return query;
        }

        public static List<Foreclosure> remove_from_recent_foreclosure_activated_by_payment_dt()
        {
            string query = base_query + remove_from_recent_foreclosure_activated_by_payment_dt_conditions();
            query = query + " order by loan_type, foreclosure_date asc";
            return get_list(query);
        }

        public static string remove_from_foreclosure_by_bankruptcy_conditions()
        {
            string query = " and ( bankruptcy_filed_date is not null and bankruptcy_discharge_date is null and bankruptcy_case_dismissed_date is null and bankruptcy_closed_date is null ) " +
                            " and ( (sale_date_completed is null) or (demand_letter_due_date > sale_date_completed) ) " +
                            " and ( fcl_type = 'A' ) ";
            return query;
        }

        public static List<Foreclosure> remove_from_foreclosure_by_bankruptcy()
        {
            string query = base_query + remove_from_foreclosure_by_bankruptcy_conditions();
            query = query + " order by loan_type, demand_letter_due_date asc";
            return get_list(query);
        }

        public static string remove_from_foreclosure_by_sale_date_conditions()
        {
            string query = " and ( sale_date > demand_letter_due_date )  " +
                            " and ( (sale_date_completed is null) or (demand_letter_due_date > sale_date_completed) ) " +
                            " and ( (loss_write_off_recvd_date is null) or (demand_letter_due_date > loss_write_off_recvd_date) ) " +
                            " and ( fcl_type = 'A' ) ";
            return query;
        }

        public static List<Foreclosure> remove_from_foreclosure_by_sale_date()
        {
            string query = base_query + remove_from_foreclosure_by_sale_date_conditions();
            query = query + " order by loan_type, demand_letter_due_date asc";
            return get_list(query);
        }

        public static string foreclosure_completed_by_sale_date_completed_conditions()
        {
            string query = " and ( sale_date_completed > demand_letter_due_date )  " +
                           " and ( fcl_type = 'A' ) ";
            return query;
        }

        public static List<Foreclosure> foreclosure_completed_by_sale_date_completed()
        {
            string query = base_query + foreclosure_completed_by_sale_date_completed_conditions();
            query = query + " order by sale_date_completed asc, loan_type";
            return get_list(query);
        }

        public static string foreclosure_completed_by_sale_date_completed_in_last_2_years_conditions()
        {
            string query = " and ( sale_date_completed > demand_letter_due_date )  " +
                           " and ( sale_date_completed > dateadd(yy, -2, getdate()) )  " +
                           " and ( fcl_type = 'A' ) ";
            return query;
        }

        public static List<Foreclosure> foreclosure_completed_by_sale_date_completed_in_last_2_years()
        {
            string query = base_query + foreclosure_completed_by_sale_date_completed_conditions();
            query = query + " order by sale_date_completed desc, loan_type";
            return get_list(query);
        }

        public static List<Foreclosure> foreclosure_completed_by_sale_date_completed_in_month_and_year(int month, int year)
        {
            string query = base_query + foreclosure_completed_by_sale_date_completed_conditions() + 
                          " and MONTH(sale_date_completed) = " + month + " and YEAR(sale_date_completed) = " + year;
            query = query + " order by sale_date_completed asc, loan_type";
            return get_list(query);
        }

        public static string remove_from_foreclosure_by_deed_in_lieu_conditions()
        {
            string query = " and (  deed_in_lieu is not null and deed_in_lieu != 'N' )  " +
                            " and ( (sale_date_completed is null) or (demand_letter_due_date > sale_date_completed) ) " +
                            " and ( bankruptcy_filed_date is null or bankruptcy_discharge_date is not null or bankruptcy_case_dismissed_date is not null or bankruptcy_closed_date is not null ) " +
                            " and ( fcl_type = 'A' ) ";
            return query;
        }

        public static List<Foreclosure> remove_from_foreclosure_by_deed_in_lieu()
        {
            string query = base_query + remove_from_foreclosure_by_deed_in_lieu_conditions();
            query = query + " order by loan_type, demand_letter_due_date asc";
            return get_list(query);
        }

        private static List<Foreclosure> get_list(String query)
        {
            List<Foreclosure> list = new List<Foreclosure>();
            DbDataReader reader = getReader(query);
            while (reader.Read())
            {
                Foreclosure foreclosure = new Foreclosure();
                foreclosure = read(reader);
                list.Add(foreclosure);
            }
            reader.Close();
            close_connection();
            return list;
        }

        public static Foreclosure get(decimal loan_id)
        {
            string query = base_query + " and loan_id = " + loan_id.ToString();
            Foreclosure foreclosure = new Foreclosure();
            DbDataReader reader = getReader(query);
            if (reader.Read())
                foreclosure = read(reader);
            reader.Close();
            return foreclosure;
        }

        private static Foreclosure read(DbDataReader reader)
        {
            Foreclosure foreclosure = new Foreclosure();
            int pos = 0;
            foreclosure.loan_id = readDBDecimal(reader, pos++);
            foreclosure.demand_letter_due_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.foreclosure_officers = readDBString(reader, pos++);
            foreclosure.fcl_type = readDBString(reader, pos++);
            foreclosure.last_reg_paid_dt = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.last_payments_balance = readDBDecimal(reader, pos++);
            foreclosure.loan_type = readDBString(reader, pos++);
            foreclosure.papers_to_attorney_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.posting_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.sale_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.sale_date_completed = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.chapter_filed = readDBString(reader, pos++);
            foreclosure.bankruptcy_filed_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.bankruptcy_discharge_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.bankruptcy_case_dismissed_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.bankruptcy_closed_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.deed_in_lieu = readDBString(reader, pos++);
            foreclosure.loss_write_off_recvd_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.fc_foreclosure_referral_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.fc_payoff_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.appraisal_ordered_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.main_default_reason_code = readDBString(reader, pos++);
            foreclosure.last_default_reason_code_change_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.last_bc_memo_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.occupancy_code = readDBString(reader, pos++);
            foreclosure.va_occupancy_code = readDBString(reader, pos++);
            foreclosure.occupancy_fcl_hud_notice_of_occ_sent_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.occupancy_prop_insp_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.last_pi_memo_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.foreclosure_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.last_inspection_memo_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.last_inspection_memo_description = readDBString(reader, pos++);
            foreclosure.memo_with_inspection_report_delinquent_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.last_bc_call_memo_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.last_bc_promised_to_pay_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure.ytd_prin_paid_amt = readDBDecimal(reader, pos++);
            foreclosure.principal_amt_from_history = readDBDecimal(reader, pos++);

            if (foreclosure.loan_id > 0)
            {
                foreclosure.loan = MsLoan.read(reader, pos);
                foreclosure.preforeclosure_events = PreforeclosureEvent.get(foreclosure);
            }
            return foreclosure;
        }

        public bool is_preforeclosure_complete()
        {
            if (this.fc_foreclosure_referral_date.isNull ||
                this.papers_to_attorney_date.isNull ||
                this.fc_payoff_date.isNull ||
                this.preforeclosure_events.is_copy_demand_letters == false ||
                this.preforeclosure_events.is_copy_notes == false ||
                this.preforeclosure_events.is_copy_deed_of_trust_notes == false ||
                this.preforeclosure_events.is_copy_title_policy == false)
            {
                return false;
            }

            if (loan.loan_type == "VA" )
            {
                if (this.appraisal_ordered_date.isNull || this.preforeclosure_events.is_notify_appraisal == false)
                    return false;
            }
            else if (loan.loan_type == "CONV")
            {
                if (this.appraisal_ordered_date.isNull)
                    return false;
            }
            return true;
        }

        public string default_reason_code_status_color()
        {
            TimeSpan ts = this.last_default_reason_code_change_date.date - this.last_bc_memo_date.date;
            if (Math.Abs(ts.Days) > 1 || this.last_default_reason_code_change_date.isNull || this.last_bc_memo_date.isNull) return "text-error";
            return "text-success";
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

        public string last_inspection_since()
        {
            if (this.last_inspection_memo_date.isNull)
                return "NA";
            else
            {
                TimeSpan timespan = (DateTime.Now - this.last_inspection_memo_date.date);
                return timespan.Days.ToString();
            }
        }

        public bool does_require_fannie_mae_call(){
            TimeSpan ts = DateTime.Today.AddDays(1) - this.last_bc_call_memo_date.date;
            if( ts.Days >= 3) return true;
            if (ts.Days == 0) return false;
            for( int i = 3 - ts.Days ; i >= 1; i--){
                DateTime date_to_call = DateTime.Today.AddDays(i);
                if (date_to_call.DayOfWeek != DayOfWeek.Saturday && date_to_call.DayOfWeek != DayOfWeek.Sunday) return false;
            }
            return true;
        }

        public decimal non_zero_balance(){
            if (this.principal_amt_from_history > 0M) return this.principal_amt_from_history;
            if( this.ytd_prin_paid_amt > 0M ) return this.ytd_prin_paid_amt;
            if (this.loan.prin_bal > 0M) return this.loan.prin_bal;
            else return this.principal_amt_from_history;
        }

        public static decimal list_total(List<Foreclosure> list){
            decimal total = 0M;
            foreach (Foreclosure f in list) total += f.non_zero_balance();
            return total;
        }
    }
}