using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

using com.sp.rmmc.foreclosures.models;
using com.sp.rmmc.common.models;

public partial class MainClass : System.Web.UI.Page
{
    protected PreservationCode preservation_code = new PreservationCode();
    protected string invalid_reasons = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        //if (IsPostBack) return;
        try
        {
            this.init();
            if (preservation_code.code.Length > 0){
                if (IsPostBack == false) 
                    populate_gui();
            }else
            {
                Response.Redirect("PreservationCodes.aspx");
            }
        }
        catch (Exception)
        {
            Response.Redirect("PreservationCodes.aspx");
        }

        this.lbl_success.Text = getStringArg("success", "");
        this.lbl_error.Text = getStringArg("error", "");
    }

    protected void init(){
        string str_code = Request.QueryString["code"];
        preservation_code = PreservationCode.get(str_code);
        preservation_code.readAllowances();

        foreach (PreservationCodeAllowance pca in preservation_code.allowances)
        {
            AllowanceObject o = new AllowanceObject(this, preservation_code, pca);
            tbl_pcas.Rows.Add(o.getRow());
        }
    }

    protected string getStringArg(string arg, string default_value)
    {
        string request_value = Request.QueryString[arg];
        if (request_value == null) return default_value;
        return request_value;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (is_valid())
        {
            populate();
            string str_code = Request.QueryString["code"];
            
            string msg = PreservationCode.update(str_code, preservation_code);
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

    protected void btnSaveNew_Click(object sender, EventArgs e)
    {
        PreservationCodeAllowance pca = new PreservationCodeAllowance();
        decimal allowance = this.convertStringToDecimal(this.txt_new_allowance.Text);
        if (allowance == 0M && this.txt_new_allowance.Text.Contains("0") == false)
        {
            this.lbl_error.Text = "Invalid Allowance, please enter a numeric value";
            return;
        }
        DateTimeObject effective_date = this.convertStringToDateTimeObject(this.txt_new_effective_date.Text);
        if ( effective_date.isNull )
        {
            this.lbl_error.Text = "Invalid Effective Date, please enter a valid date in this format mm/dd/yyyy";
            return;
        }
        pca.allowance = allowance;
        pca.effective_date = effective_date;
        pca.preservation_code_id = preservation_code.id;

        this.redirectWithDBMessage(pca.insert());
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string str_code = Request.QueryString["code"];
        preservation_code = PreservationCode.get(str_code);
        PreservationCode.delete(preservation_code);
        Response.Redirect("PreservationCodes.aspx");

        lblError.Text = "Deleted!!";
    }

    protected void populate()
    {
        preservation_code.code = txtCode.Text.Replace(" ", "");
        preservation_code.description = txtDescription.Text;
        decimal allowance = 0M;
        preservation_code.allowance = allowance;
        preservation_code.enabled = chkEnabled.Checked;
        preservation_code.general_service = txtGeneralService.Text;
        preservation_code.type = cmbType.SelectedValue;
    }

    protected void populate_gui()
    {
        txtCode.Text = preservation_code.code;
        txtDescription.Text = preservation_code.description;
        
        txtAllowance.Text = preservation_code.allowance.ToString("C");
        chkEnabled.Checked = preservation_code.enabled;
        txtGeneralService.Text = preservation_code.general_service;
        cmbType.SelectedValue = preservation_code.type;
        txt_new_effective_date.Text = DateTime.Now.ToShortDateString();
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
        return valid;
    }

    protected decimal convertStringToDecimal(string s){
        decimal d = 0.0M;
        string ds = Regex.Replace(s, "[^0-9.-]", "");
        try
        {
            d = Decimal.Parse(ds);
        }catch(Exception){
        }
        return d;
    }

    protected DateTimeObject convertStringToDateTimeObject(string s)
    {
        string ds = Regex.Replace(s, "[^0-9/-]", "");
        DateTimeObject d = new DateTimeObject();
        if (ds != "" && ds != null)
        {
            try
            {
                d.set_date( DateTime.Parse(ds) );
                return d;
            }
            catch (Exception)
            {
            }
        }
        return d;
    }

    public void redirectWithDBMessage(string message)
    {
        if( message == "" )
            this.redirect("success=Data Saved!");
        else
            this.redirect("error=Error: " + message);
    }

    public void redirect(string message)
    {
        Response.Redirect("EditPreservationCode.aspx?code=" + this.preservation_code.code + "&" + message);
    }

    protected class AllowanceObject
    {
        public TextBox txt_allowance = new TextBox();
        public TextBox txt_effective_date = new TextBox();
        public Button btn_edit = new Button();
        public Button btn_delete = new Button();
        PreservationCode pc;
        PreservationCodeAllowance pca;
        MainClass parent;

        public AllowanceObject(MainClass parent, PreservationCode pc, PreservationCodeAllowance pca)
        {
            this.txt_allowance.Text = pca.allowance.ToString("C");
            this.txt_effective_date.Text = pca.effective_date.ToString();
            this.pc = pc;
            this.pca = pca;
            this.parent = parent;
            btn_edit.Text = "Save Changes";
            btn_edit.CssClass = "btn btn-info";
            btn_edit.Click += new System.EventHandler(btn_edit_click);
            btn_delete.Text = "Delete";
            btn_delete.CssClass = "btn btn-danger";
            btn_delete.Click += new System.EventHandler(btn_delete_click);
            btn_delete.OnClientClick = "return confirm(\"Do you want to delete Allowance Entry for " + pca.allowance.ToString("C") + "? \")";
        }

        public void btn_edit_click(object sender, EventArgs e)
        {
            this.pca.allowance = this.parent.convertStringToDecimal(this.txt_allowance.Text);
            this.pca.effective_date = this.parent.convertStringToDateTimeObject(this.txt_effective_date.Text);
            this.parent.redirectWithDBMessage( this.pca.update() );
        }

        public void btn_delete_click(object sender, EventArgs e)
        {
            this.parent.redirectWithDBMessage(this.pca.delete());
        }

        public HtmlTableRow getRow()
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell = new HtmlTableCell();
            cell.Controls.Add(this.txt_allowance);
            row.Cells.Add(cell);
            cell = new HtmlTableCell();
            cell.Controls.Add(this.txt_effective_date);
            row.Cells.Add(cell);
            cell = new HtmlTableCell();
            cell.Controls.Add(this.btn_edit);
            cell.Controls.Add(this.btn_delete);
            row.Cells.Add(cell);
            return row;
        }
    }
 
}