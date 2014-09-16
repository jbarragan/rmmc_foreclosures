using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Common;

using com.sp.rmmc.common.models;

namespace com.sp.rmmc.foreclosures.models
{
    public class Bankruptcy : ForeclosuresCommonModel
    {
        public int id = 0;
        public decimal loan_id = 0M;
        public DateTimeObject demand_letter_due_date = new DateTimeObject();
        public String fcl_type = "";
        public DateTimeObject last_reg_paid_dt = new DateTimeObject();
        public DateTimeObject papers_to_attorney_date = new DateTimeObject();
        public DateTimeObject posting_date = new DateTimeObject();
        public string chapter_filed = "";
        public DateTimeObject bankruptcy_filed_date = new DateTimeObject();
        
        public String foreclosure_officers = "";
        public String loan_type = "";
        public MsLoan loan = new MsLoan();

        private static string base_query = "" +
"select base.loan_id, " +
"base.misc_dt_value as demand_letter_due_date, " +
"(select top 1 misc.misc_field_value from ms_loan_misc_fields misc where misc_field_id = 10023 and misc.loan_id = base.loan_id order by misc.misc_field_value ) as foreclosure_officers, " +
"(select top 1 ms_fcl_info.fcl_type from ms_fcl_info where ms_fcl_info.loan_id = base.loan_id order by ms_fcl_info.fcl_type) as fcl_type, " +
"(select top 1 hist.paid_dt from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'REG' order by paid_dt desc) as last_reg_paid_dt, " +
"(select top 1 ms_loan_info.loan_type from ms_loan_information ms_loan_info where ms_loan_info.loan_id = base.loan_id order by ms_loan_info.loan_type ) as loan_type, " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1000 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as papers_to_attorney_date, " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1200 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as posting_date, " +
"(select top 1 ms_credit_information.chapter_filed from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.chapter_filed ) as chapter_filed, " +
"(select top 1 ms_credit_information.bankruptcy_filed_date from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.bankruptcy_filed_date ) as bankruptcy_filed_date, " +
MsLoan.columns("base.loan_id") +
"from ms_loan_misc_fields as base " +
"where base.misc_field_id = 10020 ";

        public static List<DemandLetter> in_pre_demand_letter_mode()
        {
            string query = base_query + " and ( demand_letter_due_date > getdate() ) " +
                                        " and ( fcl_type <> 'A' or fcl_type is null ) " +
                                        " and ( last_reg_paid_dt < dateadd(dd, -30, demand_letter_due_date) or last_reg_paid_dt is null ) ";
            query = query + " order by loan_type, demand_letter_due_date asc";
            return get_list(query);
        }

        public static List<DemandLetter> remove_demand_letter_by_last_reg_payment()
        {
            string query = base_query + " and ( demand_letter_due_date > getdate() ) " +
                                        " and ( last_reg_paid_dt >= dateadd(dd, -30, demand_letter_due_date) ) ";
            query = query + " order by loan_type, demand_letter_due_date asc";
            return get_list(query);
        }

        public static List<DemandLetter> remove_demand_letter_by_foreclosure_active()
        {
            string query = base_query + " and ( demand_letter_due_date > getdate() ) " +
                                        " and ( fcl_type = 'A' ) ";
            query = query + " order by loan_type, demand_letter_due_date asc";
            return get_list(query);
        }

        public static List<DemandLetter> recent_foreclosure_activated()
        {
            string query = base_query + " and ( dateadd(dd, 90, demand_letter_due_date) > getdate() ) " +
                                        " and ( last_reg_paid_dt < dateadd(dd, -30, demand_letter_due_date) or last_reg_paid_dt is null ) " +
                                        " and ( chapter_filed is null or chapter_filed = '' ) " +
                                        " and ( fcl_type = 'A' ) ";
            query = query + " order by loan_type, demand_letter_due_date asc";
            return get_list(query);
        }

        public static List<DemandLetter> remove_from_recent_foreclosure_activated_by_payment_dt()
        {
            string query = base_query + " and ( dateadd(dd, 30, demand_letter_due_date) > getdate() ) " +
                                        " and ( last_reg_paid_dt >= dateadd(dd, -30, demand_letter_due_date) ) " +
                                        " and ( fcl_type = 'A' ) ";
            query = query + " order by loan_type, demand_letter_due_date asc";
            return get_list(query);
        }

        public static List<DemandLetter> remove_from_foreclosure_by_bankruptcy()
        {
            string query = base_query + " and ( dateadd(dd, 90, demand_letter_due_date) > getdate() ) " +
                                        " and ( chapter_filed <> '' and chapter_filed is not null  ) " +
                                        " and ( fcl_type = 'A' ) ";
            query = query + " order by loan_type, demand_letter_due_date asc";
            return get_list(query);
        }

        public string in_pre_demand_letter_tr_class()
        {
            string s = "";
            if (DateTime.Today.AddDays(5) > demand_letter_due_date.date)
                s = "error";
            else if (DateTime.Today.AddDays(10) > demand_letter_due_date.date)
                s = "warning";
            return s;
        }

        private static List<DemandLetter> get_list(String query)
        {
            List<DemandLetter> list = new List<DemandLetter>();
            DbDataReader reader = getReader(query);
            while (reader.Read())
            {
                DemandLetter demand_letter = new DemandLetter();
                demand_letter = read(reader);
                list.Add(demand_letter);
            }
            reader.Close();
            close_connection();
            return list;
        }

        public static DemandLetter get(decimal loan_id)
        {
            string query = base_query + " and loan_id = " + loan_id.ToString();
            DemandLetter demand_letter = new DemandLetter();
            DbDataReader reader = getReader(query);
            if (reader.Read())
                demand_letter = read(reader);
            reader.Close();
            return demand_letter;
        }

        private static DemandLetter read(DbDataReader reader)
        {
            DemandLetter demand_letter = new DemandLetter();
            int pos = 0;
            demand_letter.loan_id = readDBDecimal(reader, pos++);
            demand_letter.demand_letter_due_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.foreclosure_officers = readDBString(reader, pos++);
            demand_letter.fcl_type = readDBString(reader, pos++);
            demand_letter.last_reg_paid_dt = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.loan_type = readDBString(reader, pos++);
            demand_letter.papers_to_attorney_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.posting_date = readDBDateObject(reader, pos++, DateTime.Now);
            demand_letter.chapter_filed = readDBString(reader, pos++);
            demand_letter.bankruptcy_filed_date = readDBDateObject(reader, pos++, DateTime.Now);

            if (demand_letter.loan_id > 0)
            {
                demand_letter.loan = MsLoan.read(reader, pos);
            }
            return demand_letter;
        }
    }
}