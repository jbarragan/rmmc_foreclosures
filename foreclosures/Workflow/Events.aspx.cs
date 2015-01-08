using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text.RegularExpressions;

using com.sp.rmmc.foreclosures.models;
using com.sp.rmmc.common.models.events;

public partial class lossmitigation_Default : System.Web.UI.Page
{
    protected string loan_officer = "ALL";
    protected string workflow = "Demands";
    protected string loan_type = "ALL";
    protected string loan_id = "ALL";
    protected decimal decimal_loan_id = 0M;
    protected List<Event> events = new List<Event>();
    protected List<BaseForeclosure> bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> all_bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> accepted_bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> removed_bfs = new List<BaseForeclosure>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;
        this.loan_officer = getArg("loan_officer", "ALL");
        this.workflow = getArg("workflow", "Demands");
        this.loan_type = getArg("loan_type", "ALL");
        this.loan_id = stringDecimal(getArg("loan_id", "ALL"), "ALL");
        this.decimal_loan_id = stringToDecimal(this.loan_id, 0M);

        if (this.decimal_loan_id != 0M)
        {
            if (workflow == "Demands")
                BaseForeclosure.getForeclosureLoan(this.decimal_loan_id, accepted_bfs, removed_bfs, BaseForeclosure.DEMAND_TYPE);
            else if( workflow == "Foreclosed")
                BaseForeclosure.getForeclosureLoan(this.decimal_loan_id, accepted_bfs, removed_bfs, BaseForeclosure.FORECLOSED_TYPE);
        }
        else if (workflow == "Demands")
        {
            BaseForeclosure.getDemands(accepted_bfs, removed_bfs);
        }
        all_bfs.AddRange(accepted_bfs);
        all_bfs.AddRange(removed_bfs);

        populateddlLoanTypes();
        populateddlLoanOfficer();
        this.txtLoanId.Text = this.loan_id;

        filter();
        setEvents();
    }

    protected void filter()
    {
        bfs.Clear();
        foreach (BaseForeclosure bf in accepted_bfs)
        {
            if ((loan_officer == "ALL" || loan_officer == bf.foreclosure_officers) &&
                (loan_type == "ALL" || loan_type == bf.loan.loan_type) ) bfs.Add(bf);
        }
    }

    protected void setEvents()
    {
        foreach (BaseForeclosure bf in bfs)
        {
            events.AddRange(bf.getEvents());
        }
    }

    protected string getArg(string arg, string default_value)
    {        
        string request_value = Request.QueryString[arg];
        if (request_value != "" && request_value != null) return request_value;
        return default_value;
    }

    protected void populateddlLoanTypes()
    {
        string[] loan_types = {"ALL", "FHA", "CNV", "VA"};
        foreach(string lt in loan_types){
            ListItem new_li = new ListItem(lt.Trim().ToUpper());
            if (this.loan_type == lt.Trim().ToUpper()) new_li.Selected = true;
            ddlLoanType.Items.Add(new_li);
        }
    }

    protected decimal stringToDecimal(string value, decimal default_value)
    {
        if (value == "") return default_value;
        try
        {
            return Convert.ToDecimal(Regex.Replace(value, "[^0-9]", ""));
        }
        catch (Exception)
        {
            return default_value;
        }
    }

    protected string stringDecimal(string value, string default_value)
    {
        if (value == "") return default_value;
        try
        {
            return Convert.ToDecimal(Regex.Replace(value, "[^0-9]", "")).ToString();
        }
        catch (Exception)
        {
            return default_value;
        }
    }



    protected void populateddlLoanOfficer()
    {
        ListItem li = new ListItem("ALL");
        ddlLoanOfficer.Items.Add(li);
        List<String> loan_officers = new List<string>();
        foreach (BaseForeclosure bf in all_bfs)
        {
            bool is_new_loan_officer = true;
            foreach (string lo in loan_officers)
            {
                if (bf.foreclosure_officers.Trim().ToUpper() == lo)
                {
                    is_new_loan_officer = false;
                    break;
                }
            }
            if (is_new_loan_officer)
            {
                loan_officers.Add(bf.foreclosure_officers.ToUpper());
                ListItem new_li = new ListItem(bf.foreclosure_officers.Trim().ToUpper());
                if (this.loan_officer == bf.foreclosure_officers.Trim().ToUpper()) new_li.Selected = true;
                ddlLoanOfficer.Items.Add(new_li);
            }
        }
    }

    protected void ddlLoanOfficer_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.loan_officer = ddlLoanOfficer.SelectedValue.ToUpper();
        Response.Redirect(generate_url());
    }

    protected void ddlLoanType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.loan_type = ddlLoanType.SelectedValue.ToUpper();
        Response.Redirect(generate_url());
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (this.txtLoanId.Text == "" || this.txtLoanId.Text.ToUpper() == "ALL")
        {
            this.loan_id = "ALL";
            Response.Redirect(generate_url());
        }
        try
        {
            decimal dloan_id = Convert.ToDecimal(Regex.Replace(this.txtLoanId.Text, "[^0-9]", ""));
            if (dloan_id > 0)
            {
                this.loan_id = dloan_id.ToString();
                Response.Redirect(generate_url());
            }
            else
                this.lblError.Text = "Loan Number '" + txtLoanId.Text + "' is incorrect.";
        }
        catch (Exception)
        {
            this.lblError.Text = "Loan Number '" + txtLoanId.Text + "' is incorrect.";
            return;
        }
    }

    protected string generate_url()
    {
        return "Events.aspx" +
            "?workflow=" + workflow +
            "&loan_officer=" + loan_officer +
            "&loan_id=" + loan_id +
            "&loan_type=" + loan_type;
    }
}