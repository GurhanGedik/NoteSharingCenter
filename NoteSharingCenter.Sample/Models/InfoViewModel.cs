using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteSharingCenter.Sample.Models
{
    public class InfoViewModel : AppearanceModels<string>
    {
        public InfoViewModel()
        {
            Title= "Information";
        }
    }
}