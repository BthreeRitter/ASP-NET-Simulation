using ASP_NET_Simulation.Models;
using ASP_NET_Simulation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_NET_Simulation.Pages
{
    public class EditStudentModel : PageModel
    {

        private readonly StudentService _studentService;

        // Constructor that takes an instance of StudentService class as a parameter.
        public EditStudentModel(StudentService studentService)
        {
            _studentService = studentService;
        }

        // Property to bind the query string parameter "id".
        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }

        // Property to bind the Student object.
        [BindProperty]
        public Student Student { get; set; }

        // Method that runs when the page is requested with HTTP GET verb.
        // It retrieves the student's record with the specified Id from the database and assigns it to the Student property.
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

        // Method that runs when the page is submitted with HTTP POST verb.
        // It updates the student's record in the database with the values provided in the form.
        // If the model state is invalid, it returns the page with validation errors.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Loop through the model state and print validation errors to the console.
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        Console.WriteLine($"Key: {modelState.Key}, Error: {error.ErrorMessage}");
                    }
                }

                return Page();
            }

            // Update the student's record in the database.
            await _studentService.UpdateStudentAsync(Student);

            // Redirect to the StudentInfo page.
            return RedirectToPage("studentinfo");
        }

    }
}
