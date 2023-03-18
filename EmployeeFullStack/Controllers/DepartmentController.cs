using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using EmployeeFullStack.Models;
using Microsoft.Data.SqlClient; // Corrected namespace

namespace EmployeeFullStack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string query = @"
                            SELECT DepartmentId, DepartmentName 
                            FROM dbo.Department";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        table.Load(myReader);
                    }
                }
            }

            var result = table.AsEnumerable()
                .Select(row => table.Columns.Cast<DataColumn>()
                .ToDictionary(col => col.ColumnName, col => row[col]))
                .ToList();

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Department dep)
        {
            if (dep == null || string.IsNullOrWhiteSpace(dep.DepartmentName))
            {
                return BadRequest("Invalid department data.");
            }

            string query = @"
                    INSERT INTO dbo.Department (DepartmentName) 
                    VALUES (@DepartmentName)";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("EmployeeAppCon")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);

                    int result = myCommand.ExecuteNonQuery();

                    if (result > 0)
                    {
                        return Ok(new { Message = "Department added successfully." });
                    }
                    else
                    {
                        return StatusCode(500, "An error occurred while adding the department.");
                    }
                }
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] Department dep)
        {
            if (dep == null || dep.DepartmentId <= 0)
            {
                return BadRequest("Invalid department data.");
            }

            string query = @"
                            UPDATE dbo.Department 
                            SET DepartmentName = @DepartmentName
                            WHERE DepartmentId = @DepartmentId";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("EmployeeAppCon")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentId", dep.DepartmentId);
                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);

                    int result = myCommand.ExecuteNonQuery();

                    if (result > 0)
                    {
                        return Ok(new { Message = "Department updated successfully." });
                    }
                    else
                    {
                        return NotFound("Department not found.");
                    }
                }
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid department ID.");
            }

            string query = @"
                            DELETE FROM dbo.Department 
                            WHERE DepartmentId = @DepartmentId";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("EmployeeAppCon")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentId", id);

                    int result = myCommand.ExecuteNonQuery();

                    if (result > 0)
                    {
                        return Ok(new { Message = "Department deleted successfully." });
                    }
                    else
                    {
                        return NotFound("Department not found.");
                    }
                }
            }
        }
    }
}
