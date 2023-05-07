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

        public StudentService()
        {
            _dataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Students.json");
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            using StreamReader reader = new StreamReader(_dataPath);
            string json = await reader.ReadToEndAsync();
            return JsonConvert.DeserializeObject<List<Student>>(json);
        }

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
