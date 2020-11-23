using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Backend.Entities
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public ICollection<Enrolement> Enrolements { get; set; }

    }
}
