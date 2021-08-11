using BusinessLTDA.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLTDA.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeesController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly IWebHostEnvironment _webHostEnvironment ;
		public EmployeesController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
		{
			_configuration = configuration;
			_webHostEnvironment = webHostEnvironment;
		}

		[HttpGet]
		public JsonResult Get()
		{
			string query = @"
							SELECT EmployeeId, EmployeeName, Department, convert(varchar(11),DateOfJoining,120)
							as DateOfJoining, PhotoFileName
							from dbo.Employee
							";

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
			return new JsonResult(table);
		}

		[HttpPost]
		public JsonResult Post(Employee employee)
		{
			string query = @"
							INSERT INTO dbo.Employee
							(EmployeeName,Department,DateOfJoining,PhotoFileName)
							VALUES (@EmployeeName,@Department,@DateOfJoining,@PhotoFileName)
							";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
					myCommand.Parameters.AddWithValue("@Department", employee.Department);
					myCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
					myCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}
			return new JsonResult("Novo Empregado contratado");
		}

		[HttpPut]
		public JsonResult Put(Employee employee)
		{
			string query = @"
							UPDATE dbo.Employee
							SET EmployeeName = @EmployeeName,
								Department = @Department,
								DateOfJoining = @DateOfJoining,
								PhotoFileName = @PhotoFileName
							WHERE EmployeeId = @EmployeeId
							";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
					myCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
					myCommand.Parameters.AddWithValue("@Department", employee.Department);
					myCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
					myCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}
			return new JsonResult("Empregado atualizado");
		}

		[HttpDelete("{id}")]
		public JsonResult Delete(int id)
		{
			string query = @"
							DELETE FROM dbo.Employee
							WHERE EmployeeId = @EmployeeId
							";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@EmployeeId", id);
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}
			return new JsonResult("Empregado demitido");
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
				var physicalPath = _webHostEnvironment.ContentRootPath + "/Photos/" + filename;
				using(var stream = new FileStream(physicalPath, FileMode.Create))
				{
					postedFile.CopyTo(stream);
				}
				return new JsonResult(filename);
			}
			catch (Exception)
			{
				return new JsonResult("anonymous.png");
			}
		}
	}
}
