using AllProjects.Models;
using AllProjects.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AllProjects.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentOperation studentOperation;

        public StudentController(IStudentOperation _studentOperation)
        {
            studentOperation = _studentOperation;
        }
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            if (student is null)
            {
                return BadRequest();
            }
            if (studentOperation.CreateStudent(student) == 1)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Student student = studentOperation.GetStudentById(id);
            if (student is null)
            {
                return base.BadRequest();
            }

            return Ok(student);
        }
        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok(studentOperation.GetAllStudents());
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = studentOperation.GetStudentById(id);
            if (id <= 0)
            {
                return BadRequest(student);
            }
            if (studentOperation.DeleteStudent(id) == 1)
            {
                return Ok();
            }
            return NotFound();
        }
        [HttpPut]
        public IActionResult UpdateStudent(int id, Student student)
        {
            if (id <= 0 && student is null)
            {
                return BadRequest();
            }
            if (studentOperation.UpdateStudent(id, student) == 1)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
