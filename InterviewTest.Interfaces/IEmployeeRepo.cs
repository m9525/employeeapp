using InterviewTest.Server.Model;

namespace InterviewTest.Interfaces
{
    public interface IEmployeeRepo
    {
        bool AddEmployee(Employee employee);
        void BulkValueIncrement();
        bool DeleteEmployee(int id);
        List<Employee> GetAllEmployees();
        Employee GetEmployee(int id);
        bool UpdateEmployee(Employee employee);

        void PrepareRepo();
    }
}
