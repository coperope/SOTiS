using Backend.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.CQRS.Commands
{
    public class TakeTestCommand : ICommand
    {
        public CQRSRole Permission { get; } = CQRSRole.Student;
    }
}
