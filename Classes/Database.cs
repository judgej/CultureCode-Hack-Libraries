using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace NewcastleLibrary.Data
{
    /// <summary>Database</summary>
    public class Database
    {
        #region Private Constants
        private const int zero = 0;
        private const string blank = "";
        #endregion

        #region Private Members
        private string dsn_connection_string = "";
        #endregion

        #region Private Properties
        /// <summary>ConnectionString</summary>
        private string connectionstring
        {
            get
            {
                return dsn_connection_string == "" ? ConfigurationManager.ConnectionStrings["NewcastleLibrary.ConnectionString"].ConnectionString : dsn_connection_string;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>GetLibraryName</summary>
        /// <param name="code">BranchCode</param>
        /// <returns>Name</returns>
        public string GetLibraryName(int code)
        {
            SqlParameter[] sqlparameters = new SqlParameter[1];
            sqlparameters[0] = new SqlParameter("@Code", SqlDbType.Int) { Value = code };
            object result = DataHelper.CommandScalar(connectionstring, "GetLibraryName", sqlparameters);
            return result != null ? (string)result : "";
        }

        /// <summary>ListBranch</summary>
        /// <returns>List(Of Branch)</returns>
        public List<Library> ListBranch()
        {
            List<Library> items = new List<Library>();
            try
            {
                SqlParameter[] sqlparameters = new SqlParameter[0];
                SqlConnection connection = new SqlConnection(connectionstring);
                SqlDataReader reader = DataHelper.CommandQuerying(connection, "ListBranch", sqlparameters);
                while (reader.Read())
                {
                    Library item = new Library();
                    item.Code = reader["Code"] != DBNull.Value ? (int)reader["Code"] : 0;
                    item.Distance = reader["Distance"] != DBNull.Value ? (double)reader["Distance"] : 0;
                    item.Name = reader["Branch"] != DBNull.Value ? (string)reader["Branch"] : "";
                    item.Borrowers = reader["Borrowers"] != DBNull.Value ? (int)reader["Borrowers"] : 0;
                    item.Url = "Default.aspx?code=" + item.Code;
                    items.Add(item);
                }
                return items;
            }
            catch
            {
                return items;
            }
        }

        /// <summary>ListBook</summary>
        /// <param name="branch">Branch</param>
        /// <returns>List(Of Book)</returns>
        public List<Book> ListBook(int branch)
        {
            List<Book> items = new List<Book>();
            try
            {
                SqlParameter[] sqlparameters = new SqlParameter[1];
                sqlparameters[0] = new SqlParameter("@Branch", SqlDbType.Int) { Value = branch };
                SqlConnection connection = new SqlConnection(connectionstring);
                SqlDataReader reader = DataHelper.CommandQuerying(connection, "ListBook", sqlparameters);
                while (reader.Read())
                {
                    Book item = new Book();
                    item.Author = reader["Author"] != DBNull.Value ? (string)reader["Author"] : "";
                    item.Code = reader["Branch"] != DBNull.Value ? (int)reader["Branch"] : 0;
                    item.Distance = reader["Distance"] != DBNull.Value ? (double)reader["Distance"] : 0;
                    item.ISBN = reader["ISBN"] != DBNull.Value ? (string)reader["ISBN"] : "";
                    item.Title = reader["Title"] != DBNull.Value ? (string)reader["Title"] : "";
                    if (!item.ISBN.Equals("")) item.Image = string.Format("http://library.newcastle.gov.uk/ThumbnailImages/{0}.JPG", item.ISBN.Trim());
                    items.Add(item);
                }
                return items;
            }
            catch
            {
                return items;
            }
        }
        #endregion
    }
}
