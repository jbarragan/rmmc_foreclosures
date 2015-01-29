using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class BaseEvents
    {
        public decimal loan_id = 0M;
        public MsLoan loan = new MsLoan();
        public String[] fields = { };

        public List<Event> events = new List<Event>();

        public int fields_count() { return fields.Length; }

        public string query(string loan_id_column)
        {
            string s = " " + String.Join(", ", fields);
            s = s.Replace("loan_id_column", loan_id_column);
            return s;
        }

        public virtual void read(DbDataReader reader, int pos, MsLoan loan)
        {
            return;
        }
    }
}