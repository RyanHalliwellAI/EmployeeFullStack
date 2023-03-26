using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using EmployeeFullStack.Models;
using Microsoft.Data.SqlClient; // Corrected namespace

namespace EmployeeFullStack.Controllers
{

    //Route attr for the base url for all actions in controller. The responder interacts with API
    [Route("api/[controller]")]
    [ApiController]

    public class DepartmentController : ControllerBase
    {
        //Dependency injection for config
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //Get for the Departments

        [HttpGet]
        public IActionResult Get()
        {
            //SQL query
            string query = @"
                            SELECT DepartmentId, DepartmentName 
                            FROM dbo.Department";

            //Creating table, getting connectin string, opening connection, running command, returning results.
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
            //converts datatable into dictionary
            var result = table.AsEnumerable()
                .Select(row => table.Columns.Cast<DataColumn>()
                .ToDictionary(col => col.ColumnName, col => row[col]))
                .ToList();

            return Ok(result);
        }
        //Post method for new employees
        [HttpPost]
        public IActionResult Post([FromBody] Department dep)
        {
            //valildate imput
            if (dep == null || string.IsNullOrWhiteSpace(dep.DepartmentName))
            {
                return BadRequest("Invalid department data.");
            }
            // SQL Query
            string query = @"
                    INSERT INTO dbo.Department (DepartmentName) 
                    VALUES (@DepartmentName)";

            //opens connection, adds the parameters to the command,runs the command, then checks if it was added correctly.
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
        //Updating existing Departments
        [HttpPut]
        public IActionResult Put([FromBody] Department dep)
        {
            // Validate the input
            if (dep == null || dep.DepartmentId <= 0)
            {
                return BadRequest("Invalid department data.");
            }
            //SQL query
            string query = @"
                            UPDATE dbo.Department 
                            SET DepartmentName = @DepartmentName
                            WHERE DepartmentId = @DepartmentId";
            
            //opens connection, adds the parameters to the command,runs the command, then checks if it was added correctly.
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
        //Delete method with Department ID
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //Validate input
            if (id <= 0)
            {
                return BadRequest("Invalid department ID.");
            }
            // SQL QUERY
            string query = @"
                            DELETE FROM dbo.Department 
                            WHERE DepartmentId = @DepartmentId";
            
            //opens connection, adds the parameters to the command,runs the command, then checks if it was added correctly.
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
