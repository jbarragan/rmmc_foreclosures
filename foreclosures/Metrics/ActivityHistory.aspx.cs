using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.foreclosures.models;
using com.sp.rmmc.common.models;

public partial class lossmitigation_Collections_Default : System.Web.UI.Page
{
    protected class ActivityByMonth
    {
        public DateTime date = new DateTime();
        public string history_id = "";
        public bool is_current = false;

        public List<BaseForeclosure> demands = new List<BaseForeclosure>();
        public List<BaseForeclosure> preforeclosures = new List<BaseForeclosure>();
        public List<BaseForeclosure> foreclosures = new List<BaseForeclosure>();
        public List<BaseForeclosure> foreclosed = new List<BaseForeclosure>();
        public List<BaseForeclosure> deed_in_lieu = new List<BaseForeclosure>();
        private List<BaseForeclosure> pre_and_foreclosures_memory = null;
        private List<BaseForeclosure> total_memory = null;
        public ActivityByMonth(DateTime date, string history_id)
        {
            this.date = date;
            this.history_id = history_id;
        }

        public void add(ActivityByMonth a)
        {
            this.demands.AddRange(a.demands);
            this.preforeclosures.AddRange(a.preforeclosures);
            this.foreclosures.AddRange(a.foreclosures);
            this.foreclosed.AddRange(a.foreclosed);
            this.deed_in_lieu.AddRange(a.deed_in_lieu);
        }

        public List<BaseForeclosure> pre_and_foreclosures
        {
            get
            {
                if( pre_and_foreclosures_memory != null ) return pre_and_foreclosures_memory;
                pre_and_foreclosures_memory = new List<BaseForeclosure>();
                pre_and_foreclosures_memory.AddRange(foreclosures);
                pre_and_foreclosures_memory.AddRange(preforeclosures);
                return pre_and_foreclosures_memory;
            }
        }

        public List<BaseForeclosure> total
        {
            get
            {
                if( total_memory != null ) return total_memory;
                total_memory = new List<BaseForeclosure>();
                total_memory.AddRange(demands);
                total_memory.AddRange(pre_and_foreclosures);
                total_memory.AddRange(foreclosed);
                total_memory.AddRange(deed_in_lieu);
                return total_memory;
            }
        }

        public string activity_title
        {
            get { return is_current ? "Current" : date.ToString("MMMM, yyyy"); }
        }
    }

    protected List<ActivityByMonth> activity_by_month_list = new List<ActivityByMonth>();

    protected void Page_Load(object sender, EventArgs e)
    {
        populateActivityByMonthList();
    }

    protected void populateActivityByMonthList()
    {
        ActivityByMonth ca = new ActivityByMonth(DateTime.Today, "");
        ca.is_current = true;
        BaseForeclosure cquery_foreclosure = new CurrentForeclosure();
        cquery_foreclosure.getDemands(ca.demands, new List<BaseForeclosure>());
        cquery_foreclosure.getPreForeclosures(ca.preforeclosures, new List<BaseForeclosure>());
        cquery_foreclosure.getForeclosures(ca.foreclosures, new List<BaseForeclosure>());
        cquery_foreclosure.getLastMonthForeclosed(ca.foreclosed, new List<BaseForeclosure>(), DateTime.Today);
        cquery_foreclosure.getInProcessDeedInLieu(ca.deed_in_lieu, new List<BaseForeclosure>());
        activity_by_month_list.Add(ca);
        List<History> hs = (new History()).last_of_month_all();
        foreach( History h in hs )
        {
            ActivityByMonth a = new ActivityByMonth(h.history_date.date, h.id.ToString());
            BaseForeclosure query_foreclosure = new HistoryForeclosure(h);
            query_foreclosure.getDemands(a.demands, new List<BaseForeclosure>());
            query_foreclosure.getPreForeclosures(a.preforeclosures, new List<BaseForeclosure>());
            query_foreclosure.getForeclosures(a.foreclosures, new List<BaseForeclosure>());
            query_foreclosure.getLastMonthForeclosed(a.foreclosed, new List<BaseForeclosure>(), h.history_date.date);
            query_foreclosure.getInProcessDeedInLieu(a.deed_in_lieu, new List<BaseForeclosure>());
            activity_by_month_list.Add(a);
        }
    }

}