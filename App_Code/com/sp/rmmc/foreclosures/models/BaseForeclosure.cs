using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;

using System.Data.Common;

using com.sp.rmmc.common.models;
using com.sp.rmmc.common.models.events;

namespace com.sp.rmmc.foreclosures.models
{
    public class BaseForeclosure : DbModel
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

        public InspectionEvents inspection = null;
        public ReasonCodeEvents reason_code = null;
        public SaleInspectionEvents sale_inspection = null;
        public SaleConveyanceEvents sale_conveyance = null;
        public SaleHudAdviceEvents sale_hud_advice = null;
        public SaleTitlePackageEvents sale_title_package = null;
        public SaleTitleApprovalEvents sale_title_approval = null;
        public SaleClaimPartBEvents sale_claim_part_b = null;
        public SaleResultSaleEvents sale_result_sale = null;
        public SaleTransferCustodyEvents sale_transfer_of_custody = null;
        public SaleAcquisitionPaymentEvents sale_acquisition_payment = null;
        public SaleBasicClaimEvents sale_basic_claim_events = null;
        public SaleREONoticeEvents sale_reo_notice_events = null;
        public SaleFannieMaeClaim571Events sale_fannie_mae_claim_571_events = null;
        public ForeclosureMortgageStatusEvents foreclosure_mortgage_status_events = null;
        public ForeclosureAppraisalBPOEvents foreclosure_appraisal_bpo_events = null;
        public ForeclosureRfsGnmaForeclosureStatusEvents foreclosure_rfs_gnma_foreclosure_status_events = null;
        public SaleMortgageStatusEvents sale_mortgage_status_events = null;
        public SaleOriginalDocumentsOrderedEvents sale_original_documents_ordered_events = null;
        public DemandFileExpireEvents demand_expire_events = null;
        public DemandOccupancyEvents demand_occupancy_events = null;
        public XdemandMortgageStatusEvents xdemand_mortgage_status_events = null;
        public XdemandOccupancyLetterEvents xdemand_occupancy_letter_events = null;
        public FannieMaeCallsEvents fannie_mae_calls_events = null;

        public ReviewChecklist review_checklist = null;

        public string filter_reason = "";
        public string type = "";
        public const string DEMAND_TYPE = "Demand";
        public const string XDEMAND_TYPE = "Xdemand";
        public const string CURRENTXDEMAND_TYPE = "CurrentXdemand";
        public const string FORECLOSED_TYPE = "Foreclosed";
        public const string FORECLOSURE_TYPE = "Foreclosure";
        public const string PREFORECLOSURE_TYPE = "PreForeclosure";
        public const string ALL_TYPE = "All";

        protected virtual string default_columns { get { return ""; } }
        protected virtual string demands_query { get { return ""; } }
        protected virtual string foreclosures_query { get { return ""; } }
        protected virtual string foreclosed_query { get { return ""; } }
        protected virtual string all_types_query { get { return ""; } }

        public void getDemands(List<BaseForeclosure> demands, List<BaseForeclosure> removed_demands)
        {
            this.type = DEMAND_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = "select " + columns + " from " + "( " + demands_query + ")base where 1=1 " + demands_conditions();
            query = query + " order by ms_loan_type, demand_letter_due_date asc";
            List<BaseForeclosure> all = this.get_loan_list(query);
            foreach (BaseForeclosure f in all) { f.type = DEMAND_TYPE; demands.Add(f); }
            commonFilters(demands, removed_demands);
        }

        public void getXDemands(List<BaseForeclosure> xdemands, List<BaseForeclosure> demands)
        {
            this.type = XDEMAND_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = "select " + columns + " from " + "( " + demands_query + ")base where 1=1 " + demands_conditions();
            query = query + " order by demand_letter_due_date desc";
            List<BaseForeclosure> all = get_loan_list(query);
            foreach (BaseForeclosure f in all) { f.type = XDEMAND_TYPE; demands.Add(f); }
            commonFilters(demands, xdemands);
        }

        public void getCurrentXDemands(List<BaseForeclosure> xdemands, List<BaseForeclosure> demands)
        {
            this.type = CURRENTXDEMAND_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = "select " + columns + " from " + "( " + demands_query + ")base where 1=1 " + demands_conditions();
            query = query + " AND ms_loan_due_date_next_payment >= getdate() ";
            query = query + " order by demand_letter_due_date desc";
            List<BaseForeclosure> all = get_loan_list(query);
            foreach (BaseForeclosure f in all) { demands.Add(f); }
            commonFilters(demands, xdemands);
        }

        public void getForeclosed(List<BaseForeclosure> foreclosed, List<BaseForeclosure> removed_foreclosed)
        {
            this.type = FORECLOSED_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = "select " + columns + " from " + "( " + foreclosed_query + ")base where 1=1 " + foreclosed_conditions();
            query = query + " order by sale_date desc, ms_loan_type, demand_letter_due_date asc";
            List<BaseForeclosure> all = get_loan_list(query);
            foreach (BaseForeclosure f in all) { f.type = FORECLOSED_TYPE; foreclosed.Add(f); }
            commonFilters(foreclosed, removed_foreclosed);
        }

        private void commonPreAndForeclosures(List<BaseForeclosure> foreclosures, List<BaseForeclosure> removed_foreclosures)
        {
            this.type = FORECLOSURE_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = "select " + columns + " from " + "( " + foreclosures_query + ")base where 1=1 " + foreclosures_conditions();
            query = query + " order by foreclosure_date asc, ms_loan_type, demand_letter_due_date asc";
            List<BaseForeclosure> all = get_loan_list(query);
            foreach (BaseForeclosure f in all) { f.type = FORECLOSURE_TYPE; foreclosures.Add(f); }
            commonFilters(foreclosures, removed_foreclosures);
        }

        public void getForeclosures(List<BaseForeclosure> foreclosures, List<BaseForeclosure> removed_foreclosures)
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

        public void getAll(List<BaseForeclosure> all, List<BaseForeclosure> none)
        {
            this.type = ALL_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = "select " + columns + " from " + "( " + all_types_query + ")base where 1=1 " ;
            query = query + " order by loan_id";
            List<BaseForeclosure> all_bfs = get_loan_list(query);
            foreach (BaseForeclosure f in all_bfs) { f.type = ALL_TYPE; all.Add(f); }
        }

        public void getPreForeclosures(List<BaseForeclosure> PreForeclosures, List<BaseForeclosure> removed_foreclosures)
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

        public void getForeclosureLoan(decimal loan_id, List<BaseForeclosure> accepted, List<BaseForeclosure> removed, String type)
        {
            this.type = type;
            string columns = this.default_columns + getEventsColumns();
            string query = "select " + columns + " from " + "( " + all_types_query + ")base where 1=1 " + single_loan_conditions(loan_id);
            query = query + " order by ms_loan_type, demand_letter_due_date asc";
            List<BaseForeclosure> all = get_loan_list(query);
            foreach (BaseForeclosure f in all) { f.type = type; accepted.Add(f); }
            commonFilters(accepted, removed);
        }

        public List<string> columns_list()
        {
            List<string> columns = new List<string>();
            columns.AddRange(default_columns_list());
            this.type = ALL_TYPE;
            foreach (BaseEvents be in getEventsTypes()) columns.AddRange(be.column_names_list());
            if (review_checklist != null) columns.AddRange(ReviewChecklist.columns_list());
            
            return columns;
        }

        public List<string> default_columns_list()
        {
            List<string> columns = new List<string>();
            string[] common_columns = this.default_columns.Split(new string[] { " as " }, StringSplitOptions.None);
            for (int i = 1; i < common_columns.Length; i++)
            {
                columns.Add(common_columns[i].Split(',')[0]);
            }
            return columns;
        }

        public void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@demand_letter_due_date", this.demand_letter_due_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_officers", this.foreclosure_officers);
            cmd.Parameters.AddWithValue("@fcl_type", this.fcl_type);
            cmd.Parameters.AddWithValue("@last_reg_paid_dt", this.last_reg_paid_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@last_payments_balance", this.last_payments_balance);
            cmd.Parameters.AddWithValue("@papers_to_attorney_date", this.papers_to_attorney_date.toDBInsert());
            cmd.Parameters.AddWithValue("@posting_date", this.posting_date.toDBInsert());
            cmd.Parameters.AddWithValue("@chapter_filed", this.chapter_filed);
            cmd.Parameters.AddWithValue("@bankruptcy_filed_date", this.bankruptcy_filed_date.toDBInsert());
            cmd.Parameters.AddWithValue("@bankruptcy_discharge_date", this.bankruptcy_discharge_date.toDBInsert());
            cmd.Parameters.AddWithValue("@bankruptcy_case_dismissed_date", this.bankruptcy_case_dismissed_date.toDBInsert());
            cmd.Parameters.AddWithValue("@bankruptcy_closed_date", this.bankruptcy_closed_date.toDBInsert());
            cmd.Parameters.AddWithValue("@sale_date", this.sale_date.toDBInsert());
            cmd.Parameters.AddWithValue("@ytd_prin_paid_amt", this.ytd_prin_paid_amt);
            cmd.Parameters.AddWithValue("@principal_amt_from_history", this.principal_amt_from_history);
            cmd.Parameters.AddWithValue("@fc_foreclosure_referral_date", this.fc_foreclosure_referral_date.toDBInsert());
            cmd.Parameters.AddWithValue("@fc_payoff_date", this.fc_payoff_date.toDBInsert());
            cmd.Parameters.AddWithValue("@appraisal_ordered_date", this.appraisal_ordered_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_date", this.foreclosure_date.toDBInsert());
            cmd.Parameters.AddWithValue("@lm_stop_code_id", this.lm_stop_code_id);
            cmd.Parameters.AddWithValue("@memo_fc_third_party_sale_date", this.memo_fc_third_party_sale_date.toDBInsert());
            cmd.Parameters.AddWithValue("@extension_ordered_date", this.extension_ordered_date.toDBInsert());

            this.loan.addInsertParameters(cmd);

            this.type = ALL_TYPE;
            foreach (BaseEvents be in getEventsTypes()) be.addInsertParameters(cmd);
            if (this.review_checklist != null) review_checklist.addInsertParameters(cmd);
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

        public static string demands_conditions()
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

        protected virtual List<BaseForeclosure> get_loan_list(String query)
        {
            List<BaseForeclosure> list = new List<BaseForeclosure>();
            return list;
        }

        public BaseForeclosure read_loan(DbDataReader reader)
        {
            BaseForeclosure f = this;
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
                foreach (BaseEvents be in f.getEventsTypes())
                {
                    be.read(reader, pos, f.loan);
                    pos += be.fields_count();
                }
                if( f.review_checklist != null ) f.review_checklist = ReviewChecklist.read(reader, pos);
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
                es.AddRange(fannie_mae_calls_events.events);
            }
            if (this.type == CURRENTXDEMAND_TYPE)
            {
                es.AddRange(xdemand_mortgage_status_events.events);
                es.AddRange(xdemand_occupancy_letter_events.events);
            }
            if (this.type == PREFORECLOSURE_TYPE)
            {
                es.AddRange(inspection.events);
                es.AddRange(reason_code.events);
                es.AddRange(foreclosure_appraisal_bpo_events.events);
                es.AddRange(fannie_mae_calls_events.events);
            }
            if (this.type == FORECLOSURE_TYPE)
            {
                es.AddRange(inspection.events);
                es.AddRange(reason_code.events);
                es.AddRange(foreclosure_mortgage_status_events.events);
                es.AddRange(foreclosure_rfs_gnma_foreclosure_status_events.events);
                es.AddRange(foreclosure_appraisal_bpo_events.events);
                es.AddRange(fannie_mae_calls_events.events);
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
            else if (this.type == ALL_TYPE)
            {
                es.AddRange(inspection.events);
                es.AddRange(reason_code.events);
                es.AddRange(demand_expire_events.events);
                es.AddRange(demand_occupancy_events.events);
                es.AddRange(xdemand_mortgage_status_events.events);
                es.AddRange(xdemand_occupancy_letter_events.events);
                es.AddRange(foreclosure_mortgage_status_events.events);
                es.AddRange(foreclosure_rfs_gnma_foreclosure_status_events.events);
                es.AddRange(foreclosure_appraisal_bpo_events.events);
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
                es.AddRange(fannie_mae_calls_events.events);
            }
            return es;
        }

        protected virtual string getEventsColumns()
        {
            string s = "";
            foreach (BaseEvents be in this.getEventsTypes())
            {
                s += ", \n" + be.query("base.loan_id");
            }
            if (this.review_checklist != null) s += ", \n" + ReviewChecklist.columns("base.loan_id");
            return s;
        }

        protected List<BaseEvents> getEventsTypes()
        {
            List<BaseEvents> event_types = new List<BaseEvents>();
            if (this.type == DEMAND_TYPE)
            {
                if (inspection == null) inspection = new InspectionEvents();
                event_types.Add(inspection);
                if (reason_code == null) reason_code = new ReasonCodeEvents();
                event_types.Add(reason_code);
                if (foreclosure_mortgage_status_events == null) foreclosure_mortgage_status_events = new ForeclosureMortgageStatusEvents();
                event_types.Add(foreclosure_mortgage_status_events);
                if (demand_expire_events == null) demand_expire_events = new DemandFileExpireEvents();
                event_types.Add(demand_expire_events);
                if (demand_occupancy_events == null) demand_occupancy_events = new DemandOccupancyEvents();
                event_types.Add(demand_occupancy_events);
                if (fannie_mae_calls_events == null) fannie_mae_calls_events = new FannieMaeCallsEvents();
                event_types.Add(fannie_mae_calls_events);
                this.review_checklist = new ReviewChecklist();
            }
            if (this.type == CURRENTXDEMAND_TYPE)
            {
                if (xdemand_mortgage_status_events == null) xdemand_mortgage_status_events = new XdemandMortgageStatusEvents();
                event_types.Add(xdemand_mortgage_status_events);
                if (xdemand_occupancy_letter_events == null) xdemand_occupancy_letter_events = new XdemandOccupancyLetterEvents();
                event_types.Add(xdemand_occupancy_letter_events);
            }
            if (this.type == PREFORECLOSURE_TYPE)
            {
                if (inspection == null) inspection = new InspectionEvents();
                event_types.Add(inspection);
                if (reason_code == null) reason_code = new ReasonCodeEvents();
                event_types.Add(reason_code);
                if (foreclosure_appraisal_bpo_events == null) foreclosure_appraisal_bpo_events = new ForeclosureAppraisalBPOEvents();
                event_types.Add(foreclosure_appraisal_bpo_events);
                if (fannie_mae_calls_events == null) fannie_mae_calls_events = new FannieMaeCallsEvents();
                event_types.Add(fannie_mae_calls_events);
            }
            if (this.type == FORECLOSURE_TYPE)
            {
                if (inspection == null) inspection = new InspectionEvents();
                event_types.Add(inspection);
                if (reason_code == null) reason_code = new ReasonCodeEvents();
                event_types.Add(reason_code);
                if (foreclosure_appraisal_bpo_events == null) foreclosure_appraisal_bpo_events = new ForeclosureAppraisalBPOEvents();
                event_types.Add(foreclosure_appraisal_bpo_events);
                if (foreclosure_mortgage_status_events == null) foreclosure_mortgage_status_events = new ForeclosureMortgageStatusEvents();
                event_types.Add(foreclosure_mortgage_status_events);
                if (foreclosure_rfs_gnma_foreclosure_status_events == null) foreclosure_rfs_gnma_foreclosure_status_events = new ForeclosureRfsGnmaForeclosureStatusEvents();
                event_types.Add(foreclosure_rfs_gnma_foreclosure_status_events);
                if (fannie_mae_calls_events == null) fannie_mae_calls_events = new FannieMaeCallsEvents();
                event_types.Add(fannie_mae_calls_events);
            }
            if (this.type == FORECLOSED_TYPE)
            {
                if (sale_inspection == null) sale_inspection = new SaleInspectionEvents();
                event_types.Add(sale_inspection);
                if (sale_conveyance == null) sale_conveyance = new SaleConveyanceEvents();
                event_types.Add(sale_conveyance);
                if (sale_hud_advice == null) sale_hud_advice = new SaleHudAdviceEvents();
                event_types.Add(sale_hud_advice);
                if (sale_title_package == null) sale_title_package = new SaleTitlePackageEvents();
                event_types.Add(sale_title_package);
                if (sale_title_approval == null) sale_title_approval = new SaleTitleApprovalEvents();
                event_types.Add(sale_title_approval);
                if (sale_claim_part_b == null) sale_claim_part_b = new SaleClaimPartBEvents();
                event_types.Add(sale_claim_part_b);
                if (sale_result_sale == null) sale_result_sale = new SaleResultSaleEvents();
                event_types.Add(sale_result_sale);
                if (sale_transfer_of_custody == null) sale_transfer_of_custody = new SaleTransferCustodyEvents();
                event_types.Add(sale_transfer_of_custody);
                if (sale_acquisition_payment == null) sale_acquisition_payment = new SaleAcquisitionPaymentEvents();
                event_types.Add(sale_acquisition_payment);
                if (sale_basic_claim_events == null) sale_basic_claim_events = new SaleBasicClaimEvents();
                event_types.Add(sale_basic_claim_events);
                if (sale_reo_notice_events == null) sale_reo_notice_events = new SaleREONoticeEvents();
                event_types.Add(sale_reo_notice_events);
                if (sale_fannie_mae_claim_571_events == null) sale_fannie_mae_claim_571_events = new SaleFannieMaeClaim571Events();
                event_types.Add(sale_fannie_mae_claim_571_events);
                if (sale_mortgage_status_events == null) sale_mortgage_status_events = new SaleMortgageStatusEvents();
                event_types.Add(sale_mortgage_status_events);
                if (sale_original_documents_ordered_events == null) sale_original_documents_ordered_events = new SaleOriginalDocumentsOrderedEvents();
                event_types.Add(sale_original_documents_ordered_events);
            }
            if (this.type == ALL_TYPE)
            {
                if (inspection == null) inspection = new InspectionEvents();
                event_types.Add(inspection);
                if (reason_code == null) reason_code = new ReasonCodeEvents();
                event_types.Add(reason_code);
                if (demand_expire_events == null) demand_expire_events = new DemandFileExpireEvents();
                event_types.Add(demand_expire_events);
                if (demand_occupancy_events == null) demand_occupancy_events = new DemandOccupancyEvents();
                event_types.Add(demand_occupancy_events);
                if (xdemand_mortgage_status_events == null) xdemand_mortgage_status_events = new XdemandMortgageStatusEvents();
                event_types.Add(xdemand_mortgage_status_events);
                if (xdemand_occupancy_letter_events == null) xdemand_occupancy_letter_events = new XdemandOccupancyLetterEvents();
                event_types.Add(xdemand_occupancy_letter_events);
                if (foreclosure_appraisal_bpo_events == null) foreclosure_appraisal_bpo_events = new ForeclosureAppraisalBPOEvents();
                event_types.Add(foreclosure_appraisal_bpo_events);
                if (foreclosure_mortgage_status_events == null) foreclosure_mortgage_status_events = new ForeclosureMortgageStatusEvents();
                event_types.Add(foreclosure_mortgage_status_events);
                if (foreclosure_rfs_gnma_foreclosure_status_events == null) foreclosure_rfs_gnma_foreclosure_status_events = new ForeclosureRfsGnmaForeclosureStatusEvents();
                event_types.Add(foreclosure_rfs_gnma_foreclosure_status_events);
                if (sale_inspection == null) sale_inspection = new SaleInspectionEvents();
                event_types.Add(sale_inspection);
                if (sale_conveyance == null) sale_conveyance = new SaleConveyanceEvents();
                event_types.Add(sale_conveyance);
                if (sale_hud_advice == null) sale_hud_advice = new SaleHudAdviceEvents();
                event_types.Add(sale_hud_advice);
                if (sale_title_package == null) sale_title_package = new SaleTitlePackageEvents();
                event_types.Add(sale_title_package);
                if (sale_title_approval == null) sale_title_approval = new SaleTitleApprovalEvents();
                event_types.Add(sale_title_approval);
                if (sale_claim_part_b == null) sale_claim_part_b = new SaleClaimPartBEvents();
                event_types.Add(sale_claim_part_b);
                if (sale_result_sale == null) sale_result_sale = new SaleResultSaleEvents();
                event_types.Add(sale_result_sale);
                if (sale_transfer_of_custody == null) sale_transfer_of_custody = new SaleTransferCustodyEvents();
                event_types.Add(sale_transfer_of_custody);
                if (sale_acquisition_payment == null) sale_acquisition_payment = new SaleAcquisitionPaymentEvents();
                event_types.Add(sale_acquisition_payment);
                if (sale_basic_claim_events == null) sale_basic_claim_events = new SaleBasicClaimEvents();
                event_types.Add(sale_basic_claim_events);
                if (sale_reo_notice_events == null) sale_reo_notice_events = new SaleREONoticeEvents();
                event_types.Add(sale_reo_notice_events);
                if (sale_fannie_mae_claim_571_events == null) sale_fannie_mae_claim_571_events = new SaleFannieMaeClaim571Events();
                event_types.Add(sale_fannie_mae_claim_571_events);
                if (sale_mortgage_status_events == null) sale_mortgage_status_events = new SaleMortgageStatusEvents();
                event_types.Add(sale_mortgage_status_events);
                if (sale_original_documents_ordered_events == null) sale_original_documents_ordered_events = new SaleOriginalDocumentsOrderedEvents();
                event_types.Add(sale_original_documents_ordered_events);
                if (fannie_mae_calls_events == null) fannie_mae_calls_events = new FannieMaeCallsEvents();
                event_types.Add(fannie_mae_calls_events);
                this.review_checklist = new ReviewChecklist();
            }


            return event_types;
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