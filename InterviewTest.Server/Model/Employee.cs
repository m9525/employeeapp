using System.Data;
using System.Text.Json.Serialization;

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

        [JsonConstructor]
        public Employee(int id, string name, int value)
        {
            Id = id;
            Name = name;
            Value = value;
        }

        public Employee(IDataReader reader)
        {
            if (reader != null && reader.FieldCount >= 3)
            {
                Id = reader.GetInt32(0);
                Name = reader.GetString(1);
                Value = reader.GetInt32(2);
            }
        }
    }
}
