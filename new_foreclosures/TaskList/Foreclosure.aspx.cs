using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.foreclosures.models;

public partial class lossmitigation_Collections_Default : System.Web.UI.Page
{
    protected string selected_loan_officer = "";
    protected List<Foreclosure> recent_foreclosure_activated = new List<Foreclosure>();
    protected List<Foreclosure> recent_foreclosure_activated_all = new List<Foreclosure>();
    protected List<Foreclosure> recent_foreclosure_activated_fha = new List<Foreclosure>();
    protected List<Foreclosure> recent_foreclosure_activated_va = new List<Foreclosure>();
    protected List<Foreclosure> recent_foreclosure_activated_conv = new List<Foreclosure>();
    protected List<Foreclosure> foreclosure_completed_by_sale_date_completed = new List<Foreclosure>();
    protected decimal[] array_completed_checklists_loan_ids;

    protected void Page_Load(object sender, EventArgs e)
    {
        selected_loan_officer = get_selected_loan_officer();
        recent_foreclosure_activated = Foreclosure.recent_foreclosure_activated();
        populateddlLoanOfficer();
        Foreclosure.foreclosure_activated_list = new List<Foreclosure>();
        Foreclosure.remove_foreclosure_by_loss_mitigation_list = new List<Foreclosure>();

        foreach (Foreclosure f in recent_foreclosure_activated)
        {
            if (selected_loan_officer != "ALL" && f.foreclosure_officers.ToUpper() != selected_loan_officer) continue;
            if (f.is_preforeclosure_complete() == false) continue;
            recent_foreclosure_activated_all.Add(f);
            if (f.loan.loan_type == "FHA") recent_foreclosure_activated_fha.Add(f);
            if (f.loan.loan_type == "VA") recent_foreclosure_activated_va.Add(f);
            if (f.loan.loan_type == "CNV") recent_foreclosure_activated_conv.Add(f);

        }
    }

    protected string get_selected_loan_officer()
    {
        string selected_option = "ALL";
        string request_value = Request.QueryString["loan_officer"];
        if (request_value != "" && request_value != null) selected_option = request_value;
        return selected_option;
    }

    protected void populateddlLoanOfficer()
    {
        ListItem li = new ListItem("ALL");
        ddlLoanOfficer.Items.Add(li);
        List<String> loan_officers = new List<string>();
        foreach (Foreclosure dl in recent_foreclosure_activated)
        {
            bool is_new_loan_officer = true;
            foreach (string lo in loan_officers)
            {
                if (dl.foreclosure_officers.ToUpper() == lo)
                {
                    is_new_loan_officer = false;
                    break;
                }
            }
            if (is_new_loan_officer)
            {
                loan_officers.Add(dl.foreclosure_officers.ToUpper());
                ListItem new_li = new ListItem(dl.foreclosure_officers.ToUpper());
                if (selected_loan_officer == dl.foreclosure_officers.ToUpper()) new_li.Selected = true;
                ddlLoanOfficer.Items.Add(new_li);
            }
        }
    }
    protected void ddlLoanOfficer_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect("Foreclosure.aspx?loan_officer=" + ddlLoanOfficer.SelectedValue.ToUpper());
    }
}