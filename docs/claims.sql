--describe ms_fcl_item_amts_2
--select loan_id, fcl_item_recvd_expd from ms_fcl_item_amts_2 where item_id = 10600
select base.loan_id,
fcl_item_recvd_expd as part_a_filed_recvd_date,
fcl_item_amt as part_a_filed_amt,
(select top 1 ms_loan_balances.ytd_prin_paid_amt from ms_loan_balances where ms_loan_balances.loan_id = base.loan_id order by ms_loan_balances.ytd_prin_paid_amt) as loan_paid_off,
(select top 1 ms_loan_balances.prin_bal from ms_loan_balances where ms_loan_balances.loan_id = base.loan_id order by ms_loan_balances.prin_bal) as loan_current_balance,
(select top 1 ms_fcl_fha_info.fcl_debenture_rate from ms_fcl_fha_info where ms_fcl_fha_info.loan_id = base.loan_id order by ms_fcl_fha_info.fcl_debenture_rate) as debenture_rate,
(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1600 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as foreclosure_date,
(select top 1 misc.misc_dec_value from ms_loan_misc_fields misc where  misc.loan_id = base.loan_id order by misc.misc_dec_value ) as debenture_interest_paid,
(select top 1 gnma_inv.gnma_pif_int_due from ms_gnma_inv_loan gnma_inv where  gnma_inv.loan_id = base.loan_id order by gnma_inv.gnma_pif_int_due ) as interest_of_paid_off
from ms_fcl_item_amts_2 as base
where base.loan_id = 144370
and base.fcl_item_desc like '%PART A%'
