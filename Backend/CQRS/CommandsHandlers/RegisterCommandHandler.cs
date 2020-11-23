using Backend.CQRS.Commands;
using Backend.CQRS.CommandsResults;
using Backend.CQRS.Queries;
using Backend.CQRS.QueriesResults;
using Backend.Data.Repositories.Interfaces;
using Backend.Entities;
using Backend.Utils.AppSettingsClasses;
using Backend.Utils.Enums;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.CQRS.CommandsHandlers

{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterCommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSecret _jwtSecret;

        public RegisterCommandHandler(IUserRepository userRepository, IOptions<JwtSecret> jwtSecret)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtSecret = jwtSecret.Value ?? throw new ArgumentNullException(nameof(jwtSecret));
        }

        public async Task<RegisterCommandResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByUsername(request.Username);
            if (user != null)
            {
                return null;
            }
            if (request.isProffesor)
            {
                Professor professor = new Professor
                {
                    Username = request.Username,
                    Password = request.Password,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email
                };
                var result = await _userRepository.AddProfessor(professor);
                return new RegisterCommandResult
                {
                    Id = result.Entity.ProfessorId,
                    FirstName = result.Entity.FirstName,
                    LastName = result.Entity.LastName,
                    Username = result.Entity.Username
                };
            }
            else
            {
                Student student = new Student
                {
                    Username = request.Username,
                    Password = request.Password,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email
                };
                var result = await _userRepository.AddStudent(student);
                return new RegisterCommandResult
                {
                    Id = result.Entity.StudentId,
                    FirstName = result.Entity.FirstName,
                    LastName = result.Entity.LastName,
                    Username = result.Entity.Username
                };
            }
        }
    }
}
