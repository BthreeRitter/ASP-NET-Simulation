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
        // Declare a private readonly field for the StudentService
        private readonly StudentService _studentService;
        // Declare properties for search and filter criteria
        public string SearchQuery { get; set; }
        public string FinancialField { get; set; }
        public string ComparisonType { get; set; }
        public decimal? FinancialValue { get; set; }

        // Constructor to initialize the StudentService instance
        public StudentInfoModel(StudentService studentService)
        {
            _studentService = studentService;
        }

        // Property to hold the list of students
        public List<Student> Students { get; set; }

        // Method to handle GET requests asynchronously.
        // This method fetches the list of students from the StudentService and filters it
        // based on search query and financial conditions provided through the request parameters.
        public async Task OnGetAsync(string searchQuery, string financialField, string comparisonType, decimal? financialValue)
        {
            // Assign the values from the request to the properties.
            // This allows retaining the current filter settings in the UI.
            SearchQuery = searchQuery;
            FinancialField = financialField;
            ComparisonType = comparisonType;
            FinancialValue = financialValue;

            // Get the list of students from the service.
            var students = await _studentService.GetStudentsAsync();

            // Filter the students by search query if provided.
            // We use StringComparison.OrdinalIgnoreCase for case-insensitive search.
            if (!string.IsNullOrEmpty(searchQuery))
            {
                students = students.Where(s => s.Name.IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            // Filter the students by financial condition if provided.
            // We only apply the filter if all required parameters are present and valid.
            if (!string.IsNullOrEmpty(financialField) && !string.IsNullOrEmpty(comparisonType) && financialValue.HasValue)
            {
                students = students.Where(s => FilterByFinancialCondition(s, financialField, comparisonType, financialValue.Value)).ToList();
            }

            // Assign the filtered list of students to the Students property.
            // This allows displaying the filtered list in the Razor Page.
            Students = students;
        }

        // Method to filter students based on financial condition.
        // This method takes a Student object, a financial field, a comparison type, and a value as parameters.
        // It compares the specified financial field value with the provided value using the selected comparison operator.
        private bool FilterByFinancialCondition(Student student, string financialField, string comparisonType, decimal value)
        {
            // Get the value of the specified financial field using a switch expression.
            // If the financial field is not recognized, an ArgumentException is thrown.
            decimal fieldValue = financialField switch
            {
                "balance" => student.Balance,
                "financialAid" => student.FinancialAid,
                "tuition" => student.Tuition,
                "fees" => student.Fees,
                "scholarships" => student.Scholarships,
                _ => throw new ArgumentException("Invalid financial field")
            };

            // Compare the fieldValue and the value based on the specified comparison type using a switch expression.
            // If the comparison type is not recognized, an ArgumentException is thrown.
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
