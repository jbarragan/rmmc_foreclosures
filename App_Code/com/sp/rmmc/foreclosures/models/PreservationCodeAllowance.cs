using System;
using System.Collections.Generic;
using System.Web;

using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
using com.sp.rmmc.common.models.events;

namespace com.sp.rmmc.foreclosures.models
{

    public class PreservationCodeAllowance : MsSqlModel
    {
        public int id = 0;
        public int preservation_code_id = 0;
        public decimal allowance = 0M;
        public DateTimeObject effective_date = new DateTimeObject();

        private string connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["foreclosure"].ConnectionString;
        private string select_base_query = "select id, preservation_code_id, allowance, effective_date from preservation_code_allowances ";

        public PreservationCodeAllowance()
        {

        }

        public string insert()
        {
            this.setConnectionString(this.connection_string);
            SqlCommand cmd = getCommand();
            cmd.CommandText = "INSERT INTO preservation_code_allowances (preservation_code_id, allowance, effective_date) " +
                              "VALUES(@preservation_code_id, @allowance, @effective_date)";
            cmd.Parameters.AddWithValue("@preservation_code_id", this.preservation_code_id);
            cmd.Parameters.AddWithValue("@allowance", this.allowance);
            cmd.Parameters.AddWithValue("@effective_date", this.effective_date.toDBInsert());
            executeCommandStatement(cmd);
            return error;
        }

        public string update()
        {
            this.setConnectionString(this.connection_string);
            SqlCommand cmd = getCommand();
            cmd.CommandText = "UPDATE preservation_code_allowances set preservation_code_id = @preservation_code_id, allowance = @allowance, effective_date = @effective_date " +
                              "where id = @id";
            cmd.Parameters.AddWithValue("@preservation_code_id", this.preservation_code_id);
            cmd.Parameters.AddWithValue("@allowance", this.allowance);
            cmd.Parameters.AddWithValue("@effective_date", this.effective_date.toDBInsert());
            cmd.Parameters.AddWithValue("@id", this.id);
            executeCommandStatement(cmd);
            return error;
        }

        public string delete()
        {
            this.setConnectionString(this.connection_string);
            SqlCommand cmd = getCommand();
            cmd.CommandText = "DELETE preservation_code_allowances " +
                              "where id = @id";
            cmd.Parameters.AddWithValue("@id", this.id);
            executeCommandStatement(cmd);
            return error;
        }

        public List<PreservationCodeAllowance> getPreservationCodeAllowances(int preservation_code_id)
        {
            string query = select_base_query;
            query += " where preservation_code_id = " + preservation_code_id + " order by effective_date desc ";
            return this.get_list(query);
        }

        private List<PreservationCodeAllowance> get_list(String query)
        {
            List<PreservationCodeAllowance> list = new List<PreservationCodeAllowance>();
            this.setConnectionString(this.connection_string);
            DbDataReader reader = getReader(query);
            while (reader.Read()) list.Add((new PreservationCodeAllowance()).read(reader, 0));
            reader.Close();
            close_connection();
            return list;
        }

        private PreservationCodeAllowance read(DbDataReader reader, int pos)
        {
            PreservationCodeAllowance o = new PreservationCodeAllowance();
            o.id = readDBInt32(reader, pos++);
            o.preservation_code_id = readDBInt32(reader, pos++);
            o.allowance = readDBDecimal(reader, pos++);
            o.effective_date = readDBDateObject(reader, pos++);
            return o;
        }
    }
}
