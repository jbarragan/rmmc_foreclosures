using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Common;

using com.sp.rmmc.common.models;

namespace com.sp.rmmc.foreclosures.models
{
    public class Claim : ForeclosuresCommonModel
    {
        public decimal loan_id = 0M;
        public DateTimeObject submitted_date = new DateTimeObject();
        public DateTimeObject received_date = new DateTimeObject();
        public decimal claim_amount = 0M;
        public string claim_type = "";
        public decimal loan_paid_off = 0M;
        public decimal loan_current_balance = 0M;
        public decimal debenture_rate = 0M;
        public DateTimeObject foreclosure_date = new DateTimeObject();
        public decimal debenture_interest_paid = 0M;
        public decimal interest_of_payoff = 0M;
        public DateTimeObject loss_submitted_date = new DateTimeObject();
        public DateTimeObject loss_received_date = new DateTimeObject();
        public decimal loss_amount = 0M;
        public DateTimeObject last_pay_off_date = new DateTimeObject();
        public MsLoan loan = new MsLoan();
        
        private static string base_query = "" +
@"select base.loan_id, " +
"fcl_item_ord_req as submitted_date , " +
"fcl_item_recvd_expd as received_date, " +
"fcl_item_amt as claim_amount, " +
"fcl_item_desc as claim_type, " +
"(select top 1 ms_loan_balances.ytd_prin_paid_amt from ms_loan_balances where ms_loan_balances.loan_id = base.loan_id order by ms_loan_balances.ytd_prin_paid_amt) as loan_paid_off, " +
"(select top 1 ms_loan_balances.prin_bal from ms_loan_balances where ms_loan_balances.loan_id = base.loan_id order by ms_loan_balances.prin_bal) as loan_current_balance, " +
"(select top 1 ms_fcl_fha_info.fcl_debenture_rate from ms_fcl_fha_info where ms_fcl_fha_info.loan_id = base.loan_id order by ms_fcl_fha_info.fcl_debenture_rate) as debenture_rate, " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1600 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as foreclosure_date, " +
"(select top 1 misc.misc_dec_value from ms_loan_misc_fields misc where misc.loan_id = base.loan_id and misc.misc_field_id = 10151 order by misc.misc_dec_value ) as debenture_interest_paid, " +
"(select top 1 gnma_inv.gnma_pif_int_due from ms_gnma_inv_loan gnma_inv where  gnma_inv.loan_id = base.loan_id order by gnma_inv.gnma_pif_int_due ) as interest_of_paid_off, " +
"(select top 1 mfia2.fcl_item_ord_req from ms_fcl_item_amts_2 mfia2 where mfia2.loan_id = base.loan_id and mfia2.fcl_item_desc like '%LOSS%' and mfia2.fcl_item_recvd_expd is not null order by mfia2.fcl_item_recvd_expd) as loss_submitted_date, " +
"(select top 1 mfia2.fcl_item_recvd_expd from ms_fcl_item_amts_2 mfia2 where mfia2.loan_id = base.loan_id and mfia2.fcl_item_desc like '%LOSS%' and mfia2.fcl_item_recvd_expd is not null order by mfia2.fcl_item_recvd_expd) as loss_received_date, " +
"(select top 1 mfia2.fcl_item_amt from ms_fcl_item_amts_2 mfia2 where mfia2.loan_id = base.loan_id and mfia2.fcl_item_desc like '%LOSS%' and mfia2.fcl_item_recvd_expd is not null order by mfia2.fcl_item_recvd_expd) as loss_amount, " +
"(select top 1 hist.paid_dt from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'PIF' order by paid_dt desc) as last_pay_off_date, " +
MsLoan.columns("base.loan_id") +
"from ms_fcl_item_amts_2 as base " +
"where (base.fcl_item_desc like '%' AND " +
" base.fcl_item_desc not like '%LOSS%') AND base.item_id in (11900, 10600, 10700, 10300, 11200) " +
" and base.fcl_item_recvd_expd is not null and base.fcl_item_recvd_expd > dateadd(yy, -3, getdate())";

        private static string base_query2 = "" +
@" UNION " +
"select base.loan_id, " +
"fcl_ud_item_date as submitted_date , " +
"fcl_ud_item_recvd_expd as received_date, " +
"fcl_ud_item_amt as claim_amount, " +
"fcl_ud_item_desc as claim_type, " +
"(select top 1 ms_loan_balances.ytd_prin_paid_amt from ms_loan_balances where ms_loan_balances.loan_id = base.loan_id order by ms_loan_balances.ytd_prin_paid_amt) as loan_paid_off, " +
"(select top 1 ms_loan_balances.prin_bal from ms_loan_balances where ms_loan_balances.loan_id = base.loan_id order by ms_loan_balances.prin_bal) as loan_current_balance, " +
"(select top 1 ms_fcl_fha_info.fcl_debenture_rate from ms_fcl_fha_info where ms_fcl_fha_info.loan_id = base.loan_id order by ms_fcl_fha_info.fcl_debenture_rate) as debenture_rate, " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1600 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as foreclosure_date, " +
"(select top 1 misc.misc_dec_value from ms_loan_misc_fields misc where misc.loan_id = base.loan_id and misc.misc_field_id = 10151 order by misc.misc_dec_value ) as debenture_interest_paid, " +
"(select top 1 gnma_inv.gnma_pif_int_due from ms_gnma_inv_loan gnma_inv where  gnma_inv.loan_id = base.loan_id order by gnma_inv.gnma_pif_int_due ) as interest_of_paid_off, " +
"(select top 1 mfia2.fcl_item_ord_req from ms_fcl_item_amts_2 mfia2 where mfia2.loan_id = base.loan_id and mfia2.fcl_item_desc like '%LOSS%' and mfia2.fcl_item_recvd_expd is not null order by mfia2.fcl_item_recvd_expd) as loss_submitted_date, " +
"(select top 1 mfia2.fcl_item_recvd_expd from ms_fcl_item_amts_2 mfia2 where mfia2.loan_id = base.loan_id and mfia2.fcl_item_desc like '%LOSS%' and mfia2.fcl_item_recvd_expd is not null order by mfia2.fcl_item_recvd_expd) as loss_received_date, " +
"(select top 1 mfia2.fcl_item_amt from ms_fcl_item_amts_2 mfia2 where mfia2.loan_id = base.loan_id and mfia2.fcl_item_desc like '%LOSS%' and mfia2.fcl_item_recvd_expd is not null order by mfia2.fcl_item_recvd_expd) as loss_amount, " +
"(select top 1 hist.paid_dt from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'PIF' order by paid_dt desc) as last_pay_off_date, " +
MsLoan.columns("base.loan_id") +
"from ms_fcl_ud_item_amts_1 as base " +
"where (base.fcl_ud_item_desc like '%VA ACQUISITION%')  " +
" and fcl_ud_item_recvd_expd is not null and fcl_ud_item_recvd_expd > dateadd(yy, -3, getdate())";

        private static List<Claim> get_list(String query)
        {
            List<Claim> list = new List<Claim>();
            DbDataReader reader =  getReader(query);
            while (reader.Read())
            {
                Claim claim = new Claim();
                claim = read(reader);
                list.Add(claim);
            }
            reader.Close();
            close_connection();
            return list;
        }

        public static Claim get(decimal loan_id)
        {
            string query = base_query + " and loan_id = " + loan_id.ToString();
            Claim claim = new Claim();
            DbDataReader reader = getReader(query);
            if (reader.Read())
                claim = read(reader);
            reader.Close();
            return claim;
        }

        private static Claim read(DbDataReader reader)
        {
            Claim claim = new Claim();
            int pos = 0;
            claim.loan_id = readDBDecimal(reader, pos++);
            claim.submitted_date = readDBDateObject(reader, pos++, DateTime.Now);
            claim.received_date = readDBDateObject(reader, pos++, DateTime.Now);
            claim.claim_amount = readDBDecimal(reader, pos++);
            claim.claim_type = readDBString(reader, pos++);
            claim.loan_paid_off = readDBDecimal(reader, pos++);
            claim.loan_current_balance = readDBDecimal(reader, pos++);
            claim.debenture_rate = readDBDecimal(reader, pos++);
            claim.foreclosure_date = readDBDateObject(reader, pos++, DateTime.Now);
            claim.debenture_interest_paid = readDBDecimal(reader, pos++);
            claim.interest_of_payoff = readDBDecimal(reader, pos++);
            claim.loss_submitted_date = readDBDateObject(reader, pos++, DateTime.Now);
            claim.loss_received_date = readDBDateObject(reader, pos++, DateTime.Now);
            claim.loss_amount = readDBDecimal(reader, pos++);
            claim.last_pay_off_date = readDBDateObject(reader, pos++, DateTime.Now);

            if (claim.loan_id > 0)
            {
                claim.loan = MsLoan.read(reader, pos);
            }
            return claim;
        }

        public static List<Claim> getAllClaims()
        {
            string order = " order by received_date desc ";
            string query = base_query + base_query2 + order;
            return get_list(query);
        }

        public static List<Claim> getMonthlyClaims(int month, int year)
        {
            string conditions = " AND MONTH(received_date) = " + month + " AND YEAR(received_date) = " + year + " ";
            string order = " order by received_date asc ";
            string query = base_query + conditions + base_query2 + conditions + order;
            return get_list(query);
        }

        public static Claim getClaimsTotals(List<Claim> claims)
        {
            Claim claims_total = new Claim();
            foreach (Claim claim in claims)
            {
                claims_total.loan_paid_off += claim.loan_paid_off;
                claims_total.claim_amount += claim.claim_amount;
                claims_total.loan_current_balance += claim.loan_current_balance;
                claims_total.debenture_rate += claim.debenture_rate / claims.Count;
                claims_total.debenture_interest_paid += claim.debenture_interest_paid;
                claims_total.loan.int_rate += claim.loan.int_rate / claims.Count;
                claims_total.interest_of_payoff += claim.interest_of_payoff;
                claims_total.loss_amount += claim.loss_amount;
            }
            return claims_total;
        }
    }
}