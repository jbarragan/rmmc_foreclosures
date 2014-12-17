using System;
using System.Collections.Generic;
using System.Web;
using com.sp.rmmc.common.models;
using System.Data.Common;


namespace com.sp.rmmc.foreclosures.models
{
    public class ComlpetedChecklist : ForeclosuresCommonModel
    {

        public decimal loan_id = 0M;
        public DateTimeObject date = new DateTimeObject();

        private static string base_query = "" +
"select " +
"loan_id, " +
"date " +
"from completed_checklists ";

        private static string mssqlConnectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["foreclosure"].ConnectionString;

        public ComlpetedChecklist()
        {
        }

        public static string create(ComlpetedChecklist completed_checklist)
        {
            string query = "insert into completed_checklists "
                                       + " (loan_id, date) "
                                       + " values(" + completed_checklist.loan_id.ToString() + ", getdate())";
            ComlpetedChecklist.executeMsSQLStatement(mssqlConnectionString, query);
            return ComlpetedChecklist.error;
        }

        public static string delete(ComlpetedChecklist completed_checklist)
        {
            string query = "delete from completed_checklists where loan_id = " + completed_checklist.loan_id;
            ComlpetedChecklist.executeMsSQLStatement(mssqlConnectionString, query);
            return ComlpetedChecklist.error;
        }

        public static List<ComlpetedChecklist> get_demand_letters_completed_checklists(List<DemandLetter> demand_letters)
        {
            if (demand_letters.Count > 0)
            {
                string query = base_query + " where 1 = 1 and (";
                foreach (DemandLetter demand_letter in demand_letters)
                {
                    DateTimeObject minimal_completed_date = new DateTimeObject();
                    minimal_completed_date.set_date(demand_letter.demand_letter_due_date.date.AddDays(-30));
                    query += " (loan_id = " + demand_letter.loan_id + " and date >  " + minimal_completed_date.toDBValue() + ") OR ";
                }
                query += " (1 = 2))";
                return get_completed_checklists(query);
            }
            else
                return new List<ComlpetedChecklist>();
        }

        public static List<ComlpetedChecklist> getCompletedChecklists(List<BaseForeclosure> bfs)
        {
            if (bfs.Count > 0)
            {
                string query = base_query + " where 1 = 1 and (";
                foreach (BaseForeclosure bf in bfs)
                {
                    DateTimeObject minimal_completed_date = new DateTimeObject();
                    minimal_completed_date.set_date(bf.demand_letter_due_date.date.AddDays(-30));
                    query += " (loan_id = " + bf.loan_id + " and date >  " + minimal_completed_date.toDBValue() + ") OR ";
                }
                query += " (1 = 2))";
                return get_completed_checklists(query);
            }
            else
                return new List<ComlpetedChecklist>();
        }

        public static List<ComlpetedChecklist> get_foreclosure_completed_checklists(List<Foreclosure> foreclosures)
        {
            if (foreclosures.Count > 0)
            {
                string query = base_query + " where 1 = 1 and (";
                foreach (Foreclosure foreclosure in foreclosures)
                {
                    DateTimeObject minimal_completed_date = new DateTimeObject();
                    minimal_completed_date.set_date(foreclosure.demand_letter_due_date.date.AddDays(-30));
                    query += " (loan_id = " + foreclosure.loan_id + " and date >  " + minimal_completed_date.toDBValue() + ") OR ";
                }
                query += " (1 = 2))";
                return get_completed_checklists(query);
            }
            else
                return new List<ComlpetedChecklist>();
        }

        public static List<ComlpetedChecklist> get_completed_checklists(String query)
        {
            List<ComlpetedChecklist> completed_checklists = new List<ComlpetedChecklist>();
            DbDataReader reader = getMsSQLReader(mssqlConnectionString, query);
            while (reader.Read())
            {
                ComlpetedChecklist completed_checklist = new ComlpetedChecklist();
                completed_checklist = read(reader);
                completed_checklists.Add(completed_checklist);
            }
            reader.Close();
            closeMsSQLConnection();
            return completed_checklists;
        }

        public static ComlpetedChecklist get(DemandLetter demand_letter)
        {
            DateTimeObject minimal_completed_date = new DateTimeObject();
            minimal_completed_date.set_date(demand_letter.demand_letter_due_date.date.AddDays(-30));
            string query = base_query + " where loan_id = " + demand_letter.loan_id + " and date >  " + minimal_completed_date.toDBValue();
            ComlpetedChecklist completed_checklist = new ComlpetedChecklist();
            DbDataReader reader = getMsSQLReader(mssqlConnectionString, query); 
            if (reader.Read())
            {
                completed_checklist = read(reader);
            }
            reader.Close();
            closeMsSQLConnection();
            return completed_checklist;
        }

        private static ComlpetedChecklist read(DbDataReader reader)
        {
            ComlpetedChecklist completed_checklist = new ComlpetedChecklist();
            int pos = 0;
            completed_checklist.loan_id = readDBDecimal(reader, pos++);
            completed_checklist.date = readDBDateObject(reader, pos++, DateTime.Now);
            return completed_checklist;
        }

        public bool is_completed()
        {
            return this.loan_id > 0M;
        }


    }
}