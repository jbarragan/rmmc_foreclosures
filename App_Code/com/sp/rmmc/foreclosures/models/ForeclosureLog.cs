using System;
using System.Collections.Generic;
using System.Web;
using com.sp.rmmc.common.models;
using System.Data.Common;


namespace com.sp.rmmc.foreclosures.models
{
    public class ForeclosureLog : ForeclosuresCommonModel
    {
        public const string REMOVE_FROM_DEMAND_LETTER_BY_REGULAR_PAYMENT = "remove from demand letter by regular payment";
        public const string REMOVE_FROM_DEMAND_LETTER_BY_FORECLOSURE_ACTIVE = "remove from demand letter by foreclosure active";
        public const string REMOVE_FROM_DEMAND_LETTER_BY_BANKRUPTCY = "remove from demand letter by bankruptcy";
        public const string REMOVE_FROM_DEMAND_LETTER_BY_LOSS_MITIGATION = "remove from demand letter by loss mitigation";
        public const string IN_DEMAND_LETTER = "in demand letter period";
        public const string FORECLOSURE_COMPLETED_BY_SALE_DATE_COMPLETED = "foreclosure completed by sale date completed";
        public const string REMOVE_FROM_FORECLOSURE_BY_REGULAR_PAYMENT = "remove from foreclosure by regular payment";
        public const string REMOVE_FROM_FORECLOSURE_BY_LOSS_MITIGATION = "remove from foreclosure by loss mitigation";
        public const string REMOVE_FROM_FORECLOSURE_BY_BANKRUPTCY = "remove from foreclosure by bankruptcy";
        public const string REMOVE_FROM_FORECLOSURE_BY_DEED_IN_LIEU = "remove from foreclosure by deed_in_lieu";
        public const string IN_FORECLOSURE = "in foreclosure";

        public decimal loan_id = 0M;
        public DateTimeObject date = new DateTimeObject();
        public string location = "";

        private static string base_query = "" +
"select " +
"loan_id, " +
"date, " +
"location " +
"from foreclosure_logs ";

        private static string mssqlConnectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["foreclosure"].ConnectionString;

        public ForeclosureLog()
        {
        }

        public static string create(ForeclosureLog foreclosure_log)
        {
            string query = "insert into foreclosure_logs "
                                       + " (loan_id, date, location) "
                                       + " values(" + foreclosure_log.loan_id.ToString() + ", getdate(), '" + foreclosure_log.location + "')";
            ComlpetedChecklist.executeMsSQLStatement(mssqlConnectionString, query);
            return ComlpetedChecklist.error;
        }

        public static List<ForeclosureLog> get_loan_foreclosure_logs(decimal loan_id)
        {
            string query = base_query + " where loan_id = " + loan_id + " order by id asc";
            return get_foreclosure_logs(query);
        }

        public static List<ForeclosureLog> get_foreclosure_logs(String query)
        {
            List<ForeclosureLog> foreclosure_logs = new List<ForeclosureLog>();
            DbDataReader reader = getMsSQLReader(mssqlConnectionString, query);
            while (reader.Read())
            {
                ForeclosureLog foreclosure_log = new ForeclosureLog();
                foreclosure_log = read(reader);
                foreclosure_logs.Add(foreclosure_log);
            }
            reader.Close();
            closeMsSQLConnection();
            return foreclosure_logs;
        }

        public static ForeclosureLog get_loan_last_foreclosure_log(decimal loan_id)
        {
            string query = base_query + " where loan_id = " + loan_id + " order by id desc";
            ForeclosureLog foreclosure_log = new ForeclosureLog();
            DbDataReader reader = getMsSQLReader(mssqlConnectionString, query);
            if (reader.Read())
            {
                foreclosure_log = read(reader);
            }
            reader.Close();
            closeMsSQLConnection();
            return foreclosure_log;
        }

        private static ForeclosureLog read(DbDataReader reader)
        {
            ForeclosureLog foreclosure_log = new ForeclosureLog();
            int pos = 0;
            foreclosure_log.loan_id = readDBDecimal(reader, pos++);
            foreclosure_log.date = readDBDateObject(reader, pos++, DateTime.Now);
            foreclosure_log.location = readDBString(reader, pos++);
            return foreclosure_log;
        }
    }
}