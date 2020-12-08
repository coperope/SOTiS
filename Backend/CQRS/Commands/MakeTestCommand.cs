using Backend.CQRS.CommandsResults;
using Backend.Entities;
using Backend.Utils.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.CQRS.Commands
{
    public class MakeTestCommand : ICommand, IRequest<MakeTestCommandResult>
    {
        public CQRSRole Permission { get; } = CQRSRole.Professor;
        public int? UserId { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public int ProfessorId { get; set; }
        [Required]
        public int KnowledgeSpaceId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public ICollection<Question> Questions { get; set; }
    }
}
