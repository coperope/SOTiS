using Backend.CQRS.Commands;
using Backend.CQRS.CommandsResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.CQRS.Processors
{
    public class CommandProcessor : ICommandProcessor
    {
        public ICommandResult Execute(ICommand command)
        {
            // get claim (role name) from token

            // check if command accepts that role, else throw error

            // send via mediatr

            throw new NotImplementedException();
        }
    }
}
