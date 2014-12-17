using System;
using System.Collections.Generic;
using System.Web;
using com.sp.rmmc.common.models;
using System.Data.Common;


namespace com.sp.rmmc.foreclosures.models
{
    public class LMReception : ForeclosuresCommonModel
    {

        public decimal loan_id = 0M;
        public DateTimeObject date = new DateTimeObject();

        private static string base_query = "" +
"select " +
"loan_id, " +
"received " +
"from receptions ";

        private static string base_query2 = "" +
"select " +
"receptions.loan_id, " +
"receptions.received " +
"from receptions, status " + 
"where 1 = 1 and  " + 
"receptions.loan_id = status.loan_id   " + 
" and incomplete_denied > receptions.received ";

        private static string mssqlConnectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["loss_mitigation"].ConnectionString;

        public LMReception()
        {
        }

        public static List<LMReception> getLMReceptions(List<BaseForeclosure> foreclosures)
        {
            if (foreclosures.Count > 0)
            {
                string query = base_query2 + " and 1 = 1 and (";
                foreach (BaseForeclosure f in foreclosures)
                {
                    DateTimeObject minimal_completed_date = new DateTimeObject();
                    minimal_completed_date.set_date(f.demand_letter_due_date.date.AddDays(-30));
                    query += loan_in_lm_query2(f.loan_id, minimal_completed_date);
                }
                query += " (1 = 2))";
                return get_lm_receptions(query);
            }
            else
                return new List<LMReception>();
        }

        public static List<LMReception> get_demand_letters_lm_receptions(List<DemandLetter> demand_letters)
        {
            if (demand_letters.Count > 0)
            {
                string query = base_query + " where 1 = 1 and (";
                foreach (DemandLetter demand_letter in demand_letters)
                {
                    DateTimeObject minimal_completed_date = new DateTimeObject();
                    minimal_completed_date.set_date(demand_letter.demand_letter_due_date.date.AddDays(-30));
                    query += loan_in_lm_query(demand_letter.loan_id, minimal_completed_date);
                }
                query += " (1 = 2))";
                return get_lm_receptions(query);
            }
            else
                return new List<LMReception>();
        }

        public static List<LMReception> get_foreclosure_lm_receptions(List<Foreclosure> foreclosures)
        {
            if (foreclosures.Count > 0)
            {
                string query = base_query + " where 1 = 1 and (";
                foreach (Foreclosure foreclosure in foreclosures)
                {
                    DateTimeObject minimal_completed_date = new DateTimeObject();
                    minimal_completed_date.set_date(foreclosure.demand_letter_due_date.date.AddDays(-30));
                    query += loan_in_lm_query(foreclosure.loan_id, minimal_completed_date);
                }
                query += " (1 = 2))";
                return get_lm_receptions(query);
            }
            else
                return new List<LMReception>();
        }

        public static string loan_in_lm_query(decimal loan_id, DateTimeObject completed_date)
        {
            string query = " (loan_id = " + loan_id + " and received >  " + completed_date.toDBValue() + " AND ";
            query += "(select top 1 incomplete_denied from status where receptions.loan_id = status.loan_id AND incomplete_denied > receptions.received) is null ";
            query += ") OR ";
            return query;
        }

        public static string loan_in_lm_query2(decimal loan_id, DateTimeObject completed_date)
        {
            string query = " (receptions.loan_id = " + loan_id + " and received >  " + completed_date.toDBValue();
            query += ") OR ";
            return query;
        }

        public static List<LMReception> get_lm_receptions(String query)
        {
            List<LMReception> lm_receptions = new List<LMReception>();
            DbDataReader reader = getMsSQLReader(mssqlConnectionString, query);
            while (reader.Read())
            {
                LMReception lm_reception = new LMReception();
                lm_reception = read(reader);
                lm_receptions.Add(lm_reception);
            }
            reader.Close();
            closeMsSQLConnection();
            return lm_receptions;
        }

        private static LMReception read(DbDataReader reader)
        {
            LMReception lm_reception = new LMReception();
            int pos = 0;
            lm_reception.loan_id = readDBDecimal(reader, pos++);
            lm_reception.date = readDBDateObject(reader, pos++, DateTime.Now);
            return lm_reception;
        }

        public bool was_received()
        {
            return this.loan_id > 0M;
        }


    }
}