using System;
using System.Collections.Generic;
using System.Web;
using com.sp.rmmc.common.models;
using System.Data.Common;


namespace com.sp.rmmc.foreclosures.models
{
    public class PreforeclosureEvent : ForeclosuresCommonModel
    {

        public decimal loan_id = 0M;
        public bool is_copy_demand_letters = false;
        public DateTimeObject copy_demand_letters_date = new DateTimeObject();
        public bool is_copy_notes = false;
        public DateTimeObject copy_notes_date = new DateTimeObject();
        public bool is_copy_deed_of_trust_notes = false;
        public DateTimeObject copy_deed_of_trust_date = new DateTimeObject();
        public bool is_copy_bankruptcy= false;
        public DateTimeObject copy_bankruptcy_date = new DateTimeObject();
        public bool is_copy_title_policy= false;
        public DateTimeObject copy_title_policy_date = new DateTimeObject();
        public bool is_notify_appraisal= false;
        public DateTimeObject notify_appraisal_date = new DateTimeObject();

        private static string base_query = "" +
"select " +
"loan_id, " +
"is_copy_demand_letters, " +
"copy_demand_letters_date, " +
"is_copy_notes, " +
"copy_notes_date, " +
"is_copy_deed_of_trust_notes, " +
"copy_deed_of_trust_date, " +
"is_copy_bankruptcy, " +
"copy_bankruptcy_date, " +
"is_copy_title_policy, " +
"copy_title_policy_date, " +
"is_notify_appraisal, " +
"notify_appraisal_date " +
"from preforeclosure_events ";

        private static string mssqlConnectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["foreclosure"].ConnectionString;

        public PreforeclosureEvent()
        {
        }

        public static string create(PreforeclosureEvent preforeclosure_event)
        {
            string query = "insert into preforeclosure_events "
                                       + " (loan_id, is_copy_demand_letters, copy_demand_letters_date, is_copy_notes, copy_notes_date, is_copy_deed_of_trust_notes, copy_deed_of_trust_date, is_copy_bankruptcy, copy_bankruptcy_date, is_copy_title_policy, copy_title_policy_date, is_notify_appraisal, notify_appraisal_date) "
                                       + " values(" + preforeclosure_event.loan_id.ToString() + ", "
                                       + ((preforeclosure_event.is_copy_demand_letters) ? "1" : "0") + ", "
                                       + db_date_update_string(false, preforeclosure_event.is_copy_demand_letters, preforeclosure_event.copy_demand_letters_date) + ", "
                                       + ((preforeclosure_event.is_copy_notes) ? "1" : "0") + ", "
                                       + db_date_update_string(false, preforeclosure_event.is_copy_notes, preforeclosure_event.copy_notes_date) + ", "
                                       + ((preforeclosure_event.is_copy_deed_of_trust_notes) ? "1" : "0") + ", "
                                       + db_date_update_string(false, preforeclosure_event.is_copy_deed_of_trust_notes, preforeclosure_event.copy_deed_of_trust_date) + ", "
                                       + ((preforeclosure_event.is_copy_bankruptcy) ? "1" : "0") + ", "
                                       + db_date_update_string(false, preforeclosure_event.is_copy_bankruptcy, preforeclosure_event.copy_bankruptcy_date) + ", "
                                       + ((preforeclosure_event.is_copy_title_policy) ? "1" : "0") + ", "
                                       + db_date_update_string(false, preforeclosure_event.is_copy_title_policy, preforeclosure_event.copy_title_policy_date) + ", "
                                       + ((preforeclosure_event.is_notify_appraisal) ? "1" : "0") + ", "
                                       + db_date_update_string(false, preforeclosure_event.is_notify_appraisal, preforeclosure_event.notify_appraisal_date) + " " 
                                       + ")";
            PreforeclosureEvent.executeMsSQLStatement(mssqlConnectionString, query);
            return PreforeclosureEvent.error;
        }

        public static string delete(PreforeclosureEvent preforeclosure_event)
        {
            string query = "delete from preforeclosure_events where loan_id = " + preforeclosure_event.loan_id;
            PreforeclosureEvent.executeMsSQLStatement(mssqlConnectionString, query);
            return PreforeclosureEvent.error;
        }

        public static string update(PreforeclosureEvent preforeclosure_event, PreforeclosureEvent original)
        {
            string query = "update preforeclosure_events set " +
                           "is_copy_demand_letters = " + ((preforeclosure_event.is_copy_demand_letters) ? "1" : "0") + ", " +
                           "copy_demand_letters_date = " + db_date_update_string(original.is_copy_demand_letters, preforeclosure_event.is_copy_demand_letters, preforeclosure_event.copy_demand_letters_date) + ", " +
                           "is_copy_notes = " + ((preforeclosure_event.is_copy_notes) ? "1" : "0") + ", " +
                           "copy_notes_date = " + db_date_update_string(original.is_copy_notes, preforeclosure_event.is_copy_notes, preforeclosure_event.copy_notes_date) + ", " +
                           "is_copy_deed_of_trust_notes = " + ((preforeclosure_event.is_copy_deed_of_trust_notes) ? "1" : "0") + ", " +
                           "copy_deed_of_trust_date = " + db_date_update_string(original.is_copy_deed_of_trust_notes, preforeclosure_event.is_copy_deed_of_trust_notes, preforeclosure_event.copy_deed_of_trust_date) + ", " +
                           "is_copy_bankruptcy = " + ((preforeclosure_event.is_copy_bankruptcy) ? "1" : "0") + ", " +
                           "copy_bankruptcy_date = " + db_date_update_string(original.is_copy_bankruptcy, preforeclosure_event.is_copy_bankruptcy, preforeclosure_event.copy_bankruptcy_date) + ", " +
                           "is_copy_title_policy = " + ((preforeclosure_event.is_copy_title_policy) ? "1" : "0") + ", " +
                           "copy_title_policy_date = " + db_date_update_string(original.is_copy_title_policy, preforeclosure_event.is_copy_title_policy, preforeclosure_event.copy_title_policy_date) + ", " +
                           "is_notify_appraisal = " + ((preforeclosure_event.is_notify_appraisal) ? "1" : "0") + ", " +
                           "notify_appraisal_date = " + db_date_update_string(original.is_notify_appraisal, preforeclosure_event.is_notify_appraisal, preforeclosure_event.notify_appraisal_date) + " " +
                           " where loan_id = " + preforeclosure_event.loan_id;
            PreforeclosureEvent.executeMsSQLStatement(mssqlConnectionString, query);
            return PreforeclosureEvent.error;
        }

        public static PreforeclosureEvent get(Foreclosure foreclosure)
        {
            string query = base_query + " where loan_id = " + foreclosure.loan_id.ToString();
            PreforeclosureEvent preforeclosure_event = new PreforeclosureEvent();
            DbDataReader reader = getMsSQLReader(mssqlConnectionString, query);
            if (reader.Read())
            {
                preforeclosure_event = read(reader);
            }
            reader.Close();
            closeMsSQLConnection();
            return preforeclosure_event;
        }

        public static PreforeclosureEvent getFromBaseForeclosure(BaseForeclosure foreclosure)
        {
            string query = base_query + " where loan_id = " + foreclosure.loan_id.ToString();
            PreforeclosureEvent preforeclosure_event = new PreforeclosureEvent();
            DbDataReader reader = getMsSQLReader(mssqlConnectionString, query);
            if (reader.Read())
            {
                preforeclosure_event = read(reader);
            }
            reader.Close();
            closeMsSQLConnection();
            return preforeclosure_event;
        }

        private static PreforeclosureEvent read(DbDataReader reader)
        {
            PreforeclosureEvent preforeclosure_event = new PreforeclosureEvent();
            int pos = 0;
            preforeclosure_event.loan_id = readDBDecimal(reader, pos++);
            preforeclosure_event.is_copy_demand_letters = readDBYesNo(reader, pos++);
            preforeclosure_event.copy_demand_letters_date = readDBDateObject(reader, pos++, DateTime.Now);
            preforeclosure_event.is_copy_notes = readDBYesNo(reader, pos++);
            preforeclosure_event.copy_notes_date = readDBDateObject(reader, pos++, DateTime.Now);
            preforeclosure_event.is_copy_deed_of_trust_notes = readDBYesNo(reader, pos++);
            preforeclosure_event.copy_deed_of_trust_date = readDBDateObject(reader, pos++, DateTime.Now);
            preforeclosure_event.is_copy_bankruptcy = readDBYesNo(reader, pos++);
            preforeclosure_event.copy_bankruptcy_date = readDBDateObject(reader, pos++, DateTime.Now);
            preforeclosure_event.is_copy_title_policy = readDBYesNo(reader, pos++);
            preforeclosure_event.copy_title_policy_date = readDBDateObject(reader, pos++, DateTime.Now);
            preforeclosure_event.is_notify_appraisal = readDBYesNo(reader, pos++);
            preforeclosure_event.notify_appraisal_date = readDBDateObject(reader, pos++, DateTime.Now);
            return preforeclosure_event;
        }

        private static string db_date_update_string(bool original, bool changed, DateTimeObject changed_date){
            if (original != changed) return "getdate()";
            else return changed_date.toDBValue();
        }
    }
}