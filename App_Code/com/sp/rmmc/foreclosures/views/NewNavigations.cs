using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for Navigations
/// </summary>
namespace com.sp.rmmc.foreclosures.views
{
    public class NewNavigations
    {
        public NewNavigations()
        {
        }

        public static string main_nav =
    @"<div class=""navbar"">
       <div class=""navbar-inner"">
        <ul class=""nav"">
          <li>
            <a href=""Default.aspx"">Main Home</a>
          </li>
          <li>
            <a href=""TaskList/Demand.aspx"">Task List</a>
          </li>
          <li>
            <a href=""Metrics/Claims.aspx"">Metrics</a>
          </li>
          <li>
            <a href=""Utilities/PreservationCodes.aspx"">Utilities</a>
          </li>
          <li>
            <a href=""Default.aspx"">Dashboard</a>
          </li>
        </ul>
       </div>
      </div>";

        public static string sub_main_nav =
    @"<div class=""navbar"">
       <div class=""navbar-inner"">
        <ul class=""nav"">
          <li>
            <a href=""../Default.aspx"">Main Home</a>
          </li>
          <li>
            <a href=""../TaskList/Demand.aspx"">Task List</a>
          </li>
          <li>
            <a href=""../Metrics/Claims.aspx"">Metrics</a>
          </li>
          <li>
            <a href=""../Utilities/PreservationCodes.aspx"">Utilities</a>
          </li>
          <li>
            <a href=""../Default.aspx"">Dashboard</a>
          </li>
        </ul>
       </div>
      </div>";

        private static string nav_start = @"<div class=""navbar""><div class=""navbar-inner""><ul class=""nav"">";
        private static string nav_end = @"</ul></div></div>";
        private class Nav
        {
            public string link = "";
            public string text = "";
            public List<string> ad_users = new List<string>();
            public List<string> ad_departments = new List<string>();
            public List<string> ad_groups = new List<string>();

            public string link_html(string selected_text, ADUser ad_user)
            {
                string s = "";
                if (with_access(ad_user) == false) return s;
                string active_class = "";
                if (selected_text == text) active_class = @" class=""active""";
                s += @"<li" + active_class + @"><a href=""" + link + @""">" + text + "</a></li>";
                return s;
            }

            public bool with_access(ADUser ad_user)
            {
                if( ad_user == null ) return true;
                if (this.ad_users.Count == 0 && this.ad_departments.Count == 0 && this.ad_groups.Count == 0) return true;
                if (ad_user.inAnyUser(this.ad_users)) return true;
                if (ad_user.inAnyDepartments(this.ad_departments)) return true;
                if (ad_user.inAnyGroups(this.ad_groups)) return true;
                return false;
            }
        }

        private static List<Nav> sub_main_navs()
        {
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "../Default.aspx"; nav.text = "Main Home"; navs.Add(nav);
            nav = new Nav(); nav.link = "../TaskList/Demand.aspx"; nav.text = "Task List"; navs.Add(nav);
            nav = new Nav(); nav.link = "../Metrics/Claims.aspx"; nav.text = "Metrics"; navs.Add(nav);
            nav.ad_departments.Add("mis");
            nav.ad_groups.Add("servicing_fc"); nav.ad_groups.Add("foreclosure_metrics");
            nav = new Nav(); nav.link = "../Utilities/PreservationCodes.aspx"; nav.text = "Utilities"; navs.Add(nav);
            nav = new Nav(); nav.link = "../Default.aspx"; nav.text = "Dashboard"; navs.Add(nav);
            return navs;
        }

        public static string sub_main_nav_select(string selected_text, ADUser ad_user)
        {
            string s = nav_start;

            foreach (Nav n in sub_main_navs())
            {
                s += n.link_html(selected_text, ad_user);
            }
            return s + nav_end;
        }

        private static List<Nav> foreclosure_sub_main_navs()
        {
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "../Default.aspx"; nav.text = "Main Home"; navs.Add(nav);
            nav = new Nav(); nav.link = "../Workflow/Default.aspx"; nav.text = "Workflow"; navs.Add(nav);
            nav = new Nav(); nav.link = "../Metrics/Claims.aspx"; nav.text = "Metrics"; navs.Add(nav);
            nav.ad_departments.Add("mis");
            nav.ad_groups.Add("servicing_fc"); nav.ad_groups.Add("foreclosure_metrics");
            nav = new Nav(); nav.link = "../Utilities/PreservationCodes.aspx"; nav.text = "Utilities"; navs.Add(nav);
            nav = new Nav(); nav.link = "../Default.aspx"; nav.text = "Dashboard"; navs.Add(nav);
            return navs;
        }

        public static string foreclosure_sub_main_nav_select(string selected_text, ADUser ad_user)
        {
            string s = nav_start;

            foreach (Nav n in foreclosure_sub_main_navs())
            {
                s += n.link_html(selected_text, ad_user);
            }
            return s + nav_end;
        }

        private static List<Nav> main_navs()
        {
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "Default.aspx"; nav.text = "Main Home"; navs.Add(nav);
            nav = new Nav(); nav.link = "TaskList/Demand.aspx"; nav.text = "Task List"; navs.Add(nav);
            nav = new Nav(); nav.link = "Metrics/Claims.aspx"; nav.text = "Metrics"; navs.Add(nav);
            nav.ad_departments.Add("mis");
            nav.ad_groups.Add("servicing_fc"); nav.ad_groups.Add("foreclosure_metrics");
            nav = new Nav(); nav.link = "Utilities/PreservationCodes.aspx"; nav.text = "Utilities"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx"; nav.text = "Dashboard"; navs.Add(nav);
            return navs;
        }

        public static string main_nav_select(string selected_text, ADUser ad_user)
        {
            string s = nav_start;

            foreach (Nav n in main_navs())
            {
                s += n.link_html(selected_text, ad_user);
            }
            return s + nav_end;
        }

        public static bool main_nav_access(string selected_text, ADUser ad_user)
        {
            foreach (Nav n in main_navs())
            {
                if (n.text == selected_text) return n.with_access(ad_user);
            }
            return true;
        }

        private static List<Nav> foreclosure_main_navs()
        {
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "Default.aspx"; nav.text = "Main Home"; navs.Add(nav);
            nav = new Nav(); nav.link = "Workflow/Default.aspx"; nav.text = "Workflow"; navs.Add(nav);
            nav = new Nav(); nav.link = "Metrics/Claims.aspx"; nav.text = "Metrics"; navs.Add(nav);
            nav.ad_departments.Add("mis");
            nav.ad_groups.Add("servicing_fc"); nav.ad_groups.Add("foreclosure_metrics");
            nav = new Nav(); nav.link = "Utilities/PreservationCodes.aspx"; nav.text = "Utilities"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx"; nav.text = "Dashboard"; navs.Add(nav);
            return navs;
        }

        public static string foreclosure_main_nav_select(string selected_text, ADUser ad_user)
        {
            string s = nav_start;

            foreach (Nav n in foreclosure_main_navs())
            {
                s += n.link_html(selected_text, ad_user);
            }
            return s + nav_end;
        }

        public static bool foreclosure_main_nav_access(string selected_text, ADUser ad_user)
        {
            foreach (Nav n in main_navs())
            {
                if (n.text == selected_text) return n.with_access(ad_user);
            }
            return true;
        }

        public static string task_list_nav =
    @"<div class=""navbar"">
       <div class=""navbar-inner"">
        <ul class=""nav"">
          <li>
            <a href=""Demand.aspx"">Demand Queue</a>
          </li>
          <li>
            <a href=""PreForeclosure.aspx"">Pre-Foreclosure Queue</a>
          </li>
          <li>
            <a href=""Foreclosure.aspx"">Foreclosure Queue</a>
          </li>
        </ul>
       </div>
      </div>";


        public static string task_list_nav_select(string active)
        {
            string s = nav_start;
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "Demand.aspx"; nav.text = "Demand Queue"; navs.Add(nav);
            nav = new Nav(); nav.link = "PreForeclosure.aspx"; nav.text = "Pre-Foreclosure Queue"; navs.Add(nav);
            nav = new Nav(); nav.link = "Foreclosure.aspx"; nav.text = "Foreclosure Queue"; navs.Add(nav);
            string active_class = "";
            foreach (Nav n in navs)
            {
                if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                s += @"<li" + active_class + @"><a href=""" + n.link + @""">" + n.text + "</a></li>";
            }
            return s + nav_end;
        }

        public static string metrics_list_nav_select(string active)
        {
            string s = nav_start;
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "Claims.aspx"; nav.text = "Claims"; navs.Add(nav);
            nav = new Nav(); nav.link = "Foreclosures.aspx"; nav.text = "Foreclosures By Sale Date Completed"; navs.Add(nav);
            nav = new Nav(); nav.link = "FannieMaeCalls.aspx"; nav.text = "Fannie Mae Calls"; navs.Add(nav);
            string active_class = "";
            foreach (Nav n in navs)
            {
                if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                s += @"<li" + active_class + @"><a href=""" + n.link + @""">" + n.text + "</a></li>";
            }
            return s + nav_end;
        }

        public static string workflow_list_nav_select(string active, string version)
        {
            string s = nav_start;
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "Default.aspx?workflow=Demands&version=" + version; nav.text = "Demands"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=Demands that Became Current&version=" + version; nav.text = "Demands that Became Current"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=Removed Demands&version=" + version; nav.text = "Removed Demands"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=PreForeclosures&version=" + version; nav.text = "PreForeclosures"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=Foreclosures&version=" + version; nav.text = "Foreclosures"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=Foreclosed&version=" + version; nav.text = "Foreclosed"; navs.Add(nav);
            nav = new Nav(); nav.link = "History.aspx?version=" + version; nav.text = "History"; navs.Add(nav);
            string active_class = "";
            foreach (Nav n in navs)
            {
                if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                s += @"<li" + active_class + @"><a href=""" + n.link + @""">" + n.text + "</a></li>";
            }
            return s + nav_end;
        }


        public static string utilities_nav =
    @"<div class=""navbar"">
       <div class=""navbar-inner"">
        <ul class=""nav"">
          <li>
            <a href=""PreservationCodes.aspx"">Preservation Codes</a>
          </li>
          <li>
            <a href=""Disbursements.aspx"">Disbursements</a>
          </li>
        </ul>
       </div>
      </div>";

        public static string metrics_nav =
    @"<div class=""navbar"">
       <div class=""navbar-inner"">
        <ul class=""nav"">
          <li>
            <a href=""Claims.aspx"">Claims</a>
          </li>
        </ul>
       </div>
      </div>";
    }
}