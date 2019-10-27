using NoteSharingCenter.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteSharingCenter.Repository
{
    public class NoteRepository
    {
        private Repository<Note> note = new Repository<Note>();
        public  List<Note> GetAllNote()
        {
            return note.List();
        }
    }
}
