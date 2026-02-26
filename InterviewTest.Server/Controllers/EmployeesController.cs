using InterviewTest.Interfaces;
using InterviewTest.Server.Model;
using Microsoft.AspNetCore.Mvc;

namespace InterviewTest.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepo _repo;

        public EmployeesController(IEmployeeRepo repo) 
        { 
            _repo = repo;
        }

        [HttpGet]
        public List<Employee> Get()
        {
            return _repo.GetAllEmployees();
        }

        [HttpGet("{id}")]
        public Employee Get(int id)
        {
            return _repo.GetEmployee(id);
        }

        [HttpPost("add")]
        public bool Add(Employee employee)
        {
            return _repo.AddEmployee(employee);
        }

        [HttpPut("update")] 
        public bool Update(Employee employee)
        {
            return _repo.UpdateEmployee(employee);
        }

        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return _repo.DeleteEmployee(id);
        }

        [HttpGet("increase")]
        public void Increase()
        {
            _repo.BulkValueIncrement();
        }
    }
}
