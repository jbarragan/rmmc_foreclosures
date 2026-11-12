using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class SaleOriginalDocumentsOrderedEvents : BaseEvents
    {
        public DateTimeObject last_memo_original_docs_date = new DateTimeObject();
        public DateTimeObject sale_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as sale_original_documents_ordered_events_loan_id",
             "(select top 1 memo.memo_notify_dt  from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'FC- Original Docs%' order by memo.memo_notify_dt desc ) as sale_original_documents_ordered_events_last_memo_original_docs_date",
             "(select top 1 mfia1.fcl_item_date from ms_fcl_item_amts_1 mfia1 where mfia1.item_id = 1600 and mfia1.loan_id = loan_id_column order by mfia1.fcl_item_date ) as sale_original_documents_ordered_events_sale_date"
            };

        public SaleOriginalDocumentsOrderedEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {   
            if (this.sale_date.isNull) return;
            if (this.last_memo_original_docs_date.isNull == false) return;
            if (this.sale_date.date.AddDays(5) > this.loan.event_todays_date) return;

            Event e = new Event();
            e.loan = this.loan;
            e.type = Event.ALERT;
            e.name = "No Original Documents Ordered";
            e.description = "No Original Documents Ordered with Sale Date: " + this.sale_date.ToShortDateString();
            this.events.Add(e);
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.last_memo_original_docs_date = DbModel.readDBDateObject(reader, pos++);
            this.sale_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@sale_original_documents_ordered_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@sale_original_documents_ordered_events_last_memo_original_docs_date", this.last_memo_original_docs_date.toDBInsert());
            cmd.Parameters.AddWithValue("@sale_original_documents_ordered_events_sale_date", this.sale_date.toDBInsert());
        }
    }
}