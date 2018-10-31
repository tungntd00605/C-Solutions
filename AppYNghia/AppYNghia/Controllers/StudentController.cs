using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppYNghia.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppYNghia.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentContext _context;

        public StudentController(StudentContext context)
        {
            _context = context;
        }

        [HttpGet("/student")]
        public IActionResult Index()
        {
            return new JsonResult(_context.Students.ToList());
        }

        [HttpGet("/student/{id:int}")]
        public IActionResult Get_One(long id)
        {
            return new JsonResult(_context.Students.Find(id));
        }

        [HttpGet("/student/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Store(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            return new JsonResult(student);
        }

        [HttpPut("/student/{id:int}")]  
        public IActionResult Update(long id, Student student)
        {
            var oldStudent = _context.Students.Find(id);
            if (oldStudent == null)
            {
                return NotFound();
            }

            oldStudent.Name = student.Name;
            oldStudent.RollNumber = student.RollNumber;
            _context.Students.Update(oldStudent);
            _context.SaveChanges();
            return new JsonResult(_context.Students.Find(id));
        }

        [HttpDelete("/student/{id:int}")]
        public IActionResult Delete(long id)
        {
            var removeStudent = _context.Students.Find(id);
            if (removeStudent == null)
            {
                return NotFound();
            }
            _context.Students.Remove(removeStudent);
            _context.SaveChanges();
            return new JsonResult(_context.Students.Find(id));
        }
    }
}