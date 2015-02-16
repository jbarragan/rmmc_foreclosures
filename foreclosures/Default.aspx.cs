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
    protected string workflow = "ALL";
    protected string loan_type = "ALL";
    protected string event_type = "ALL";
    protected string loan_id = "ALL";
    protected string version = "current";
    protected decimal decimal_loan_id = 0M;
    protected List<Event> alert_events = new List<Event>();
    protected List<Event> warning_events = new List<Event>();
    protected List<BaseForeclosure> bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> all_bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> accepted_bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> removed_bfs = new List<BaseForeclosure>();
    protected History history = null;

    protected void Page_Load(object sender, EventArgs e)
    {   
        this.loan_officer = getArg("loan_officer", "ALL");
        this.workflow = getArg("workflow", "ALL");
        this.loan_type = getArg("loan_type", "ALL");
        this.event_type = getArg("event_type", "ALL");
        this.loan_id = stringDecimal(getArg("loan_id", "ALL"), "ALL");
        this.decimal_loan_id = stringToDecimal(this.loan_id, 0M);
        this.version = getArg("version", "current");

        int iverstion = stringToInt(version, 0);
        if (Page.IsPostBack) return;
        BaseForeclosure query_foreclosure = new CurrentForeclosure();
        if (iverstion > 0)
        {
            history = (new History()).get(iverstion);
            if (history != null) query_foreclosure = new HistoryForeclosure(iverstion);
        }
        if (workflow == "Demands" || workflow == "ALL")
        {
            List<BaseForeclosure> abfs = new List<BaseForeclosure>();
            List<BaseForeclosure> rbfs = new List<BaseForeclosure>();
            query_foreclosure.getDemands(abfs, rbfs);
            accepted_bfs.AddRange(abfs);// removed_bfs.AddRange(rbfs);
        }
        if (workflow == "Demands that Became Current" || workflow == "ALL")
        {
            List<BaseForeclosure> abfs = new List<BaseForeclosure>();
            List<BaseForeclosure> rbfs = new List<BaseForeclosure>();
            query_foreclosure.getCurrentXDemands(abfs, rbfs);
            accepted_bfs.AddRange(abfs);// removed_bfs.AddRange(rbfs);
        }
        if (workflow == "PreForeclosures" || workflow == "ALL")
        {
            List<BaseForeclosure> abfs = new List<BaseForeclosure>();
            List<BaseForeclosure> rbfs = new List<BaseForeclosure>();
            query_foreclosure.getPreForeclosures(abfs, rbfs);
            accepted_bfs.AddRange(abfs);// removed_bfs.AddRange(rbfs);
        }
        if (workflow == "Foreclosures" || workflow == "ALL")
        {
            List<BaseForeclosure> abfs = new List<BaseForeclosure>();
            List<BaseForeclosure> rbfs = new List<BaseForeclosure>();
            query_foreclosure.getForeclosures(abfs, rbfs);
            accepted_bfs.AddRange(abfs);// removed_bfs.AddRange(rbfs);
        }
        if (workflow == "Foreclosed" || workflow == "ALL")
        {
            List<BaseForeclosure> abfs = new List<BaseForeclosure>();
            List<BaseForeclosure> rbfs = new List<BaseForeclosure>();
            query_foreclosure.getForeclosed(abfs, rbfs);
            accepted_bfs.AddRange(abfs);// removed_bfs.AddRange(rbfs);
        }
        all_bfs.AddRange(accepted_bfs);
        filter();
    }

    protected void filter()
    {
        bfs.Clear();
        foreach (BaseForeclosure bf in accepted_bfs)
        {
            foreach (Event e in bf.getEvents())
            {
                e.parent_type = bf.type;
                if (event_type == "ALL" || e.type.ToString() == event_type)
                {
                    if (e.type == Event.ALERT) alert_events.Add(e);
                    if (e.type == Event.WARNING) warning_events.Add(e);
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