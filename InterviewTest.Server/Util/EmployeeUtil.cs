using InterviewTest.Server.Model;
using Microsoft.Data.Sqlite;

namespace InterviewTest.Server.Util
{
    /// <summary>
    /// Database, extension, etc for Employee related operations
    /// </summary>
    public static class EmployeeUtil
    {        
        private static readonly string EmployeeConnectionString = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" }.ConnectionString;
        // private static readonly string EmployeeConnectionString = new SqliteConnectionStringBuilder() { DataSource = "C://repos/employeeapp/InterviewTest.Server/SqliteDB.db" }.ConnectionString;

        public static List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();       

            using (var connection = new SqliteConnection(EmployeeConnectionString))
            {
                connection.Open();

                var queryCmd = connection.CreateCommand();
                queryCmd.CommandText = @"SELECT ID, Name, Value FROM Employees";
                using (var reader = queryCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee(reader));                        
                    }
                }
            }
            return employees;
        }

        /// <summary>
        /// Increment the field `Value` by 1 where the field `Name` starts with ‘E’, by 10 where `Name` starts with ‘G’ and all others by 100.
        /// </summary>
        /// <returns></returns>
        public static void BulkValueIncrement()
        {
            var query = "UPDATE Employees SET Value = CASE WHEN Name LIKE 'E%' THEN Value + 1 WHEN Name LIKE 'G%' THEN Value + 10 ELSE Value + 100 END";
            
            using (var connection = new SqliteConnection(EmployeeConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var queryCmd = connection.CreateCommand();
                    queryCmd.CommandText = query;
                    queryCmd.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
            // TODO: best to return List<Employee> with updated values, but for now just return void           
        }

        public static Employee GetEmployee(int id)
        {            
            using (var connection = new SqliteConnection(EmployeeConnectionString))
            {
                connection.Open();
                var queryCmd = connection.CreateCommand();
                queryCmd.CommandText = @"SELECT ID, Name, Value FROM Employees WHERE Id = @id";
                queryCmd.Parameters.AddWithValue("@id", id);
                using (var reader = queryCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Employee(reader);                        
                    }
                }
            }
            return null;
        }

        public static bool AddEmployee(Employee employee)
        {            
            using (var connection = new SqliteConnection(EmployeeConnectionString))
            {
                connection.Open();
                var insertCmd = connection.CreateCommand(); // can do transaction here
                // without ID, since it's auto-incremented in the DB, we can just insert Name and Value
                insertCmd.CommandText = @"INSERT INTO Employees(Name, Value) VALUES (@name, @value)";
                insertCmd.Parameters.AddWithValue("@name", employee.Name);
                insertCmd.Parameters.AddWithValue("@value", employee.Value);
                return insertCmd.ExecuteNonQuery() > 0;
            }
        }

        public static bool UpdateEmployee(Employee employee)
        {
            using (var connection = new SqliteConnection(EmployeeConnectionString))
            {
                connection.Open();
                var updateCmd = connection.CreateCommand();                // TODO: transaction?
             
                updateCmd.CommandText = @"UPDATE Employees SET Name=@name, Value=@value WHERE Id = @id";
                updateCmd.Parameters.AddWithValue("@id", employee.Id);
                updateCmd.Parameters.AddWithValue("@name", employee.Name);
                updateCmd.Parameters.AddWithValue("@value", employee.Value);
                return updateCmd.ExecuteNonQuery() > 0;
            }
        }

        public static bool DeleteEmployee(int id)
        {            
            using (var connection = new SqliteConnection(EmployeeConnectionString))
            {
                connection.Open();
                var deleteCmd = connection.CreateCommand(); // can do transaction here
                deleteCmd.CommandText = @"DELETE FROM Employees WHERE Id = @id";
                deleteCmd.Parameters.AddWithValue("@id", id);
                return deleteCmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
