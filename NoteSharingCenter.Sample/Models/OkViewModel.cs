using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteSharingCenter.Sample.Models
{
    public class OkViewModel : AppearanceModels<string>
    {
        public OkViewModel()
        {
            Title = "Transaction Successful";
        }
    }
}