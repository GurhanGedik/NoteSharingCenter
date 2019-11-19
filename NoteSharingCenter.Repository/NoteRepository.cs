using NoteSharingCenter.Entity;
using NoteSharingCenter.Entity.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteSharingCenter.Repository
{
    public class NoteRepository : ManagerBase<Note>
    {
        public override int Delete(Note note)
        {
            NoteRepository nr = new NoteRepository();
            LikedRepository lr = new LikedRepository();
            CommentRepository cr = new CommentRepository();
            UserRepository ur = new UserRepository();

            List<Liked> Likedd = lr.List(x => x.Note.Id == note.Id);
            List<Comment> Comment = cr.List(x => x.Note.Id == note.Id);
            foreach (var item in Likedd)
            {
                lr.Delete(item);
            }
            foreach (var item in Comment)
            {
                cr.Delete(item);
            }

            return base.Delete(note);
        }

        public RepositoryLayerResult<Note> Updatee(Note data)
        {
            RepositoryLayerResult<Note> layerResult = new RepositoryLayerResult<Note>();
            layerResult.Result= Find(x => x.Id == data.Id);
            layerResult.Result.IsDraft = data.IsDraft;
            layerResult.Result.CategoryId = data.CategoryId;
            layerResult.Result.Text = data.Text;
            layerResult.Result.Title = data.Title;

            if (string.IsNullOrEmpty(data.NoteImageFilename) == false)
            {
                layerResult.Result.NoteImageFilename = data.NoteImageFilename;
            }

            if (base.Update(layerResult.Result) == 0)
            {
                layerResult.AddError(ErrorMessageCode.UserCouldNotUpdate, "Failed to update note.");
            }

            return layerResult;
        }

    }
}
