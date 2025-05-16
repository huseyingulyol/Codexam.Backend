//using Codexam.WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Codexam.WebAPI.Entities;

namespace Codexam.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ExamController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExamController(AppDbContext context)
        {
            _context = context;
        }
        

        [HttpGet("/api/Exams")]
        public async Task<ActionResult<IEnumerable<Exam>>> GetExams()
        {
            return await _context.Exams.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Exam>> GetExam(int id)
        {
            var exam = await _context.Exams.FindAsync(id);

            if (exam == null)
            {
                return NotFound();
            }

            return exam;
        }

        [HttpPost]
        public async Task<ActionResult<Exam>> CreateExam(int userId, string name)
        {
            _context.Exams.Add(new Exam() { Name = name, UserId = userId});
            await _context.SaveChangesAsync();
            return Ok(new { message = "Sınav oluşturuldu.", userId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExam(int id, Exam exam)
        {
            if (id != exam.Id)
            {
                return BadRequest();
            }

            _context.Entry(exam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Exams.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExam(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
