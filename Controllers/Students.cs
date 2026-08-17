using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_last_version.models;
using System.Security.Cryptography;

namespace Student_last_version.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class Students : ControllerBase
    {

        public Students(AppDBContexts db)
        {

            _db = db;

        }
        private readonly AppDBContexts _db;






        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var studs = await _db.Students.ToListAsync();
            return Ok(studs);
        }









        [HttpGet("{id}")]
        public async Task<IActionResult> GetSudentById([FromRoute] int id)
        {
            var c = await _db.Students.SingleOrDefaultAsync(x => x.Id == id);
            if (c == null)
            {
                return NotFound($"student Id {id} not exists");
            }
            return Ok(c);
        }







        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] Student stud)
        {

            await _db.Students.AddAsync(stud);


            await _db.SaveChangesAsync();


            return Ok(stud);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student std)
        {
            var c = await _db.Students.FindAsync(id);

            if (c == null)
            {
                return NotFound($" item id {id} not exists !");
            }



            c.Name = std.Name;
            c.Age = std.Age;

            _db.SaveChanges();

            return Ok(std);





        }





        [HttpDelete("{id}")]
        public async Task<ActionResult>DeleteStudent(int id)
        {

            var S = await _db.Students.SingleOrDefaultAsync(x => x.Id == id);

            if (S == null)
            {
                return NotFound($" item id {id} not exists !");
            }

            _db.Students.Remove(S);
            await _db.SaveChangesAsync();
            return Ok();







        }





















    }   



}   
    









