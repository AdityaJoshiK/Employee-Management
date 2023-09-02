using Employee_Management.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Employee_Management.Controllers
{
    public class EmployeeController : Controller
    {
        private IConfiguration Configuration;
        public EmployeeController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        public IActionResult Index()
        {
            string str = this.Configuration.GetConnectionString("myConnectionString");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_EMP_Employee_SelectAll";
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();

            return View("EmployeeList",dt);
        }

        public IActionResult Delete(int EmployeeID)
        {
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_EMP_Employee_DeleteByPK";
            cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
            cmd.ExecuteNonQuery();
            conn.Close();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Save(EmployeeModel modelEmployee)
        {
            if (ModelState.IsValid)
            {
                #region Insert & Update
                string str = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                if (modelEmployee.EmployeeID == 0)
                {
                    cmd.CommandText = "PR_EMP_Employee_Insert";
                }
                else
                {
                    cmd.CommandText = "PR_EMP_Employee_UpdateByPK";
                    cmd.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = modelEmployee.EmployeeID;
                }

                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = modelEmployee.FirstName;
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = modelEmployee.LastName;
                cmd.Parameters.Add("@DesignationID", SqlDbType.Int).Value = modelEmployee.DesignationID;
                cmd.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = modelEmployee.DepartmentID;
                cmd.Parameters.Add("@Salary", SqlDbType.Decimal).Value = modelEmployee.Salary;
                cmd.Parameters.Add("@JoiningDate", SqlDbType.Date).Value = modelEmployee.JoiningDate;
                cmd.Parameters.Add("@ReportingPersonID", SqlDbType.Int).Value = modelEmployee.ReportingPersonID;
                cmd.Parameters.Add("@CreationDate", SqlDbType.DateTime).Value = modelEmployee.CreationDate;
                cmd.Parameters.Add("@ModificationDate", SqlDbType.DateTime).Value = modelEmployee.ModificationDate;
                cmd.Parameters.Add("@PhotoPath", SqlDbType.NVarChar).Value = modelEmployee.PhotoPath;

                if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
                {
                    if (modelEmployee.EmployeeID == 0)
                    {
                        TempData["EmployeeInsertMsg"] = "Successfully Inserted";
                    }
                    else
                    {
                        TempData["EmployeeInsertMsg"] = "Successfully Updated";
                    }
                }
                conn.Close();
                #endregion
            }
            return RedirectToAction("Index");
        }

        public IActionResult Back()
        {
            return View("Index");
        }

        public IActionResult Add(int? EmployeeID)
        {
            #region SelectByPK
            if (EmployeeID != null)
            {
                string str = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Employee_SelectByPK";
                cmd.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = EmployeeID;
                EmployeeModel modelEmployee = new EmployeeModel();
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);

                foreach (DataRow dr in dt.Rows)
                {
                    modelEmployee.EmployeeID = Convert.ToInt32(dr["EmployeeID"]);
                    modelEmployee.FirstName = dr["FirstName"].ToString();
                    modelEmployee.LastName = dr["LastName"].ToString();
                    modelEmployee.DesignationID = Convert.ToInt32(dr["DesignationID"]);
                    modelEmployee.DepartmentID = dr["DepartmentID"] != DBNull.Value ? Convert.ToInt32(dr["DepartmentID"]) : (int?)null;
                    modelEmployee.Salary = dr["Salary"] != DBNull.Value ? Convert.ToDecimal(dr["Salary"]) : (decimal?)null;
                    modelEmployee.JoiningDate = Convert.ToDateTime(dr["JoiningDate"]);
                    modelEmployee.ReportingPersonID = dr["ReportingPersonID"] != DBNull.Value ? Convert.ToInt32(dr["ReportingPersonID"]) : (int?)null;
                    modelEmployee.CreationDate = dr["CreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreationDate"]) : (DateTime?)null;
                    modelEmployee.ModificationDate = dr["ModificationDate"] != DBNull.Value ? Convert.ToDateTime(dr["ModificationDate"]) : (DateTime?)null;
                    modelEmployee.PhotoPath = dr["PhotoPath"] != DBNull.Value ? dr["PhotoPath"].ToString() : null;
                }

                conn.Close();

                return View("EmployeeAddEdit", modelEmployee);
            }
            #endregion

            return View("EmployeeAddEdit");
        }


    }
}
