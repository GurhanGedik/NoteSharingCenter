using NoteSharingCenter.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteSharingCenter.Repository
{
    public class CategoryRepository
    {
        private Repository<Category> Category = new Repository<Category>();

        public List<Category> GetCategories()
        {
            return Category.List();
        }

        public Category GetCategoryById(int id)
        {
            return Category.Find(X => X.Id == id);
        }
    }
}
