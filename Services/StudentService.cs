using ASP_NET_Simulation.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_NET_Simulation.Services
{
    public class StudentService
    {
        private readonly string _dataPath;

        // Constructor for the StudentService class.
        // Initializes the _dataPath variable with the path to the Students.json file.
        public StudentService()
        {
            _dataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Students.json");
        }

        // Method to fetch the list of students asynchronously.
        // Reads the contents of the Students.json file, deserializes the JSON, and returns a list of Student objects.
        public async Task<List<Student>> GetStudentsAsync()
        {
            using StreamReader reader = new StreamReader(_dataPath);
            string json = await reader.ReadToEndAsync();
            return JsonConvert.DeserializeObject<List<Student>>(json);
        }

        // Method to update a student's information asynchronously.
        // Accepts a Student object with updated information, finds the matching student in the list,
        // replaces the old student data with the updated data, and saves the changes to the Students.json file.
        public async Task UpdateStudentAsync(Student updatedStudent)
        {
            List<Student> students = await GetStudentsAsync();
            int index = students.FindIndex(s => s.Id == updatedStudent.Id);
            if (index != -1)
            {
                students[index] = updatedStudent;
                string json = JsonConvert.SerializeObject(students, Formatting.Indented);
                await File.WriteAllTextAsync(_dataPath, json);
            }
        }
    }

}
