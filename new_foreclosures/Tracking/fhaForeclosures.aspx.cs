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
    protected List<Foreclosed> inspection_ordered_NOT_ordered_1_day_after_foreclosure_alert_foreclosed = new List<Foreclosed>();
    protected List<Foreclosed> eviction_NOT_completed_45_day_after_foreclosure_alert_foreclosed = new List<Foreclosed>();
    protected List<Foreclosed> conveyance_part_a_30_after_posession_alert_foreclosed = new List<Foreclosed>();
    protected List<Foreclosed> title_approval_45_after_conveyance_part_a_alert_foreclosed = new List<Foreclosed>();
    protected List<Foreclosed> claim_part_b_15_after_title_approval_alert_foreclosed = new List<Foreclosed>();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        all_foreclosed = Foreclosed.all_fha();

        foreach (com.sp.rmmc.foreclosures.models.Foreclosed foreclosed in all_foreclosed)
        {
            if (foreclosed.fc_3rd_party_sale_date.isNull == false) continue;
            if (foreclosed.is_attorneys_posted_date_5_months_before_due_date_alert() == true)
                attorneys_posted_date_5_months_before_due_date_foreclosed.Add(foreclosed);
            if (foreclosed.is_inspection_ordered_NOT_ordered_1_day_after_foreclosure_alert() == true)
                inspection_ordered_NOT_ordered_1_day_after_foreclosure_alert_foreclosed.Add(foreclosed);
            if (foreclosed.is_eviction_NOT_completed_45_day_after_foreclosure_alert() == true)
                eviction_NOT_completed_45_day_after_foreclosure_alert_foreclosed.Add(foreclosed);
            if (foreclosed.is_conveyance_part_a_30_after_posession_alert() == true)
                conveyance_part_a_30_after_posession_alert_foreclosed.Add(foreclosed);
            if (foreclosed.is_title_approval_45_after_conveyance_part_a_alert() == true)
                title_approval_45_after_conveyance_part_a_alert_foreclosed.Add(foreclosed);
            if (foreclosed.is_claim_part_b_15_after_title_approval_alert() == true)
                claim_part_b_15_after_title_approval_alert_foreclosed.Add(foreclosed);
            

        }
    }

}
