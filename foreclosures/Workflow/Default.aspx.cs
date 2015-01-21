using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.foreclosures.models;

public partial class lossmitigation_Default : System.Web.UI.Page
{
    protected string loan_officer = "ALL";
    protected string workflow = "Demands";
    protected string loan_type = "ALL";
    protected string event_type = "ALL";
    protected List<BaseForeclosure> bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> all_bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> accepted_bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> removed_bfs = new List<BaseForeclosure>();
    protected decimal[] array_completed_checklists_loan_ids;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.loan_officer = getArg("loan_officer", "ALL");
        this.workflow = getArg("workflow", "Demands");
        this.loan_type = getArg("loan_type", "ALL");
        this.event_type = getArg("event_type", "ALL");

        if (workflow == "Demands")
            BaseForeclosure.getDemands(accepted_bfs, removed_bfs);
        if (workflow == "Removed Demands")
            BaseForeclosure.getXDemands(accepted_bfs, removed_bfs);
        else if (workflow == "PreForeclosures")
            BaseForeclosure.getPreForeclosures(accepted_bfs, removed_bfs);
        else if (workflow == "Foreclosures")
            BaseForeclosure.getForeclosures(accepted_bfs, removed_bfs);
        else if (workflow == "Foreclosed")
            BaseForeclosure.getForeclosed(accepted_bfs, removed_bfs);
        all_bfs.AddRange(accepted_bfs);
        all_bfs.AddRange(removed_bfs);

        populateddlLoanTypes();
        populateddlLoanOfficer();
        populateddlEventType();

        filter();
        getCompletedChecklists();
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

    protected void getCompletedChecklists()
    {
        List<ComlpetedChecklist> completed_checklists = ComlpetedChecklist.getCompletedChecklists(this.bfs);
        List<decimal> completed_checklists_loan_ids = new List<decimal>();
        foreach (ComlpetedChecklist completed_checklist in completed_checklists)
            completed_checklists_loan_ids.Add(completed_checklist.loan_id);
        array_completed_checklists_loan_ids = completed_checklists_loan_ids.ToArray();
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


    protected void populateddlEventType()
    {
        ListItem li = new ListItem("ALL");
        ddlEventType.Items.Add(li);
        List<String> event_types = new List<string>();
        foreach (BaseForeclosure bf in all_bfs)
        {
            foreach( com.sp.rmmc.common.models.events.Event ev in bf.getEvents())
            {
                bool is_new_event_type = true;

                foreach (string evt in event_types)
                {
                    if (ev.name == evt)
                    {
                        is_new_event_type = false;
                        break;
                    }
                }
                if (is_new_event_type)
                {
                    event_types.Add(ev.name);
                    ListItem new_li = new ListItem(ev.name);
                    if (this.event_type == ev.name) new_li.Selected = true;
                    ddlEventType.Items.Add(new_li);
                }
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
            "&event_type=" + event_type;
    }

    protected void ddlEventType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.event_type = ddlEventType.SelectedValue;
        Response.Redirect(generate_url());
    }
}