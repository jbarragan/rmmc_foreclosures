using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

using com.sp.rmmc.foreclosures.models;
using com.sp.rmmc.common.models;

public partial class lossmitigation_Collections_Default : System.Web.UI.Page
{
    protected LoanPreservationDisbursement disbursement = new LoanPreservationDisbursement();
    protected string invalid_reasons = "";
    protected decimal loan_id = 0M;
    protected int fcl_misc_disb_id = 0;
    protected com.sp.rmmc.foreclosures.models.MsLoan loan = new com.sp.rmmc.foreclosures.models.MsLoan();
    protected List<PreservationCode> available_preservation_codes = new List<PreservationCode>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        load_disbursement();
        available_preservation_codes = PreservationCode.get_available_preservation_codes(loan.loan_type);
        populate_gui();
    }

    protected void init_disbursement(LoanPreservationDisbursement disbursement)
    {
        disbursement.loan_id = loan_id;
        disbursement.fcl_misc_disb_id = fcl_misc_disb_id;
    }

    protected void load_disbursement()
    {
        load_params();
        LoanPreservationDisbursement search_disbursement = new LoanPreservationDisbursement();
        init_disbursement(search_disbursement);
        disbursement = LoanPreservationDisbursement.get(search_disbursement);
    }

    protected void load_params()
    {
        string sloan_id = Request.QueryString["loan_id"];
        string sfcl_misc_disb_id = Request.QueryString["fcl_misc_disb_id"];
        try
        {
            loan_id = Convert.ToDecimal(sloan_id);
            fcl_misc_disb_id = Convert.ToInt32(sfcl_misc_disb_id);
            loan = com.sp.rmmc.foreclosures.models.MsLoan.get_ms_loan(loan_id);
        }
        catch (Exception) { }
        if (loan_id == 0M || loan.loan_id == 0M) Response.Redirect("PreservationCodes.aspx");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (is_valid())
        {
            load_disbursement();
            bool is_new = disbursement.loan_id == 0M;
            populate();
            string msg = "";
            if (is_new)
                msg = LoanPreservationDisbursement.insert(disbursement);
            else
                msg = LoanPreservationDisbursement.update(disbursement);

            if (msg.Length > 0)
            {
                lblError.Text = msg;
                lblSuccess.Text = "";
            }
            else
            {
                lblError.Text = "";
                lblSuccess.Text = "success";
                Response.Redirect("LoanDisbursements.aspx?loan_id=" + loan.loan_id.ToString());
            }
        }
        else
        {
            lblError.Text = invalid_reasons;
            lblSuccess.Text = "";
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        load_disbursement();
        bool is_new = disbursement.loan_id == 0M;
        populate();
        string msg = "";
        if (is_new)
            Response.Redirect("LoanDisbursements.aspx?loan_id=" + loan.loan_id.ToString());
        else
        {
            msg = LoanPreservationDisbursement.delete(disbursement);
        }
        if (msg.Length > 0)
        {
            lblError.Text = msg;
            lblSuccess.Text = "";
        }
        else
        {
            lblError.Text = "";
            lblSuccess.Text = "success";
            Response.Redirect("LoanDisbursements.aspx?loan_id=" + loan.loan_id.ToString());
        }
        //string str_code = Request.QueryString["code"];
        //preservation_code = PreservationCode.get(str_code);
        //PreservationCode.delete(preservation_code);
        //Response.Redirect("PreservationCodes.aspx");

        //lblError.Text = "Deleted!!";
    }

    protected void populate()
    {
        init_disbursement(disbursement);

        disbursement.fcl_misc_ordered_date = convert2DateTimeObject(txtOrderedDate.Text);
        disbursement.fcl_misc_completed_date = convert2DateTimeObject(txtCompletedDate.Text);
        disbursement.fcl_misc_paid_date = convert2DateTimeObject(txtPaidDate.Text);
        disbursement.fcl_misc_actual_amt = convert2Decimal(Regex.Replace(this.txtActual.Text, "[^0-9.]", ""));
        disbursement.fcl_misc_claimable_amt = convert2Decimal(Regex.Replace(this.txtClaimable.Text, "[^0-9.]", ""));
        disbursement.fcl_misc_disb_type = "PNP";
        disbursement.fcl_misc_description = this.cmbPreservationCode.SelectedValue;
    }

    protected void populate_gui()
    {
        if (disbursement.fcl_misc_description != "")
            cmbPreservationCode.Items.Add(new ListItem(disbursement.fcl_misc_description, disbursement.fcl_misc_description));
        foreach (PreservationCode preservation_code in available_preservation_codes)
            cmbPreservationCode.Items.Add(new ListItem(preservation_code.code + " " + preservation_code.description + " " + preservation_code.allowance.ToString("C"), 
                                                       preservation_code.code + preservation_code.description));
        this.txtActual.Text = disbursement.fcl_misc_actual_amt.ToString("C");
        this.txtClaimable.Text = disbursement.fcl_misc_claimable_amt.ToString("C");
        this.txtOrderedDate.Text = disbursement.fcl_misc_ordered_date.ToShortDateString();
        this.txtPaidDate.Text = disbursement.fcl_misc_paid_date.ToShortDateString();
        this.txtCompletedDate.Text = disbursement.fcl_misc_completed_date.ToShortDateString();
    }

    protected bool is_valid()
    {
        bool valid = true;
        invalid_reasons = "";

        if (!valid_decimal(Regex.Replace(this.txtActual.Text, "[^0-9.]", "")))
        {
            valid = false;
            invalid_reasons += "Actual Amount needs to be an number.<br/>";
        }

        if (!valid_decimal(Regex.Replace(this.txtClaimable.Text, "[^0-9.]", "")))
        {
            valid = false;
            invalid_reasons += "Claimable Amount needs to be an number.<br/>";
        }

        if (!valid_date(this.txtPaidDate.Text))
        {
            valid = false;
            invalid_reasons += "Paid Date needs to be in Date Format MM/DD/YYYY.<br/>";
        }

        if (!valid_date(this.txtOrderedDate.Text))
        {
            valid = false;
            invalid_reasons += "Ordered Date needs to be in Date Format MM/DD/YYYY.<br/>";
        }

        if (!valid_date(this.txtCompletedDate.Text))
        {
            valid = false;
            invalid_reasons += "Completed Date needs to be in Date Format MM/DD/YYYY.<br/>";
        }

        return valid;
    }

    bool valid_decimal(String str_decimal)
    {
        bool valid = true;
        if (str_decimal.Length > 0)
        {
            try
            {
                Convert.ToDecimal(str_decimal);
            }
            catch (Exception)
            {
                valid = false;
            }
        }
        return valid;
    }

    decimal convert2Decimal(String str_decimal)
    {
        decimal return_decimal = 0M;
        if (str_decimal.Length > 0)
        {
            try
            {
                return_decimal = Convert.ToDecimal(str_decimal);
            }
            catch (Exception) { }
        }
        return return_decimal;
    }

    bool valid_date(String str_date)
    {
        bool valid = true;
        if (str_date.Length > 0)
        {
            try
            {
                DateTime.Parse(str_date);
            }
            catch (Exception)
            {
                valid = false;
            }
        }
        return valid;
    }

    DateTimeObject convert2DateTimeObject(String str_date)
    {
        DateTimeObject return_datetime = new DateTimeObject();
        if (str_date.Length > 0)
        {
            try
            {
                DateTime date = DateTime.Parse(str_date);
                return_datetime.set_date(date);
            }
            catch (Exception) { }
        }
        return return_datetime;
    }
 
}