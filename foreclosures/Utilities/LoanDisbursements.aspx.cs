using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.foreclosures.models;
using System.Text.RegularExpressions;

public partial class lossmitigation_Collections_Default : System.Web.UI.Page
{
    protected List<LoanPreservationDisbursement> all_disbursements = new List<LoanPreservationDisbursement>();
    protected List<LoanPreservationDisbursement> no_code_disbursements = new List<LoanPreservationDisbursement>();
    protected List<LoanPreservationDisbursement> with_correct_amount_disbursements = new List<LoanPreservationDisbursement>();
    protected List<LoanPreservationDisbursement> with_more_amount_disbursements = new List<LoanPreservationDisbursement>();
    protected List<LoanPreservationDisbursement> with_less_amount_disbursements = new List<LoanPreservationDisbursement>();
    protected List<PreservationCode> all_preservation_codes = new List<PreservationCode>();
    protected decimal loan_id = 0M;
    protected MsLoan loan = null;
    protected int next_fcl_misc_disb_id = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        string sloan_id = Request.QueryString["loan_id"];
        try
        {
            loan_id = Convert.ToDecimal(sloan_id);
            loan = MsLoan.get_ms_loan(loan_id);
        }
        catch (Exception){}
        if (loan_id == 0M || loan.loan_id == 0M ) Response.Redirect("PreservationCodes.aspx");
        all_disbursements = LoanPreservationDisbursement.get_loan_preservation_disbursements(loan_id);
        all_preservation_codes = PreservationCode.get_all_preservation_codes();
        foreach (LoanPreservationDisbursement disbursement in all_disbursements)
        {
            Match m = Regex.Match(disbursement.fcl_misc_description, "^[A-Za-z][0-9][0-9]");
            if (m.Success)
            {
                PreservationCode preservation_code = all_preservation_codes.Find(delegate(PreservationCode pc) { return pc.code == m.Value; });
                if (preservation_code != null)
                {
                    disbursement.preservation_code = preservation_code;
                    if (preservation_code.allowance == disbursement.fcl_misc_actual_amt)
                        with_correct_amount_disbursements.Add(disbursement);
                    else if (preservation_code.allowance < disbursement.fcl_misc_actual_amt)
                        with_more_amount_disbursements.Add(disbursement);
                    else
                        with_less_amount_disbursements.Add(disbursement);
                }
                else
                    no_code_disbursements.Add(disbursement);
            }
            else
                no_code_disbursements.Add(disbursement);
        }

        int max_fcl_misc_disb_id = 0;
        foreach (LoanPreservationDisbursement disbursement in LoanPreservationDisbursement.get_loan_all_disbursements(loan_id))
            if (max_fcl_misc_disb_id < disbursement.fcl_misc_disb_id) max_fcl_misc_disb_id = disbursement.fcl_misc_disb_id;
        next_fcl_misc_disb_id = max_fcl_misc_disb_id + 1;
        
    }
}