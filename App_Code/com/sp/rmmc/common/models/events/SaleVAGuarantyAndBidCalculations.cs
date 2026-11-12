using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class SaleVAGuarantyAndBidCalculations : BaseEvents
    {
        public decimal va_guaranty_amount = 0M;
        public decimal original_prin_balance = 0M;
        public decimal appraisal_fee = 0M;
        public decimal appraisal_value = 0M;

        // Formula fields
        public int days_of_interest = 0;
        public decimal accured_interest = 0M;
        public decimal liq_espe = 0M;
        public decimal liq_exp_before = 0M;
        public decimal liq_exp_after = 0M;
        public decimal escrow_advances_prior = 0M;
        public decimal escrow_advances_after = 0M;
        public decimal total_indebtedness = 0M;
        public decimal payment_total_indebtedness = 0M;
        public decimal property_preservation_before = 0M;
        public decimal property_preservation_after = 0M;
        public decimal credits = 0M;
        public decimal va_cost_factor = 0M;
        public decimal net_value = 0M;
        public decimal bid = 0M;
        public decimal max_guaranty_pct = 0M;
        public decimal guaranty = 0M;
        public decimal max_guaranty_amount = 0M;
        public decimal principal_and_interest_only = 0M;
        public decimal proceeds_of_sale = 0M;
        public decimal claim_amount = 0M;
        public decimal gross_claim_amount = 0M;
        public decimal potential_total_amount_paid_by_va = 0M;
        

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as sale_va_guaranty_and_bid_calcs_loan_id",
             "(select top 1 fcl_ud.fcl_item_amt from ms_fcl_ud_item_amts_2 fcl_ud where fcl_ud.loan_id = loan_id_column and fcl_ud.fcl_item_name like 'VA GUARAN%' order by fcl_item_ord_req desc) as sale_va_guaranty_and_bid_calcs_va_guaranty_amount",
             "(select top 1 mlb.original_prin_balance from ms_loan_balances mlb where  mlb.loan_id = loan_id_column order by mlb.original_prin_balance ) as sale_va_guaranty_and_bid_calcs_original_prin_balance",
             "(select sum(mfmd.fcl_misc_actual_amt) from ms_fcl_misc_disbursement mfmd where mfmd.loan_id = loan_id_column and fcl_misc_disb_type = 'APP' group by fcl_misc_disb_type ) as sale_va_guaranty_and_bid_calcs_appraisal_fee",
             "(select top 1 ms_fcl.fcl_item_amt from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1100 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_date ) as sale_va_guaranty_and_bid_calcs_appraisal_value"
            };

        public SaleVAGuarantyAndBidCalculations()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            calculate_values();
        }

        public void set_external_sources(int days_of_interest,
            decimal liq_espe,
            decimal liq_exp_before,
            decimal liq_exp_after,
            decimal escrow_advances_prior,
            decimal escrow_advances_after,
            decimal property_preservation_before,
            decimal property_preservation_after,
            decimal appraisal_fee,
            decimal credits,
            decimal cost_factor)
        {
            this.days_of_interest = days_of_interest;
            this.liq_espe = liq_espe;
            this.liq_exp_before = liq_exp_before;
            this.liq_exp_after = liq_exp_after;
            this.escrow_advances_prior = escrow_advances_prior;
            this.escrow_advances_after = escrow_advances_after;
            this.property_preservation_before = property_preservation_before;
            this.property_preservation_after = property_preservation_after;
            this.appraisal_fee = appraisal_fee;
            this.credits = credits;
            this.va_cost_factor = cost_factor;
        }

        public void calculate_values()
        {
            this.accured_interest = this.loan.prin_bal * this.loan.int_rate * this.days_of_interest / 36500;
            this.principal_and_interest_only = this.accured_interest + this.loan.prin_bal;
            this.total_indebtedness = this.loan.prin_bal + this.accured_interest +
                this.liq_espe + this.escrow_advances_prior + this.appraisal_fee + this.property_preservation_before;
            this.payment_total_indebtedness = this.principal_and_interest_only +
                this.liq_exp_before + this.liq_exp_after + this.escrow_advances_prior + this.escrow_advances_after +
                this.property_preservation_before + this.property_preservation_after;
            this.net_value = this.appraisal_value * (100.0M - va_cost_factor) / 100.0M;
            this.bid = this.net_value > this.total_indebtedness ? total_indebtedness : this.net_value;
            this.proceeds_of_sale = this.bid;
            this.claim_amount = this.payment_total_indebtedness - proceeds_of_sale;

            this.max_guaranty_pct = (this.original_prin_balance >= 0M ) ? this.va_guaranty_amount / this.original_prin_balance : 0M;
            this.guaranty = this.total_indebtedness * this.max_guaranty_pct;
            this.max_guaranty_amount = (this.guaranty >= this.va_guaranty_amount) ? this.va_guaranty_amount : this.guaranty;

            this.gross_claim_amount = ((this.claim_amount >= this.max_guaranty_amount) ? this.max_guaranty_amount : this.claim_amount ) + 400;
            this.potential_total_amount_paid_by_va = this.gross_claim_amount + proceeds_of_sale;
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.va_guaranty_amount = DbModel.readDBDecimal(reader, pos++);
            this.original_prin_balance = DbModel.readDBDecimal(reader, pos++);
            this.appraisal_fee = DbModel.readDBDecimal(reader, pos++);
            this.appraisal_value = DbModel.readDBDecimal(reader, pos++);
            
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@sale_va_guaranty_and_bid_calcs_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@sale_va_guaranty_and_bid_calcs_va_guaranty_amount", this.va_guaranty_amount);
            cmd.Parameters.AddWithValue("@sale_va_guaranty_and_bid_calcs_original_prin_balance", this.original_prin_balance);
            cmd.Parameters.AddWithValue("@sale_va_guaranty_and_bid_calcs_appraisal_fee", this.appraisal_fee);
            cmd.Parameters.AddWithValue("@sale_va_guaranty_and_bid_calcs_appraisal_value", this.appraisal_value);
        }
    }
}