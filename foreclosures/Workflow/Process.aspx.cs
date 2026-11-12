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
    protected string loan_id = "ALL";
    protected string version = "current";
    protected decimal decimal_loan_id = 0M;
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
        if (Page.IsPostBack == true) return;
        this.version = getArg("version", "current");
        int iverstion = stringToInt(version, 0);
        return;
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
