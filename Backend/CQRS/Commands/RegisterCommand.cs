using Backend.CQRS.CommandsResults;
using Backend.Utils.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Backend.CQRS.Commands
{
    public class RegisterCommand : ICommand, IRequest<RegisterCommandResult>
    {
        public CQRSRole Permission { get; } = CQRSRole.All;
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public bool isProffesor { get; set; } = false;
    }
}
