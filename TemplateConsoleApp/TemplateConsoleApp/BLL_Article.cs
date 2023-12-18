using System;
using System.Collections.Generic;
using System.Text;

 
    public class BLL_Article
    {
            public static void Add(Article p)
            {
                DAL_Article.Add(p);
            }

            public static void Update(Article CurArticle, Article NewArticle)
            {
                DAL_Article.Update(CurArticle, NewArticle);            
            }
     
            public static void Delete(int pRef)
            {
                DAL_Article.Delete(pRef);
            }
            
            public static Article GetById(int pRef)
            {
                return DAL_Article.SelectById(pRef);
            }

            public static List<Article> GetAll()
            {
                return DAL_Article.SelectAll();
            }
    }

