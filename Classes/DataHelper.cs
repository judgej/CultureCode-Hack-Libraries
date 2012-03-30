using System;
using System.Data;
using System.Data.SqlClient;

namespace NewcastleLibrary.Data
{
    /// <summary>
    /// Provides encapsulated data access facilities
    /// </summary>
    /// <change_control>
    ///		<change date="29 July 2006">
    ///			<author>Paul Rawlings</author>
    ///			<description>Creation</description>
    ///		</change>
    /// </change_control>
    public class DataHelper
    {
        static int m_CommandTimeout = 60;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataHelper() { }

        /// <summary>
        /// Performs a Sql Query
        /// </summary>
        /// <param name="pstrDSN">Database Connection String</param>
        /// <param name="pstrCommand">Stored Procedure or SQL to execute</param>
        /// <param name="pstrTableName">Name of the Dataset table to fill</param>
        /// <param name="pobjDataSet">Dataset to be filled with the results of the query</param>
        static public void SqlQuery(string pstrDSN, string pstrCommand, string pstrTableName, DataSet pobjDataSet)
        {
            Query(pstrDSN, pstrCommand, CommandType.Text, null, pstrTableName, pobjDataSet);
        }
        /// <summary>
        /// Performs a Sql Query
        /// </summary>
        /// <param name="pstrDSN">Database Connection String</param>
        /// <param name="pstrCommand">Stored Procedure or SQL to execute</param>
        /// <param name="pobjParameters">SqlParameter Array. Can created using the following code:-
        /// SqlParameter[] objParameters=new SqlParameter[2];
        /// objParameters[0]=new SqlParameter("myParam1","1");
        /// objParameters[1]=new SqlParameter("myParam2","22"); 
        /// </param>
        /// <param name="pstrTableName">Name of the Dataset table to fill</param>
        /// <param name="pobjDataSet">Dataset to be filled with the results of the query</param>
        static public void SqlQuery(string pstrDSN, string pstrCommand, SqlParameter[] pobjParameters, string pstrTableName, DataSet pobjDataSet)
        {
            Query(pstrDSN, pstrCommand, CommandType.Text, pobjParameters, pstrTableName, pobjDataSet);
        }
        /// <summary>
        /// Executes a Stored Procedure Query
        /// </summary>
        /// <param name="pstrDSN">Database Connection String</param>
        /// <param name="pstrCommand">Stored Procedure or SQL to execute</param>
        /// <param name="pstrTableName">Name of the Dataset table to fill</param>
        /// <param name="pobjDataSet">Dataset to be filled with the results of the query</param>
        static public void CommandQuery(string pstrDSN, string pstrCommand, string pstrTableName, DataSet pobjDataSet)
        {
            Query(pstrDSN, pstrCommand, CommandType.StoredProcedure, null, pstrTableName, pobjDataSet);
        }
        /// <summary>
        /// Executes a Stored Procedure Query and returns a DataSet
        /// </summary>
        /// <param name="pstrDSN">Database Connection String</param>
        /// <param name="pstrCommand">Stored Procedure or SQL to execute</param>
        /// <param name="pobjParameters">SqlParameter Array. Can created using the following code:-
        /// <code>
        /// SqlParameter[] objParameters=new SqlParameter[2];
        /// objParameters[0]=new SqlParameter("myParam1","1");
        /// objParameters[1]=new SqlParameter("myParam2","22"); 
        /// </code> 
        /// <param name="pstrTableName">Name of the Dataset table to fill</param>
        /// <param name="pobjDataSet">Dataset to be filled with the results of the query</param>
        static public void CommandQuery(string pstrDSN, string pstrCommand, SqlParameter[] pobjParameters, string pstrTableName, DataSet pobjDataSet)
        {
            Query(pstrDSN, pstrCommand, CommandType.StoredProcedure, pobjParameters, pstrTableName, pobjDataSet);
        }

        static public int CommandExecute(string pstrDSN, string pstrCommand, SqlParameter[] pobjParameters)
        {
            using (SqlConnection objCon = new SqlConnection(pstrDSN))
            {

                objCon.Open();
                try
                {
                    // Create a Command based on the parameters and connection
                    using (SqlCommand objCmd = new SqlCommand(pstrCommand, objCon))
                    {
                        objCmd.CommandTimeout = m_CommandTimeout;
                        // Set the type of the Command
                        objCmd.CommandType = CommandType.StoredProcedure;
                        // Add any Parameters to the Command if there are any
                        if (pobjParameters != null)
                        {
                            foreach (SqlParameter objParam in pobjParameters)
                            {
                                objCmd.Parameters.Add(objParam);
                            }
                        }
                        return objCmd.ExecuteNonQuery();
                    }
                }
                catch (Exception err)
                {
                    throw err;
                }
                finally
                {
                    objCon.Close();
                }
            }

        }

        /// <summary>
        /// Executes a Stored Procedure Query and returns a DataReader
        /// </summary>
        /// <param name="pstrDSN">Database Connection String</param>
        /// <param name="pstrCommand">Stored Procedure or SQL to execute</param>
        /// <param name="pobjParameters">SqlParameter Array. Can created using the following code:-
        /// SqlParameter[] objParameters=new SqlParameter[2];
        /// objParameters[0]=new SqlParameter("myParam1","1");
        /// objParameters[1]=new SqlParameter("myParam2","22"); 
        /// <returns>SqlDataReader</returns>
        static public SqlDataReader CommandQuery(string pstrDSN, string pstrCommand, SqlParameter[] pobjParameters)
        {
            return Query(pstrDSN, pstrCommand, CommandType.StoredProcedure, pobjParameters);

        }

        static public SqlDataReader CommandQuerying(SqlConnection objCons, string pstrCommand, SqlParameter[] pobjParameters)
        {
            return Querying(objCons, pstrCommand, CommandType.StoredProcedure, pobjParameters);
        }

        static public SqlDataReader Querying(SqlConnection objCon, string pstrCommand, CommandType pobjCommandType, SqlParameter[] pobjParameters)
        {
            // Create a Command based on the parameters and connection
            SqlCommand objCmd = new SqlCommand(pstrCommand, objCon);
            // Set the type of the Command
            objCmd.CommandType = pobjCommandType;
            // Add any Parameters to the Command if there are any
            if (pobjParameters != null)
            {
                foreach (SqlParameter objParam in pobjParameters)
                {
                    objCmd.Parameters.Add(objParam);
                }
            }
            SqlDataReader myReader = null;
            objCmd.CommandTimeout = m_CommandTimeout;
            try
            {
                objCon.Open();
                {
                    myReader = objCmd.ExecuteReader();
                }
            }
            catch (Exception err)
            {
                //objCon.Close();
                throw err;
            }
            finally
            {
                //bjCon.Close();
            }
            return myReader;
        }


        /// <summary>
        /// Perfoms a SqlCLient Query
        /// </summary>
        /// <param name="pstrDSN">Database Connection String</param>
        /// <param name="pstrCommand">Stored Procedure or SQL to execute</param>
        /// <param name="pobjCommandType">Type of Command eg CommandType.StoredProcedure or CommandType.Text </param> 
        /// <param name="pobjParameters">SqlParameter Array. Can created using the following code:-
        /// SqlParameter[] objParameters=new SqlParameter[2];
        /// objParameters[0]=new SqlParameter("myParam1","1");
        /// objParameters[1]=new SqlParameter("myParam2","22"); 
        /// </param>
        /// <param name="pstrTableName">Name of the Dataset table to fill</param>
        /// <param name="pobjDataSet">Dataset to be filled with the results of the query</param>
        static public void Query(string pstrDSN, string pstrCommand, CommandType pobjCommandType, SqlParameter[] pobjParameters, string pstrTableName, DataSet pobjDataSet)
        {
            // Set up a Connection
            using (SqlConnection objCon = new SqlConnection(pstrDSN))
            {

                objCon.Open();
                try
                {
                    // Create a Command based on the parameters and connection
                    using (SqlCommand objCmd = new SqlCommand(pstrCommand, objCon))
                    {
                        objCmd.CommandTimeout = m_CommandTimeout;
                        // Set the type of the Command
                        objCmd.CommandType = pobjCommandType;
                        // Add any Parameters to the Command if there are any
                        if (pobjParameters != null)
                        {
                            foreach (SqlParameter objParam in pobjParameters)
                            {
                                objCmd.Parameters.Add(objParam);
                            }
                        }
                        // Use a Dataadapter to fill th parameter supplied dataset
                        using (SqlDataAdapter objDA = new SqlDataAdapter(objCmd))
                        {
                            try
                            {
                                objDA.Fill(pobjDataSet, pstrTableName);
                            }
                            catch (Exception err)
                            {
                                throw err;
                            }
                        }

                    }
                }
                catch (Exception err)
                {
                    throw err;
                }
                finally
                {
                    objCon.Close();
                }
            }
        }

        /// <summary>
        /// Perfoms a SqlCLient Query for a DataReader
        /// </summary>
        /// <param name="pstrDSN">Database Connection String</param>
        /// <param name="pstrCommand">Stored Procedure or SQL to execute</param>
        /// <param name="pobjCommandType">Type of Command eg CommandType.StoredProcedure or CommandType.Text </param> 
        /// <param name="pobjParameters">SqlParameter Array. Can created using the following code:-
        /// SqlParameter[] objParameters=new SqlParameter[2];
        /// objParameters[0]=new SqlParameter("myParam1","1");
        /// objParameters[1]=new SqlParameter("myParam2","22"); 
        /// </param>
        /// <returns>SqlDataReader</returns>
        static public SqlDataReader Query(string pstrDSN, string pstrCommand, CommandType pobjCommandType, SqlParameter[] pobjParameters)
        {
            // Set up a Connection
            SqlConnection objCon = new SqlConnection(pstrDSN);
            // Create a Command based on the parameters and connection
            SqlCommand objCmd = new SqlCommand(pstrCommand, objCon);
            // Set the type of the Command
            objCmd.CommandType = pobjCommandType;
            // Add any Parameters to the Command if there are any
            if (pobjParameters != null)
            {
                foreach (SqlParameter objParam in pobjParameters)
                {
                    objCmd.Parameters.Add(objParam);
                }
            }
            objCmd.CommandTimeout = m_CommandTimeout;
            objCon.Open();
            SqlDataReader myReader = null;
            try
            {
                myReader = objCmd.ExecuteReader();
            }
            catch (Exception err)
            {
                //objCon.Close();
                throw err;
            }
            finally
            {
                //objCon.Close();
            }
            return myReader;
        }


        /// <summary>
        /// Executes a Stored procedure which returns a single value
        /// </summary>
        /// <param name="pstrDSN">Database Connection String</param>
        /// <param name="pstrCommand">Stored Procedure or SQL to execute</param>
        /// <param name="pobjParameters">SqlParameter Array. Can created using the following code:-
        /// SqlParameter[] objParameters=new SqlParameter[2];
        /// objParameters[0]=new SqlParameter("myParam1","1");
        /// objParameters[1]=new SqlParameter("myParam2","22"); 
        /// </param>
        /// <returns></returns>
        static public object CommandScalar(string pstrDSN, string pstrCommand, SqlParameter[] pobjParameters)
        {
            return Scalar(pstrDSN, pstrCommand, CommandType.StoredProcedure, pobjParameters);
        }
        /// <summary>
        /// Executes a Scalar Stored procedure which returns a single value
        /// </summary>
        /// <param name="pstrDSN">Database Connection String</param>
        /// <param name="pstrCommand">Stored Procedure or SQL to execute</param>
        /// <returns></returns>
        static public object CommandScalar(string pstrDSN, string pstrCommand)
        {
            return Scalar(pstrDSN, pstrCommand, CommandType.StoredProcedure, null);
        }
        /// <summary>
        /// Executes a Scalar Query procedure which returns a single value
        /// </summary>
        /// <param name="pstrDSN">Database Connection String</param>
        /// <param name="pstrCommand">Stored Procedure or SQL to execute</param>
        /// <param name="pobjParameters">SqlParameter Array. Can created using the following code:-
        /// SqlParameter[] objParameters=new SqlParameter[2];
        /// objParameters[0]=new SqlParameter("myParam1","1");
        /// objParameters[1]=new SqlParameter("myParam2","22"); 
        /// </param>
        /// <returns>Scalar Single value as an object</returns>
        static public object SqlScalar(string pstrDSN, string pstrCommand, SqlParameter[] pobjParameters)
        {
            return Scalar(pstrDSN, pstrCommand, CommandType.Text, pobjParameters);
        }
        /// <summary>
        /// Executes a Scalar Query which returns a single value
        /// </summary>
        /// <param name="pstrDSN">Database Connection String</param>
        /// <param name="pstrCommand">Stored Procedure or SQL to execute</param>
        /// <returns>Scalar Single value as an object</returns>
        static public object SqlScalar(string pstrDSN, string pstrCommand)
        {
            return Scalar(pstrDSN, pstrCommand, CommandType.Text, null);
        }
        /// <summary>
        /// Executes a Scalar Query or Stored procedure which returns a single value
        /// </summary>
        /// <param name="pstrDSN">Database Connection String</param>
        /// <param name="pstrCommand">Stored Procedure or SQL to execute</param>
        /// <param name="pobjCommandType">Type of Command eg CommandType.StoredProcedure or CommandType.Text </param> 
        /// <param name="pobjParameters">SqlParameter Array. Can created using the following code:-
        /// SqlParameter[] objParameters=new SqlParameter[2];
        /// objParameters[0]=new SqlParameter("myParam1","1");
        /// objParameters[1]=new SqlParameter("myParam2","22"); 
        /// </param>
        /// <returns>Scalar Single value as an object</returns>
        static public object Scalar(string pstrDSN, string pstrCommand, CommandType pobjCommandType, SqlParameter[] pobjParameters)
        {
            // Set up a Connection

            using (SqlConnection objCon = new SqlConnection(pstrDSN))
            {
                objCon.Open();
                try
                {
                    // Create a Command based on the parameters and connection
                    using (SqlCommand objCmd = new SqlCommand(pstrCommand, objCon))
                    {
                        objCmd.CommandTimeout = m_CommandTimeout;
                        // Set the type of the Command
                        objCmd.CommandType = pobjCommandType;
                        // Add any Parameters to the Command if there are any
                        if (pobjParameters != null)
                        {
                            foreach (SqlParameter objParam in pobjParameters)
                            {
                                objCmd.Parameters.Add(objParam);
                            }
                        }
                        return objCmd.ExecuteScalar();
                    }
                }
                catch (Exception err)
                {
                    throw err;
                }
                finally
                {
                    objCon.Close();
                }
            }

        }
        /// <summary>
        /// Persists a Dataset or Typed Dataset
        /// </summary>
        /// <param name="strDSN">Database Connection String</param>
        /// <param name="pstrEmptyQuerySQL">SQL Query that returns the columns matching the dataset to be persisted with no rows.
        /// eg select * from titles where is is null
        /// </param>
        /// <param name="tableName">Name of the Dataset table to fill</param>
        /// <param name="pobjDataSet">The Dataset to persist</param>
        static public void Persist(string pstrDSN, string pstrEmptyQuerySQL, string pstrTableName, DataSet pobjDataSet)
        {
            using (SqlConnection objConnnection = new SqlConnection(pstrDSN))
            {
                objConnnection.Open();
                try
                {
                    using (SqlCommand objCommand = new SqlCommand(pstrEmptyQuerySQL, objConnnection))
                    {
                        objCommand.CommandTimeout = m_CommandTimeout;
                        using (SqlDataAdapter objAdapter = new SqlDataAdapter(objCommand))
                        {
                            using (SqlCommandBuilder objBuilder = new SqlCommandBuilder(objAdapter))
                            {
                                DataSet ds = pobjDataSet.GetChanges();
                                if (ds != null && ds.HasChanges())
                                {
                                    objAdapter.Update(ds, pstrTableName);
                                    ds.AcceptChanges();
                                }
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    throw err;
                }
                finally
                {
                    objConnnection.Close();
                }
            }
        }
        /// <summary>
        /// Persists a Dataset or Typed Dataset
        /// </summary>
        /// <param name="strDSN">Database Connection String</param>
        /// <param name="pobjInsertCommand">SqlCommand containing either a Stored Procedure call to be used for 
        /// inserting new records in the dataset
        /// </param>
        /// <param name="pobjUpdateCommand">SqlCommand containing either a Stored Procedure call to be used for 
        /// updating modified records in the dataset
        /// </param>
        /// <param name="pobjDeleteCommand">SqlCommand containing either a Stored Procedure call to be used for 
        /// deleting deleted records in the dataset
        /// </param>
        /// <param name="tableName">Name of the Dataset table to fill</param>
        /// <param name="pobjDataSet">The Dataset to persist</param>
        /// <example>
        /// <code>
        /// SqlCommand objInsertCommand=new SqlCommand();
        ///	objInsertCommand.CommandText="usp_Test_Persist";
        ///	objInsertCommand.CommandType=CommandType.StoredProcedure;
        ///	objInsertCommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int,4,"id"));
        ///	objInsertCommand.Parameters[0].Direction =ParameterDirection.InputOutput;  
        ///	objInsertCommand.Parameters.Add(new SqlParameter("@name",SqlDbType.VarChar,50,"name"));
        ///	objInsertCommand.Connection=con; 
        ///	DataHelper.Persist("Provider=...",objInsertCommand,null,"test",ds)
        /// </code> 
        /// </example> 
        static public int Persist(string pstrDSN, SqlCommand pobjInsertCommand, SqlCommand pobjUpdateCommand, SqlCommand pobjDeleteCommand, string pstrTableName, DataSet pobjDataSet)
        {
            int rowsUpdated = 0;
            using (SqlConnection objConnnection = new SqlConnection(pstrDSN))
            {
                objConnnection.Open();
                try
                {
                    using (SqlDataAdapter objAdapter = new SqlDataAdapter())
                    {
                        if (pobjInsertCommand != null) pobjInsertCommand.Connection = objConnnection;
                        if (pobjUpdateCommand != null) pobjUpdateCommand.Connection = objConnnection;
                        if (pobjDeleteCommand != null) pobjDeleteCommand.Connection = objConnnection;

                        if (pobjInsertCommand != null) objAdapter.InsertCommand = pobjInsertCommand;
                        if (pobjUpdateCommand != null) objAdapter.UpdateCommand = pobjUpdateCommand;
                        if (pobjDeleteCommand != null) objAdapter.DeleteCommand = pobjDeleteCommand;

                        rowsUpdated = objAdapter.Update(pobjDataSet, pstrTableName);

                    }
                }
                catch (Exception err)
                {
                    throw err;
                }
                finally
                {
                    objConnnection.Close();
                }

            }
            return rowsUpdated;
        }
        /// <summary>
        /// Persists a Dataset or Typed Dataset
        /// </summary>
        /// <param name="strDSN">Database Connection String</param>
        /// <param name="pobjPersistCommand">SqlCommand containing either a Stored Procedure call to be used for 
        /// inserting and updating records in the dataset
        /// </param>
        /// <param name="pobjDeleteCommand">SqlCommand containing either a Stored Procedure call to be used for 
        /// deleting deleted records in the dataset
        /// </param>
        /// <param name="tableName">Name of the Dataset table to fill</param>
        /// <param name="pobjDataSet">The Dataset to persist</param>
        /// <example>
        /// <code>
        /// SqlCommand objInsertCommand=new SqlCommand();
        ///	objInsertCommand.CommandText="usp_Test_Persist";
        ///	objInsertCommand.CommandType=CommandType.StoredProcedure;
        ///	objInsertCommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int,4,"id"));
        ///	objInsertCommand.Parameters[0].Direction =ParameterDirection.InputOutput;  
        ///	objInsertCommand.Parameters.Add(new SqlParameter("@name",SqlDbType.VarChar,50,"name"));
        ///	objInsertCommand.Connection=con; 
        ///	DataHelper.Persist("Provider=...",objInsertCommand,null,"test",ds)
        /// </code> 
        /// </example> 
        static public int Persist(string pstrDSN, SqlCommand pobjPersistCommand, SqlCommand pobjDeleteCommand, string pstrTableName, DataSet pobjDataSet)
        {
            return Persist(pstrDSN, pobjPersistCommand, pobjPersistCommand, pobjDeleteCommand, pstrTableName, pobjDataSet);
        }
        /// <summary>
        /// Persists a Dataset or Typed Dataset
        /// </summary>
        /// <param name="strDSN">Database Connection String</param>
        /// <param name="pobjPersistCommand">SqlCommand containing either a Stored Procedure call to be used for 
        /// inserting and updating records in the dataset
        /// </param>
        /// <param name="tableName">Name of the Dataset table to fill</param>
        /// <param name="pobjDataSet">The Dataset to persist</param>
        /// <example>
        /// <code>
        /// SqlCommand objInsertCommand=new SqlCommand();
        ///	objInsertCommand.CommandText="usp_Test_Persist";
        ///	objInsertCommand.CommandType=CommandType.StoredProcedure;
        ///	objInsertCommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int,4,"id"));
        ///	objInsertCommand.Parameters[0].Direction =ParameterDirection.InputOutput;  
        ///	objInsertCommand.Parameters.Add(new SqlParameter("@name",SqlDbType.VarChar,50,"name"));
        ///	objInsertCommand.Connection=con; 
        ///	DataHelper.Persist("Provider=...",objInsertCommand,null,"test",ds)
        /// </code> 
        /// </example> 
        static public void Persist(string pstrDSN, SqlCommand pobjPersistCommand, string pstrTableName, DataSet pobjDataSet)
        {
            Persist(pstrDSN, pobjPersistCommand, pobjPersistCommand, null, pstrTableName, pobjDataSet);
        }
        /// <summary>
        /// Perfoms a SqlCLient Query
        /// </summary>
        /// <param name="pstrDSN">Database Connection String</param>
        /// <param name="pstrCommand">Stored Procedure or SQL to execute</param>
        /// <param name="pobjParameters">SqlParameter Array. Can created using the following code:-
        /// SqlParameter[] objParameters=new SqlParameter[2];
        /// objParameters[0]=new SqlParameter("myParam1","1");
        /// objParameters[1]=new SqlParameter("myParam2","22"); 
        /// </param>
    }
}