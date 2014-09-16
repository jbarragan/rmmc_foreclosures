using System;
using System.Collections.Generic;
using System.Web;
using com.sp.rmmc.common.models;
using System.Data.Common;


namespace com.sp.rmmc.foreclosures.models
{
    public class LoanPreservationDisbursement : ForeclosuresCommonModel
    {
        public decimal loan_id = 0M;
        public int fcl_misc_disb_id = 0;
        public string fcl_misc_disb_type = "";
        public DateTimeObject fcl_misc_ordered_date = new DateTimeObject();
        public DateTimeObject fcl_misc_completed_date = new DateTimeObject();
        public DateTimeObject fcl_misc_paid_date = new DateTimeObject();
        public string fcl_misc_description = "";
        public decimal fcl_misc_actual_amt = 0M;
        public decimal fcl_misc_claimable_amt = 0M;

        public PreservationCode preservation_code = new PreservationCode();
        public MsLoan loan = new MsLoan();

        private static string base_query = "" +
"select " +
"loan_id, " +
"fcl_misc_disb_id, " +
"fcl_misc_disb_type, " +
"fcl_misc_ordered_date, " +
"fcl_misc_completed_date, " +
"fcl_misc_paid_date, " +
"fcl_misc_description, " +
"fcl_misc_actual_amt, " +
"fcl_misc_claimable_amt, " +
MsLoan.columns("base.loan_id") +
"from ms_fcl_misc_disbursement as base ";

        public LoanPreservationDisbursement()
        {
        }

        public static string insert(LoanPreservationDisbursement disbursement)
        {
            string query = "insert into ms_fcl_misc_disbursement "
                                    + " (loan_id, fcl_misc_disb_id, fcl_misc_disb_type, fcl_misc_ordered_date, fcl_misc_completed_date, fcl_misc_paid_date, fcl_misc_description, fcl_misc_actual_amt, fcl_misc_claimable_amt) "
                              + " values("
                              + disbursement.loan_id.ToString() + ", "
                              + disbursement.fcl_misc_disb_id.ToString() + ", "
                              + toDBString(disbursement.fcl_misc_disb_type) + ", "
                              + disbursement.fcl_misc_ordered_date.toSQLAnywhereDBValue() + ", "
                              + disbursement.fcl_misc_completed_date.toSQLAnywhereDBValue() + ", "
                              + disbursement.fcl_misc_paid_date.toSQLAnywhereDBValue() + ", "
                              + toDBString(disbursement.fcl_misc_description) + ", "
                              + disbursement.fcl_misc_actual_amt.ToString() + ", "
                              + disbursement.fcl_misc_claimable_amt.ToString() + ")";
            LoanPreservationDisbursement.executeStatement(query);
            return LoanPreservationDisbursement.error;
        }

        public static string update(LoanPreservationDisbursement disbursement)
        {
            string query = "update ms_fcl_misc_disbursement set "
                              + " fcl_misc_disb_type = " + toDBString(disbursement.fcl_misc_disb_type) + ", "
                              + " fcl_misc_ordered_date = " + disbursement.fcl_misc_ordered_date.toSQLAnywhereDBValue() + ", "
                              + " fcl_misc_completed_date = " + disbursement.fcl_misc_completed_date.toSQLAnywhereDBValue() + ", "
                              + " fcl_misc_paid_date = " + disbursement.fcl_misc_paid_date.toSQLAnywhereDBValue() + ", "
                              + " fcl_misc_description = " + toDBString(disbursement.fcl_misc_description) + ", "
                              + " fcl_misc_actual_amt = " + disbursement.fcl_misc_actual_amt.ToString() + ", "
                              + " fcl_misc_claimable_amt = " + disbursement.fcl_misc_claimable_amt.ToString() + " "
                              + " where loan_id = " + disbursement.loan_id.ToString() + " and fcl_misc_disb_id = " + disbursement.fcl_misc_disb_id;
            LoanPreservationDisbursement.executeStatement(query);
            return LoanPreservationDisbursement.error;
        }

        public static string delete(LoanPreservationDisbursement disbursement)
        {
            string query = "delete from ms_fcl_misc_disbursement where loan_id = " + disbursement.loan_id.ToString() + " and fcl_misc_disb_id = " + disbursement.fcl_misc_disb_id;
            LoanPreservationDisbursement.executeStatement(query);
            return LoanPreservationDisbursement.error;
        }

        public static List<LoanPreservationDisbursement> get_loan_preservation_disbursements(decimal loan_id)
        {
            string query = base_query + " where loan_id = " + loan_id.ToString() + " and fcl_misc_disb_type = 'PNP'";
            query = query + " order by fcl_misc_disb_id";
            return get_disbursements(query);
        }

        public static List<LoanPreservationDisbursement> get_loan_all_disbursements(decimal loan_id)
        {
            string query = base_query + " where loan_id = " + loan_id.ToString();
            query = query + " order by fcl_misc_disb_id";
            return get_disbursements(query);
        }

        public static List<LoanPreservationDisbursement> get_disbursements(string query)
        {
            List<LoanPreservationDisbursement> disbursements = new List<LoanPreservationDisbursement>();
            DbDataReader reader = getReader(query);
            while (reader.Read())
            {
                LoanPreservationDisbursement disbursement = new LoanPreservationDisbursement();
                disbursement = read(reader);
                disbursements.Add(disbursement);
            }
            reader.Close();
            close_connection();
            return disbursements;
        }

        public static LoanPreservationDisbursement get(LoanPreservationDisbursement disbursement)
        {
            string query = base_query + " where loan_id = " + disbursement.loan_id.ToString() + " and fcl_misc_disb_id = " + disbursement.fcl_misc_disb_id;
            LoanPreservationDisbursement ret_disbursement = new LoanPreservationDisbursement();
            DbDataReader reader = getReader(query); 
            if (reader.Read())
            {
                ret_disbursement = read(reader);
            }
            reader.Close();
            close_connection();
            return ret_disbursement;
        }

        private static LoanPreservationDisbursement read(DbDataReader reader)
        {
            LoanPreservationDisbursement disbursement = new LoanPreservationDisbursement();
            int pos = 0;
            disbursement.loan_id = readDBDecimal(reader, pos++);
            disbursement.fcl_misc_disb_id = readDBInt32(reader, pos++);
            disbursement.fcl_misc_disb_type = readDBString(reader, pos++);
            disbursement.fcl_misc_ordered_date = readDBDateObject(reader, pos++, DateTime.Now);
            disbursement.fcl_misc_completed_date = readDBDateObject(reader, pos++, DateTime.Now);
            disbursement.fcl_misc_paid_date = readDBDateObject(reader, pos++, DateTime.Now);
            disbursement.fcl_misc_description = readDBString(reader, pos++);
            disbursement.fcl_misc_actual_amt = readDBDecimal(reader, pos++);
            disbursement.fcl_misc_claimable_amt = readDBDecimal(reader, pos++);
            return disbursement;
        }

    }
}