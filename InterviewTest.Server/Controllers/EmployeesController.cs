using InterviewTest.Server.Model;
using InterviewTest.Server.Util;
using Microsoft.AspNetCore.Mvc;
using static InterviewTest.Server.Util.EmployeeUtil;

namespace InterviewTest.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        [HttpGet]
        public List<Employee> Get()
        {
            return GetAllEmployees();
        }

        [HttpGet("{id}")]
        public Employee Get(int id)
        {
            return GetEmployee(id);
        }

        [HttpPost("add")]
        public bool Add(Employee employee) // TODO: string name, int value
        {
            return AddEmployee(employee);
        }

        [HttpPut("update")] // TODO: int id, string name, int value
        public bool Update(Employee employee)
        {
            return UpdateEmployee(employee);
        }

        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return DeleteEmployee(id);
        }

        [HttpGet("increase")]
        public void Increase()
        {
            BulkValueIncrement();
        }
    }
}
