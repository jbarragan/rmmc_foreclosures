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
        public DateTimeObject part_a_filed_recvd_date = new DateTimeObject();
        public decimal part_a_filed_amt = 0M;
        public string claim_type = "";
        public decimal loan_paid_off = 0M;
        public decimal loan_current_balance = 0M;
        public decimal debenture_rate = 0M;
        public DateTimeObject foreclosure_date = new DateTimeObject();
        public decimal debenture_interest_paid = 0M;
        public decimal interest_of_payoff = 0M;
        public DateTimeObject part_b_filed_recvd_date = new DateTimeObject();
        public decimal part_b_filed_amt = 0M;
        public DateTimeObject loss_filed_recvd_date = new DateTimeObject();
        public decimal loss_filed_amt = 0M;
        public MsLoan loan = new MsLoan();
        
        private static string base_query = "" +
@"select base.loan_id, " +
"fcl_item_recvd_expd as part_a_filed_recvd_date, " +
"fcl_item_amt as part_a_filed_amt, " +
"fcl_item_desc as claim_type, " +
"(select top 1 ms_loan_balances.ytd_prin_paid_amt from ms_loan_balances where ms_loan_balances.loan_id = base.loan_id order by ms_loan_balances.ytd_prin_paid_amt) as loan_paid_off, " +
"(select top 1 ms_loan_balances.prin_bal from ms_loan_balances where ms_loan_balances.loan_id = base.loan_id order by ms_loan_balances.prin_bal) as loan_current_balance, " +
"(select top 1 ms_fcl_fha_info.fcl_debenture_rate from ms_fcl_fha_info where ms_fcl_fha_info.loan_id = base.loan_id order by ms_fcl_fha_info.fcl_debenture_rate) as debenture_rate, " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1600 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as foreclosure_date, " +
"(select top 1 misc.misc_dec_value from ms_loan_misc_fields misc where misc.loan_id = base.loan_id and misc.misc_field_id = 10151 order by misc.misc_dec_value ) as debenture_interest_paid, " +
"(select top 1 gnma_inv.gnma_pif_int_due from ms_gnma_inv_loan gnma_inv where  gnma_inv.loan_id = base.loan_id order by gnma_inv.gnma_pif_int_due ) as interest_of_paid_off, " +
"(select top 1 mfia2.fcl_item_recvd_expd from ms_fcl_item_amts_2 mfia2 where mfia2.loan_id = base.loan_id and mfia2.fcl_item_desc like '%PART B%' and mfia2.fcl_item_recvd_expd is not null order by mfia2.fcl_item_recvd_expd) as part_b_filed_recvd_date, " +
"(select top 1 mfia2.fcl_item_amt from ms_fcl_item_amts_2 mfia2 where mfia2.loan_id = base.loan_id and mfia2.fcl_item_desc like '%PART B%' and mfia2.fcl_item_recvd_expd is not null order by mfia2.fcl_item_recvd_expd) as part_b_filed_amt, " +
"(select top 1 mfia2.fcl_item_recvd_expd from ms_fcl_item_amts_2 mfia2 where mfia2.loan_id = base.loan_id and mfia2.fcl_item_desc like '%LOSS%' and mfia2.fcl_item_recvd_expd is not null order by mfia2.fcl_item_recvd_expd) as loss_filed_recvd_date, " +
"(select top 1 mfia2.fcl_item_amt from ms_fcl_item_amts_2 mfia2 where mfia2.loan_id = base.loan_id and mfia2.fcl_item_desc like '%LOSS%' and mfia2.fcl_item_recvd_expd is not null order by mfia2.fcl_item_recvd_expd) as loss_filed_amt, " +
MsLoan.columns("base.loan_id") +
"from ms_fcl_item_amts_2 as base " +
"where (base.fcl_item_desc like '%PART A%' OR base.fcl_item_desc like '%VA BASIC CLAIM%' OR base.fcl_item_desc like '%FANNIE MAE CLAIM%')  and part_a_filed_recvd_date is not null";

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
            claim.part_a_filed_recvd_date = readDBDateObject(reader, pos++, DateTime.Now);
            claim.part_a_filed_amt = readDBDecimal(reader, pos++);
            claim.claim_type = readDBString(reader, pos++);
            claim.loan_paid_off = readDBDecimal(reader, pos++);
            claim.loan_current_balance = readDBDecimal(reader, pos++);
            claim.debenture_rate = readDBDecimal(reader, pos++);
            claim.foreclosure_date = readDBDateObject(reader, pos++, DateTime.Now);
            claim.debenture_interest_paid = readDBDecimal(reader, pos++);
            claim.interest_of_payoff = readDBDecimal(reader, pos++);
            claim.part_b_filed_recvd_date = readDBDateObject(reader, pos++, DateTime.Now);
            claim.part_b_filed_amt = readDBDecimal(reader, pos++);
            claim.loss_filed_recvd_date = readDBDateObject(reader, pos++, DateTime.Now);
            claim.loss_filed_amt = readDBDecimal(reader, pos++);

            if (claim.loan_id > 0)
            {
                claim.loan = MsLoan.read(reader, pos);
            }
            return claim;
        }

        public static List<Claim> getAllClaims()
        {
            string query = base_query + " order by part_a_filed_recvd_date desc";
            return get_list(query);
        }

        public static List<Claim> getMonthlyClaims(int month, int year)
        {
            string conditions = " AND MONTH(part_a_filed_recvd_date) = " + month + " AND YEAR(part_a_filed_recvd_date) = " + year + " ";
            string query = base_query + conditions + " order by part_a_filed_recvd_date desc";
            return get_list(query);
        }

        public static Claim getClaimsTotals(List<Claim> claims)
        {
            Claim claims_total = new Claim();
            foreach (Claim claim in claims)
            {
                claims_total.loan_paid_off += claim.loan_paid_off;
                claims_total.loan_current_balance += claim.loan_current_balance;
                claims_total.part_a_filed_amt += claim.part_a_filed_amt;
                claims_total.part_b_filed_amt += claim.part_b_filed_amt;
                claims_total.loss_filed_amt += claim.loss_filed_amt;
                claims_total.debenture_rate += claim.debenture_rate / claims.Count;
                claims_total.debenture_interest_paid += claim.debenture_interest_paid;
                claims_total.loan.int_rate += claim.loan.int_rate / claims.Count;
                claims_total.interest_of_payoff += claim.interest_of_payoff;
            }
            return claims_total;
        }
    }
}