using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.foreclosures.models;
using com.sp.rmmc.common.models;

public partial class lossmitigation_Collections_Default : System.Web.UI.Page
{
    protected Foreclosure foreclosure = new Foreclosure();
    protected string version = "current";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        try
        {
            string loan_id_str = Request.QueryString["loan_id"];
            decimal loan_id = Convert.ToDecimal(loan_id_str);
            foreclosure = Foreclosure.get(loan_id);
            display_gui();
        }
        catch (Exception)
        {
            Response.Redirect("../");
        }
    }

    private void display_gui()
    {
        this.chkFcForeclosureReferral.Checked = (foreclosure.fc_foreclosure_referral_date.isNull == false && foreclosure.demand_letter_due_date.date < foreclosure.fc_foreclosure_referral_date.date);
        this.chkPapersToAttorney.Checked = (foreclosure.papers_to_attorney_date.isNull == false && foreclosure.demand_letter_due_date.date < foreclosure.papers_to_attorney_date.date);
        this.chkFcPayoff.Checked = (foreclosure.fc_payoff_date.isNull == false && foreclosure.demand_letter_due_date.date < foreclosure.fc_payoff_date.date);
        this.chkCopyDemandLetters.Checked = foreclosure.preforeclosure_events.is_copy_demand_letters;
        this.chkCopyNotes.Checked = foreclosure.preforeclosure_events.is_copy_notes;
        this.chkCopyDeedOfTrust.Checked = foreclosure.preforeclosure_events.is_copy_deed_of_trust_notes;
        this.chkCopyBankruptcy.Checked = foreclosure.preforeclosure_events.is_copy_bankruptcy;
        this.chkCopyTitlePolicy.Checked = foreclosure.preforeclosure_events.is_copy_title_policy;
        this.chkAppraisalOrdered.Checked = (foreclosure.appraisal_ordered_date.isNull == false && DateTime.Today < foreclosure.appraisal_ordered_date.date.AddMonths(6));
        this.chkNotifyAppraisal.Checked = foreclosure.preforeclosure_events.is_notify_appraisal;
    }

    private void populate_preforeclosure_event()
    {
        foreclosure.preforeclosure_events.is_copy_demand_letters = this.chkCopyDemandLetters.Checked;
        foreclosure.preforeclosure_events.is_copy_notes = this.chkCopyNotes.Checked;
        foreclosure.preforeclosure_events.is_copy_deed_of_trust_notes = this.chkCopyDeedOfTrust.Checked;
        foreclosure.preforeclosure_events.is_copy_bankruptcy = this.chkCopyBankruptcy.Checked;
        foreclosure.preforeclosure_events.is_copy_title_policy = this.chkCopyTitlePolicy.Checked;
        foreclosure.preforeclosure_events.is_notify_appraisal = this.chkNotifyAppraisal.Checked;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string loan_id_str = Request.QueryString["loan_id"];
        decimal loan_id = Convert.ToDecimal(loan_id_str);
        foreclosure = Foreclosure.get(loan_id);
        populate_preforeclosure_event();
        string msg = "";
        if (foreclosure.preforeclosure_events.loan_id > 0M)
        {
            Foreclosure original = Foreclosure.get(loan_id);
            msg = PreforeclosureEvent.update(foreclosure.preforeclosure_events, original.preforeclosure_events);
        }
        else
        {
            foreclosure.preforeclosure_events.loan_id = foreclosure.loan_id;
            msg = PreforeclosureEvent.create(foreclosure.preforeclosure_events);
        }
        if (msg.Length > 0)
        {
            this.lblError.Text = msg;
            this.lblSuccess.Text = "";
        }
        else
        {
            this.lblError.Text = "";
            this.lblSuccess.Text = "Success";
        }
    }

}