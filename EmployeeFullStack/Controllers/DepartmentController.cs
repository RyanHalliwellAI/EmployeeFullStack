﻿using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using EmployeeFullStack.Models;

using Microsoft.Data.SqlClient;

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
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            // Convert DataTable to List of Dictionaries
            var result = table.AsEnumerable()
                .Select(row => table.Columns.Cast<DataColumn>()
                .ToDictionary(col => col.ColumnName, col => row[col]))
                .ToList();

            return Ok(result);

            //return new JsonResult(table);
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

                    // Execute non-query for insert
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
        public IActionResult Put(Department dep)
        {
            string query = @"
                            update dbo.Department 
                            set DepartmentName = @DepartmentName
                            where DepartmentId = @DepartmentId";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentId", dep.DepartmentId);
                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();

                }
            }


            // Convert DataTable to List of Dictionaries
            var result = table.AsEnumerable()
                .Select(row => table.Columns.Cast<DataColumn>()
                .ToDictionary(col => col.ColumnName, col => row[col]))
                .ToList();

            return Ok(result);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            string query = @"
                            delete from dbo.Department 
                            where DepartmentId = @DepartmentId";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {

                    myCommand.Parameters.AddWithValue("@DepartmentId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();

                }
            }

            // Convert DataTable to List of Dictionaries
            var result = table.AsEnumerable()
                .Select(row => table.Columns.Cast<DataColumn>()
                .ToDictionary(col => col.ColumnName, col => row[col]))
                .ToList();

            return Ok(result);
        }
    }

}

