using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.common.tools;
using com.sp.rmmc.foreclosures.models;

public partial class lossmitigation_Collections_Default : System.Web.UI.Page
{
    protected List<PreservationCode> all_preservation_codes = new List<PreservationCode>();
    protected List<TypeList> preservation_loan_types = new List<TypeList>();


    protected class ServiceList : List<PreservationCode>{}
    protected class TypeList : List<ServiceList>{}

    protected TypeList currentTypeList = new TypeList();
    protected ServiceList currentServiceList = new ServiceList();

    protected void Page_Load(object sender, EventArgs e)
    {
        all_preservation_codes = PreservationCode.get_all_preservation_codes();
        if (!IsPostBack)
        {
            string currentType = "";
            string currentService = "";
            foreach (PreservationCode pc in all_preservation_codes)
            {
                if (pc.type != currentType)
                {
                    if (currentServiceList.Count > 0) currentTypeList.Add(currentServiceList);
                    if (currentTypeList.Count > 0) preservation_loan_types.Add(currentTypeList);
                    currentTypeList = new TypeList();
                    currentServiceList = new ServiceList();
                }
                else if (pc.general_service != currentService)
                {
                    if (currentServiceList.Count > 0) currentTypeList.Add(currentServiceList);
                    currentServiceList = new ServiceList();
                }
                currentServiceList.Add(pc);
                currentType = pc.type;
                currentService = pc.general_service;
            }
            if (currentServiceList.Count > 0) currentTypeList.Add(currentServiceList);
            if (currentTypeList.Count > 0) preservation_loan_types.Add(currentTypeList);
        }

    }
    protected void btnLoanDisbursements_Click(object sender, EventArgs e)
    {
        Response.Redirect("LoanDisbursements.aspx?loan_id=" + this.txtLoanId.Text);
    }

    protected void lbExport_Click(object sender, EventArgs e)
    {
        string filename = "PreservationCodes";
        CSVExport.writeHeadersAndCSV(filename, "Code,Type,Description,Service,Allowance,Effective Date,Enabled", this.all_preservation_codes);
    }
}