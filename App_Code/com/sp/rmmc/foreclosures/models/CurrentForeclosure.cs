using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Common;

using com.sp.rmmc.common.models;
using com.sp.rmmc.common.models.events;

namespace com.sp.rmmc.foreclosures.models
{
    public class CurrentForeclosure : BaseForeclosure
    {
        protected override string default_columns
        {
            get
            {
                return @"
with
history_reg as (
	select loan_id,
		max(paid_dt) as paid_dt,
		max(due_dt) as due_dt,
		max(prin_amt) as prin_amt,
		max(int_amt) as int_amt,
		max(curtailment_amount) as curtailment_amount
	from ms_loan_history
	where trans_type_cd = 'REG'
	and paid_dt = (select max(paid_dt) from ms_loan_history m where m.loan_id = ms_loan_history.loan_id and m.trans_type_cd = 'REG' group by loan_id)
	and due_dt = (select max(due_dt) from ms_loan_history m where m.loan_id = ms_loan_history.loan_id and m.trans_type_cd = 'REG' group by loan_id)
	group by loan_id
),
history_after_reg_regr as (
	select loan_id,
		sum(pmt_amt) as pmt_amt
	from ms_loan_history
	where (trans_type_cd = 'REG' or trans_type_cd = 'REGR')
	and paid_dt >= (select max(paid_dt) from ms_loan_history m where m.loan_id = ms_loan_history.loan_id and m.trans_type_cd = 'REG' group by loan_id)
	group by loan_id
),
history_pif as (
	select loan_id,
		abs(sum(prin_amt)) as prin_amt
	from ms_loan_history
	where trans_type_cd = 'PIF'
	group by loan_id
),
history_prin_bal as (
	select loan_id,
		abs(convert(numeric, uad_audit_data_bf_chg)) as prin_bl
	from ms_user_audit_history_data
	where uad_audit_field_name = 'prin_bal'
	and isnumeric(uad_audit_data_bf_chg) = 1 
	and ms_user_audit_history_data.uad_audit_actual_chg_dt = (select max(m.uad_audit_actual_chg_dt) from ms_user_audit_history_data m where m.loan_id = ms_user_audit_history_data.loan_id and m.uad_audit_field_name = 'prin_bal' and isnumeric(m.uad_audit_data_bf_chg) = 1  group by m.loan_id)
),
memo_fc_referral_date as (
	select loan_id,
		max(memo_create_dt) as memo_create_dt
	from ms_loan_memo
	where memo_type_id = 139
	group by loan_id
),
memo_fc_payoff_date as (
	select loan_id,
		max(memo_create_dt) as memo_create_dt
	from ms_loan_memo
	where memo_type_id = 148
	group by loan_id
),
lm_stop_code as (
	select loan_id,
		max(stop_code_id) as stop_code_id
	from ms_loan_stop_code
	where stop_code_id = 153
	and ms_loan_stop_code.last_updated = (select max(m.last_updated) from ms_loan_stop_code m where m.loan_id = ms_loan_stop_code.loan_id and m.stop_code_id = 153)
	group by loan_id
),
memo_fc_third_party_sale as (
	select loan_id,
		max(memo_create_dt) as memo_create_dt
	from ms_loan_memo
	join ms_memo_types on ms_loan_memo.memo_type_id = ms_memo_types.memo_type_id
	where ms_memo_types.memo_type_desc like 'FC-3rd%'
	group by loan_id
),
memo_fc_stopped as (
	select loan_id,
		max(memo_notify_dt) as memo_notify_dt
	from ms_loan_memo
	join ms_memo_types on ms_loan_memo.memo_type_id = ms_memo_types.memo_type_id
	where ms_memo_types.memo_type_desc like 'FC-Foreclosure Stopped%'
	group by loan_id
),
memo_fc_review as (
	select loan_id,
		max(memo_create_dt) as memo_created_dt
	from ms_loan_memo
	join ms_memo_types on ms_loan_memo.memo_type_id = ms_memo_types.memo_type_id
	where ms_memo_types.memo_type_desc like 'FC-FORECLOSURE REVI%'
	group by loan_id
),
mfia2_sale_losses as (
	select
		loan_id,
		sum(fcl_item_amt) as fcl_item_amt
	from ms_fcl_item_amts_2
	where ms_fcl_item_amts_2.fcl_item_desc like '%LOSS%'
	and ms_fcl_item_amts_2.item_id in (11900, 10600, 10700, 10300, 11200)
	group by loan_id
)
select top 500 base.loan_id as loan_id,
ms_loan_misc_fields.misc_dt_value as demand_letter_due_date,
mslmf_foreclosure_officers.misc_field_value as foreclosure_officers,
ms_fcl_info.fcl_type,
history_reg.paid_dt as last_reg_paid_dt,
history_after_reg_regr.pmt_amt as last_payments_balance,
ms_fcl_item_amts_1.fcl_item_date as papers_to_attorney_date,
mfia1_posting.fcl_item_date as posting_date,
ms_credit_information.chapter_filed,
ms_credit_information.bankruptcy_filed_date,
ms_bankruptcy_info.discharge_date as bankruptcy_discharge_date,
ms_bankruptcy_info.case_dismissed_date as bankruptcy_case_dismissed_date,
ms_bankruptcy_info.closed_date as bankruptcy_closed_date,
mfia1_sale_date.fcl_item_date as sale_date,
ms_loan_balances.ytd_prin_paid_amt as ytd_prin_paid_amt,
history_pif.prin_amt as principal_amt_from_history,
history_prin_bal.prin_bl as audit_prin_bal,
memo_fc_referral_date.memo_create_dt as fc_foreclosure_referral_date,
memo_fc_payoff_date.memo_create_dt as fc_payoff_date,
mfia1_appraisal_ordered.fcl_item_date as appraisal_ordered_date,
mfia1_posting.fcl_item_recvd_expd as foreclosure_date,
lm_stop_code.stop_code_id as lm_stop_code_id,
memo_fc_third_party_sale.memo_create_dt as memo_fc_third_party_sale_date,
mfia2_extension_ordered.fcl_item_ord_req as extension_ordered_date,
ms_fcl_info_deed_in_lieu.fcl_deed_in_lieu as fcl_deed_in_lieu,
(null)  as lm_date,
mfia1_possession.fcl_item_date as possession_date,
mfia1_deed_from_borrower.fcl_item_date as deed_from_borrower_received_date,
memo_fc_stopped.memo_notify_dt as foreclosure_stopped_memo_date,
mfia2_sale_losses.fcl_item_amt as sale_losses,
memo_fc_review.memo_created_dt as memo_fc_foreclosure_review_date, " +
com.sp.rmmc.common.models.MsLoan.columns("base.loan_id") + " ";
            }
        }

        public static string joins = @"
left join ms_loan_misc_fields on ms_loan_misc_fields.loan_id = base.loan_id and ms_loan_misc_fields.misc_field_id = 10020
left join ms_loan_misc_fields mslmf_foreclosure_officers on mslmf_foreclosure_officers.loan_id = base.loan_id and mslmf_foreclosure_officers.misc_field_id = 10023
left join ms_fcl_info on ms_fcl_info.loan_id = base.loan_id
left join ms_fcl_info ms_fcl_info_deed_in_lieu on ms_fcl_info_deed_in_lieu.fcl_deed_in_lieu = 'Y' and ms_fcl_info_deed_in_lieu.loan_id = base.loan_id
left join history_reg on history_reg.loan_id = base.loan_id
left join history_after_reg_regr on history_after_reg_regr.loan_id = base.loan_id
left join history_pif on history_pif.loan_id = base.loan_id
left join history_prin_bal on history_prin_bal.loan_id = base.loan_id
left join ms_fcl_item_amts_1 on ms_fcl_item_amts_1.loan_id = base.loan_id and ms_fcl_item_amts_1.item_id = 1000
left join ms_fcl_item_amts_1 mfia1_posting on mfia1_posting.loan_id = base.loan_id and mfia1_posting.item_id = 1200
left join ms_credit_information on ms_credit_information.loan_id = base.loan_id
left join ms_bankruptcy_info on ms_bankruptcy_info.loan_id = base.loan_id
left join ms_fcl_item_amts_1 mfia1_sale_date on mfia1_sale_date.loan_id = base.loan_id and mfia1_sale_date.item_id = 1600
left join ms_loan_balances on ms_loan_balances.loan_id = base.loan_id
left join memo_fc_referral_date on memo_fc_referral_date.loan_id = base.loan_id
left join memo_fc_payoff_date on memo_fc_payoff_date.loan_id = base.loan_id
left join memo_fc_third_party_sale on memo_fc_third_party_sale.loan_id = base.loan_id
left join memo_fc_stopped on memo_fc_stopped.loan_id = base.loan_id
left join memo_fc_review on memo_fc_review.loan_id = base.loan_id
left join mfia2_sale_losses on mfia2_sale_losses.loan_id = base.loan_id
left join ms_fcl_item_amts_1 mfia1_appraisal_ordered on mfia1_appraisal_ordered.loan_id = base.loan_id and mfia1_appraisal_ordered.item_id = 1600
left join lm_stop_code on lm_stop_code.loan_id = base.loan_id
left join ms_fcl_item_amts_1 mfia1_possession on mfia1_possession.loan_id = base.loan_id and mfia1_possession.item_id = 1900
left join ms_fcl_item_amts_1 mfia1_deed_from_borrower on mfia1_deed_from_borrower.loan_id = base.loan_id and mfia1_deed_from_borrower.item_id = 3900
left join ms_fcl_item_amts_2 mfia2_extension_ordered on mfia2_extension_ordered.loan_id = base.loan_id and mfia2_extension_ordered.item_id = 10800
";

        protected override string demands_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id " +
                    " from ms_loan_misc_fields as base " +
                    " left join ms_fcl_info on ms_fcl_info.loan_id = base.loan_id " +
                    " left join ms_loan_misc_fields on ms_loan_misc_fields.misc_field_id = 10020 and ms_loan_misc_fields.loan_id = base.loan_id " +
                    " left join ms_fcl_item_amts_1 mfia1_sale_date on mfia1_sale_date.item_id = 1600 and mfia1_sale_date.loan_id = base.loan_id " +
                    " left join ms_loan_balances on ms_loan_balances.loan_id = base.loan_id " +
                    " where base.misc_field_id = 10020 " +
                    " and ( (select top 1 memo.memo_notify_dt from ms_loan_memo memo join ms_memo_types on memo.memo_type_id = ms_memo_types.memo_type_id where memo.loan_id = base.loan_id and ms_memo_types.memo_type_desc like 'FC-Foreclosure Stopped%' order by memo.memo_notify_dt desc ) is null " +
                    "       or base.misc_dt_value >=  (select top 1 memo.memo_notify_dt from ms_loan_memo memo join ms_memo_types on memo.memo_type_id = ms_memo_types.memo_type_id where memo.loan_id = base.loan_id and ms_memo_types.memo_type_desc like 'FC-Foreclosure Stopped%' order by memo.memo_notify_dt desc ) ) " +
                    ") ";
            }
        }

        protected override string foreclosures_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id \n" +
                    " from ms_loan_misc_fields as base \n" +
                    " where base.misc_field_id = 10020 \n" +
                    " and ( (select top 1 memo.memo_notify_dt  from ms_loan_memo memo join ms_memo_types on memo.memo_type_id = ms_memo_types.memo_type_id where memo.loan_id = base.loan_id and ms_memo_types.memo_type_desc like 'FC-Foreclosure Stopped%' order by memo.memo_notify_dt desc ) is null \n" +
                    "       or base.misc_dt_value <  (select top 1 memo.memo_notify_dt  from ms_loan_memo memo join ms_memo_types on memo.memo_type_id = ms_memo_types.memo_type_id where memo.loan_id = base.loan_id and ms_memo_types.memo_type_desc like 'FC-Foreclosure Stopped%' order by memo.memo_notify_dt desc ) \n" +
                    "       or (select top 1 ms_fcl_info.fcl_type from ms_fcl_info where ms_fcl_info.loan_id = base.loan_id order by ms_fcl_info.fcl_type) = 'A' \n" +
                    "     ) \n" +
                    ") ";
            }
        }

        protected override string removed_foreclosures_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id \n" +
                    " from ms_loan_misc_fields as base \n" +
                    " where base.misc_field_id = 10020 \n" +
                    " and ( (select top 1 memo.memo_notify_dt  from ms_loan_memo memo join ms_memo_types on memo.memo_type_id = ms_memo_types.memo_type_id where memo.loan_id = base.loan_id and ms_memo_types.memo_type_desc like 'FC-Foreclosure Stopped%' order by memo.memo_notify_dt desc ) is null \n" +
                    "       or base.misc_dt_value <  (select top 1 memo.memo_notify_dt  from ms_loan_memo memo join ms_memo_types on memo.memo_type_id = ms_memo_types.memo_type_id where memo.loan_id = base.loan_id and ms_memo_types.memo_type_desc like 'FC-Foreclosure Stopped%' order by memo.memo_notify_dt desc ) \n" +
                    "       or (select top 1 ms_fcl_info.fcl_type from ms_fcl_info where ms_fcl_info.loan_id = base.loan_id order by ms_fcl_info.fcl_type) = 'A' \n" +
                    //"       or (select top 1 ms_fcl_info.fcl_type from ms_fcl_info where ms_fcl_info.loan_id = base.loan_id order by ms_fcl_info.fcl_type) = 'C' \n" +
                    "     ) \n" +
                    ") ";
            }
        }

        protected override string foreclosed_query
        {
            get
            {
                return
            "( " +
            " select base.loan_id " +
            " from ms_fcl_item_amts_1 as base " +
            " where base.item_id = 1600 " +
            " and base.fcl_item_date > '2014-01-01' " +
            ") ";
            }
        }

        protected override string deed_in_lieu_query
        {
            get
            {
                return
            "( " +
            " select base.loan_id " +
            " from ms_fcl_info as base " +
            " where base.fcl_deed_in_lieu = 'Y' " +
            ") ";
            }
        }

        protected override string all_types_query
        {
            get
            {
                return
                    demands_query +
                    " UNION " + 
                    foreclosures_query +
                    " UNION " +
                    foreclosed_query +
                    " UNION " +
                    deed_in_lieu_query +
                    " ";
            }
        }


        public CurrentForeclosure()
        {
            
        }

        protected override List<BaseForeclosure> get_loan_list(String query)
        {
            List<BaseForeclosure> list = new List<BaseForeclosure>();
            SybaseModel model = new SybaseModel();

            DbDataReader reader = model.getReader(query);
            while (reader.Read())
            {
                BaseForeclosure f = new BaseForeclosure();
                f.type = this.type;
                f.read_loan(reader);
                list.Add(f);
            }
            reader.Close();
            model.close_connection();
            return list;
        }

        protected override string getEventsColumns()
        {
            string s = "";
            foreach (BaseEvents be in this.getEventsTypes())
            {
                s += ", \n" + be.query("base.loan_id");
            }
            if (this.review_checklist != null) s += ", \n" + ReviewChecklist.columns("base.loan_id");
            return s;
        }

        protected override void get_lm_date(List<BaseForeclosure> accepted)
        {
            foreach (BaseForeclosure f in accepted)
            {
                if (f.loan_id == 141236M)
                    f.loan_id = 141236M;
                if (f.lm_stop_code_id > 0)
                {
                    f.lm_date.set_date(this.loan.event_todays_date);
                }
            }
            List<LMReception> all_lm_receptions = new List<LMReception>();//LMReception.getLMReceptions(accepted);
            Dictionary<decimal, LMReception> loan_lm_receptions = new Dictionary<decimal, LMReception>();
            foreach (LMReception lm_reception in all_lm_receptions)
            {
                if (loan_lm_receptions.ContainsKey(lm_reception.loan_id)) continue;
                loan_lm_receptions.Add(lm_reception.loan_id, lm_reception);
            }
            foreach (BaseForeclosure f in accepted)
            {
                if (f.loan_id == 141236M)
                    f.loan_id = 141236M;
                if (loan_lm_receptions.ContainsKey(f.loan_id))
                {
                    f.lm_date = loan_lm_receptions[f.loan_id].date;
                }
            }
        }
    }
}
