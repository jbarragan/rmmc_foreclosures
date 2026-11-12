using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class PreForeclosureFNMAReviewEvents : BaseEvents
    {
        public DateTimeObject memo_15_day_review_dt = new DateTimeObject();
        public DateTimeObject memo_30_day_review_dt = new DateTimeObject();
        public DateTimeObject foreclosure_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as preforeclosure_fnma_review_events_loan_id",
             "(select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1200 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_recvd_expd ) as preforeclosure_fnma_review_events_foreclosure_date",
             "(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'FC-15 Day REVIEW' order by memo_create_dt desc ) as preforeclosure_fnma_review_events_memo_15_day_review_dt",
             "(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'FC-30 Day REVIEW' order by memo_create_dt desc ) as preforeclosure_fnma_review_events_memo_30_day_review_dt",
            };

        public PreForeclosureFNMAReviewEvents()
        {   
            this.fields = class_fields;
        }

        private void load_events()
        {
            if( this.loan.loan_id == 139062M )
                this.loan.loan_id = 139062M;
            if (this.loan.loan_type != "CNV") return;
            if (this.foreclosure_date.isNull) return;
            DateTime today = DateTime.Now;
            if (this.foreclosure_date.date < today) return;
            if (this.foreclosure_date.date > today.AddDays(40)) return;
            if (this.foreclosure_date.date > today.AddDays(9) ){
                if (this.memo_30_day_review_dt.isNull == true ||
                    this.memo_30_day_review_dt.date < this.foreclosure_date.date.AddDays(-40))
                {
                    Event e = new Event();
                    e.loan = this.loan;
                    if (this.foreclosure_date.date > today.AddDays(35)) e.type = Event.WARNING;
                    else e.type = Event.ALERT;
                    e.name = "Fannie Mae PreForeclosure Review required";
                    e.description = "Fannie Mae PreForeclosure Review required for foreclosure date: " + this.foreclosure_date.ToString();
                    this.events.Add(e);
                }
            }

            if (this.foreclosure_date.date > today.AddDays(16)) return;
            if (this.foreclosure_date.date > today)
            {
                if (this.memo_15_day_review_dt.isNull == true ||
                    this.memo_15_day_review_dt.date < this.foreclosure_date.date.AddDays(-16))
                {
                    Event e = new Event();
                    e.loan = this.loan;
                    if (this.foreclosure_date.date > today.AddDays(12)) e.type = Event.WARNING;
                    else e.type = Event.ALERT;
                    e.name = "Fannie Mae PreForeclosure Review required";
                    e.description = "Fannie Mae PreForeclosure Review required for foreclosure date: " + this.foreclosure_date.ToString();
                    this.events.Add(e);
                }
            }
            
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.foreclosure_date = DbModel.readDBDateObject(reader, pos++);
            this.memo_15_day_review_dt = DbModel.readDBDateObject(reader, pos++);
            this.memo_30_day_review_dt = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@preforeclosure_fnma_review_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@preforeclosure_fnma_review_events_foreclosure_date", this.foreclosure_date.toDBInsert());
            cmd.Parameters.AddWithValue("@preforeclosure_fnma_review_events_memo_15_day_review_dt", this.memo_15_day_review_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@preforeclosure_fnma_review_events_memo_30_day_review_dt", this.memo_30_day_review_dt.toDBInsert());
        }
    }
}