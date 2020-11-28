using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.CQRS.CommandsResults
{
    public class SubmitTestCommandResult : ICommandResult
    {
        public bool Success { get; set; }
        public int EnrolementId { get; set; }
    }
}
