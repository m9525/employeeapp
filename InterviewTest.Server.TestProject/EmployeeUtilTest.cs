using InterviewTest.Server.Model;
using InterviewTest.Server.Util;

namespace InterviewTest.Server.TestProject
{
    [TestClass]
    public sealed class EmployeeUtilTest
    {
        [TestMethod]
        public void Given_No_Filter_Return_all_Employees()
        {
            var actual = EmployeeUtil.GetAllEmployees();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);

            // random check, is Stephen there?
            Assert.IsTrue(actual.Any(e => e.Name == "Stephen"));
        }

        [TestMethod]
        public void Given_Employee_Id_Exists_Return_Employee()
        {
            int id = 64; // Stephen's Id
            var actual = EmployeeUtil.GetEmployee(id);
            Assert.IsNotNull(actual);
            Assert.AreEqual(id, actual.Id);
            Assert.AreEqual("Stephen", actual.Name);
        }

        [TestMethod]
        public void Given_Employee_Id_Exists_Delete_Employee()
        {
            int id = 72; // Yuri's ID            
            // now you see            
            var employee72 = EmployeeUtil.GetEmployee(id);            

            // now you don't
            bool deleted = EmployeeUtil.DeleteEmployee(id);
            Assert.IsTrue(deleted);
            var allEmployees = EmployeeUtil.GetAllEmployees();
            Assert.IsFalse(allEmployees.Any(e => e.Id == id));

            var ghostEmployee72 = EmployeeUtil.GetEmployee(id);
            Assert.IsNull(ghostEmployee72);

            // TODO: add the employee back for other tests
            Assert.IsTrue(EmployeeUtil.AddEmployee(employee72));
            allEmployees = EmployeeUtil.GetAllEmployees();
            Assert.IsFalse(allEmployees.Any(e => e.Name.Equals(employee72.Name))); // ID will change, but name should be the same
        }

        [TestMethod]
        public void Given_Employee_Exist_UpdateEmployee()
        {
            var allEmployees = EmployeeUtil.GetAllEmployees();            
            var anyId = new Random().Next(allEmployees.Count);

            var randomEmployee = EmployeeUtil.GetEmployee(anyId);
            Assert.IsNotNull(randomEmployee);
            Assert.AreEqual(anyId, randomEmployee.Id);

            string originalName = randomEmployee.Name;
            // update name
            string updatedName = originalName + "_Updated";
            randomEmployee.Name = updatedName;
            int originalValue = randomEmployee.Value;
            int updatedValue = randomEmployee.Value + 100;

            randomEmployee.Value = updatedValue;

            bool updated = EmployeeUtil.UpdateEmployee(randomEmployee); // AddEmployee will update if ID exists
            Assert.IsTrue(updated);

            var updatedEmployee = EmployeeUtil.GetEmployee(anyId);
            Assert.IsNotNull(updatedEmployee);
            Assert.AreEqual(updatedName, updatedEmployee.Name);
            Assert.AreEqual(updatedValue, updatedEmployee.Value); 
        }


        [TestMethod]
        public void Given_EmployeeE_Increase1_EmployeeG_Increase10_EmployeeOther_Increase100_When_Calling_Bulk_Increment()
        {
            var employees = EmployeeUtil.GetAllEmployees();
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

            EmployeeUtil.BulkValueIncrement();
            var updatedEmployees = EmployeeUtil.GetAllEmployees();
            int actualIncreasedSum = updatedEmployees.Select(e => e.Value).Sum();
            
            Assert.AreEqual(increasedSum, actualIncreasedSum);
        }

        [TestMethod]
        public void Given_All_Employees_Sum_NameStartsWithA_B_C()
        {
            var employees = EmployeeUtil.GetAllEmployees();
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
            var emp = new Employee(0, "Hello", 999);
            Assert.IsTrue(EmployeeUtil.AddEmployee(emp));
                var allEmployees = EmployeeUtil.GetAllEmployees();
            Assert.IsNotNull(allEmployees);
            Assert.IsTrue(allEmployees.Any(e => e.Name == emp.Name && e.Value == emp.Value));
        }
    }
}
