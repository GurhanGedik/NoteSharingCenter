using NoteSharingCenter.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteSharingCenter.Repository
{
    public class LikedRepository
    {
        private Repository<Liked> Liked = new Repository<Liked>();

        public List<Liked> GetCategories()
        {
            return Liked.List();
        }

    }
}
