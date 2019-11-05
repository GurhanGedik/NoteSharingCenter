using NoteSharingCenter.Entity.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteSharingCenter.Repository
{
    public class RepositoryLayerResult<T> where T : class
    {
        public List<ErrorMessageObj> Errors { get; set; }
        public T Result { get; set; }

        public RepositoryLayerResult()
        {
            Errors = new List<ErrorMessageObj>();
        }

        public void AddError(ErrorMessageCode code, string message)
        {
            Errors.Add(new ErrorMessageObj() { Code = code, Message = message });
        }
    }
}
