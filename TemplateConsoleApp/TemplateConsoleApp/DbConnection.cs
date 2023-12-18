using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

  
    public class DbConnection
    {
        static string StrCnn = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source = ArticlesDB.accdb";
        public static OleDbConnection GetConnection()
        {
            return new OleDbConnection(StrCnn);
        }
        
    }

