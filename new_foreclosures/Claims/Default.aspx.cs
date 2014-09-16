using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.foreclosures.models;
using com.sp.rmmc.common.models;

public partial class lossmitigation_Collections_Default : System.Web.UI.Page
{
    protected class ClaimsByMonth
    {
        public DateTime month = new DateTime();
        public List<Claim> claims = new List<Claim>();

        public ClaimsByMonth(DateTime month)
        {
            this.month = month;
        }
    }

    protected List<ClaimsByMonth> claims_by_month_list = new List<ClaimsByMonth>();

    protected void Page_Load(object sender, EventArgs e)
    {
        populateClaimsByMonthList();
    }

    protected void populateClaimsByMonthList()
    {
        List<Claim> all_claims = Claim.getAllClaims();
        ClaimsByMonth claims_by_month = new ClaimsByMonth(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
        foreach (Claim claim in all_claims)
        {
            while (claim.part_a_filed_recvd_date.date < claims_by_month.month)
            {
                claims_by_month_list.Add(claims_by_month);
                claims_by_month = new ClaimsByMonth(claims_by_month.month.AddMonths(-1));
            }
            claims_by_month.claims.Add(claim);
        }
        claims_by_month_list.Add(claims_by_month);
    }

}