using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.foreclosures.models;

public partial class Foreclosure_Demnad_Letters_ManagementReview : System.Web.UI.Page
{
    protected List<DemandLetter> in_pre_demand_letter_mode = new List<DemandLetter>();
    protected List<DemandLetter> in_demand_letter_expired_mode = new List<DemandLetter>();
    protected List<DemandLetter> completed_demand_letter_mode = new List<DemandLetter>();
    protected decimal[] array_completed_checklists_loan_ids;

    protected void Page_Load(object sender, EventArgs e)
    {
        in_pre_demand_letter_mode = DemandLetter.in_pre_demand_letter_mode();
        in_demand_letter_expired_mode = DemandLetter.in_demand_letter_expired_mode();

        List<ComlpetedChecklist> completed_checklists = ComlpetedChecklist.get_demand_letters_completed_checklists(in_pre_demand_letter_mode);
        List<ComlpetedChecklist> completed_expired_checklists = ComlpetedChecklist.get_demand_letters_completed_checklists(in_demand_letter_expired_mode);
        List<decimal> completed_checklists_loan_ids = new List<decimal>();
        foreach (ComlpetedChecklist completed_checklist in completed_checklists)
            completed_checklists_loan_ids.Add(completed_checklist.loan_id);
        foreach (ComlpetedChecklist completed_checklist in completed_expired_checklists)
            completed_checklists_loan_ids.Add(completed_checklist.loan_id);
        array_completed_checklists_loan_ids = completed_checklists_loan_ids.ToArray();
        foreach (DemandLetter demand_letter in in_pre_demand_letter_mode)
            if (Array.IndexOf(array_completed_checklists_loan_ids, demand_letter.loan_id) >= 0)
                completed_demand_letter_mode.Add(demand_letter);

        foreach (DemandLetter demand_letter in in_demand_letter_expired_mode)
            if (Array.IndexOf(array_completed_checklists_loan_ids, demand_letter.loan_id) >= 0)
                completed_demand_letter_mode.Add(demand_letter);

    }
}