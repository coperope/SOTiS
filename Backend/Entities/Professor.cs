using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Backend.Entities
{
    public class Professor
    {
        public int ProfessorId { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Test> Tests { get; set; }
    }
}
