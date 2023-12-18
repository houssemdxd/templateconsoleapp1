using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUtilities;
namespace TemplateConsoleApp
{
    class ConsoleUserInterface
    {
        public static Article GetArticleFromInterface()
        {
            Article Ar = new Article();
            Console.WriteLine("\n **** Saisie d'un Article **** ");
            Console.Write(" Reference  : ");
            Ar.Reference = Int32.Parse(Console.ReadLine());
            Console.Write(" Designation  : ");
            Ar.Designation = Console.ReadLine();
            Console.Write(" Categorie  : ");
            Ar.Categorie = Console.ReadLine();
            Console.Write(" Prix  : ");
            Ar.Prix = Single.Parse(Console.ReadLine());
            Console.Write(" Date de fabrication  : ");
            Ar.DateFabrication = DateTime.Parse(Console.ReadLine());
            Console.Write("Promo (O / N) : ");
            Ar.Promo = Console.ReadLine() == "O" ? true : false;

            return Ar;
        }

        public static void ShowArticle(Article Ar)
        {
            Console.Write("\n Reference : " + Ar.Reference);
            Console.Write("\n Designation : " + Ar.Designation);
            Console.Write("\n Categorie : " + Ar.Categorie);
            Console.Write("\n Prix : " + Ar.Prix);
            Console.Write("\n Date de fabrication : " + Ar.DateFabrication.ToShortDateString());
            Console.Write("\n Promo : " + Ar.Promo.ToString());
        }

        static char ChoixAction()
        {
            char Choix = ' ';
            do
            {
                Console.Clear();
                Console.WriteLine("***************************************************** \n");
                Console.WriteLine("                         MENU                         \n");
                Console.WriteLine("  1 : AJOUT");
                Console.WriteLine("  2 : SUPPRESSION");
                Console.WriteLine("  3 : MODIFICATION");
                Console.WriteLine("  4 : CONSULTATION");
                Console.WriteLine("  5 : QUITTER");

                Console.WriteLine("\n*****************************************************");
                Console.Write("Donner votre choix : ");
                string str = Console.ReadLine();
                if (str.Length != 0)
                    Choix = Char.ToLower(str.Trim()[0]);
            }
            while (Choix != '1' && Choix != '2' && Choix != '3' && Choix != '4' && Choix != '5');
            return Choix;
        }

        public static void Menu()
        {
            char Choix;
            do
            {
                Choix = ChoixAction();
                try
                {
                    
                    switch (Choix)
                    {
                        case '1':
                            {
                                Console.Clear();
                                Console.WriteLine("\n************ Ajout ************");
                                BLL_Article.Add(GetArticleFromInterface());
                                break;
                            }

                        case '2':
                            {
                                Console.Clear();
                                Console.WriteLine("\n************ Suppression ************");
                                Console.Write("\n\nDonner la Reference de l'Article à supprimer : ");
                                BLL_Article.Delete(Int32.Parse(Console.ReadLine()));
                                break;
                              
                            }

                        case '3':
                            {
                                int Reference;
                                Article Ar;
                                Console.Clear();
                                Console.WriteLine("\n************ Modification ************");
                                Console.Write("\n\nDonner la Reference de l'Article à modifier : ");
                                Reference = Int32.Parse(Console.ReadLine());
                                Ar = BLL_Article.GetById(Reference);
                                ShowArticle(Ar);
                                Console.WriteLine("\nDonner les nouvelles données de l'Article :");
                                BLL_Article.Update(Ar, ConsoleUserInterface.GetArticleFromInterface());
                                break;
                            }

                        case '4':
                            {
                                Console.Clear();
                                Console.WriteLine("\n************ Consultation ************");
                                List<Article> LesArticles = BLL_Article.GetAll();
                                foreach (Article Ar in LesArticles)
                                {
                                    Console.WriteLine("\n \n ------------------------- ");
                                    ConsoleUserInterface.ShowArticle(Ar);
                                }
                                Console.WriteLine("\nTapez un caractère pour revenir au menu");
                                Console.Read();
                                break;
                            }
                    }
                }
                catch (MyException MyEx)
                {
                    Console.WriteLine("\n----------------------------------------------\n" +
                                      MyEx.MyExceptionTitle + " : " + MyEx.MyExceptionMessage +
                                      "\n-----------------------------------------------\n");
                    Console.WriteLine("\nTapez un caractère pour revenir au menu");
                    Console.Read();
                    
                }
            } while (Choix != '5');
        }
    }
   
}
