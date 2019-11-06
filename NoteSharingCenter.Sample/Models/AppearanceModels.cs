using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteSharingCenter.Sample.Models
{
    public class AppearanceModels<T>
    {
        public List<T> Items { get; set; }
        public string Header { get; set; }
        public string Title { get; set; }
        public bool IsRedirecting { get; set; }
        public string RedirectinUrl { get; set; }
        public int RedirectingTimeout { get; set; }

        public AppearanceModels()
        {
            Header = "Redirecting...";
            Title = "Invalid Transaction";
            IsRedirecting = true;
            RedirectinUrl = "/Home/Index";
            RedirectingTimeout = 10;
            Items = new List<T>();
        }
    }
}