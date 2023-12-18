
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace MyUtilities
{
    public static class DataBaseAccessUtilities
    {

        public static int NonQueryRequest(OleDbCommand MyCommand)
        {
            try
            {
                try
                {
                    MyCommand.Connection.Open();
                }
                catch (OleDbException e)
                {
                    throw new MyException(e,"DataBase Error", "Erreur de connexion à la base", "DAL");
                }

                return MyCommand.ExecuteNonQuery();
            }
            catch (OleDbException e)
            {
                throw new MyException(e, "DataBase Error", e.Message, "DAL");
            }
            finally
            {
                MyCommand.Connection.Close();
            }

        }
        public static int NonQueryRequest(string StrRequest, OleDbConnection MyConnection)
        {
            try
            {
                try
                {
                    MyConnection.Open();
                }
                catch (OleDbException e)
                {
                    throw new MyException(e, "DataBase Error", "Erreur de connexion à la base", "DAL");
                }

                OleDbCommand MyCommand = new OleDbCommand(StrRequest, MyConnection);
                return MyCommand.ExecuteNonQuery();
            }
            catch (OleDbException e)
            {
                throw new MyException(e, "DataBase Error", "Erreur d'éxecution de la requête : \n", "DAL");
            }
            finally
            {
                MyConnection.Close();
            }

        }        

        public static object ScalarRequest(OleDbCommand MyCommand)
        {
            try
            {
                try
                {
                    MyCommand.Connection.Open();
                }
                catch (OleDbException e)
                {
                    throw new MyException(e, "DataBase Error", "Erreur de connexion à la base", "DAL");
                }

                return MyCommand.ExecuteScalar();
            }
            catch (OleDbException e)
            {
                throw new MyException(e, "DataBase Error", "Erreur d'éxecution de la requête : \n", "DAL");
            }
            finally
            {
                MyCommand.Connection.Close();
            }
        }
        public static object ScalarRequest(string StrRequest, OleDbConnection MyConnection)
        {
            try
            {
                try
                {
                    MyConnection.Open();
                }
                catch (OleDbException e)
                {
                    throw new MyException(e, "DataBase Error", "Erreur de connexion à la base", "DAL");
                }
                OleDbCommand MyCommand = new OleDbCommand(StrRequest, MyConnection);

                return MyCommand.ExecuteScalar();
            }
            catch (OleDbException e)
            {
                throw new MyException(e, "DataBase Error", "Erreur d'éxecution de la requête : \n", "DAL");
            }
            finally
            {
                MyConnection.Close();
            }
        }      

        public static DataTable DisconnectedSelectRequest(OleDbCommand MyCommand)
        {
            try
            {
                DataTable Table;
                OleDbDataAdapter SelectAdapter = new OleDbDataAdapter(MyCommand);
                Table = new DataTable();
                SelectAdapter.Fill(Table);
                return Table;
            }
            catch (OleDbException e)
            {
                //throw new MyException(e, "DataBase Error", "Erreur d'éxecution de la requête de sélection : \n", "DAL");
                throw new MyException(e, "DataBase Errors", e.Message, "DAL");
            }
            finally
            {
                MyCommand.Connection.Close();
            }
        }
        public static DataTable DisconnectedSelectRequest(string StrSelectRequest, OleDbConnection MyConnection)
        {
            try
            {
                DataTable Table;
                OleDbCommand SelectCommand = new OleDbCommand(StrSelectRequest, MyConnection);
                OleDbDataAdapter SelectAdapter = new OleDbDataAdapter(SelectCommand);
                Table = new DataTable();
                SelectAdapter.Fill(Table);
                return Table;
            }
            catch (OleDbException e)
            {
                
                throw new MyException(e, "DataBase Error", "Erreur d'éxecution de la requête de sélection : \n", "DAL");
                 
            }
            finally
            {
                MyConnection.Close();
            }
        }

        public static OleDbDataReader ConnectedSelectRequest(OleDbCommand MyCommand)
        {
            try
            {
                MyCommand.Connection.Open();
                OleDbDataReader dr = MyCommand.ExecuteReader();
                return dr;
                
            }
            catch (OleDbException e)
            {
                //throw new MyException(e, "DataBase Error", "Erreur d'éxecution de la requête de sélection : \n", "DAL");
                throw new MyException(e, "DataBase Errors", e.Message, "DAL");
            }
            finally
            {
                MyCommand.Connection.Close();
            }
        }
       

        public static bool CheckFieldValueExistence(string TableName, string FieldName, OleDbType FieldType, object FieldValue, OleDbConnection MyConnection)
        {
            try
            {
                string StrRequest = "SELECT COUNT(" + FieldName + ") FROM " + TableName + " WHERE ((" + FieldName + " = @" + FieldName + ")";
                StrRequest += "OR ( (@" + (FieldName + 1).ToString() + " IS NULL)AND (" + FieldName + " IS NULL)))";
                OleDbCommand Command = new OleDbCommand(StrRequest, MyConnection);                
                Command.Parameters.Add( "@"+FieldName, FieldType).Value = FieldValue;
                Command.Parameters.Add( "@"+FieldName+1, FieldType).Value = FieldValue;        
                return ((int)DataBaseAccessUtilities.ScalarRequest(Command) != 0);
            }
            catch (OleDbException e)
            {
                throw new MyException(e, "DataBase Error", "Erreur d'éxecution de la requête de vérification de l'existence de la valeur : \n", "DAL");
            }
            finally
            {
                MyConnection.Close();
            }

        }
        
        public static object GetMaxFieldValue(OleDbConnection MyConnection, string TableName, string FieldName)
        {
            try
            {
                string StrMaxRequest = "SELECT MAX(" + FieldName + ") FROM " + TableName;

                OleDbCommand Command = new OleDbCommand(StrMaxRequest, MyConnection);               
                return (DataBaseAccessUtilities.ScalarRequest(Command));

            }
            catch (OleDbException e)
            {
                throw new MyException(e, "DataBase Error", "Erreur d'éxecution de la requête de vérification de l'existence de la valeur : \n", "DAL");
            }
            finally
            {
                MyConnection.Close();
            }
        }

    }

    public class MyException : Exception
    {

        string _Level;
        string _MyExceptionTitle;
        string _MyExceptionMessage;


        public string Level
        {
            get
            {
                return this._Level;
            }
        }

        public string MyExceptionTitle
        {
            get
            {
                return this._MyExceptionTitle;
            }
        }

        public string MyExceptionMessage
        {
            get
            {
                return this._MyExceptionMessage.ToString();
            }
        }


        public MyException(string MyExceptionTitle, string MyExceptionMessage, string lev)
        {
            this._Level = lev;
            this._MyExceptionTitle = MyExceptionTitle;
            this._MyExceptionMessage = MyExceptionMessage;
        }

        public MyException(Exception e, string MyExceptionTitle, string MyExceptionMessage, string lev) : base(e.Message)
        {
            this._Level = lev;
            this._MyExceptionTitle = MyExceptionTitle;
            this._MyExceptionMessage = MyExceptionMessage;
        }

    }
}
