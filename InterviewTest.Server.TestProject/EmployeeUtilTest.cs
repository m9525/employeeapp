using InterviewTest.Repo;
using InterviewTest.Server.Model;

namespace InterviewTest.Server.TestProject
{
    [TestClass]
    public sealed class EmployeeUtilTest
    {
        [TestMethod]
        public void Given_No_Filter_Return_all_Employees()
        {
            SqlLiteEmployeeRepo repo = new SqlLiteEmployeeRepo(); repo.PrepareRepo(); // ensure the repo is ready before testing
            var actual = repo.GetAllEmployees();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);

            // random check, is Stephen there?
            Assert.IsTrue(actual.Any(e => e.Name == "Stephen"));
        }

        [TestMethod]
        public void Given_Employee_Id_Exists_Return_Employee()
        {
            SqlLiteEmployeeRepo repo = new SqlLiteEmployeeRepo(); repo.PrepareRepo(); // ensure the repo is ready before testing
            int id = 64; // Stephen's Id
            var actual = repo.GetEmployee(id);
            Assert.IsNotNull(actual);
            Assert.AreEqual(id, actual.Id);
            Assert.AreEqual("Stephen", actual.Name);
        }

        [TestMethod]
        public void Given_Employee_Id_Exists_Delete_Employee()
        {
            SqlLiteEmployeeRepo repo = new SqlLiteEmployeeRepo(); repo.PrepareRepo(); // ensure the repo is ready before testing
            int id = 72; // Yuri's ID            
            // now you see            
            var employee72 = repo.GetEmployee(id);            

            // now you don't
            bool deleted = repo.DeleteEmployee(id);
            Assert.IsTrue(deleted);
            var allEmployees = repo.GetAllEmployees();
            Assert.IsFalse(allEmployees.Any(e => e.Id == id));

            var ghostEmployee72 = repo.GetEmployee(id);
            Assert.IsNull(ghostEmployee72);

            // TODO: add the employee back for other tests
            Assert.IsTrue(repo.AddEmployee(employee72));
            allEmployees = repo.GetAllEmployees();
            Assert.IsTrue(allEmployees.Any(e => e.Name.Equals(employee72.Name))); // ID will change, but name should be the same
        }

        [TestMethod]
        public void Given_Employee_Exist_UpdateEmployee()
        {
            SqlLiteEmployeeRepo repo = new SqlLiteEmployeeRepo(); repo.PrepareRepo(); // ensure the repo is ready before testing
            var allEmployees = repo.GetAllEmployees();            
            var anyId = new Random().Next(allEmployees.Count);

            var randomEmployee = repo.GetEmployee(anyId);
            Assert.IsNotNull(randomEmployee);
            Assert.AreEqual(anyId, randomEmployee.Id);

            string originalName = randomEmployee.Name;
            // update name
            string updatedName = originalName + "_Updated";
            randomEmployee.Name = updatedName;
            int originalValue = randomEmployee.Value;
            int updatedValue = randomEmployee.Value + 100;

            randomEmployee.Value = updatedValue;

            bool updated = repo.UpdateEmployee(randomEmployee); // AddEmployee will update if ID exists
            Assert.IsTrue(updated);

            var updatedEmployee = repo.GetEmployee(anyId);
            Assert.IsNotNull(updatedEmployee);
            Assert.AreEqual(updatedName, updatedEmployee.Name);
            Assert.AreEqual(updatedValue, updatedEmployee.Value); 
        }


        [TestMethod]
        public void Given_EmployeeE_Increase1_EmployeeG_Increase10_EmployeeOther_Increase100_When_Calling_Bulk_Increment()
        {
            SqlLiteEmployeeRepo repo = new SqlLiteEmployeeRepo(); repo.PrepareRepo(); // ensure the repo is ready before testing
            var employees = repo.GetAllEmployees();
            Assert.IsNotNull(employees);

            // total at start
            int originalSum = employees.Select(e => e.Value).Sum();

            int increasedSum = employees.Select(e =>
            {
                if (e.Name.StartsWith("E"))
                    return e.Value + 1;
                else if (e.Name.StartsWith("G"))
                    return e.Value + 10;
                else
                    return e.Value + 100;
            }).Sum();

            Assert.IsLessThan(increasedSum, originalSum);

            repo.BulkValueIncrement();
            var updatedEmployees = repo.GetAllEmployees();
            int actualIncreasedSum = updatedEmployees.Select(e => e.Value).Sum();
            
            Assert.AreEqual(increasedSum, actualIncreasedSum);
        }

        [TestMethod]
        public void Given_All_Employees_Sum_NameStartsWithA_B_C()
        {
            SqlLiteEmployeeRepo repo = new SqlLiteEmployeeRepo(); repo.PrepareRepo(); // ensure the repo is ready before testing
            var employees = repo.GetAllEmployees();
            Assert.IsNotNull(employees);
            int abcSum = employees.Select(e =>
            {
                if (e.Name.StartsWith("A") || e.Name.StartsWith("B") || e.Name.StartsWith("C")) return e.Value; else return 0;
            }).Sum();
            Assert.IsTrue(abcSum > 0); // 4678 on first run. < 11171
        }

        [TestMethod]
        public void Given_New_Employee_Then_Add_Successfully()
        {
            SqlLiteEmployeeRepo repo = new SqlLiteEmployeeRepo(); repo.PrepareRepo(); // ensure the repo is ready before testing
            var emp = new Employee(0, "Hello", 999);
            Assert.IsTrue(repo.AddEmployee(emp));
                var allEmployees = repo.GetAllEmployees();
            Assert.IsNotNull(allEmployees);
            Assert.IsTrue(allEmployees.Any(e => e.Name == emp.Name && e.Value == emp.Value));
        }
    }
}
