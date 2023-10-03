using MsaTec.Abstractions.Application.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsaTec.Application.Mediators
{
    public class CommandResult : ICommandResult
    {
        public dynamic Result { get; set; }
        public List<string> MessageError { get; set; } = new List<string>();
        public bool IsSuccess { get ; set ; }
    }
}
