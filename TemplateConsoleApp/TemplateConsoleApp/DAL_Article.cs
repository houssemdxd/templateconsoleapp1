using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using MyUtilities;


    public class DAL_Article
    {
       
        private static bool CheckEntityUnicity(int EntityKey)
        {
            // il faut que la concaténation du Nom et du préNom soit unique
            int NbOccs = 0;
            using (OleDbConnection Cnn = DbConnection.GetConnection())
            {
                string StrSQL = "SELECT COUNT(*) FROM Article WHERE Reference = @EntityKey";
                OleDbCommand Cmd = new OleDbCommand(StrSQL, Cnn);
                Cmd.Parameters.AddWithValue("@EntityKey", EntityKey);
                NbOccs = (int)DataBaseAccessUtilities.ScalarRequest(Cmd);
            }

            if (NbOccs == 0)
                return true;
            else
                return false;
        }
        private static Article GetEntityFromDataRow(DataRow dr)
        {
            Article p = new Article();
            p.Reference = (int)dr["Reference"];
            p.Designation = dr["Designation"] == System.DBNull.Value ? "" : (string)dr["Designation"];
            p.Categorie = dr["Categorie"] == System.DBNull.Value ? "" : (string)dr["Categorie"];

            // Champ qui accepte la valeur nulle
            if (dr["Prix"] == System.DBNull.Value)
                    p.Prix = null;
                else
                    p.Prix = (float)dr["Prix"];

            p.DateFabrication = (DateTime)dr["DateFabrication"];
            p.Promo = (bool)dr["Promo"];

            return p;
        }
        private static List<Article> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<Article> L = new List<Article>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(DAL_Article.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }

        public static void Add(Article p)
        {
            using (OleDbConnection Cnn = DbConnection.GetConnection())
            {
                // Test de l'unicité du Nom de la Article  
                if (DAL_Article.CheckEntityUnicity(p.Reference) == true)
                {
                    // Ok on peut ajouter
                    string StrSQL = "INSERT INTO Article (Reference, Designation, Categorie, Prix, DateFabrication, Promo) VALUES (@Reference, @Designation, @Categorie," +
                    " @Prix,  @DateFabrication, @Promo)";

                    OleDbCommand Cmd = new OleDbCommand(StrSQL, Cnn);
                    Cmd.Parameters.Add("@Reference", OleDbType.Integer).Value = p.Reference;
                    Cmd.Parameters.Add("@Designation", OleDbType.VarChar).Value = p.Designation;
                    Cmd.Parameters.Add("@Categorie", OleDbType.VarChar).Value = p.Categorie;
                    Cmd.Parameters.Add("@Prix", OleDbType.Single).Value = p.Prix;
                    Cmd.Parameters.Add("@DateFabrication", OleDbType.Date).Value = p.DateFabrication;
                    Cmd.Parameters.Add("@Promo", OleDbType.Boolean).Value = p.Promo;

                    DataBaseAccessUtilities.NonQueryRequest(Cmd);
                }
                else
                {
                    throw new MyException("Erreur dans l'ajout d'une Article", "la référence est déjà utilisée", "DAL");
                }
            }
        }
        public static void Delete(int EntityKey)
        {
            using (OleDbConnection Cnn = DbConnection.GetConnection())
            {
                string StrSQL = "DELETE FROM Article WHERE Reference = @EntityKey";
                OleDbCommand Cmd = new OleDbCommand(StrSQL, Cnn);
                Cmd.Parameters.Add("@EntityKey", OleDbType.UnsignedBigInt).Value = EntityKey;
                DataBaseAccessUtilities.NonQueryRequest(Cmd);
            }
        }
        public static void Update(Article CurArticle, Article NewArticle)
        {
            using (OleDbConnection Cnn = DbConnection.GetConnection())
            {
                if ((CurArticle.Reference != NewArticle.Reference) && (DAL_Article.CheckEntityUnicity(NewArticle.Reference) == false))
                {
                    throw new MyException("Erreur dans la modification d'un Article", "La nouvelle Reference est déjà utilisée", "DAL");
                }
                else
                {
                    string StrSQL = "UPDATE Article SET Reference=@Reference, Designation= @Designation, Categorie = @Categorie, Prix = @Prix, DateFabrication = @DateFabrication, Promo=@Promo WHERE Reference = @CurReference";
                    OleDbCommand Cmd = new OleDbCommand(StrSQL, Cnn);
                    Cmd.Parameters.Add("@Reference", OleDbType.Integer).Value = NewArticle.Reference;
                    Cmd.Parameters.Add("@Designation", OleDbType.VarChar).Value = NewArticle.Designation;
                    Cmd.Parameters.Add("@Categorie", OleDbType.VarChar).Value = NewArticle.Categorie;
                    Cmd.Parameters.Add("@Prix", OleDbType.Single).Value = NewArticle.Prix;
                    Cmd.Parameters.Add("@DateFabrication", OleDbType.Date).Value = NewArticle.DateFabrication;
                    Cmd.Parameters.Add("@Promo", OleDbType.Boolean).Value = NewArticle.Promo;
                    Cmd.Parameters.Add("@CurReference", OleDbType.Integer).Value = CurArticle.Reference;

                    DataBaseAccessUtilities.NonQueryRequest(Cmd);
                }
            }
        }
        public static Article SelectById(int EntityKey)
        {
            using (OleDbConnection Cnn = DbConnection.GetConnection())
            {
                string StrSQL = "SELECT * FROM Article WHERE Reference = @EntityKey";
                OleDbCommand Cmd = new OleDbCommand(StrSQL, Cnn); Cmd.Parameters.AddWithValue("@EntityKey", EntityKey);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Cmd);
                if (dt != null && dt.Rows.Count != 0)
                    return DAL_Article.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
 
        // A définir l'une des deux de ces deux méthodes
        // La version connectée de Select et plus rapide que la version déconnectée
        public static List<Article> SelectAll()
        {
            List<Article> ListeArticles = new List<Article>();
            Article p;

            using (OleDbConnection Cnn = DbConnection.GetConnection())
            {
                try
                {
                    string StrSQL = "SELECT * FROM Article ";
                    OleDbCommand Cmd = new OleDbCommand(StrSQL, Cnn);
                    Cnn.Open();
                    OleDbDataReader dr = Cmd.ExecuteReader();
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            p = new Article();                                               
                            p.Reference = dr.GetInt16(0);
                            p.Designation = dr.GetString(1);
                            p.Categorie = dr.GetString(2);
                            p.Prix = dr.GetFloat(3);                           
                            p.DateFabrication = dr.GetDateTime(4);
                            p.Promo = dr.GetBoolean(5);
                            ListeArticles.Add(p);
                        }
                    }
                    return ListeArticles;
                }
                catch (OleDbException e)
                {
                    //throw new MyException(e, "DataBase Error", "Erreur d'éxecution de la requête de sélection : \n", "DAL");
                    throw new MyException(e, "DataBase Errors", e.Message, "DAL");
                }
                finally
                {
                    Cnn.Close();
                }

            }

        }
        
      
        //public static List<Article> DisconnectedSelectAll()
        //{
        //    DataTable dt;
        //    using (OleDbConnection Cnn = DbConnection.GetConnection())
        //    {
        //        string StrSQL = "SELECT * FROM Article ";
        //        OleDbCommand Cmd = new OleDbCommand(StrSQL, Cnn);
        //        dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Cmd);
        //    }
        //    return GetListFromDataTable(dt);
        //}
    }

