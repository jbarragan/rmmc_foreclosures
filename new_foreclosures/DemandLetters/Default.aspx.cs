using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.foreclosures.models;

public partial class lossmitigation_Collections_Default : System.Web.UI.Page
{
    protected List<DemandLetter> in_pre_demand_letter_mode = new List<DemandLetter>();
    protected List<DemandLetter> in_demand_letter_expired_mode = new List<DemandLetter>();
    protected decimal[] array_completed_checklists_loan_ids;
    protected decimal[] array_lm_reception_loan_ids;
    protected List<DemandLetter> remove_demand_letter_by_last_reg_payment = new List<DemandLetter>();
    protected List<DemandLetter> remove_demand_letter_by_foreclosure_active = new List<DemandLetter>();
    protected List<DemandLetter> empty_demand_letters = new List<DemandLetter>();
    protected List<DemandLetter> remove_lm_activity = new List<DemandLetter>();
    protected List<DemandLetter> remove_from_demand_letter_by_bankruptcy = new List<DemandLetter>();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        DemandLetter.in_pre_demand_letter_mode_list = new List<DemandLetter>();
        DemandLetter.remove_demand_letter_by_loss_mitigation_list = new List<DemandLetter>();
        in_pre_demand_letter_mode =  DemandLetter.in_pre_demand_letter_mode();
        in_demand_letter_expired_mode = DemandLetter.in_demand_letter_expired_mode();

        List<ComlpetedChecklist> completed_checklists = ComlpetedChecklist.get_demand_letters_completed_checklists(in_pre_demand_letter_mode);
        List<ComlpetedChecklist> completed_expired_checklists = ComlpetedChecklist.get_demand_letters_completed_checklists(in_demand_letter_expired_mode);
        List<decimal> completed_checklists_loan_ids = new List<decimal>();
        foreach (ComlpetedChecklist completed_checklist in completed_checklists)
            completed_checklists_loan_ids.Add(completed_checklist.loan_id);
        foreach (ComlpetedChecklist completed_checklist in completed_expired_checklists)
            completed_checklists_loan_ids.Add(completed_checklist.loan_id);
        array_completed_checklists_loan_ids = completed_checklists_loan_ids.ToArray();

        remove_lm_activity = DemandLetter.remove_from_demand_letter_by_loss_mitigation();
        remove_demand_letter_by_last_reg_payment = DemandLetter.remove_demand_letter_by_last_reg_payment();
        remove_demand_letter_by_foreclosure_active = DemandLetter.remove_demand_letter_by_foreclosure_active();
        remove_from_demand_letter_by_bankruptcy = DemandLetter.remove_from_demand_letter_by_bankruptcy();
    }
}