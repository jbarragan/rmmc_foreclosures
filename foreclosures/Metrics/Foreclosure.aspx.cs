using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.foreclosures.models;

public partial class lossmitigation_Collections_Default : System.Web.UI.Page
{
    protected List<Foreclosure> recent_foreclosure_activated = new List<Foreclosure>();
    protected List<Foreclosure> foreclosure_completed_by_sale_date_completed = new List<Foreclosure>();
    protected List<Foreclosure> remove_from_recent_foreclosure_activated_by_payment_dt = new List<Foreclosure>();
    protected List<DemandLetter> empty_demand_letters = new List<DemandLetter>();
    protected List<Foreclosure> remove_from_foreclosure_by_lm_received = new List<Foreclosure>();
    protected List<Foreclosure> remove_from_foreclosure_by_bankruptcy = new List<Foreclosure>();
    protected List<Foreclosure> remove_from_foreclosure_by_sale = new List<Foreclosure>();
    protected List<Foreclosure> remove_from_foreclosure_by_deed_in_lieu = new List<Foreclosure>();

    protected void Page_Load(object sender, EventArgs e)
    {
        Foreclosure.foreclosure_activated_list = new List<Foreclosure>();
        Foreclosure.remove_foreclosure_by_loss_mitigation_list = new List<Foreclosure>();
        recent_foreclosure_activated = Foreclosure.recent_foreclosure_activated();
        foreclosure_completed_by_sale_date_completed = Foreclosure.foreclosure_completed_by_sale_date_completed();
        remove_from_foreclosure_by_bankruptcy = Foreclosure.remove_from_foreclosure_by_bankruptcy();
        remove_from_recent_foreclosure_activated_by_payment_dt = Foreclosure.remove_from_recent_foreclosure_activated_by_payment_dt();
        remove_from_foreclosure_by_sale = Foreclosure.remove_from_foreclosure_by_sale_date();
        remove_from_foreclosure_by_deed_in_lieu = Foreclosure.remove_from_foreclosure_by_deed_in_lieu();
        remove_from_foreclosure_by_lm_received = Foreclosure.remove_foreclosure_by_loss_mitigation(); 
    }
}