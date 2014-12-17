using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.foreclosures.models;
using com.sp.rmmc.common.models;

public partial class lossmitigation_Collections_Default : System.Web.UI.Page
{
    protected class ForeclosuresByMonth
    {
        public DateTime month = new DateTime();
        public List<Foreclosure> foreclosures = new List<Foreclosure>();

        public ForeclosuresByMonth(DateTime month)
        {
            this.month = month;
        }
    }

    protected List<ForeclosuresByMonth> foreclosures_by_month_list = new List<ForeclosuresByMonth>();

    protected void Page_Load(object sender, EventArgs e)
    {
        populateForeclosuresByMonthList();
    }

    protected void populateForeclosuresByMonthList()
    {
        List<Foreclosure> foreclosures = Foreclosure.foreclosure_completed_by_sale_date_completed_in_last_2_years();
        ForeclosuresByMonth foreclosures_by_month = new ForeclosuresByMonth(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
        foreach (Foreclosure f in foreclosures)
        {
            while (f.sale_date_completed.date < foreclosures_by_month.month)
            {
                foreclosures_by_month_list.Add(foreclosures_by_month);
                foreclosures_by_month = new ForeclosuresByMonth(foreclosures_by_month.month.AddMonths(-1));
            }
            foreclosures_by_month.foreclosures.Add(f);
        }
        foreclosures_by_month_list.Add(foreclosures_by_month);
    }

}