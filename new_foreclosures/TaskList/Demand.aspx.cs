using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.foreclosures.models;

public partial class lossmitigation_Collections_Default : System.Web.UI.Page
{
    protected List<DemandLetter> all_active_demands = new List<DemandLetter>();
    protected string selected_loan_officer = "";
    protected List<DemandLetter> active_demands = new List<DemandLetter>();
    protected List<DemandLetter> active_demands_fha = new List<DemandLetter>();
    protected List<DemandLetter> active_demands_va = new List<DemandLetter>();
    protected List<DemandLetter> active_demands_conv = new List<DemandLetter>();
    protected List<DemandLetter> all_in_demand_letter_expired_mode = new List<DemandLetter>();
    protected List<DemandLetter> in_demand_letter_expired_mode = new List<DemandLetter>();
    protected List<DemandLetter> in_demand_letter_expired_mode_fha = new List<DemandLetter>();
    protected List<DemandLetter> in_demand_letter_expired_mode_va = new List<DemandLetter>();
    protected List<DemandLetter> in_demand_letter_expired_mode_conv = new List<DemandLetter>();
    protected decimal[] array_completed_checklists_loan_ids;
    protected decimal[] array_lm_reception_loan_ids;
    protected List<DemandLetter> all_remove_demand_letter_by_last_reg_payment = new List<DemandLetter>();
    protected List<DemandLetter> remove_demand_letter_by_last_reg_payment = new List<DemandLetter>();
    protected List<DemandLetter> remove_demand_letter_by_last_reg_payment_fha = new List<DemandLetter>();
    protected List<DemandLetter> remove_demand_letter_by_last_reg_payment_fha_mortgage_status = new List<DemandLetter>();
    protected List<DemandLetter> remove_demand_letter_by_last_reg_payment_fha_current_occupancy_letter = new List<DemandLetter>();
    protected List<DemandLetter> remove_demand_letter_by_last_reg_payment_fha_other = new List<DemandLetter>();
    
    protected List<DemandLetter> remove_demand_letter_by_last_reg_payment_va = new List<DemandLetter>();
    protected List<DemandLetter> remove_demand_letter_by_last_reg_payment_conv = new List<DemandLetter>();
    protected List<DemandLetter> empty_demand_letters = new List<DemandLetter>();
    protected List<DemandLetter> all_remove_lm_activity = new List<DemandLetter>();
    protected List<DemandLetter> remove_lm_activity = new List<DemandLetter>();
    protected List<DemandLetter> remove_lm_activity_fha = new List<DemandLetter>();
    protected List<DemandLetter> remove_lm_activity_va = new List<DemandLetter>();
    protected List<DemandLetter> remove_lm_activity_conv = new List<DemandLetter>();
    protected List<DemandLetter> all_remove_from_demand_letter_by_bankruptcy = new List<DemandLetter>();
    protected List<DemandLetter> remove_from_demand_letter_by_bankruptcy = new List<DemandLetter>();
    protected List<DemandLetter> remove_from_demand_letter_by_bankruptcy_fha = new List<DemandLetter>();
    protected List<DemandLetter> remove_from_demand_letter_by_bankruptcy_va = new List<DemandLetter>();
    protected List<DemandLetter> remove_from_demand_letter_by_bankruptcy_conv = new List<DemandLetter>();
    protected List<DemandLetter> all_inspection_expired = new List<DemandLetter>();
    protected List<DemandLetter> inspection_expired = new List<DemandLetter>();
    protected List<DemandLetter> inspection_expired_fha = new List<DemandLetter>();
    protected List<DemandLetter> inspection_expired_va = new List<DemandLetter>();
    protected List<DemandLetter> inspection_expired_conv = new List<DemandLetter>();

    protected List<DemandLetter> require_fannie_mae_call = new List<DemandLetter>();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        selected_loan_officer = get_selected_loan_officer();
        all_active_demands = DemandLetter.in_pre_demand_letter_mode();
        populateddlLoanOfficer();
        DemandLetter.in_pre_demand_letter_mode_list = new List<DemandLetter>();
        DemandLetter.remove_demand_letter_by_loss_mitigation_list = new List<DemandLetter>();
        foreach (DemandLetter dl in all_active_demands)
        {
            if (selected_loan_officer != "ALL" && dl.foreclosure_officers.ToUpper() != selected_loan_officer) continue;
            active_demands.Add(dl);
            if (dl.loan.loan_type == "FHA") active_demands_fha.Add(dl);
            if (dl.loan.loan_type == "CNV") active_demands_conv.Add(dl);
            if (dl.loan.loan_type == "VA") active_demands_va.Add(dl);
        }
        all_in_demand_letter_expired_mode = DemandLetter.in_demand_letter_expired_mode();
        foreach (DemandLetter dl in all_in_demand_letter_expired_mode)
        {
            if (selected_loan_officer != "ALL" && dl.foreclosure_officers.ToUpper() != selected_loan_officer) continue;
            in_demand_letter_expired_mode.Add(dl);
            if (dl.loan.loan_type == "FHA") in_demand_letter_expired_mode_fha.Add(dl);
            if (dl.loan.loan_type == "CNV") in_demand_letter_expired_mode_conv.Add(dl);
            if (dl.loan.loan_type == "VA") in_demand_letter_expired_mode_va.Add(dl);
        }
        all_inspection_expired= DemandLetter.inspections_expired_mode();
        foreach (DemandLetter dl in all_inspection_expired)
        {
            if (selected_loan_officer != "ALL" && dl.foreclosure_officers.ToUpper() != selected_loan_officer) continue;
            inspection_expired.Add(dl);
            if (dl.loan.loan_type == "FHA") inspection_expired_fha.Add(dl);
            if (dl.loan.loan_type == "CNV") inspection_expired_conv.Add(dl);
            if (dl.loan.loan_type == "VA") inspection_expired_va.Add(dl);
        }
        all_remove_demand_letter_by_last_reg_payment = DemandLetter.remove_demand_letter_by_last_reg_payment();
        foreach (DemandLetter dl in all_remove_demand_letter_by_last_reg_payment)
        {
            if (selected_loan_officer != "ALL" && dl.foreclosure_officers.ToUpper() != selected_loan_officer) continue;
            remove_demand_letter_by_last_reg_payment.Add(dl);
            if (dl.loan.loan_type == "FHA") remove_demand_letter_by_last_reg_payment_fha.Add(dl);
            if (dl.loan.loan_type == "CNV") remove_demand_letter_by_last_reg_payment_conv.Add(dl);
            if (dl.loan.loan_type == "VA") remove_demand_letter_by_last_reg_payment_va.Add(dl);
        }

        foreach (DemandLetter dl in remove_demand_letter_by_last_reg_payment_fha)
        {
            bool with_both = true;
            if (dl.mortgage_status != "20") { remove_demand_letter_by_last_reg_payment_fha_mortgage_status.Add(dl); with_both = false; }
            if (dl.nl_curent_occupancy_letter_date.isNull == true) { remove_demand_letter_by_last_reg_payment_fha_current_occupancy_letter.Add(dl); continue; with_both = false; }
            if( with_both ) remove_demand_letter_by_last_reg_payment_fha_other.Add(dl);
        }

        List<ComlpetedChecklist> completed_checklists = ComlpetedChecklist.get_demand_letters_completed_checklists(active_demands);
        List<ComlpetedChecklist> completed_expired_checklists = ComlpetedChecklist.get_demand_letters_completed_checklists(in_demand_letter_expired_mode);
        List<decimal> completed_checklists_loan_ids = new List<decimal>();
        foreach (ComlpetedChecklist completed_checklist in completed_checklists)
            completed_checklists_loan_ids.Add(completed_checklist.loan_id);
        foreach (ComlpetedChecklist completed_checklist in completed_expired_checklists)
            completed_checklists_loan_ids.Add(completed_checklist.loan_id);
        array_completed_checklists_loan_ids = completed_checklists_loan_ids.ToArray();

        all_remove_lm_activity = DemandLetter.remove_from_demand_letter_by_loss_mitigation();
        foreach (DemandLetter dl in all_remove_lm_activity)
        {
            if (selected_loan_officer != "ALL" && dl.foreclosure_officers.ToUpper() != selected_loan_officer) continue;
            remove_lm_activity.Add(dl);
            if (dl.loan.loan_type == "FHA") remove_lm_activity_fha.Add(dl);
            if (dl.loan.loan_type == "CNV") remove_lm_activity_conv.Add(dl);
            if (dl.loan.loan_type == "VA") remove_lm_activity_va.Add(dl);
        }
        all_remove_from_demand_letter_by_bankruptcy = DemandLetter.remove_from_demand_letter_by_bankruptcy();
        foreach (DemandLetter dl in all_remove_from_demand_letter_by_bankruptcy)
        {
            if (selected_loan_officer != "ALL" && dl.foreclosure_officers.ToUpper() != selected_loan_officer) continue;
            remove_from_demand_letter_by_bankruptcy.Add(dl);
            if (dl.loan.loan_type == "FHA") remove_from_demand_letter_by_bankruptcy_fha.Add(dl);
            if (dl.loan.loan_type == "CNV") remove_from_demand_letter_by_bankruptcy_conv.Add(dl);
            if (dl.loan.loan_type == "VA") remove_from_demand_letter_by_bankruptcy_va.Add(dl);
        }
        
        foreach (DemandLetter dl in DemandLetter.fannie_mae_notification_mode())
        {
            if (selected_loan_officer != "ALL" && dl.foreclosure_officers.ToUpper() != selected_loan_officer) continue;
            if (dl.loan.loan_type == "CNV" ) this.require_fannie_mae_call.Add(dl);
        }
    
    }

    protected string get_selected_loan_officer()
    {
        string selected_option = "ALL";
        string request_value = Request.QueryString["loan_officer"];
        if (request_value != "" && request_value != null) selected_option = request_value;
        return selected_option;
    }

    protected void populateddlLoanOfficer()
    {
        ListItem li = new ListItem("ALL");
        ddlLoanOfficer.Items.Add(li);
        List<String> loan_officers = new List<string>();
        foreach (DemandLetter dl in all_active_demands)
        {
            bool is_new_loan_officer = true;
            foreach (string lo in loan_officers)
            {
                if (dl.foreclosure_officers.ToUpper() == lo)
                {
                    is_new_loan_officer = false;
                    break;
                }
            }
            if (is_new_loan_officer)
            {
                loan_officers.Add(dl.foreclosure_officers.ToUpper());
                ListItem new_li = new ListItem(dl.foreclosure_officers.ToUpper());
                if (selected_loan_officer == dl.foreclosure_officers.ToUpper()) new_li.Selected = true;
                ddlLoanOfficer.Items.Add(new_li);
            }
        }
    }
    protected void ddlLoanOfficer_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect("Demand.aspx?loan_officer=" + ddlLoanOfficer.SelectedValue.ToUpper());
    }
}