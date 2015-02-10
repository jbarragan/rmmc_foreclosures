using System;
using System.Collections.Generic;
using System.Web;

using System.Data.OleDb;
using System.Data.Common;
using System.Data.SqlClient;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.foreclosures.models
{
    public class ReviewChecklist : ForeclosuresCommonModel
    {
        public decimal loan_id = 0M;
        public string pi_subject = "";
        public DateTimeObject pi_create_dt = new DateTimeObject();
        public string pi_text = ""; // 52
        public string bc_first_right_party_contact_text = ""; // 83
        public string bc_promised_to_pay_text = ""; // 134
        public string pi_initial_vacant_inspection_text = ""; // 54
        public string bc_reason_for_deliquency_text = ""; // 82,83,84,85,86,87,88,89,90,91,92,93,94,122,123,125,134
        public string default_reason_code = ""; //select default_reason_code from ms_credit_information
        public DateTimeObject last_taxes_paid_due_date = new DateTimeObject();
        public DateTimeObject last_insurance_paid_due_date = new DateTimeObject();
        public DateTimeObject last_taxes_paid_date = new DateTimeObject();
        public DateTimeObject last_insurance_paid_date = new DateTimeObject();
        public string corr_name = ""; // ms_address_information_addl
        public string corr_address2 = ""; // ms_address_information_addl
        public string corr_address1 = ""; // ms_address_information_addl
        public string corr_city = ""; // ms_address_information_addl
        public string corr_state = ""; // ms_address_information_addl
        public string corr_zip = ""; // ms_address_information_addl
        public string fc_other_fc_action_text = ""; // 70,'FC-Other FC Action'

        public DateTimeObject pi_dt = new DateTimeObject();
        public DateTimeObject bc_first_right_party_contact_dt = new DateTimeObject();
        public DateTimeObject bc_promised_to_pay_dt = new DateTimeObject();
        public DateTimeObject pi_initial_vacant_inspection_dt = new DateTimeObject();
        public DateTimeObject bc_reason_for_deliquency_dt = new DateTimeObject();
        public DateTimeObject fc_other_fc_action_dt = new DateTimeObject();



        private static string base_query = @"
select base.loan_id,
    (select top 1 memo.memo_subject from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_category_id = 19 order by memo_id desc) as pi_subject,
    (select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_category_id = 19 order by memo_id desc) as pi_create_dt,  
    (select top 1 memo.memo_text from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_category_id = 19 order by memo_id desc) as pi_text,  
    (select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_category_id = 19 order by memo_id desc) as pi_dt,
    (select top 1 memo.memo_text from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id = 83 order by memo_id desc) as bc_first_right_party_contact_text,
    (select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id = 83 order by memo_id desc) as bc_first_right_party_contact_dt, 
    (select top 1 memo.memo_text from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id = 134 order by memo_id desc) as bc_promised_to_pay_text,   
    (select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id = 134 order by memo_id desc) as bc_promised_to_pay_dt,
    (select top 1 memo.memo_text from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id = 54 order by memo_id desc) as pi_initial_vacant_inspection_text,
    (select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id = 54 order by memo_id desc) as pi_initial_vacant_inspection_dt,  
    (select top 1 memo.memo_text from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id IN (82,83,84,85,86,87,88,89,90,91,92,93,94,122,123,125,134) order by memo_id desc) as bc_reason_for_deliquency_text,   
    (select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id IN (82,83,84,85,86,87,88,89,90,91,92,93,94,122,123,125,134) order by memo_id desc) as bc_reason_for_deliquency_dt,  
    (select top 1 credit.default_reason_code from ms_credit_information credit where credit.loan_id = base.loan_id order by credit.default_reason_code desc) as default_reason_code,
    (select top 1 history.due_dt from ms_loan_history history where history.loan_id = base.loan_id and history.trans_desc like 'T&I Dis' and history.ba_type_id = 22 order by history.due_dt desc) as last_taxes_paid_due_date,
    (select top 1 history.due_dt from ms_loan_history history where history.loan_id = base.loan_id and history.trans_desc like 'T&I Dis' and history.ba_type_id != 22 order by history.due_dt desc) as last_insurance_paid_due_date,
    (select top 1 history.paid_dt from ms_loan_history history where history.loan_id = base.loan_id and history.trans_desc like 'T&I Dis' and history.ba_type_id = 22 order by history.paid_dt desc) as last_taxes_paid_date,
    (select top 1 history.paid_dt from ms_loan_history history where history.loan_id = base.loan_id and history.trans_desc like 'T&I Dis' and history.ba_type_id != 22 order by history.paid_dt desc) as last_insurance_paid_date, 
    (select top 1 corr.corr_name from ms_address_information_addl corr where corr.loan_id = base.loan_id order by corr.corr_id desc) as corr_name,
    (select top 1 corr.corr_address_2 from ms_address_information_addl corr where corr.loan_id = base.loan_id order by corr.corr_id desc) as corr_address2,
    (select top 1 corr.corr_address_1 from ms_address_information_addl corr where corr.loan_id = base.loan_id order by corr.corr_id desc) as corr_address1,
    (select top 1 corr.corr_city from ms_address_information_addl corr where corr.loan_id = base.loan_id order by corr.corr_id desc) as corr_city,
    (select top 1 corr.corr_state from ms_address_information_addl corr where corr.loan_id = base.loan_id order by corr.corr_id desc) as corr_state,
    (select top 1 corr.corr_zip from ms_address_information_addl corr where corr.loan_id = base.loan_id order by corr.corr_id desc) as corr_zip,
    (select top 1 memo.memo_text from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id = 70 order by memo_id desc) as fc_other_fc_action_text,
    (select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id = 70 order by memo_id desc) as fc_other_fc_action_dt
from ms_loan_information base
";
        public ReviewChecklist()
        {
        }

        public static ReviewChecklist getReviewChecklist(decimal loan_id)
        {
            ReviewChecklist review_checklist = new ReviewChecklist();
            string query = base_query + " and base.loan_id = " + loan_id.ToString();
            OleDbDataReader reader = getReader(query);
            if (reader.Read())
            {
                review_checklist = read(reader, 0);
            }
            close_connection();
            return review_checklist;
        }

        public static ReviewChecklist read(DbDataReader reader, int pos)
        {
            ReviewChecklist review_checklist = new ReviewChecklist();
            review_checklist.loan_id = readDBDecimal(reader, pos++);
            review_checklist.pi_subject = readDBString(reader, pos++);
            review_checklist.pi_create_dt = readDBDateObject(reader, pos++, DateTime.Now);
            review_checklist.pi_text = readDBString(reader, pos++);
            review_checklist.pi_dt = readDBDateObject(reader, pos++, DateTime.Now);
            review_checklist.bc_first_right_party_contact_text = readDBString(reader, pos++);
            review_checklist.bc_first_right_party_contact_dt = readDBDateObject(reader, pos++, DateTime.Now);
            review_checklist.bc_promised_to_pay_text = readDBString(reader, pos++);
            review_checklist.bc_promised_to_pay_dt = readDBDateObject(reader, pos++, DateTime.Now);
            review_checklist.pi_initial_vacant_inspection_text = readDBString(reader, pos++);
            review_checklist.pi_initial_vacant_inspection_dt = readDBDateObject(reader, pos++, DateTime.Now);
            review_checklist.bc_reason_for_deliquency_text = readDBString(reader, pos++);
            review_checklist.bc_reason_for_deliquency_dt = readDBDateObject(reader, pos++, DateTime.Now);
            review_checklist.default_reason_code = readDBString(reader, pos++);
            review_checklist.last_taxes_paid_due_date = readDBDateObject(reader, pos++, DateTime.Now);
            review_checklist.last_insurance_paid_due_date = readDBDateObject(reader, pos++, DateTime.Now);
            review_checklist.last_taxes_paid_date = readDBDateObject(reader, pos++, DateTime.Now);
            review_checklist.last_insurance_paid_date = readDBDateObject(reader, pos++, DateTime.Now);
            review_checklist.corr_name = readDBString(reader, pos++);
            review_checklist.corr_address2 = readDBString(reader, pos++);
            review_checklist.corr_address1 = readDBString(reader, pos++);
            review_checklist.corr_city = readDBString(reader, pos++);
            review_checklist.corr_state = readDBString(reader, pos++);
            review_checklist.corr_zip = readDBString(reader, pos++);
            review_checklist.fc_other_fc_action_text = readDBString(reader, pos++);
            review_checklist.fc_other_fc_action_dt = readDBDateObject(reader, pos++, DateTime.Now);
            return review_checklist;
        }

        public static string columns(string loan_id_column)
        {
            string s = "" +
            " (select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.loan_id  ) as review_checklist_loan_id, " +
            " (select top 1 memo.memo_subject from ms_loan_memo memo where memo.loan_id = " + loan_id_column + " and memo.memo_category_id = 19 and dateadd(dd, 45, memo.memo_create_dt) > getdate()  order by memo_id desc) as review_checklist_pi_subject, " +
            " (select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = " + loan_id_column + " and memo.memo_category_id = 19 and dateadd(dd, 45, memo.memo_create_dt) > getdate() order by memo_id desc) as review_checklist_pi_create_dt, " +
            " (select top 1 memo.memo_text from ms_loan_memo memo where memo.loan_id = " + loan_id_column + " and memo.memo_category_id = 19 and dateadd(dd, 45, memo.memo_create_dt) > getdate() order by memo_id desc) as review_checklist_pi_text, " +
            " (select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = " + loan_id_column + " and memo.memo_category_id = 19 and dateadd(dd, 45, memo.memo_create_dt) > getdate() order by memo_id desc) as review_checklist_pi_dt, " +
            " (select top 1 memo.memo_text from ms_loan_memo memo where memo.loan_id = " + loan_id_column + " and memo.memo_type_id = 83 and dateadd(dd, 45, memo.memo_create_dt) > getdate() order by memo_id desc) as review_checklist_bc_first_right_party_contact_text, " +
            " (select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = " + loan_id_column + " and memo.memo_type_id = 83 and dateadd(dd, 45, memo.memo_create_dt) > getdate() order by memo_id desc) as review_checklist_bc_first_right_party_contact_dt, " +
            " (select top 1 memo.memo_text from ms_loan_memo memo where memo.loan_id = " + loan_id_column + " and memo.memo_type_id = 134 and dateadd(dd, 45, memo.memo_create_dt) > getdate() order by memo_id desc) as review_checklist_bc_promised_to_pay_text, " +
            " (select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = " + loan_id_column + " and memo.memo_type_id = 134 and dateadd(dd, 45, memo.memo_create_dt) > getdate() order by memo_id desc) as review_checklist_bc_promised_to_pay_dt, " +
            " (select top 1 memo.memo_text from ms_loan_memo memo where memo.loan_id = " + loan_id_column + " and memo.memo_type_id = 54 and dateadd(dd, 45, memo.memo_create_dt) > getdate() order by memo_id desc) as review_checklist_pi_initial_vacant_inspection_text, " +
            " (select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = " + loan_id_column + " and memo.memo_type_id = 54 and dateadd(dd, 45, memo.memo_create_dt) > getdate() order by memo_id desc) as review_checklist_pi_initial_vacant_inspection_dt, " +
            " (select top 1 memo.memo_text from ms_loan_memo memo where memo.loan_id = " + loan_id_column + " and memo.memo_type_id IN (82,83,84,85,86,87,88,89,90,91,92,93,94,122,123,125,134) and dateadd(dd, 45, memo.memo_create_dt) > getdate() order by memo_id desc) as review_checklist_bc_reason_for_deliquency_text, " +
            " (select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = " + loan_id_column + " and memo.memo_type_id IN (82,83,84,85,86,87,88,89,90,91,92,93,94,122,123,125,134) and dateadd(dd, 45, memo.memo_create_dt) > getdate() order by memo_id desc) as review_checklist_bc_reason_for_deliquency_dt, " +
            " (select top 1 credit.default_reason_code from ms_credit_information credit where credit.loan_id = " + loan_id_column + " order by credit.default_reason_code desc) as review_checklist_default_reason_code, " +
            " (select top 1 history.due_dt from ms_loan_history history where history.loan_id = " + loan_id_column + " and history.trans_desc like 'T&I Dis' and history.ba_type_id = 22 order by history.due_dt desc) as review_checklist_last_taxes_paid_due_date, " +
            " (select top 1 history.due_dt from ms_loan_history history where history.loan_id = " + loan_id_column + " and history.trans_desc like 'T&I Dis' and history.ba_type_id != 22 order by history.due_dt desc) as review_checklist_last_insurance_paid_due_date, " +
            " (select top 1 history.paid_dt from ms_loan_history history where history.loan_id = " + loan_id_column + " and history.trans_desc like 'T&I Dis' and history.ba_type_id = 22 order by history.paid_dt desc) as review_checklist_last_taxes_paid_date, " +
            " (select top 1 history.paid_dt from ms_loan_history history where history.loan_id = " + loan_id_column + " and history.trans_desc like 'T&I Dis' and history.ba_type_id != 22 order by history.paid_dt desc) as review_checklist_last_insurance_paid_date, " +
            " (select top 1 corr.corr_name from ms_address_information_addl corr where corr.loan_id = " + loan_id_column + " order by corr.corr_id desc) as review_checklist_corr_name, " +
            " (select top 1 corr.corr_address_2 from ms_address_information_addl corr where corr.loan_id = " + loan_id_column + " order by corr.corr_id desc) as review_checklist_corr_address2, " +
            " (select top 1 corr.corr_address_1 from ms_address_information_addl corr where corr.loan_id = " + loan_id_column + " order by corr.corr_id desc) as review_checklist_corr_address1, " +
            " (select top 1 corr.corr_city from ms_address_information_addl corr where corr.loan_id = " + loan_id_column + " order by corr.corr_id desc) as review_checklist_corr_city, " +
            " (select top 1 corr.corr_state from ms_address_information_addl corr where corr.loan_id = " + loan_id_column + " order by corr.corr_id desc) as review_checklist_corr_state, " +
            " (select top 1 corr.corr_zip from ms_address_information_addl corr where corr.loan_id = " + loan_id_column + " order by corr.corr_id desc) as review_checklist_corr_zip, " +
            " (select top 1 memo.memo_text from ms_loan_memo memo where memo.loan_id = " + loan_id_column + " and memo.memo_type_id = 70 and dateadd(dd, 45, memo.memo_create_dt) > getdate() order by memo_id desc) as review_checklist_fc_other_fc_action_text, " +
            " (select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = " + loan_id_column + " and memo.memo_type_id = 70 and dateadd(dd, 45, memo.memo_create_dt) > getdate() order by memo_id desc) as review_checklist_fc_other_fc_action_dt ";
            return s;
        }

        public static List<string> columns_list()
        {
            List<string> columns = new List<string>();
            string[] common_columns = ReviewChecklist.columns("").Split(new string[] { " as " }, StringSplitOptions.None);
            for (int i = 1; i < common_columns.Length; i++)
            {
                columns.Add(common_columns[i].Split(',')[0]);
            }
            return columns;
        }

        public static string columns_name_query()
        {
            return " " + String.Join(", ", ReviewChecklist.columns_list().ToArray());
        }

        public void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@review_checklist_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@review_checklist_pi_subject", this.pi_subject);
            cmd.Parameters.AddWithValue("@review_checklist_pi_create_dt", this.pi_create_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@review_checklist_pi_text", this.pi_text);
            cmd.Parameters.AddWithValue("@review_checklist_pi_dt", this.pi_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@review_checklist_bc_first_right_party_contact_text", this.bc_first_right_party_contact_text);
            cmd.Parameters.AddWithValue("@review_checklist_bc_first_right_party_contact_dt", this.bc_first_right_party_contact_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@review_checklist_bc_promised_to_pay_text", this.bc_promised_to_pay_text);
            cmd.Parameters.AddWithValue("@review_checklist_bc_promised_to_pay_dt", this.bc_promised_to_pay_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@review_checklist_pi_initial_vacant_inspection_text", this.pi_initial_vacant_inspection_text);
            cmd.Parameters.AddWithValue("@review_checklist_pi_initial_vacant_inspection_dt", this.pi_initial_vacant_inspection_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@review_checklist_bc_reason_for_deliquency_text", this.bc_reason_for_deliquency_text);
            cmd.Parameters.AddWithValue("@review_checklist_bc_reason_for_deliquency_dt", this.bc_reason_for_deliquency_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@review_checklist_default_reason_code", this.default_reason_code);
            cmd.Parameters.AddWithValue("@review_checklist_last_taxes_paid_due_date", this.last_taxes_paid_due_date.toDBInsert());
            cmd.Parameters.AddWithValue("@review_checklist_last_insurance_paid_due_date", this.last_insurance_paid_due_date.toDBInsert());
            cmd.Parameters.AddWithValue("@review_checklist_last_taxes_paid_date", this.last_taxes_paid_date.toDBInsert());
            cmd.Parameters.AddWithValue("@review_checklist_last_insurance_paid_date", this.last_insurance_paid_date.toDBInsert());
            cmd.Parameters.AddWithValue("@review_checklist_corr_name", this.corr_name);
            cmd.Parameters.AddWithValue("@review_checklist_corr_address2", this.corr_address2);
            cmd.Parameters.AddWithValue("@review_checklist_corr_address1", this.corr_address1);
            cmd.Parameters.AddWithValue("@review_checklist_corr_city", this.corr_city);
            cmd.Parameters.AddWithValue("@review_checklist_corr_state", this.corr_state);
            cmd.Parameters.AddWithValue("@review_checklist_corr_zip ", this.corr_zip);
            cmd.Parameters.AddWithValue("@review_checklist_fc_other_fc_action_text", this.fc_other_fc_action_text);
            cmd.Parameters.AddWithValue("@review_checklist_fc_other_fc_action_dt", this.fc_other_fc_action_dt.toDBInsert());
        }
    }
}