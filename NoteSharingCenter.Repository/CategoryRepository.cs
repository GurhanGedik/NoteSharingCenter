using NoteSharingCenter.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteSharingCenter.Repository
{
    public class CategoryRepository : ManagerBase<Category>
    {
        public override int Delete(Category category)
        {
            NoteRepository nr = new NoteRepository();
            LikedRepository lr = new LikedRepository();
            CommentRepository cr = new CommentRepository();

            foreach (Note note in category.Notes.ToList())
            {
                foreach (Liked like in note.Likes.ToList())
                {
                    lr.Delete(like);
                }

                foreach (Comment comment in note.Comments.ToList())
                {
                    cr.Delete(comment);
                }

                nr.Delete(note);
            }

            return base.Delete(category);
        }
    }
}
