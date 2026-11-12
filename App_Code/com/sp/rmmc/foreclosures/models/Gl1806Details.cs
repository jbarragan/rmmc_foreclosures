using System;
using System.Collections.Generic;
using System.Web;

using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
using com.sp.rmmc.common.models.events;

namespace com.sp.rmmc.foreclosures.models
{

    public class Gl1806Details : MsSqlModel
    {
        public decimal loan_id = 0M;
        public string name = "";
        public DateTimeObject transaction_date = new DateTimeObject();
        public string description = "";
        public string invoice = "";
        public string check_number = "";
        public decimal amount = 0M;

        private string connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["foreclosure"].ConnectionString;
        private string select_base_query =
            @"SELECT [LOAN #] as loan_id, NAME, [DATE] as transaction_date, DESCRIPTION, Invoice, 
            [CHECK #] as check_number, AMOUNT
            FROM tbl_gl1806 ";

        private string select_amount_sum_and_last_query =
            @"select [loan #] as loan_id, 
            (select top 1 name from tbl_gl1806 ti where ti.[loan #] = base.[loan #]) as name, 
            (select top 1 [date] from tbl_gl1806 ti where ti.[loan #] = base.[loan #] order by [date] desc) as transaction_date, 
            (select top 1 description from tbl_gl1806 ti where ti.[loan #] = base.[loan #] order by [date] desc) as description, 
            (select top 1 invoice from tbl_gl1806 ti where ti.[loan #] = base.[loan #] order by [date] desc) as Invoice, 
            (select top 1 [check #] from tbl_gl1806 ti where ti.[loan #] = base.[loan #] order by [date] desc) as check_number, 
            sum(amount) as amount
            from tbl_gl1806 base ";

        public Gl1806Details()
        {
            
        }

        public List<Gl1806Details> getLoanDetails(decimal loan_id)
        {
            string query = select_base_query;
            query += " where [Loan #] = " + loan_id.ToString();
            query += " order by [DATE] desc ";
            return this.get_list(query);
        }

        public List<Gl1806Details> getSpecificLoanDetails(List<decimal> loan_ids)
        {
            if( loan_ids.Count <= 0 ) return new List<Gl1806Details>();
            string query = select_amount_sum_and_last_query;
            query += " where [loan #] in (";
            bool first_loan_id = true;
            foreach(decimal loan_id in loan_ids){
                if(first_loan_id == false) query += ", ";
                else first_loan_id = false;
                query += loan_id.ToString();
            }
            query += ") ";
            query += "group by [loan #] ";
            query += "order by [loan #] ";
            return this.get_list(query);
        }

        private List<Gl1806Details> get_list(String query)
        {
            List<Gl1806Details> details = new List<Gl1806Details>();
            this.setConnectionString(this.connection_string);
            DbDataReader reader = getReader(query);
            while (reader.Read()) details.Add((new Gl1806Details()).read(reader, 0));
            reader.Close();
            close_connection();
            return details;
        }

        private Gl1806Details read(DbDataReader reader, int pos)
        {
            Gl1806Details d = new Gl1806Details();
            d.loan_id = readDBDoubleDecimal(reader, pos++);
            d.name = readDBString(reader, pos++);
            d.transaction_date = readDBDateObject(reader, pos++);
            d.description = readDBString(reader, pos++);
            d.invoice = readDBString(reader, pos++);
            d.check_number = readDBString(reader, pos++);
            d.amount = readDBDecimal(reader, pos++);
            return d;
        }
    }
}