using System.Threading.Tasks;
using ASP_NET_Simulation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASP_NET_Simulation.Models;
using ASP_NET_Simulation.Services;

namespace ASP_NET_Simulation.Pages
{
    public class StudentInfoModel : PageModel
    {
        private readonly StudentService _studentService;

        public StudentInfoModel(StudentService studentService)
        {
            _studentService = studentService;
        }

        public List<Student> Students { get; set; }

        public async Task OnGetAsync()
        {
            Students = await _studentService.GetStudentsAsync();
        }
    }
}
