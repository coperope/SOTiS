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

namespace Backend.CQRS.QueriesHandlers
{
    public class AuthenticateQueryHandler : IRequestHandler<AuthenticateQuery, AuthenticateQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSecret _jwtSecret;

        public AuthenticateQueryHandler(IUserRepository userRepository, IOptions<JwtSecret> jwtSecret)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtSecret = jwtSecret.Value ?? throw new ArgumentNullException(nameof(jwtSecret));
        }

        public async Task<AuthenticateQueryResult> Handle(AuthenticateQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByUsername(request.Username);

            string password = null;

            if (user == null)
            {
                return null;
            } 
            else if (user.GetType() == typeof(Student))
            {
                password = ((Student)user).Password;

            }
            else if (user.GetType() == typeof(Professor))
            {
                password = ((Professor)user).Password;
            }

            if (!request.Password.Equals(password))
            {
                return null;
            }

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            if (user.GetType() == typeof(Student))
            {
                Student st = (Student)user;

                return new AuthenticateQueryResult
                {
                    Id = st.StudentId,
                    Token = token,
                    FirstName = st.FirstName,
                    LastName = st.LastName,
                    Username = st.Username
                };
            }
            else 
            {
                Professor prof = (Professor)user;

                return new AuthenticateQueryResult
                {
                    Id = prof.ProfessorId,
                    Token = token,
                    FirstName = prof.FirstName,
                    LastName = prof.LastName,
                    Username = prof.Username
                };
            }

        }

        private string generateJwtToken(object user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret.Secret);

            if (user.GetType() == typeof(Student))
            {
                Student st = (Student)user;
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("Username", st.Username) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            else
            {
                Professor prof = (Professor)user;
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("Username", prof.Username) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }

            
        }
    }
}
