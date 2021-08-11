using BusinessLTDA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLTDA.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DepartmentsController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		public DepartmentsController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpGet]
		public JsonResult Get()
		{
			string query = @"
							SELECT DepartmentId, DepartmentName from dbo.Department
							";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
			SqlDataReader myReader;
			using(SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using(SqlCommand myCommand = new SqlCommand(query, myCon))
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
		public JsonResult Post(Department department)
		{
			string query = @"
							INSERT INTO dbo.Department
							VALUES (@DepartmentName)
							";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}
			return new JsonResult("Novo Departamento criado");
		}

		[HttpPut]
		public JsonResult Put(Department department)
		{
			string query = @"
							UPDATE dbo.Department
							SET DepartmentName = @DepartmentName
							WHERE DepartmentId = @DepartmentId
							";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);
					myCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}
			return new JsonResult("Departamento atualizado");
		}

		[HttpDelete("{id}")]
		public JsonResult Delete(int id)
		{
			string query = @"
							DELETE FROM dbo.Department
							WHERE DepartmentId = @DepartmentId
							";

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
			return new JsonResult("Departamento excluído");
		}
	}
}
