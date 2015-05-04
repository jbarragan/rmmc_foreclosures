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
    protected string workflow = "Demands";
    protected string loan_type = "ALL";
    protected string event_type = "ALL";
    protected string version = "current";
    protected string sublist = "ALL";
    protected List<BaseForeclosure> bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> all_bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> accepted_bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> removed_bfs = new List<BaseForeclosure>();
    protected History history = null;
    protected decimal[] array_completed_checklists_loan_ids;
    protected string debug_msg = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.loan_officer = getArg("loan_officer", "ALL");
        this.workflow = getArg("workflow", "Demands");
        this.loan_type = getArg("loan_type", "ALL");
        this.event_type = getArg("event_type", "ALL");
        this.sublist = getArg("sublist", "ALL");
        this.version = getArg("version", "current");
        int iverstion = stringToInt(version, 0);

        BaseForeclosure query_foreclosure = new CurrentForeclosure();
        if (iverstion > 0)
        {
            history = (new History()).get(iverstion);
            if( history != null ) query_foreclosure = new HistoryForeclosure(iverstion);
        }

        if (workflow == "Demands")
            query_foreclosure.getDemands(accepted_bfs, removed_bfs);
        if (workflow == "FHA Demands Daily Status")
            query_foreclosure.getCurrentXDemands(accepted_bfs, removed_bfs);
        if (workflow == "Removed Demands")
            query_foreclosure.getXDemands(accepted_bfs, removed_bfs);
        else if (workflow == "PreForeclosures")
            query_foreclosure.getPreForeclosures(accepted_bfs, removed_bfs);
        else if (workflow == "Foreclosures")
            query_foreclosure.getForeclosures(accepted_bfs, removed_bfs);
        if (workflow == "Removed Foreclosures")
            query_foreclosure.getXForeclosure(accepted_bfs, removed_bfs);
        else if (workflow == "Foreclosed")
        {
            if (sublist == "Third Party Sales") query_foreclosure.getForeclosedThirdPartySale(accepted_bfs, removed_bfs);
            else query_foreclosure.getForeclosed(accepted_bfs, removed_bfs);
        }
        else if (workflow == "Deed In Lieu")
        {
            if (sublist == "In Process") query_foreclosure.getInProcessDeedInLieu(accepted_bfs, removed_bfs);
            else query_foreclosure.getDeedInLieu(accepted_bfs, removed_bfs);
        }

        //debug_msg = query_foreclosure.getQuery();

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
                if (belongToSublist(bf) == false) continue;
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

    protected bool belongToSublist(BaseForeclosure bf)
    {
        if (sublist == "ALL") return true;
        if (sublist == "Probates")
        {
            if (workflow != "Foreclosures") return true;
            if (bf.foreclosure_probate_events == null ||
                bf.foreclosure_probate_events.probate_start_date.isNull) return false;
        }
        if (sublist == "VA Appraisals")
        {
            if (workflow != "Foreclosures") return true;
            if (bf.loan.loan_type != "VA" || bf.foreclosure_va_appraisal_events == null ||
                bf.foreclosure_va_appraisal_events.appraisal_ordered_date.isNull) return false;
        }
        if (sublist == "REO Properties")
        {
            if (workflow != "Foreclosed") return true;
            if (bf.sale_conveyance == null ||
                bf.sale_conveyance.reo_date.isNull) return false;
        }
        if (sublist == "Final")
        {
            if (workflow != "Deed In Lieu") return true;
            if (bf.possession_date.isNull == true && bf.deed_from_borrower_received_date.isNull == true) return false;
        } 
        return true;

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
            "&sublist=" + sublist +
            "&loan_officer=" + loan_officer +
            "&loan_type=" + loan_type +
            "&event_type=" + event_type +
            "&version=" + version;
    }

    protected void ddlEventType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.event_type = ddlEventType.SelectedValue;
        Response.Redirect(generate_url());
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