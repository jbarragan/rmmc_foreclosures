using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ADTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ADUser ad_user = new ADUser(HttpContext.Current.User.Identity.Name.ToString());
        this.lblXml.Text = ad_user.getXMLData();
        this.lblHtml.Text = ad_user.getHTMLData();
        lblUserIdentity.Text = HttpContext.Current.User.Identity.Name.ToString();
    }
}