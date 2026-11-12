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
    protected PreservationCode preservation_code = new PreservationCode();
    protected string invalid_reasons = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        preservation_code = new PreservationCode();
        if (this.IsPostBack == false)
            this.txtEffectiveDate.Text = DateTime.Now.ToShortDateString();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (is_valid())
        {
            populate();
            PreservationCode existing_preservation_code = PreservationCode.get(preservation_code.code);
            string msg = "";
            if (existing_preservation_code.code.Length > 0)
            {
                msg = "Preservation Code Information cannot be created. Preservation Code already exists.";
            }
            else
            {
                msg = PreservationCode.create(preservation_code);

                if (msg.Length == 0)
                {
                    PreservationCodeAllowance pca = new PreservationCodeAllowance();
                    pca.preservation_code_id = preservation_code.id;
                    pca.allowance = preservation_code.allowance;
                    DateTimeObject now = new DateTimeObject();
                    now.set_date(DateTime.Now);
                    pca.effective_date = preservation_code.effective_date;
                    msg = pca.insert();
                }
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
            }
        }
        else
        {
            lblError.Text = invalid_reasons;
            lblSuccess.Text = "";
        }
    }

    protected void populate(){
        preservation_code.code = txtCode.Text.Replace(" ","").ToUpper();
        preservation_code.description = txtDescription.Text;
        decimal allowance = 0M;
        string str_allowance = Regex.Replace(this.txtAllowance.Text, "[^0-9.]", "");
        try
        {
            allowance = Convert.ToDecimal(str_allowance);
        }
        catch (Exception){}
        preservation_code.effective_date = this.convertStringToDateTimeObject( txtEffectiveDate.Text );
        preservation_code.allowance = allowance;
        preservation_code.enabled = chkEnabled.Checked;
        preservation_code.general_service = txtGeneralService.Text;
        preservation_code.type = cmbType.SelectedValue;
    }


    protected DateTimeObject convertStringToDateTimeObject(string s)
    {
        string ds = Regex.Replace(s, "[^0-9/-]", "");
        DateTimeObject d = new DateTimeObject();
        if (ds != "" && ds != null)
        {
            try
            {
                d.set_date(DateTime.Parse(ds));
                return d;
            }
            catch (Exception)
            {
            }
        }
        return d;
    }

    protected bool is_valid()
    {
        bool valid = true;
        invalid_reasons = "";

        if (txtCode.Text.Replace(" ","").Length == 0)
        {
            valid = false;
            invalid_reasons += "Preservation Code is required.<br/>";
        }
        
        string str_allowance = Regex.Replace(this.txtAllowance.Text, "[^0-9.]", "");
        try
        {
            decimal allowance = Convert.ToDecimal(str_allowance);
        }
        catch (Exception)
        {
            valid = false;
            invalid_reasons += "Allowance needs to be a number.<br/>";
        }
        DateTimeObject effective_date = this.convertStringToDateTimeObject(this.txtEffectiveDate.Text);
        if(effective_date.isNull)
        {
            valid = false;
            invalid_reasons += "Effective Date needs to be a date in format mm/dd/yyyy.<br/>";
        }
        return valid;
    }
 
}