using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Common;

using com.sp.rmmc.common.models;
using com.sp.rmmc.common.models.events;

namespace com.sp.rmmc.foreclosures.models
{
    public class BaseForeclosure : SybaseModel
    {
        public decimal loan_id = 0M;
        public DateTimeObject demand_letter_due_date = new DateTimeObject();
        public String foreclosure_officers = "";
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
        public DateTimeObject fc_foreclosure_referral_date = new DateTimeObject();
        public DateTimeObject fc_payoff_date = new DateTimeObject();
        public DateTimeObject appraisal_ordered_date = new DateTimeObject();
        public DateTimeObject foreclosure_date = new DateTimeObject();
        public int lm_stop_code_id = 0;
        public DateTimeObject memo_fc_third_party_sale_date = new DateTimeObject();
        public DateTimeObject extension_ordered_date = new DateTimeObject();

        public DateTimeObject sale_date = new DateTimeObject();
        public decimal ytd_prin_paid_amt = 0M;
        public decimal principal_amt_from_history = 0M;

        public com.sp.rmmc.common.models.MsLoan loan = new com.sp.rmmc.common.models.MsLoan();
        public PreforeclosureEvent preforeclosure_events = new PreforeclosureEvent();

        public InspectionEvents inspection = new InspectionEvents();
        public ReasonCodeEvents reason_code = new ReasonCodeEvents();
        public SaleInspectionEvents sale_inspection = new SaleInspectionEvents();
        public SaleConveyanceEvents sale_conveyance = new SaleConveyanceEvents();
        public SaleHudAdviceEvents sale_hud_advice = new SaleHudAdviceEvents();
        public SaleTitlePackageEvents sale_title_package = new SaleTitlePackageEvents();
        public SaleTitleApprovalEvents sale_title_approval = new SaleTitleApprovalEvents();
        public SaleClaimPartBEvents sale_claim_part_b = new SaleClaimPartBEvents();
        public SaleResultSaleEvents sale_result_sale = new SaleResultSaleEvents();
        public SaleTransferCustodyEvents sale_transfer_of_custody = new SaleTransferCustodyEvents();
        public SaleAcquisitionPaymentEvents sale_acquisition_payment = new SaleAcquisitionPaymentEvents();
        public SaleBasicClaimEvents sale_basic_claim_events = new SaleBasicClaimEvents();
        public SaleREONoticeEvents sale_reo_notice_events = new SaleREONoticeEvents();
        public SaleFannieMaeClaim571Events sale_fannie_mae_claim_571_events = new SaleFannieMaeClaim571Events();
        public ForeclosureMortgageStatusEvents foreclosure_mortgage_status_events = new ForeclosureMortgageStatusEvents();
        public ForeclosureAppraisalBPOEvents foreclosure_appraisal_bpo_events = new ForeclosureAppraisalBPOEvents();
        public ForeclosureRfsGnmaForeclosureStatusEvents foreclosure_rfs_gnma_foreclosure_status_events = new ForeclosureRfsGnmaForeclosureStatusEvents();
        public SaleMortgageStatusEvents sale_mortgage_status_events = new SaleMortgageStatusEvents();
        public SaleOriginalDocumentsOrderedEvents sale_original_documents_ordered_events = new SaleOriginalDocumentsOrderedEvents();
        public DemandFileExpireEvents demand_expire_events = new DemandFileExpireEvents();
        public DemandOccupancyEvents demand_occupancy_events = new DemandOccupancyEvents();
        public XdemandMortgageStatusEvents xdemand_mortgage_status_events = new XdemandMortgageStatusEvents();
        public XdemandOccupancyLetterEvents xdemand_occupancy_letter_events = new XdemandOccupancyLetterEvents();

        public ReviewChecklist review_checklist = new ReviewChecklist();

        public string filter_reason = "";
        public string type = "";
        public const string DEMAND_TYPE = "Demand";
        public const string XDEMAND_TYPE = "Xdemand";
        public const string FORECLOSED_TYPE = "Foreclosed";
        public const string FORECLOSURE_TYPE = "Foreclosure";
        public const string PREFORECLOSURE_TYPE = "PreForeclosure";

        private static string columns_query = "" +
" base.loan_id, " +
"(select top 1 misc.misc_dt_value from ms_loan_misc_fields misc where misc_field_id = 10020 and misc.loan_id = base.loan_id order by misc.misc_dt_value ) as demand_letter_due_date, " +
"(select top 1 misc.misc_field_value from ms_loan_misc_fields misc where misc_field_id = 10023 and misc.loan_id = base.loan_id order by misc.misc_field_value ) as foreclosure_officers, " +
"(select top 1 ms_fcl_info.fcl_type from ms_fcl_info where ms_fcl_info.loan_id = base.loan_id order by ms_fcl_info.fcl_type) as fcl_type, " +
"(select top 1 hist.paid_dt from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'REG' order by paid_dt desc) as last_reg_paid_dt, " +
"(select sum(hist.pmt_amt) from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd in ('REG', 'REGR') and hist.paid_dt >=  (select top 1 hist.paid_dt from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'REG' order by paid_dt desc) ) as last_payments_balance, " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1000 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as papers_to_attorney_date, " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1200 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as posting_date, " +
"(select top 1 ms_credit_information.chapter_filed from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.chapter_filed ) as chapter_filed, " +
"(select top 1 ms_credit_information.bankruptcy_filed_date from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.bankruptcy_filed_date ) as bankruptcy_filed_date, " +
"(select top 1 ms_bankruptcy_info.discharge_date from ms_bankruptcy_info where ms_bankruptcy_info.loan_id = base.loan_id order by ms_bankruptcy_info.discharge_date ) as bankruptcy_discharge_date, " +
"(select top 1 ms_bankruptcy_info.case_dismissed_date from ms_bankruptcy_info where ms_bankruptcy_info.loan_id = base.loan_id order by ms_bankruptcy_info.case_dismissed_date ) as bankruptcy_case_dismissed_date, " +
"(select top 1 ms_bankruptcy_info.closed_date from ms_bankruptcy_info where ms_bankruptcy_info.loan_id = base.loan_id order by ms_bankruptcy_info.closed_date ) as bankruptcy_closed_date, " +
"(select top 1 mfia1.fcl_item_date from ms_fcl_item_amts_1 mfia1 where mfia1.item_id = 1600 and mfia1.loan_id = base.loan_id order by mfia1.fcl_item_date ) as sale_date, " + 
"(select top 1 ms_loan_balances.ytd_prin_paid_amt from ms_loan_balances where ms_loan_balances.loan_id = base.loan_id order by ms_loan_balances.ytd_prin_paid_amt) as ytd_prin_paid_amt, " +
"(select top 1 abs(ms_loan_history.prin_amt) from ms_loan_history where ms_loan_history.loan_id = base.loan_id and ms_loan_history.trans_type_cd = 'PIF'  order by ms_loan_history.paid_dt desc) as principal_amt_from_history, " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id = 139 order by memo_id desc) as fc_foreclosure_referral_date, " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id = 148 order by memo_id desc) as fc_payoff_date, " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1100 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date )  as appraisal_ordered_date, " +
"(select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1200 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_recvd_expd ) as foreclosure_date, " +
"(select top 1 mlsc.stop_code_id from ms_loan_stop_code mlsc where mlsc.stop_code_id = 153 and mlsc.loan_id = base.loan_id order by mlsc.stop_code_id ) as lm_stop_code_id, " +
"(select top 1 memo.memo_create_dt  from ms_loan_memo memo, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'FC-3rd%' order by memo.memo_create_dt desc ) as memo_fc_third_party_sale_date, " +
"(select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 10800 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_ord_req )  as extension_ordered_date, " + 
com.sp.rmmc.common.models.MsLoan.columns("base.loan_id") + ", " +
new InspectionEvents().query("base.loan_id") + ", " +
new ReasonCodeEvents().query("base.loan_id") + ", " +
new SaleInspectionEvents().query("base.loan_id") + ", " +
new SaleConveyanceEvents().query("base.loan_id") + ", " +
new SaleHudAdviceEvents().query("base.loan_id") + ", " +
new SaleTitlePackageEvents().query("base.loan_id") + ", " +
new SaleTitleApprovalEvents().query("base.loan_id") + ", " +
new SaleClaimPartBEvents().query("base.loan_id") + ", " +
new SaleResultSaleEvents().query("base.loan_id") + ", " +
new SaleTransferCustodyEvents().query("base.loan_id") + ", " +
new SaleAcquisitionPaymentEvents().query("base.loan_id") + ", " +
new SaleBasicClaimEvents().query("base.loan_id") + ", " +
new SaleREONoticeEvents().query("base.loan_id") + ", " +
new SaleFannieMaeClaim571Events().query("base.loan_id") + ", " +
new ForeclosureMortgageStatusEvents().query("base.loan_id") + ", " +
new ForeclosureAppraisalBPOEvents().query("base.loan_id") + ", " +
new ForeclosureRfsGnmaForeclosureStatusEvents().query("base.loan_id") + ", " +
new SaleMortgageStatusEvents().query("base.loan_id") + ", " +
new SaleOriginalDocumentsOrderedEvents().query("base.loan_id") + ", " +
new DemandFileExpireEvents().query("base.loan_id") + ", " +
new DemandOccupancyEvents().query("base.loan_id") + ", " +
new XdemandMortgageStatusEvents().query("base.loan_id") + ", " +
new XdemandOccupancyLetterEvents().query("base.loan_id") + ", " +
ReviewChecklist.columns("base.loan_id") + " ";

        private static string demands_base_query = "" +
"select * from " +
"( " +
columns_query + " " +
"from ms_loan_misc_fields as base " +
"where base.misc_field_id = 10020 " +
" ) base where 1=1 ";

        private static string foreclosed_base_query = "" +
"select * from " +
"( " +
columns_query + " " +
"from ms_fcl_item_amts_1 as base " +
"where base.item_id = 1600 " +
"and base.fcl_item_date > '2014-01-01' " +
" ) base where 1=1 ";

        private static string base_query = "" + 
"select " +
columns_query + " from " +
"( " +
    "( " +
    " select base.loan_id " +
    " from ms_loan_misc_fields as base " +
    " where base.misc_field_id = 10020 " +
    ") " +
    "UNION " +
    "( " +
    " select base.loan_id " +
    " from ms_fcl_item_amts_1 as base " +
    " where base.item_id = 1600 " +
    " and base.fcl_item_date > '2014-01-01' " +
    ") " +
    "UNION " +
    "( " +
    " select base.loan_id " +
    " from  ms_fcl_info as base " +
    " where base.fcl_type = 'A' " +
    ") " +
    " " +
")base where 1=1 ";


        public static void getDemands(List<BaseForeclosure> demands, List<BaseForeclosure> removed_demands)
        {
            string query = base_query + demand_conditions();
            query = query + " order by ms_loan_type, demand_letter_due_date asc";
            List<BaseForeclosure> all = get_list(query);
            foreach (BaseForeclosure f in all) { f.type = DEMAND_TYPE; demands.Add(f); }
            commonFilters(demands, removed_demands);
        }

        public static void getXDemands(List<BaseForeclosure> xdemands, List<BaseForeclosure> demands)
        {
            string query = base_query + demand_conditions();
            query = query + " order by demand_letter_due_date desc";
            List<BaseForeclosure> all = get_list(query);
            foreach (BaseForeclosure f in all) { f.type = XDEMAND_TYPE; demands.Add(f); }
            commonFilters(demands, xdemands);
        }

        public static void getForeclosed(List<BaseForeclosure> foreclosed, List<BaseForeclosure> removed_foreclosed)
        {
            string query = base_query + foreclosed_conditions();
            query = query + " order by sale_date desc, ms_loan_type, demand_letter_due_date asc";
            List<BaseForeclosure> all = get_list(query);
            foreach (BaseForeclosure f in all) { f.type = FORECLOSED_TYPE; foreclosed.Add(f); }
            commonFilters(foreclosed, removed_foreclosed);
        }

        private static void commonPreAndForeclosures(List<BaseForeclosure> foreclosures, List<BaseForeclosure> removed_foreclosures)
        {
            string query = base_query + foreclosures_conditions();
            query = query + " order by foreclosure_date asc, ms_loan_type, demand_letter_due_date asc";
            List<BaseForeclosure> all = get_list(query);
            foreach (BaseForeclosure f in all) { f.type = FORECLOSURE_TYPE; foreclosures.Add(f); }
            commonFilters(foreclosures, removed_foreclosures);
        }

        public static void getForeclosures(List<BaseForeclosure> foreclosures, List<BaseForeclosure> removed_foreclosures)
        {
            List<BaseForeclosure> both = new List<BaseForeclosure>();
            commonPreAndForeclosures(both, removed_foreclosures);
            foreach (BaseForeclosure bf in both)
            {
                bf.preforeclosure_events = PreforeclosureEvent.getFromBaseForeclosure(bf);
                if (bf.is_preforeclosure_complete() == true)
                {
                    bf.type = FORECLOSURE_TYPE;
                    foreclosures.Add(bf);
                }
            }
        }

        public static void getPreForeclosures(List<BaseForeclosure> PreForeclosures, List<BaseForeclosure> removed_foreclosures)
        {
            List<BaseForeclosure> both = new List<BaseForeclosure>();
            commonPreAndForeclosures(both, removed_foreclosures);
            foreach (BaseForeclosure bf in both)
            {
                bf.preforeclosure_events = PreforeclosureEvent.getFromBaseForeclosure(bf);
                if (bf.is_preforeclosure_complete() == false)
                {
                    bf.type = PREFORECLOSURE_TYPE;
                    PreForeclosures.Add(bf);
                }
            }
        }

        public static void getForeclosureLoan(decimal loan_id, List<BaseForeclosure> accepted, List<BaseForeclosure> removed, String type)
        {
            string query = base_query + single_loan_conditions(loan_id);
            query = query + " order by ms_loan_type, demand_letter_due_date asc";
            List<BaseForeclosure> all = get_list(query);
            foreach (BaseForeclosure f in all) { f.type = type; accepted.Add(f); f.preforeclosure_events = PreforeclosureEvent.getFromBaseForeclosure(f); }
            commonFilters(accepted, removed);
        }

        private static void commonFilters(List<BaseForeclosure> accepted, List<BaseForeclosure> removed){
            List<BaseForeclosure> all_accepted = new List<BaseForeclosure>();
            foreach (BaseForeclosure f in accepted) all_accepted.Add(f);
            remove_received_payment(all_accepted, removed);
            remove_bankruptcy(all_accepted, removed);
            remove_third_party_sale(all_accepted, removed);
            remove_lm(all_accepted, removed);
            foreach(BaseForeclosure f in all_accepted)
                if (f.filter_reason != "") accepted.Remove(f);
        }

        private static void remove_third_party_sale(List<BaseForeclosure> accepted, List<BaseForeclosure> removed)
        {
            foreach (BaseForeclosure f in accepted)
            {
                if (removed.Contains(f)) continue;
                if (f.memo_fc_third_party_sale_date.isNull == false)
                {
                    f.filter_reason = "Third Party Sale";
                    removed.Add(f);
                }
            }
        }

        private static void remove_bankruptcy(List<BaseForeclosure> accepted, List<BaseForeclosure> removed)
        {
            foreach (BaseForeclosure f in accepted)
            {
                if (removed.Contains(f)) continue;
                if (f.bankruptcy_filed_date.isNull == false &&
                    (   f.bankruptcy_discharge_date.isNull == true &&
                        f.bankruptcy_case_dismissed_date.isNull == true &&
                        f.bankruptcy_closed_date.isNull == true
                    )
                   )
                {   
                    f.filter_reason = "Bankruptcy";
                    removed.Add(f);
                }
            }
        }

        private static void remove_received_payment(List<BaseForeclosure> accepted, List<BaseForeclosure> removed)
        {   
            foreach (BaseForeclosure f in accepted)
            {
                //if (f.loan_id == 152526M)
                //    f.filter_reason = "";
                if (removed.Contains(f)) continue;
                if ((f.last_reg_paid_dt.isNull == false &&
                    f.last_reg_paid_dt.date >= f.demand_letter_due_date.date.AddDays(-33) &&
                    f.last_payments_balance >= 0)  ||
                    f.loan.due_date_next_payment.date >= f.demand_letter_due_date.date )
                {   
                    f.filter_reason = "Received Payment";
                    removed.Add(f);
                }
            }
        }

        public static string demand_conditions()
        {
            string query = " and ( demand_letter_due_date is not null ) \n" +
                           " and ( demand_letter_due_date >= dateadd(dd, -366, getdate()) ) \n" +
                           " and ( fcl_type <> 'A' or fcl_type is null ) \n" +
                           " and ( sale_date is null ) \n" +
                           " and ( ms_loan_prin_bal > 0 )";
            return query;
        }

        public static string foreclosed_conditions()
        {
            string query = " and ( sale_date is not null ) \n" +
                           "";
            return query;
        }

        public static string foreclosures_conditions()
        {
            string query = " and ( fcl_type = 'A' ) \n" +
                           " and ( sale_date is null ) \n" +
                           " and ( ms_loan_prin_bal > 0 )";
            return query;
        }

        public static string single_loan_conditions(decimal loan_id)
        {
            string query = " and ( ms_loan_id = " + loan_id + " )";
            return query;
        }

        public static void remove_lm(List<BaseForeclosure> accepted, List<BaseForeclosure> removed)
        {
            foreach (BaseForeclosure f in accepted)
            {
                if (removed.Contains(f)) continue;
                if (f.lm_stop_code_id > 0)
                {
                    f.filter_reason = "Loss Mitigation";
                    removed.Add(f);
                }
            }
            List<LMReception> demand_letters_lm_receptions = LMReception.getLMReceptions(accepted);
            List<decimal> list_lm_reception_loan_ids = new List<decimal>();
            decimal[] array_lm_reception_loan_ids;
            foreach (LMReception lm_reception in demand_letters_lm_receptions)
                list_lm_reception_loan_ids.Add(lm_reception.loan_id);
            array_lm_reception_loan_ids = list_lm_reception_loan_ids.ToArray();
            foreach (BaseForeclosure f in accepted)
            {
                if (removed.Contains(f)) continue;
                if (Array.IndexOf(array_lm_reception_loan_ids, f.loan_id) >= 0)
                {
                    f.filter_reason = "Loss Mitigation";
                    removed.Add(f);
                }
            }
        }

        private static List<BaseForeclosure> get_list(String query)
        {
            List<BaseForeclosure> list = new List<BaseForeclosure>();
            DbDataReader reader = getReader(query);
            while (reader.Read())
            {
                BaseForeclosure f = new BaseForeclosure();
                f = read(reader);
                list.Add(f);
            }
            reader.Close();
            close_connection();
            return list;
        }

        private static BaseForeclosure read(DbDataReader reader)
        {
            BaseForeclosure f = new BaseForeclosure();
            int pos = 0;
            f.loan_id = readDBDecimal(reader, pos++);
            f.demand_letter_due_date = readDBDateObject(reader, pos++);
            f.foreclosure_officers = readDBString(reader, pos++);
            f.fcl_type = readDBString(reader, pos++);
            f.last_reg_paid_dt = readDBDateObject(reader, pos++);
            f.last_payments_balance = readDBDecimal(reader, pos++);
            f.papers_to_attorney_date = readDBDateObject(reader, pos++);
            f.posting_date = readDBDateObject(reader, pos++);
            f.chapter_filed = readDBString(reader, pos++);
            f.bankruptcy_filed_date = readDBDateObject(reader, pos++);
            f.bankruptcy_discharge_date = readDBDateObject(reader, pos++);
            f.bankruptcy_case_dismissed_date = readDBDateObject(reader, pos++);
            f.bankruptcy_closed_date = readDBDateObject(reader, pos++);
            f.sale_date = readDBDateObject(reader, pos++);
            f.ytd_prin_paid_amt = readDBDecimal(reader, pos++);
            f.principal_amt_from_history = readDBDecimal(reader, pos++);
            f.fc_foreclosure_referral_date = readDBDateObject(reader, pos++);
            f.fc_payoff_date = readDBDateObject(reader, pos++);
            f.appraisal_ordered_date = readDBDateObject(reader, pos++);
            f.foreclosure_date = readDBDateObject(reader, pos++);
            f.lm_stop_code_id = readDBInt(reader, pos++);
            f.memo_fc_third_party_sale_date = readDBDateObject(reader, pos++);
            f.extension_ordered_date = readDBDateObject(reader, pos++);
            
            if (f.loan_id > 0)
            {
                f.loan = com.sp.rmmc.common.models.MsLoan.read(reader, pos);
                pos += com.sp.rmmc.common.models.MsLoan.COLUMNS;
                f.inspection.read(reader, pos, f.loan);
                pos += f.inspection.fields_count();
                f.reason_code.read(reader, pos, f.loan);
                pos += f.reason_code.fields_count();
                f.sale_inspection.read(reader, pos, f.loan);
                pos += f.sale_inspection.fields_count();
                f.sale_conveyance.read(reader, pos, f.loan);
                pos += f.sale_conveyance.fields_count();
                f.sale_hud_advice.read(reader, pos, f.loan);
                pos += f.sale_hud_advice.fields_count();
                f.sale_title_package.read(reader, pos, f.loan);
                pos += f.sale_title_package.fields_count();
                f.sale_title_approval.read(reader, pos, f.loan);
                pos += f.sale_title_approval.fields_count();   
                f.sale_claim_part_b.read(reader, pos, f.loan);
                pos += f.sale_claim_part_b.fields_count();
                f.sale_result_sale.read(reader, pos, f.loan);
                pos += f.sale_result_sale.fields_count();
                f.sale_transfer_of_custody.read(reader, pos, f.loan);
                pos += f.sale_transfer_of_custody.fields_count();
                f.sale_acquisition_payment.read(reader, pos, f.loan);
                pos += f.sale_acquisition_payment.fields_count();
                f.sale_basic_claim_events.read(reader, pos, f.loan);
                pos += f.sale_basic_claim_events.fields_count();
                f.sale_reo_notice_events.read(reader, pos, f.loan);
                pos += f.sale_reo_notice_events.fields_count();
                f.sale_fannie_mae_claim_571_events.read(reader, pos, f.loan);
                pos += f.sale_fannie_mae_claim_571_events.fields_count();
                f.foreclosure_mortgage_status_events.read(reader, pos, f.loan);
                pos += f.foreclosure_mortgage_status_events.fields_count();
                f.foreclosure_appraisal_bpo_events.read(reader, pos, f.loan);
                pos += f.foreclosure_appraisal_bpo_events.fields_count();
                f.foreclosure_rfs_gnma_foreclosure_status_events.read(reader, pos, f.loan);
                pos += f.foreclosure_rfs_gnma_foreclosure_status_events.fields_count();
                f.sale_mortgage_status_events.read(reader, pos, f.loan);
                pos += f.sale_mortgage_status_events.fields_count();
                f.sale_original_documents_ordered_events.read(reader, pos, f.loan);
                pos += f.sale_original_documents_ordered_events.fields_count();
                f.demand_expire_events.read(reader, pos, f.loan);
                pos += f.demand_expire_events.fields_count();
                f.demand_occupancy_events.read(reader, pos, f.loan);
                pos += f.demand_occupancy_events.fields_count();
                f.xdemand_mortgage_status_events.read(reader, pos, f.loan);
                pos += f.xdemand_mortgage_status_events.fields_count();
                f.xdemand_occupancy_letter_events.read(reader, pos, f.loan);
                pos += f.xdemand_occupancy_letter_events.fields_count();
                
                
                f.review_checklist = ReviewChecklist.read(reader, pos);
            }
            return f;
        }

        public string months_demand_date_after_delinquent()
        {
            if (this.loan.due_date_next_payment.isNull || this.demand_letter_due_date.isNull)
                return "NA";
            else
            {
                TimeSpan timespan = (this.demand_letter_due_date.date - this.loan.due_date_next_payment.date);
                return ((timespan.Days) / 30).ToString();
            }
        }

        public string months_demand_date_after_delinquent_color()
        {
            if (this.loan.due_date_next_payment.isNull || this.demand_letter_due_date.isNull)
                return "text-error";
            else
            {
                TimeSpan timespan = (this.demand_letter_due_date.date - this.loan.due_date_next_payment.date);
                int max_month_wait = this.max_months_waits();
                if (((timespan.Days) / 30) > max_month_wait) return "text-error";
                else return "text-success";
            }
        }

        public int max_months_waits()
        {
            if (this.loan.loan_type == "VA") return 6;
            if (this.loan.loan_type == "CNV") return 5;
            if (this.loan.loan_type == "FHA")
            {
                if (this.extension_ordered_date.isNull) return 3;
                else return 100000000;
            }
            return 3;
        }

        public List<Event> getEvents()
        {
            List<Event> es = new List<Event>();
            es.AddRange(internalEvents());
            if (this.type == DEMAND_TYPE)
            {
                es.AddRange(inspection.events);
                es.AddRange(reason_code.events);
                es.AddRange(foreclosure_mortgage_status_events.events);
                es.AddRange(demand_expire_events.events);
                es.AddRange(demand_occupancy_events.events);
            }
            if (this.type == XDEMAND_TYPE)
            {
                es.AddRange(xdemand_mortgage_status_events.events);
                es.AddRange(xdemand_occupancy_letter_events.events);
            }
            if (this.type == PREFORECLOSURE_TYPE)
            {
                es.AddRange(inspection.events);
                es.AddRange(reason_code.events);
                es.AddRange(foreclosure_appraisal_bpo_events.events);
            }
            if (this.type == FORECLOSURE_TYPE)
            {
                es.AddRange(inspection.events);
                es.AddRange(reason_code.events);
                es.AddRange(foreclosure_mortgage_status_events.events);
                es.AddRange(foreclosure_rfs_gnma_foreclosure_status_events.events);
                es.AddRange(foreclosure_appraisal_bpo_events.events);
            }
            else if (this.type == FORECLOSED_TYPE)
            {
                es.AddRange(sale_inspection.events);
                es.AddRange(sale_conveyance.events);
                es.AddRange(sale_hud_advice.events);
                es.AddRange(sale_title_package.events);
                es.AddRange(sale_title_approval.events);
                es.AddRange(sale_claim_part_b.events);
                es.AddRange(sale_result_sale.events);
                es.AddRange(sale_transfer_of_custody.events);
                es.AddRange(sale_acquisition_payment.events);
                es.AddRange(sale_basic_claim_events.events);
                es.AddRange(sale_reo_notice_events.events);
                es.AddRange(sale_fannie_mae_claim_571_events.events);
                es.AddRange(sale_mortgage_status_events.events);
                es.AddRange(sale_original_documents_ordered_events.events);
            }
            return es;
        }

        private List<Event> internalEvents()
        {
            List<Event> es = new List<Event>();
            if (this.type == DEMAND_TYPE)
            {
                if (months_demand_date_after_delinquent_color() == "text-error")
                {
                    Event e = new Event();
                    e.loan = this.loan;
                    e.type = Event.WARNING;
                    e.name = "Demanded after " + this.max_months_waits().ToString() + "-months of delinquent.";
                    e.description = "Demand due date(" + this.demand_letter_due_date.ToShortDateString() + ") is more than " + 
                        this.max_months_waits().ToString() + 
                        "-months after delinquent date(" + this.loan.due_date_next_payment.ToShortDateString() + ")";
                    e.loan = this.loan;
                    es.Add(e);
                }
            }
            if (this.type == FORECLOSURE_TYPE)
            {
                if (this.papers_to_attorney_date.isNull )
                {
                    Event e = new Event();
                    e.loan = this.loan;
                    e.type = Event.WARNING;
                    e.name = "Papers not sent to Attorneys.";
                    e.description = "Papers not sent to Attorneys";
                    e.loan = this.loan;
                    es.Add(e);
                }
            }
            return es;
        }

        public decimal non_zero_balance()
        {
            if (this.loan.prin_bal > 0M) return this.loan.prin_bal;
            if (this.principal_amt_from_history > 0M) return this.principal_amt_from_history;
            if (this.ytd_prin_paid_amt > 0M) return this.ytd_prin_paid_amt;
            else return this.principal_amt_from_history;
        }

        public string getEventStatusColor()
        {
            string s = "";
            List<Event> events = getEvents();
            foreach(Event e in events){
                if( e.type == Event.WARNING ) s = e.getEventStatusColor();
                if (e.type == Event.ALERT) { s = e.getEventStatusColor(); break; }
            }
            return s;
        }

        public bool is_preforeclosure_complete()
        {
            if (this.fc_foreclosure_referral_date.isNull ||
                this.papers_to_attorney_date.isNull ||
                this.fc_payoff_date.isNull ||
                this.preforeclosure_events.is_copy_demand_letters == false ||
                this.preforeclosure_events.is_copy_notes == false ||
                this.preforeclosure_events.is_copy_deed_of_trust_notes == false)
            {
                return false;
            }

            if (loan.loan_type == "VA")
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

    }
}