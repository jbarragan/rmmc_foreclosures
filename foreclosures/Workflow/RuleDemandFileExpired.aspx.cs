using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

using System.Text.RegularExpressions;

using com.sp.rmmc.foreclosures.models;
using com.sp.rmmc.common.models;
using com.sp.rmmc.common.models.events;

public partial class lossmitigation_Default : System.Web.UI.Page
{
    protected string loan_officer = "ALL";
    protected string workflow = "Demands";
    protected string loan_type = "ALL";
    protected string event_type = "ALL";
    protected List<String> events_list = new List<string>();
    protected string loan_id = "ALL";
    protected string version = "current";
    protected decimal decimal_loan_id = 0M;
    protected string sublist = "ALL";
    protected decimal[] array_completed_checklists_loan_ids;
    protected List<Event> events = new List<Event>();
    protected List<BaseForeclosure> bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> all_bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> accepted_bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> removed_bfs = new List<BaseForeclosure>();
    protected BaseForeclosure foreclosure = null;
    protected ComlpetedChecklist foreclosure_review_checklist = new ComlpetedChecklist();
    protected History history = null;
    protected string output = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.workflow = "Demands";
        this.event_type = "ALL";
        this.sublist = getArg("sublist", "ALL");
        this.version = getArg("version", "current");
        int iversion = stringToInt(version, 0);
        this.events_list.Add("Demand File Expired Alert");
        this.events_list.Add("Demand File Expired Warning");

        BaseForeclosure query_foreclosure = new CurrentForeclosure();
        if (iversion > 0)
        {
            history = (new History()).get(iversion);
            if (history != null) query_foreclosure = new HistoryForeclosure(history);
        }

        if (workflow == "Demands")
        {
            query_foreclosure.getDemands(accepted_bfs, removed_bfs);
            query_foreclosure.assignGl1806Details(accepted_bfs);
        }
        if (workflow == "FHA Demands Daily Status")
            query_foreclosure.getCurrentXDemands(accepted_bfs, removed_bfs);
        if (workflow == "Removed Demands")
            query_foreclosure.getXDemands(accepted_bfs, removed_bfs);
        else if (workflow == "PreForeclosures")
        {
            query_foreclosure.getPreForeclosures(accepted_bfs, removed_bfs);
            query_foreclosure.assignGl1806Details(accepted_bfs);
        }
        else if (workflow == "Foreclosures")
        {
            query_foreclosure.getForeclosures(accepted_bfs, removed_bfs);
            query_foreclosure.assignGl1806Details(accepted_bfs);
        }
        if (workflow == "Removed Foreclosures")
            query_foreclosure.getXForeclosure(accepted_bfs, removed_bfs);
        else if (workflow == "Foreclosed")
        {
            if (sublist == "Third Party Sales") query_foreclosure.getForeclosedThirdPartySale(accepted_bfs, removed_bfs);
            else query_foreclosure.getForeclosed(accepted_bfs, removed_bfs);
            query_foreclosure.assignGl1806Details(accepted_bfs);
        }
        else if (workflow == "Deed In Lieu")
        {
            if (sublist == "In Process") query_foreclosure.getInProcessDeedInLieu(accepted_bfs, removed_bfs);
            else query_foreclosure.getDeedInLieu(accepted_bfs, removed_bfs);
        }
        all_bfs.AddRange(accepted_bfs);
        all_bfs.AddRange(removed_bfs);

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
                foreach (com.sp.rmmc.common.models.events.Event e in bf.getEvents())
                {
                    if( this.events_list.Contains(e.name))
                    {
                        events.Add(e);
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
        if (sublist == "Evictions")
        {
            if (workflow != "Foreclosed") return true;
            if (bf.sale_eviction_process_events == null ||
                bf.sale_eviction_process_events.eviction_initiated_date.isNull) return false;
        }
        if (sublist == "Final")
        {
            if (workflow != "Deed In Lieu") return true;
            if (bf.possession_date.isNull == true && bf.deed_from_borrower_received_date.isNull == true) return false;
        }
        return true;
    }

    protected void getCompletedChecklists()
    {
        List<ComlpetedChecklist> completed_checklists = ComlpetedChecklist.getCompletedChecklists(this.bfs);
        List<decimal> completed_checklists_loan_ids = new List<decimal>();
        foreach (ComlpetedChecklist completed_checklist in completed_checklists)
            completed_checklists_loan_ids.Add(completed_checklist.loan_id);
        array_completed_checklists_loan_ids = completed_checklists_loan_ids.ToArray();
    }

    protected string getArg(string arg, string default_value)
    {
        string request_value = Request.QueryString[arg];
        if (request_value != "" && request_value != null) return request_value;
        return default_value;
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
}
