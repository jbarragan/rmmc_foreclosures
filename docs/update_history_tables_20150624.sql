
alter table history_loan_details add
	sale_va_guaranty_and_bid_calcs_loan_id decimal(18,0),
	sale_va_guaranty_and_bid_calcs_va_guaranty_amount  decimal(18,2),
	sale_va_guaranty_and_bid_calcs_original_prin_balance  decimal(18,2),
	sale_va_guaranty_and_bid_calcs_appraisal_fee  decimal(18,2),
	sale_va_guaranty_and_bid_calcs_appraisal_value  decimal(18,2);