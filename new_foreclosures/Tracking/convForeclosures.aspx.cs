using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.foreclosures.models;
using com.sp.rmmc.common.models;

public partial class lossmitigation_Collections_Default : System.Web.UI.Page
{
    protected List<Foreclosed> all_foreclosed = new List<Foreclosed>();
    protected List<Foreclosed> attorneys_posted_date_5_months_before_due_date_foreclosed = new List<Foreclosed>();
    protected List<Foreclosed> fannie_mae_claim_30_after_foreclosure_date_foreclosed = new List<Foreclosed>();

    protected void Page_Load(object sender, EventArgs e)
    {
        all_foreclosed = Foreclosed.all_conv();

        foreach (com.sp.rmmc.foreclosures.models.Foreclosed foreclosed in all_foreclosed)
        {
            if (foreclosed.is_attorneys_posted_date_5_months_before_due_date_alert() == true)
                attorneys_posted_date_5_months_before_due_date_foreclosed.Add(foreclosed);
            if (foreclosed.is_fannie_mae_claim_30_after_foreclosure_date_alert() == true)
                fannie_mae_claim_30_after_foreclosure_date_foreclosed.Add(foreclosed);
        }
    }

}
