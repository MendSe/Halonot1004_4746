using System;
using System.Linq;
using DAL.Entities;
using DAL.Models;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new DBContext())
            {
                // Add a new Catalogue
                var catalogue = new Catalogue { Type = "Test Catalogue" };
                context.Catalogue.Add(catalogue);
                context.SaveChanges();

                // Retrieve and display the added Catalogue
                var addedCatalogue = context.Catalogue.FirstOrDefault(c => c.Type == "Test Catalogue");
                if (addedCatalogue != null)
                {
                    Console.WriteLine($"Catalogue added: {addedCatalogue.Type}");
                }
                else
                {
                    Console.WriteLine("Failed to add Catalogue.");
                }

                // Add similar tests for other entities (Games, GPU, RAM, Servers)

                // Add a list of games

                var game1 = new Games { Id = 1 };
                var game2 = new Games { Id = 2 };





            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
