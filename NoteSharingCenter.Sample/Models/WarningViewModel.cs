using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteSharingCenter.Sample.Models
{
    public class WarningViewModel : AppearanceModels<string>
    {
        public WarningViewModel()
        {
            Title = "Warning!";
        }
    }
}