using NoteSharingCenter.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteSharingCenter.Repository
{
    public class RepositoryBase
    {
        protected static DatabaseContext db;
        private static object _lock = new object();

        protected RepositoryBase()
        {
           CreateContext();
        }

        private static void CreateContext()
        {
            if (db == null)
            {
                lock (_lock)
                {
                    if (db == null)
                    {
                        db = new DatabaseContext();
                    }
                }
            }
        }
    }
}
