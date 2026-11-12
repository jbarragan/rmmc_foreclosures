using System;
using System.Collections.Generic;
using System.Web;
using com.sp.rmmc.common.models;
using com.sp.rmmc.common.tools;
using System.Data.Common;


namespace com.sp.rmmc.foreclosures.models
{
    public class PreservationCode : ForeclosuresCommonModel, ICSVExport
    {
        public int id = 0;
        public string code = "";
        public string description = "";
        public decimal allowance = 0M;
        public DateTimeObject effective_date = new DateTimeObject();
        public string type = "";
        public string general_service = "";
        public bool enabled = true;

        public string validation_error = "";

        public List<PreservationCodeAllowance> allowances = new List<PreservationCodeAllowance>();

        private static string base_query = "" +
"select " +
"id, " +
"code, " +
"description, " +
"(select top 1 pca.allowance from preservation_code_allowances pca where pca.preservation_code_id = pc.id order by effective_date desc ) as allowance, " +
"(select top 1 pca.effective_date from preservation_code_allowances pca where pca.preservation_code_id = pc.id order by effective_date desc ) as effective_date, " +
"type, " +
"general_service, " +
"enabled " +
"from preservation_codes pc ";

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
            preservation_code.id = PreservationCode.executeMsSQLStatement(mssqlConnectionString, query);
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

        public static List<PreservationCode> get_all_preservation_codes_by_order(string order_by)
        {
            //order by code
            string query = base_query + " " + order_by;
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
            preservation_code.id = readDBInt32(reader, pos++);
            preservation_code.code = readDBString(reader, pos++);
            preservation_code.description = readDBString(reader, pos++);
            preservation_code.allowance = readDBDecimal(reader, pos++);
            preservation_code.effective_date = readDBDateObject(reader, pos++, DateTime.Now);
            preservation_code.type = readDBString(reader, pos++);
            preservation_code.general_service = readDBString(reader, pos++).Trim();
            preservation_code.enabled = readDBYesNo(reader, pos++);
            return preservation_code;
        }

        public void readAllowances()
        {
            this.allowances = (new PreservationCodeAllowance().getPreservationCodeAllowances(this.id));
        }

        public String to_csv()
        {
            return this.code.ToString() + "," + this.type + ",\"" + this.description.Replace("\"", "\"\"") + "\"" +
                ",\"" + this.general_service + "\",\"" + this.allowance.ToString("C") + "\"" +
                "," + this.effective_date.ToString() + "," + (this.enabled ? "true" : "false");
        }

        public PreservationCodeAllowance getPCA()
        {
            PreservationCodeAllowance pca = new PreservationCodeAllowance();
            pca.preservation_code_id = this.id;
            pca.allowance = this.allowance;
            pca.effective_date = this.effective_date;
            return pca;
        }

    }
}