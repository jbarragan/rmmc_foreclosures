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
    protected History history = null;
    protected string output = "";
    protected List<Gl1806Summary> summaries = new List<Gl1806Summary>();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.version = getArg("version", "current");
        int iverstion = stringToInt(version, 0);
        
        if (iverstion > 0)
        {
            history = (new History()).get(iverstion);
        }
        summaries = (new Gl1806Summary()).all();
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

}