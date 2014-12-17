using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.foreclosures.models;
using com.sp.rmmc.common.models;

public partial class lossmitigation_Collections_Default : System.Web.UI.Page
{
    protected List<Foreclosure> foreclosures = new List<Foreclosure>();
    protected DateTime date = new DateTime();

    protected void Page_Load(object sender, EventArgs e)
    {
        populateForeclosures();
    }

    protected void populateForeclosures()
    {
        try
        {
            string month_s = Request.QueryString["month"];
            int month = Convert.ToInt32(month_s);

            string year_s = Request.QueryString["year"];
            int year = Convert.ToInt32(year_s);
            foreclosures = Foreclosure.foreclosure_completed_by_sale_date_completed_in_month_and_year(month, year);
            date = new DateTime(year, month, 1);
        }
        catch (Exception)
        {
            Response.Redirect("../");
        }
    }

}