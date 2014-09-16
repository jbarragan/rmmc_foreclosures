using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.foreclosures.models;
using com.sp.rmmc.common.models;

public partial class lossmitigation_Collections_Default : System.Web.UI.Page
{
    protected DemandLetter demand_letter = new DemandLetter();

    protected void Page_Load(object sender, EventArgs e)
{
        try
        {
            string loan_id_str = Request.QueryString["loan_id"];
            decimal loan_id = Convert.ToDecimal(loan_id_str);
            demand_letter = DemandLetter.get(loan_id);
            display_complete_incomplete();
        }
        catch (Exception)
        {
            Response.Redirect("../");
        }
    }

    protected void display_complete_incomplete()
    {
        ComlpetedChecklist completd_checklist =  ComlpetedChecklist.get(demand_letter);
        if (completd_checklist.is_completed())
        {
            display_completed();
        }
        else
        {
            display_incompleted();
        }
    }

    private void display_completed()
    {
        this.btnComplete.Visible = this.btnCompleteTop.Visible = false;
        this.btnIncomplete.Visible = this.btnIncompleteTop.Visible = true;
        this.lblComplete.Visible = true;
        this.lblIncomplete.Visible = false;
    }

    private void display_incompleted()
    {
        this.btnComplete.Visible = this.btnCompleteTop.Visible = true;
        this.btnIncomplete.Visible = this.btnIncompleteTop.Visible = false;
        this.lblComplete.Visible = false;
        this.lblIncomplete.Visible = true;
    }

    protected void btnComplete_Click(object sender, EventArgs e)
    {
        ComlpetedChecklist completd_checklist = new ComlpetedChecklist();
        completd_checklist.loan_id = demand_letter.loan_id;
        completd_checklist.date = new DateTimeObject();
        ComlpetedChecklist.create(completd_checklist);
        display_complete_incomplete();
    }

    protected void btnIncomplete_Click(object sender, EventArgs e)
    {
        ComlpetedChecklist completd_checklist = new ComlpetedChecklist();
        completd_checklist.loan_id = demand_letter.loan_id;
        completd_checklist.date = new DateTimeObject();
        ComlpetedChecklist.delete(completd_checklist);
        display_complete_incomplete();
    }
}