using System.Data;

namespace InterviewTest.Server.Model
{
    public class Employee
    {
        /// <summary>
        /// Identity of employee, should be unique and auto-incremented by the database
        /// </summary>
        public int Id { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public Employee(IDataReader reader)
        {
            if (reader != null)
            {
                Id = reader.GetInt32(0);
                Name = reader.GetString(1);
                Value = reader.GetInt32(2);
            }
        }
    }
}
