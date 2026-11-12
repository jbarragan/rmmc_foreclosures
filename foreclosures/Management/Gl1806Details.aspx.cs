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
    protected string version = "current";
    protected decimal loan_id = 0M;
    protected History history = null;
    protected string output = "";
    protected List<Gl1806Details> details = new List<Gl1806Details>();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.version = getArg("version", "current");
        this.loan_id = stringToDecimal(getArg("loan_id", "0"), 0);
        int iverstion = stringToInt(version, 0);
        if (loan_id == 0) Response.Redirect("../");
        if (iverstion > 0)
        {
            history = (new History()).get(iverstion);
        }
        details = (new Gl1806Details()).getLoanDetails(loan_id);
        return;
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