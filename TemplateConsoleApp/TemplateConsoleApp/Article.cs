using System;


    public class Article
    {
        public int Reference { get; set; }
        public string Designation { get; set; }
        public string Categorie { get; set; } 
        public float? Prix { get; set; }    
        public DateTime DateFabrication { get; set; }      
        public bool Promo { get; set; }  
       
        public Article()
        {
        }
        public Article(int pReference, string pDesignation, string pCategorie, float? pPrix, DateTime pDateFabrication, bool pPromo)
        {
            Reference = pReference;
            Designation = pDesignation;
            Prix = pPrix;
            DateFabrication = pDateFabrication;
            Categorie = pCategorie;
            Promo = pPromo;
        }
    }

