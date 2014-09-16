using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Common;

using com.sp.rmmc.common.models;

namespace com.sp.rmmc.foreclosures.models
{
    public class Foreclosed : ForeclosuresCommonModel
    {
        public int id = 0;
        public decimal loan_id = 0M;
        public decimal loan_paid_off = 0M;
        public decimal loan_current_balance = 0M;
        public decimal gnma_pif_prin_bal = 0M;
        
        public DateTimeObject attorneys_posted_date = new DateTimeObject();
        public string fs_eviction_initiated = "";
        public DateTimeObject writ_of_posession_ordered_date = new DateTimeObject();
        public DateTimeObject eviction_stop_date = new DateTimeObject();
        public DateTimeObject fcl_eviction_date = new DateTimeObject();
        public DateTimeObject secured_date = new DateTimeObject();
        public DateTimeObject possesion_date = new DateTimeObject();
        public DateTimeObject part_a_ordered_date = new DateTimeObject();
        public DateTimeObject part_a_received_date = new DateTimeObject();
        public decimal part_a_payment_amount = 0M;
        public DateTimeObject title_package_ordered_date = new DateTimeObject();
        public DateTimeObject title_approval_ordered_date = new DateTimeObject();
        public DateTimeObject part_bcd_ordered_date = new DateTimeObject();
        public DateTimeObject part_bcd_received_date = new DateTimeObject();
        public decimal part_bcd_payment_amount = 0M;
        public DateTimeObject pi_conveyance_inspection_dt = new DateTimeObject();

        public DateTimeObject fc_results_of_sale_date = new DateTimeObject();
        public DateTimeObject transfer_of_custody_date = new DateTimeObject();
        public DateTimeObject va_acquisition_payment_date = new DateTimeObject();
        public decimal va_acquisition_payment_amount = 0M;
        public DateTimeObject va_basic_claim_date = new DateTimeObject();
        public DateTimeObject va_summary_claim_date = new DateTimeObject();
        public decimal va_claim_amount = 0M;

        public DateTimeObject fc_reo_notification_date = new DateTimeObject();
        public DateTimeObject fannie_mae_claim_571_date = new DateTimeObject();
        public DateTimeObject fannie_mae_claim_571_check_date = new DateTimeObject();
        public decimal fannie_mae_claim_571_amount = 0M;
        public DateTimeObject supplemental_date = new DateTimeObject();
        public DateTimeObject supplemental_check_date = new DateTimeObject();
        public decimal supplemental_amount = 0M;

        public DateTimeObject fc_3rd_party_sale_date = new DateTimeObject();
        public DateTimeObject foreclosure_date = new DateTimeObject();

        public MsLoan loan = new MsLoan();


        private static string base_query = "" +
"select base.loan_id,                                                                                                                                                                                    " +
"(select top 1 ms_loan_balances.ytd_prin_paid_amt from ms_loan_balances where ms_loan_balances.loan_id = base.loan_id order by ms_loan_balances.ytd_prin_paid_amt) as loan_paid_off,                     " +
"(select top 1 ms_loan_balances.prin_bal from ms_loan_balances where ms_loan_balances.loan_id = base.loan_id order by ms_loan_balances.prin_bal) as loan_current_balance,                                " +
"(select top 1 ms_gnma_inv_loan.gnma_pif_prin_bal from ms_gnma_inv_loan where ms_gnma_inv_loan.loan_id = base.loan_id order by ms_gnma_inv_loan.gnma_pif_prin_bal) as gnma_pif_prin_bal,                 " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1200 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as attorneys_posted_date,               " +
"(select top 1 misc.misc_field_value from ms_loan_misc_fields misc where misc_field_id = 10084 and misc.loan_id = base.loan_id order by misc.misc_field_value ) as fs_eviction_initiated,                " +
"(select top 1 misc.misc_dt_value from ms_loan_misc_fields misc where misc_field_id = 10153 and misc.loan_id = base.loan_id order by misc.misc_dt_value ) as writ_of_posession_ordered_date,             " +
"(select top 1 misc.misc_dt_value from ms_loan_misc_fields misc where misc_field_id = 10154 and misc.loan_id = base.loan_id order by misc.misc_dt_value ) as eviction_stop_date,                         " +
"(select top 1 occupancy.fcl_eviction_date from ms_loan_occupancy occupancy  where occupancy.loan_id = base.loan_id order by occupancy.fcl_eviction_date ) as fcl_eviction_date,                         " +
"(select top 1 occupancy.secured_date from ms_loan_occupancy occupancy  where occupancy.loan_id = base.loan_id order by occupancy.secured_date ) as secured_date,                                        " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1900 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as possesion_date,                      " +
"(select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11200 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_ord_req ) as part_a_ordered_date,          " +
"(select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11200 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_recvd_expd ) as part_a_received_date,   " +
"(select top 1 ms_fcl.fcl_item_amt from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11200 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_amt ) as part_a_payment_amount,                " +
"(select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11500 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_ord_req ) as title_package_ordered_date,   " +
"(select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11600 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_ord_req ) as title_approval_ordered_date,  " +
"(select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11900 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_ord_req ) as part_bcd_ordered_date,        " +
"(select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11900 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_recvd_expd ) as part_bcd_received_date, " +
"(select top 1 ms_fcl.fcl_item_amt from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11900 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_amt ) as part_bcd_payment_amount,              " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id = 57 order by memo_id desc) as pi_conveyance_inspection_dt,                            " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id = 136 order by memo_id desc) as fc_results_of_sale_date,                               " +
"(select top 1 fcl_ud.fcl_ud_item_date from ms_fcl_ud_item_amts_1 fcl_ud where fcl_ud.loan_id = base.loan_id and fcl_ud.fcl_ud_item_desc like '%QUISITION PAY%' order by fcl_ud_item_date desc) as transfer_of_custody_date, " +
"(select top 1 fcl_ud.fcl_ud_item_recvd_expd from ms_fcl_ud_item_amts_1 fcl_ud where fcl_ud.loan_id = base.loan_id and fcl_ud.fcl_ud_item_desc like '%QUISITION PAY%' order by fcl_ud_item_recvd_expd desc) as va_acquisition_payment_date, " +
"(select top 1 fcl_ud.fcl_ud_item_amt from ms_fcl_ud_item_amts_1 fcl_ud where fcl_ud.loan_id = base.loan_id and fcl_ud.fcl_ud_item_desc like '%QUISITION PAY%' order by fcl_ud_item_amt desc) as va_acquisition_payment_amount, " +
"(select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 10700 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_ord_req ) as va_basic_claim_date,          " +
"(select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 10700 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_recvd_expd ) as va_summary_claim_date,  " +
"(select top 1 ms_fcl.fcl_item_amt from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 10700 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_amt ) as va_claim_amount,                      " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id = 135 order by memo_id desc) as fc_reo_notification_date,                              " +
"(select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 10300 and ms_fcl.loan_id = base.loan_id and ms_fcl.fcl_item_desc like '%571%' order by ms_fcl.fcl_item_ord_req ) as fannie_mae_claim_571_date, " +
"(select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 10300 and ms_fcl.loan_id = base.loan_id and ms_fcl.fcl_item_desc like '%571%' order by ms_fcl.fcl_item_recvd_expd ) as fannie_mae_claim_571_check_date, " +
"(select top 1 ms_fcl.fcl_item_amt from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 10300 and ms_fcl.loan_id = base.loan_id and ms_fcl.fcl_item_desc like '%571%' order by ms_fcl.fcl_item_amt ) as fannie_mae_claim_571_amount, " +
"(select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 10500 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_ord_req ) as supplemental_claim_date, " +
"(select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 10500 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_recvd_expd ) as supplemental_claim_check_date, " +
"(select top 1 ms_fcl.fcl_item_amt from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 10500 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_amt ) as supplemental_claim_amount, " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id = 137 order by memo_id desc) as fc_3rd_party_sale_date,                              " +
"base.fcl_item_date as foreclosure_date,                                                                                                                                                                 " +
MsLoan.columns("base.loan_id") +
"from ms_fcl_item_amts_1 as base       " +
"where base.item_id = 1600             " +
"and base.fcl_item_date > '2014-01-01' ";

        public static List<Foreclosed> all()
        {
            string query = base_query + " order by base.fcl_item_date";
            return get_list(query);
        }

        public static List<Foreclosed> all_va()
        {
            string conditions = " and ms_loan_type = 'VA' ";
            string query = base_query + conditions + " order by base.fcl_item_date";
            return get_list(query);
        }

        public static List<Foreclosed> all_conv()
        {
            string conditions = " and ms_loan_type = 'CNV' ";
            string query = base_query + conditions + " order by base.fcl_item_date";
            return get_list(query);
        }

        public static List<Foreclosed> all_fha()
        {
            string conditions = " and ms_loan_type = 'FHA' ";
            string query = base_query + conditions + " order by base.fcl_item_date";
            return get_list(query);
        }

        private static List<Foreclosed> get_list(String query)
        {
            List<Foreclosed> list = new List<Foreclosed>();
            DbDataReader reader =  getReader(query);
            while (reader.Read())
            {
                Foreclosed foreclosed = new Foreclosed();
                foreclosed = read(reader);
                list.Add(foreclosed);
            }
            reader.Close();
            close_connection();
            return list;
        }

        private static Foreclosed read(DbDataReader reader)
        {
            Foreclosed foreclosed = new Foreclosed();
            int pos = 0;
            foreclosed.loan_id = readDBDecimal(reader, pos++);
            foreclosed.loan_paid_off = readDBDecimal(reader, pos++);
            foreclosed.loan_current_balance = readDBDecimal(reader, pos++);
            foreclosed.gnma_pif_prin_bal = readDBDecimal(reader, pos++);
            foreclosed.attorneys_posted_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.fs_eviction_initiated = readDBString(reader, pos++);
            foreclosed.writ_of_posession_ordered_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.eviction_stop_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.fcl_eviction_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.secured_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.possesion_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.part_a_ordered_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.part_a_received_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.part_a_payment_amount = readDBDecimal(reader, pos++);
            foreclosed.title_package_ordered_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.title_approval_ordered_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.part_bcd_ordered_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.part_bcd_received_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.part_bcd_payment_amount = readDBDecimal(reader, pos++);
            foreclosed.pi_conveyance_inspection_dt = readDBDateObject(reader, pos++, DateTime.Now);

            foreclosed.fc_results_of_sale_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.transfer_of_custody_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.va_acquisition_payment_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.va_acquisition_payment_amount = readDBDecimal(reader, pos++);
            foreclosed.va_basic_claim_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.va_summary_claim_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.va_claim_amount = readDBDecimal(reader, pos++);
            
            foreclosed.fc_reo_notification_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.fannie_mae_claim_571_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.fannie_mae_claim_571_check_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.fannie_mae_claim_571_amount = readDBDecimal(reader, pos++);
            foreclosed.supplemental_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.supplemental_check_date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosed.supplemental_amount = readDBDecimal(reader, pos++);
            foreclosed.fc_3rd_party_sale_date = readDBDateObject(reader, pos++, DateTime.Now);

            foreclosed.foreclosure_date = readDBDateObject(reader, pos++, DateTime.Now);

            if (foreclosed.loan_id > 0)
            {
                foreclosed.loan = MsLoan.read(reader, pos);
            }
            return foreclosed;
        }

        public decimal getUPC()
        {
            if( loan_paid_off > 0.0M ) return loan_paid_off;
            if (loan_current_balance > 0.0M) return loan_current_balance;
            return gnma_pif_prin_bal;
            
        }

        public bool is_attorneys_posted_date_5_months_before_due_date_alert()
        {
            return this.attorneys_posted_date.date.AddMonths(-5) < this.loan.due_date_next_payment.date;
        }

        public bool is_inspection_ordered_NOT_ordered_1_day_after_foreclosure_alert()
        {
            return this.pi_conveyance_inspection_dt.date.AddDays(-2) > this.foreclosure_date.date;
        }

        public bool is_eviction_NOT_completed_45_day_after_foreclosure_alert()
        {
            return this.fcl_eviction_date.date.AddDays(-45) > this.foreclosure_date.date;
        }

        public bool is_conveyance_part_a_30_after_posession_alert()
        {
            return this.part_a_ordered_date.date.AddDays(-31) > this.possesion_date.date;
        }

        public bool is_fannie_mae_claim_30_after_foreclosure_date_alert()
        {
            return this.fannie_mae_claim_571_date.date.AddDays(-31) > this.foreclosure_date.date;
        }

        public bool is_transfer_of_custody_15_after_results_of_sale_date_alert()
        {
            return this.transfer_of_custody_date.date.AddDays(-16) > this.fc_results_of_sale_date.date;
        }

        public bool is_va_basic_claim_45_after_foreclosure_date_alert()
        {
            return this.va_basic_claim_date.date.AddDays(-46) > this.foreclosure_date.date;
        }

        public bool is_title_approval_45_after_conveyance_part_a_alert()
        {
            return this.title_approval_ordered_date.date.AddDays(-46) > this.part_a_received_date.date;
        }

        public bool is_title_approval_40_after_conveyance_part_a_warning()
        {
            return (this.title_approval_ordered_date.date.AddDays(-39) > this.part_a_received_date.date) && this.title_approval_ordered_date.isNull;
        }

        public bool is_claim_part_b_15_after_title_approval_alert()
        {
            return this.part_bcd_ordered_date.date.AddDays(-16) > this.title_approval_ordered_date.date;
        }

        public bool is_claim_part_b_10_after_title_approval_warning()
        {
            return (this.part_bcd_ordered_date.date.AddDays(-10) > this.title_approval_ordered_date.date) && this.part_bcd_ordered_date.isNull;
        }
    }
}