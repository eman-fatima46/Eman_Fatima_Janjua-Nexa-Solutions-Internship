using Microsoft.AspNetCore.Mvc;
using PartB_ASPNetCore_Basics.Models;

namespace PartB_ASPNetCore_Basics.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private static readonly List<Student> Students = new()
    {
        new Student
        {
            Id = 1,
            Name = "Ali",
            Department = "Computer Science"
        },

        new Student
        {
            Id = 2,
            Name = "Sara",
            Department = "Software Engineering"
        }
    };

    private readonly ILogger<StudentsController> _logger;

    public StudentsController(
        ILogger<StudentsController> logger)
    {
        _logger = logger;
    }


    [HttpGet]
    public IActionResult GetStudents()
    {
        _logger.LogInformation("Getting all students.");

        return Ok(Students);
    }


    [HttpGet("{id}")]
    public IActionResult GetStudent(int id)
    {
        Student? student = Students
            .FirstOrDefault(student => student.Id == id);

        if (student == null)
        {
            _logger.LogWarning(
                "Student with ID {StudentId} was not found.",
                id
            );

            return NotFound(
                $"Student with ID {id} was not found."
            );
        }

        return Ok(student);
    }


    [HttpPost]
    public IActionResult CreateStudent(Student student)
    {
        if (student.Id <= 0)
        {
            return BadRequest(
                "Student ID must be greater than zero."
            );
        }

        if (string.IsNullOrWhiteSpace(student.Name))
        {
            return BadRequest(
                "Student name is required."
            );
        }

        bool idExists = Students.Any(
            existingStudent =>
                existingStudent.Id == student.Id
        );

        if (idExists)
        {
            return BadRequest(
                "A student with this ID already exists."
            );
        }

        Students.Add(student);

        _logger.LogInformation(
            "Student {StudentName} was created.",
            student.Name
        );

        return CreatedAtAction(
            nameof(GetStudent),
            new { id = student.Id },
            student
        );
    }
}