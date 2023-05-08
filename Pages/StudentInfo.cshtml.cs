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
        public string SearchQuery { get; set; }
        public string FinancialField { get; set; }
        public string ComparisonType { get; set; }
        public decimal? FinancialValue { get; set; }


        public StudentInfoModel(StudentService studentService)
        {
            _studentService = studentService;
        }

        public List<Student> Students { get; set; }

        public async Task OnGetAsync(string searchQuery, string financialField, string comparisonType, decimal? financialValue)
        {
            SearchQuery = searchQuery;
            FinancialField = financialField;
            ComparisonType = comparisonType;
            FinancialValue = financialValue;

            var students = await _studentService.GetStudentsAsync();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                students = students.Where(s => s.Name.IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            if (!string.IsNullOrEmpty(financialField) && !string.IsNullOrEmpty(comparisonType) && financialValue.HasValue)
            {
                students = students.Where(s => FilterByFinancialCondition(s, financialField, comparisonType, financialValue.Value)).ToList();
            }

            Students = students;
        }

        private bool FilterByFinancialCondition(Student student, string financialField, string comparisonType, decimal value)
        {
            decimal fieldValue = financialField switch
            {
                "balance" => student.Balance,
                "financialAid" => student.FinancialAid,
                "tuition" => student.Tuition,
                "fees" => student.Fees,
                "scholarships" => student.Scholarships,
                _ => throw new ArgumentException("Invalid financial field")
            };

            return comparisonType switch
            {
                "lessThan" => fieldValue < value,
                "equalTo" => fieldValue == value,
                "greaterThan" => fieldValue > value,
                _ => throw new ArgumentException("Invalid comparison type")
            };
        }


    }
}
