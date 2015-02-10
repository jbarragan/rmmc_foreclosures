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
                return "" +
" base.loan_id as loan_id, " +
"(select top 1 misc.misc_dt_value from ms_loan_misc_fields misc where misc_field_id = 10020 and misc.loan_id = base.loan_id order by misc.misc_dt_value ) as demand_letter_due_date, " +
"(select top 1 misc.misc_field_value from ms_loan_misc_fields misc where misc_field_id = 10023 and misc.loan_id = base.loan_id order by misc.misc_field_value ) as foreclosure_officers, " +
"(select top 1 ms_fcl_info.fcl_type from ms_fcl_info where ms_fcl_info.loan_id = base.loan_id order by ms_fcl_info.fcl_type) as fcl_type, " +
"(select top 1 hist.paid_dt from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'REG' order by paid_dt desc) as last_reg_paid_dt, " +
"(select sum(hist.pmt_amt) from ms_loan_history hist where hist.loan_id = base.loan_id and (hist.trans_type_cd = 'REG' or  hist.trans_type_cd = 'REGR') and hist.paid_dt >=  (select top 1 hist.paid_dt from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'REG' order by paid_dt desc) ) as last_payments_balance, " +
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
"(select top 1 memo.memo_create_dt  from ms_loan_memo memo join ms_memo_types on memo.memo_type_id = ms_memo_types.memo_type_id where memo.loan_id = base.loan_id and ms_memo_types.memo_type_desc like 'FC-3rd%' order by memo.memo_create_dt desc ) as memo_fc_third_party_sale_date, " +
"(select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 10800 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_ord_req )  as extension_ordered_date, " +
com.sp.rmmc.common.models.MsLoan.columns("base.loan_id") + " ";
            }
        }

        protected override string demands_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id " +
                    " from ms_loan_misc_fields as base " +
                    " where base.misc_field_id = 10020 " +
                    ") ";
            }
        }

        protected override string foreclosures_query
        {
            get
            {
                return
            "( " +
            " select base.loan_id " +
            " from  ms_fcl_info as base " +
            " where base.fcl_type = 'A' " +
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

        protected override string all_types_query
        {
            get
            {
                return
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
    }
}
