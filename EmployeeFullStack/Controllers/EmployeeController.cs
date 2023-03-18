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
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;


        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string query = @"
                            SELECT EmployeeId, EmployeeName, Department,
                            convert(varchar(10), DateOfJoining, 120) as DateOfJoining, PhotoFileName
                            FROM dbo.Employee";

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
        public IActionResult Post([FromBody] Employee emp)
        {
            if (emp == null || string.IsNullOrWhiteSpace(emp.EmployeeName))
            {
                return BadRequest("Invalid employee data.");
            }

            string query = @"
                    INSERT INTO dbo.Employee (EmployeeName, Department, DateOfJoining, PhotoFileName) 
                    VALUES (@EmployeeName, @Department, @DateOfJoining, @PhotoFileName)";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("EmployeeAppCon")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    myCommand.Parameters.AddWithValue("@Department", emp.Department);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);

                    int result = myCommand.ExecuteNonQuery();

                    if (result > 0)
                    {
                        return Ok(new { Message = "Employee added successfully." });
                    }
                    else
                    {
                        return StatusCode(500, "An error occurred while adding the employee.");
                    }
                }
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] Employee emp)
        {
            if (emp == null || emp.EmployeeId <= 0)
            {
                return BadRequest("Invalid employee data.");
            }

            string query = @"
                            UPDATE dbo.Employee 
                            SET EmployeeName = @EmployeeName,
                                Department = @Department,
                                DateOfJoining = @DateOfJoining,
                                PhotoFileName = @PhotoFileName
                            WHERE EmployeeId = @EmployeeId";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("EmployeeAppCon")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
                    myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    myCommand.Parameters.AddWithValue("@Department", emp.Department);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);

                    int result = myCommand.ExecuteNonQuery();

                    if (result > 0)
                    {
                        return Ok(new { Message = "Employee updated successfully." });
                    }
                    else
                    {
                        return NotFound("Employee not found.");
                    }
                }
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid employee ID.");
            }

            string query = @"
                            DELETE FROM dbo.Employee 
                            WHERE EmployeeId = @EmployeeId";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("EmployeeAppCon")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", id);

                    int result = myCommand.ExecuteNonQuery();

                    if (result > 0)
                    {
                        return Ok(new { Message = "Employee deleted successfully." });
                    }
                    else
                    {
                        return NotFound("Employee not found.");
                    }
                }
            }
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try 
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
            catch(Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }

    }
}
