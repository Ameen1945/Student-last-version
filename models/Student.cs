using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace Student_last_version.models
{
    public class Student
    {
        [Key]
        public int Id{ get; set; }

        public int? Age{ get; set; }


        public string? Name{ get; set; }










    }
}
