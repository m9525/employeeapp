using InterviewTest.Interfaces;
using InterviewTest.Server.Model;
using Microsoft.Data.Sqlite;

namespace InterviewTest.Repo
{
    public class SqlLiteEmployeeRepo : IEmployeeRepo
    {
        private readonly string EmployeeConnectionString = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" }.ConnectionString;

        public List<Employee> GetAllEmployees()
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
        public void BulkValueIncrement()
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

        public Employee GetEmployee(int id)
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

        public bool AddEmployee(Employee employee)
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

        public bool UpdateEmployee(Employee employee)
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

        public bool DeleteEmployee(int id)
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

        public void PrepareRepo()
        {
            using (var connection = new SqliteConnection(EmployeeConnectionString))
            {
                connection.Open();

                var delTableCmd = connection.CreateCommand();
                delTableCmd.CommandText = "DROP TABLE IF EXISTS Employees";
                delTableCmd.ExecuteNonQuery();

                var createTableCmd = connection.CreateCommand();
                createTableCmd.CommandText = "CREATE TABLE Employees(ID INTEGER PRIMARY KEY AUTOINCREMENT, Name VARCHAR(50) NOT NULL, Value INT NOT NULL)";
                createTableCmd.ExecuteNonQuery();

                //Fill with data
                using (var transaction = connection.BeginTransaction())
                {
                    var insertCmd = connection.CreateCommand();
                    insertCmd.CommandText = @"INSERT INTO Employees(Name, Value) VALUES
                        ('Abul', 1357),
                        ('Adolfo', 1224),
                        ('Alexander', 2296),
                        ('Amber', 1145),
                        ('Amy', 4359),
                        ('Andy', 1966),
                        ('Anna', 4040),
                        ('Antony', 449),
                        ('Ashley', 8151),
                        ('Borja', 9428),
                        ('Cecilia', 2136),
                        ('Christopher', 9035),
                        ('Dan', 1475),
                        ('Dario', 284),
                        ('David', 948),
                        ('Elike', 1860),
                        ('Ella', 4549),
                        ('Ellie', 5736),
                        ('Elliot', 1020),
                        ('Emily', 7658),
                        ('Faye', 7399),
                        ('Fern', 1422),
                        ('Francisco', 5028),
                        ('Frank', 3281),
                        ('Gary', 9190),
                        ('Germaine', 6437),
                        ('Greg', 5929),
                        ('Harvey', 8471),
                        ('Helen', 963),
                        ('Huzairi', 9491),
                        ('Izmi', 8324),
                        ('James', 6994),
                        ('Jarek', 6581),
                        ('Jim', 202),
                        ('John', 261),
                        ('Jose', 1605),
                        ('Josef', 3714),
                        ('Karthik', 4828),
                        ('Katrin', 5393),
                        ('Lee', 269),
                        ('Luke', 5926),
                        ('Madiha', 2329),
                        ('Marc', 3651),
                        ('Marina', 6903),
                        ('Mark', 3368),
                        ('Marzena', 7515),
                        ('Mohamed', 1080),
                        ('Nichole', 1221),
                        ('Nikita', 8520),
                        ('Oliver', 2868),
                        ('Patryk', 1418),
                        ('Paul', 4332),
                        ('Ralph', 1581),
                        ('Raymond', 7393),
                        ('Roman', 4056),
                        ('Ryan', 252),
                        ('Sara', 2618),
                        ('Sean', 691),
                        ('Seb', 5395),
                        ('Sergey', 8282),
                        ('Shaheen', 3721),
                        ('Sharni', 7737),
                        ('Sinu', 3349),
                        ('Stephen', 8105),
                        ('Tim', 8386),
                        ('Tina', 5133),
                        ('Tom', 7553),
                        ('Tony', 4432),
                        ('Tracy', 1771),
                        ('Tristan', 2030),
                        ('Victor', 1046),
                        ('Yury', 1854)";
                    insertCmd.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
        }
    }
}