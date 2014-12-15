using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

/// <summary>
/// Summary description for InspectionEvents
/// </summary>
namespace com.sp.rmmc.common.models.events
{
    public class InspectionEvents : BaseEvents
    {
        private static String[] data_sources = 
            {
             " (select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_placer order by ms_loan_info.loan_id  ) as inspection_events_loan_id, "
            };

        public static InspectionEvents read(DbDataReader reader, int pos)
        {
            InspectionEvents data = new InspectionEvents();
            data.loan_id = DbModel.readDBDecimal(reader, pos++);
            return data;
        }

        public static string columns(string loan_id_column)
        {
            string s = String.Join(", ", data_sources);
            s = s.Replace("loan_id_placer", loan_id_column);
            return s;
        }
    }
}