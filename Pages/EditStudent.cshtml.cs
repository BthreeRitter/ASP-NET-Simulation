using ASP_NET_Simulation.Models;
using ASP_NET_Simulation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_NET_Simulation.Pages
{
    public class EditStudentModel : PageModel
    {
        private readonly StudentService _studentService;

        public EditStudentModel(StudentService studentService)
        {
            _studentService = studentService;
        }

        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }

        [BindProperty]
        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!Id.HasValue)
            {
                return NotFound();
            }

            var students = await _studentService.GetStudentsAsync();
            Student = students.Find(s => s.Id == Id.Value);

            if (Student == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        Console.WriteLine($"Key: {modelState.Key}, Error: {error.ErrorMessage}");
                    }
                }

                return Page();
            }

            await _studentService.UpdateStudentAsync(Student);
            return RedirectToPage("studentinfo"); // This line redirects you back to the StudentInfo page
        }

    }
}
