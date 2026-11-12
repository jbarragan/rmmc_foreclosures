using System;
using System.Collections.Generic;
using System.Web;

using com.sp.rmmc.common.models;
using com.sp.rmmc.common.tools;

namespace com.sp.rmmc.common.models.events
{
    public class Event : ICSVExport
    {
        public MsLoan loan = new MsLoan();
        public static int WARNING = 0;
        public static int ALERT = 1;
        public String name = "";
        public String description = "";
        public int type = WARNING;
        public DateTimeObject date = new DateTimeObject();
        public string parent_type = "";


        public string getEventStatusColor()
        {
            string s = "";            
            if (type == Event.WARNING) s = "warning";
            if (type == Event.ALERT) s = "error"; 
            return s;
        }

        public string typeToString()
        {
            if (this.type == Event.WARNING) return "Warning";
            if (this.type == Event.ALERT) return "Alert";
            return "";
        }

        public String to_csv()
        {
            return this.loan.loan_id.ToString() + ",\"" +
                   this.loan.loan_name + "\",\"" +
                   this.loan.loan_type + "\",\"" +
                   ((this.type == WARNING) ? "Warning" : "Alert") + "\",\"" +
                   this.parent_type + "\",\"" +
                   this.name + "\",\"" +
                   this.description + "\","; 
        }
    }
}