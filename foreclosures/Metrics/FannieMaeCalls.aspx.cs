using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

using com.sp.rmmc.foreclosures.models;

public partial class lossmitigation_Default : System.Web.UI.Page
{
    protected string loan_officer = "ALL";
    protected string workflow = "ALL";
    protected string loan_type = "ALL";
    protected string event_type = "FANNIE MAE loan last call Warning.";
    protected string version = "current";
    protected List<BaseForeclosure> bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> all_bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> accepted_bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> removed_bfs = new List<BaseForeclosure>();
    protected History history = null;
    protected decimal[] array_completed_checklists_loan_ids;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.loan_officer = getArg("loan_officer", "ALL");
        this.loan_type = getArg("loan_type", "ALL");
        this.version = getArg("version", "current");
        int iverstion = stringToInt(version, 0);

        BaseForeclosure query_foreclosure = new CurrentForeclosure();
        if (iverstion > 0)
        {
            history = (new History()).get(iverstion);
            if( history != null ) query_foreclosure = new HistoryForeclosure(iverstion);
        }

        List<BaseForeclosure> accepted_demands_bfs = new List<BaseForeclosure>();
        query_foreclosure.getDemands(accepted_demands_bfs, removed_bfs);
        List<BaseForeclosure> accepted_preforeclosures_bfs = new List<BaseForeclosure>();
        query_foreclosure.getPreForeclosures(accepted_preforeclosures_bfs, removed_bfs);
        List<BaseForeclosure> accepted_foreclosures_bfs = new List<BaseForeclosure>();
        query_foreclosure.getForeclosures(accepted_foreclosures_bfs, removed_bfs);
        accepted_bfs.AddRange(accepted_demands_bfs);
        accepted_bfs.AddRange(accepted_preforeclosures_bfs);
        accepted_bfs.AddRange(accepted_foreclosures_bfs);
        all_bfs.AddRange(accepted_bfs);
        all_bfs.AddRange(removed_bfs);

        populateddlLoanTypes();
        populateddlLoanOfficer();

        filter();
    }

    protected void filter()
    {
        bfs.Clear();
        foreach (BaseForeclosure bf in accepted_bfs)
        {
            if ((loan_officer == "ALL" || loan_officer == bf.foreclosure_officers) &&
                (loan_type == "ALL" || loan_type == bf.loan.loan_type))
            {
                if (this.event_type == "ALL")
                {
                    bfs.Add(bf);
                }
                else
                {
                    foreach (com.sp.rmmc.common.models.events.Event e in bf.getEvents())
                    {
                        if (e.name == this.event_type || this.event_type == "ALL")
                        {
                            bfs.Add(bf);
                            break;
                        }
                    }
                }
                
            }
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

    protected string generate_url()
    {
        return "Default.aspx" +
            "?workflow=" + workflow +
            "&loan_officer=" + loan_officer +
            "&loan_type=" + loan_type +
            "&event_type=" + event_type +
            "&version=" + version;
    }

    protected int stringToInt(string value, int default_value)
    {
        if (value == "") return default_value;
        try
        {
            return Convert.ToInt32(Regex.Replace(value, "[^0-9]", ""));
        }
        catch (Exception)
        {
            return default_value;
        }
    }

}