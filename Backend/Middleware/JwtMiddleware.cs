using Backend.Data.Repositories.Interfaces;
using Backend.Entities;
using Backend.Utils.AppSettingsClasses;
using Backend.Utils.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSecret _jwtSecret;

        public JwtMiddleware(RequestDelegate next, IOptions<JwtSecret> appSettings)
        {
            _next = next;
            _jwtSecret = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserRepository userRepository)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context, token, userRepository);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, string token, IUserRepository userRepository)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSecret.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var username = (jwtToken.Claims.First(x => x.Type == "Username").Value).ToString();

                // attach user to context on successful jwt validation
                populateContext(username, context, userRepository);
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }

        private async void populateContext(string username, HttpContext context, IUserRepository userRepository)
        {
            var user = await userRepository.GetUserByUsername(username);

            if (user == null)
            {
                return;
            }

            if (user.GetType() == typeof(Student))
            {
                context.Items["UserId"] = ((Student)user).StudentId;
                context.Items["UserRole"] = CQRSRole.Student;

            } else if (user.GetType() == typeof(Professor))
            {
                context.Items["UserId"] = ((Professor)user).ProfessorId;
                context.Items["UserRole"] = CQRSRole.Professor;
            }

        }
    }
}
