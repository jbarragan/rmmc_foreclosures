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
    protected string loan_officer = "ALL";
    protected string workflow = "Demands";
    protected string loan_type = "ALL";
    protected string loan_id = "ALL";
    protected string version = "current";
    protected decimal decimal_loan_id = 0M;

    protected int days_of_interest = 0;
    protected decimal liq_espe = 0M;
    protected decimal liq_exp_before = 0M;
    protected decimal liq_exp_after = 0M;
    protected decimal escrow_advances_prior = 0M;
    protected decimal escrow_advances_after = 0M;
    protected decimal property_preservation_before = 0M;
    protected decimal property_preservation_after = 0M;
    protected decimal appraisal_fee = 0M;
    protected decimal credits = 0M;
    protected decimal cost_factor = 0M;

    protected List<Event> events = new List<Event>();
    protected List<BaseForeclosure> bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> all_bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> accepted_bfs = new List<BaseForeclosure>();
    protected List<BaseForeclosure> removed_bfs = new List<BaseForeclosure>();
    protected BaseForeclosure foreclosure = null;
    protected ComlpetedChecklist foreclosure_review_checklist = new ComlpetedChecklist();
    protected History history = null;
    protected string output = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack == true) return;
        this.version = getArg("version", "current");
        int iverstion = stringToInt(version, 0);
        this.loan_id = getArg("loan_id", "");
        this.decimal_loan_id = stringToDecimal(loan_id, 0M);

        BaseForeclosure query_foreclosure = new CurrentForeclosure();
        this.txtLoanId.Text = this.loan_id;
        if (iverstion > 0)
        {
            history = (new History()).get(iverstion);
            if (history != null) query_foreclosure = new HistoryForeclosure(history);
        }
        query_foreclosure.getForeclosureLoan(this.decimal_loan_id, accepted_bfs, removed_bfs, BaseForeclosure.ALL_TYPE);
        if (accepted_bfs.Count > 0) foreclosure = accepted_bfs[0];

        days_of_interest = stringToInt(getArg("days_of_interest", "0"), 0);
        liq_espe = stringToDecimal(getArg("liq_espe", "0"), 0M);
        liq_exp_before = stringToDecimal(getArg("liq_exp_before", "0"), 0M);
        liq_exp_after = stringToDecimal(getArg("liq_exp_after", "0"), 0M);
        escrow_advances_prior = stringToDecimal(getArg("escrow_advances_prior", "0"), 0M);
        escrow_advances_after = stringToDecimal(getArg("escrow_advances_after", "0"), 0M);
        property_preservation_before = stringToDecimal(getArg("property_preservation_before", "0"), 0M);
        property_preservation_after = stringToDecimal(getArg("property_preservation_after", "0"), 0M);
        appraisal_fee = stringToDecimal(
            getArg("appraisal_fee", (foreclosure == null || foreclosure.sale_va_guaranty_and_bid_calcs == null) ? "0" : 
                        foreclosure.sale_va_guaranty_and_bid_calcs.appraisal_fee.ToString() )
            , 0M);

        credits = stringToDecimal(getArg("credits", "0"), 0M);
        cost_factor = stringToDecimal(getArg("cost_factor", "0"), 0M);

        txtDaysOfInterest.Text = String.Format("{0:0}", days_of_interest);
        txtLiqEspe.Text = String.Format("{0:0.00}", liq_espe);
        txtLiqExpBefore.Text = String.Format("{0:0.00}", liq_exp_before);
        txtLiqExpAfter.Text = String.Format("{0:0.00}", liq_exp_after);
        txtEscrowAdvancesPrior.Text = String.Format("{0:0.00}", escrow_advances_prior);
        txtEscrowAdvancesAfter.Text = String.Format("{0:0.00}", escrow_advances_after);
        txtPropertyPreservationBefore.Text = String.Format("{0:0.00}", property_preservation_before);
        txtPropertyPreservationAfter.Text = String.Format("{0:0.00}", property_preservation_after);
        txtCredits.Text = String.Format("{0:0.00}", credits);
        txtAppraisalFee.Text = String.Format("{0:0.00}", appraisal_fee);
        txtCostFactor.Text = String.Format("{0:0.00}", cost_factor);

        if (foreclosure == null || foreclosure.sale_va_guaranty_and_bid_calcs == null) return;

        foreclosure.sale_va_guaranty_and_bid_calcs.set_external_sources(days_of_interest,
            liq_espe,
            liq_exp_before,
            liq_exp_after,
            escrow_advances_prior,
            escrow_advances_after,
            property_preservation_before,
            property_preservation_after,
            appraisal_fee,
            credits,
            cost_factor);
        foreclosure.sale_va_guaranty_and_bid_calcs.calculate_values();
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

    protected decimal stringToDecimal(string value, decimal default_value)
    {
        if (value == "") return default_value;
        try
        {
            return Convert.ToDecimal(Regex.Replace(value, "[^0-9.]", ""));
        }
        catch (Exception)
        {
            return default_value;
        }
    }

    protected string generate_url()
    {
        return "VaGuarantyAndBidCalculations.aspx" +
            "?loan_id=" + loan_id +
            "&workflow=" + workflow +
            "&days_of_interest=" + this.txtDaysOfInterest.Text +
            "&liq_espe=" + this.txtLiqEspe.Text +
            "&liq_exp_before=" + this.txtLiqExpBefore.Text +
            "&liq_exp_after=" + this.txtLiqExpAfter.Text +
            "&escrow_advances_prior=" + this.txtEscrowAdvancesPrior.Text +
            "&escrow_advances_after=" + this.txtEscrowAdvancesAfter.Text +
            "&property_preservation_before=" + this.txtPropertyPreservationBefore.Text +
            "&property_preservation_after=" + this.txtPropertyPreservationAfter.Text +
            "&credits=" + this.txtCredits.Text +
            "&cost_factor=" + this.txtCostFactor.Text +
            "&appraisal_fee=" + this.txtAppraisalFee.Text +
            "&version=" + version +
            "#InCalculations";
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        this.loan_id = this.txtLoanId.Text;
        Response.Redirect(generate_url());
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        this.loan_id = this.txtLoanId.Text;
        Response.Redirect(generate_url());
    }
}
