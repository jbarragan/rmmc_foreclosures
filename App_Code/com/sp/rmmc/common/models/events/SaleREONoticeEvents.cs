using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class SaleREONoticeEvents : BaseEvents
    {
        public DateTimeObject reo_notice_date = new DateTimeObject();
        public DateTimeObject sale_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as sale_reo_notice_events_loan_id",
             "(select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = loan_id_column and memo.memo_type_id = 135 order by memo_id desc) as sale_reo_notice_events_reo_notice_date", 
             "(select top 1 mfia1.fcl_item_date from ms_fcl_item_amts_1 mfia1 where mfia1.item_id = 1600 and mfia1.loan_id = loan_id_column order by mfia1.fcl_item_date ) as sale_reo_notice_events_sale_date",
            };

        public SaleREONoticeEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.loan.loan_type != "CNV") return;
            if (this.sale_date.isNull) return;
            //if (this.sale_date.date >= DateTime.Today.AddDays(-1)) return;

            if (this.reo_notice_date.isNull)
            {
                Event e = new Event();
                e.loan = this.loan;
                if (sale_date.date < DateTime.Today.AddDays(1)) e.type = Event.ALERT;                    
                else e.type = Event.WARNING;
                e.name = "No REO Notification Alert";
                e.description = "No REO Notification for Sale Date " + this.sale_date.ToShortDateString();
                this.events.Add(e);
            }
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.reo_notice_date = DbModel.readDBDateObject(reader, pos++);
            this.sale_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }        
    }
}