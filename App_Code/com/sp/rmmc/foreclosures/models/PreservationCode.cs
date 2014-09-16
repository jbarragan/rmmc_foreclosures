using System;
using System.Collections.Generic;
using System.Web;
using com.sp.rmmc.common.models;
using System.Data.Common;


namespace com.sp.rmmc.foreclosures.models
{
    public class PreservationCode : ForeclosuresCommonModel
    {
        
        public string code = "";
        public string description = "";
        public decimal allowance = 0M;
        public string type = "";
        public string general_service = "";
        public bool enabled = true;

        private static string base_query = "" +
"select " +
"code, " +
"description, " +
"allowance, " +
"type, " +
"general_service, " +
"enabled " +
"from preservation_codes ";

        private static string mssqlConnectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["foreclosure"].ConnectionString;

        public PreservationCode()
        {
        }

        public static string create(PreservationCode preservation_code)
        {
            string query = "insert into preservation_codes "
                                    + " (code, description, allowance, type, general_service, enabled) "
                              + " values("
                              + toDBString(preservation_code.code) + ", "
                              + toDBString(preservation_code.description) + ", "
                              + preservation_code.allowance + ", "
                              + toDBString(preservation_code.type) + ", "
                              + toDBString(preservation_code.general_service) + ", "
                              + (preservation_code.enabled ? "1" : "0")
                              + ")";
            PreservationCode.executeMsSQLStatement(mssqlConnectionString, query);
            return PreservationCode.error;
        }

        public static string update(string code, PreservationCode preservation_code)
        {
            string query = "update preservation_codes "
                              + " set code = " + toDBString(preservation_code.code) + ", " 
                              +     " description = " + toDBString(preservation_code.description) + ", " 
                              +     " allowance = " + preservation_code.allowance +  ", "
                              +     " type = " + toDBString(preservation_code.type) + ", " 
                              +     " general_service = " + toDBString(preservation_code.general_service) + ", " 
                              +     " enabled = " + (preservation_code.enabled ? "1": "0") + " " 
                              + " where code = " + toDBString(code);
            PreservationCode.executeMsSQLStatement(mssqlConnectionString, query);
            return PreservationCode.error;
        }

        public static string delete(PreservationCode preservation_code)
        {
            string query = "delete from preservation_codes where code = " + toDBString(preservation_code.code);
            PreservationCode.executeMsSQLStatement(mssqlConnectionString, query);
            return PreservationCode.error;
        }

        public static List<PreservationCode> get_all_preservation_codes()
        {
            string query = base_query + " order by type, general_service, code";
            return get_preservation_codes(query);
        }

        public static List<PreservationCode> get_available_preservation_codes(string loan_type)
        {
            string query = base_query + " where type like '" + loan_type + "' and enabled = 1 order by code";
            return get_preservation_codes(query);
        }

        public static List<PreservationCode> get_preservation_codes(string query)
        {
            List<PreservationCode> preservation_codes = new List<PreservationCode>();
            DbDataReader reader = getMsSQLReader(mssqlConnectionString, query);
            while (reader.Read())
            {
                PreservationCode preservation_code = new PreservationCode();
                preservation_code = read(reader);
                preservation_codes.Add(preservation_code);
            }
            reader.Close();
            closeMsSQLConnection();
            return preservation_codes;
        }

        public static PreservationCode get(string code)
        {
            string query = base_query + " where code = " + toDBString(code);
            PreservationCode preservation_code = new PreservationCode();
            DbDataReader reader = getMsSQLReader(mssqlConnectionString, query); 
            if (reader.Read())
            {
                preservation_code = read(reader);
            }
            reader.Close();
            closeMsSQLConnection();
            return preservation_code;
        }

        private static PreservationCode read(DbDataReader reader)
        {
            PreservationCode preservation_code = new PreservationCode();
            int pos = 0;
            preservation_code.code = readDBString(reader, pos++);
            preservation_code.description = readDBString(reader, pos++);
            preservation_code.allowance = readDBDecimal(reader, pos++);
            preservation_code.type = readDBString(reader, pos++);
            preservation_code.general_service = readDBString(reader, pos++);
            preservation_code.enabled = readDBYesNo(reader, pos++);
            return preservation_code;
        }

    }
}