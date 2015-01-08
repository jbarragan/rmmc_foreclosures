using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class SaleAcquisitionPaymentEvents : BaseEvents
    {
        public DateTimeObject acquisition_payment_date = new DateTimeObject();
        public DateTimeObject transfer_of_custody_date = new DateTimeObject();

        private static String[] class_fields = 
            {
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as sale_acquisition_payment_events_loan_id",
             "(select top 1 fcl_ud.fcl_ud_item_recvd_expd from ms_fcl_ud_item_amts_1 fcl_ud where fcl_ud.loan_id = loan_id_column and fcl_ud.fcl_ud_item_desc like '%QUISITION PAY%' order by fcl_ud_item_recvd_expd desc) as sale_acquisition_payment_events_acquisition_payment_date",
             "(select top 1 fcl_ud.fcl_ud_item_date from ms_fcl_ud_item_amts_1 fcl_ud where fcl_ud.loan_id = loan_id_column and fcl_ud.fcl_ud_item_desc like '%QUISITION PAY%' order by fcl_ud_item_date desc) as sale_acquisition_payment_events_transfer_of_custody_date"
            };

        public SaleAcquisitionPaymentEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.loan.loan_type != "VA") return;
            if (this.transfer_of_custody_date.isNull) return;
            if (this.transfer_of_custody_date.date <= this.acquisition_payment_date.date && this.acquisition_payment_date.isNull == false) return;
            if (transfer_of_custody_date.date > DateTime.Today) return;

            if (this.acquisition_payment_date.isNull)
            {
                Event e = new Event();
                e.loan = this.loan;
                if (transfer_of_custody_date.date > DateTime.Today.AddDays(-3))
                {
                    e.type = Event.WARNING;
                    e.name = "No Acquisition Payment Date Warning";
                }
                else
                {
                    e.name = "No Acquisition Payment  Date Alert";
                    e.type = Event.ALERT;
                }
                e.description = "No Acquisition Payment  Date with Transfer of Custody Date " + this.transfer_of_custody_date.ToShortDateString();
                this.events.Add(e);
            }

            if (this.acquisition_payment_date.isNull == false && this.transfer_of_custody_date.date < this.acquisition_payment_date.date)
            {
                Event e = new Event();
                e.loan = this.loan;
                if (transfer_of_custody_date.date > this.acquisition_payment_date.date.AddDays(-3))
                {
                    e.type = Event.WARNING;
                    e.name = "Acquisition Payment  Date Warning";
                }
                else
                {
                    e.name = "Acquisition Payment  Date Alert";
                    e.type = Event.ALERT;
                }
                e.description = "Acquisition Payment  Date on " + this.acquisition_payment_date.ToShortDateString() + " with Transfer of Custody Date " + this.transfer_of_custody_date.ToShortDateString();
                this.events.Add(e);
            }
        }

        public void read(DbDataReader reader, int pos, MsLoan loan)
        {
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.acquisition_payment_date = DbModel.readDBDateObject(reader, pos++);
            this.transfer_of_custody_date = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }        
    }
}