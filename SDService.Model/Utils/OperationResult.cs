using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDService.Model.Utils
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public OperationResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
    }

    public class OperationResult<TEntity> : OperationResult
    {
        public TEntity Entity { get; set; }
        public OperationResult(bool isSuccess)
            : base(isSuccess) { }
    }
}
