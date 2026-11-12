using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

using com.sp.rmmc.foreclosures.models;
using com.sp.rmmc.common.tools;

public partial class lossmitigation_Default : System.Web.UI.Page
{

    protected List<LMInProcess> bfs = new List<LMInProcess>();
    protected string debug_msg = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        LMInProcess lm = new LMInProcess();
        bfs = lm.all();
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

    protected void lbExport_Click(object sender, EventArgs e)
    {
        string filename = "Loss Mitigation In Process";
        CSVExport.writeHeadersAndCSV(filename, "Loan#,Loan Type,Loan Name,UPB,Mortgage Status,Mortgage Status Date,Trial Payments Stop Code", this.bfs);
    }

}