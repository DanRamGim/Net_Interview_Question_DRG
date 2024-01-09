using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;

namespace Net_Interview_Question
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new DBModelContext())
            {
                int bulkload = 0;

                db.DBModel.RemoveRange(db.DBModel);
                db.SaveChanges();
                try
                {
                    while (bulkload != 1 && bulkload != 2)
                    {
                        Console.Write("If you want to load a file with your questionnaire, please type 1, if you want to load them manually please type 2: \n");
                        try
                        {
                            bulkload = Int32.Parse(Console.ReadLine());
                        }
                        catch (Exception)
                        {
                            Console.Write("That was not a number please try again: \n");
                        }
                    }

                    if (bulkload == 2)
                    {
                        Console.Write("Enter your name: \n");
                        var name = Console.ReadLine();

                        Console.Write("Enter your question: \n");
                        var question = Console.ReadLine();

                        Console.Write("Enter your answer: \n");
                        var answer = Console.ReadLine();

                        Console.Write("Enter your question tags: \n");
                        var questiontags = Console.ReadLine();

                        Console.Write("Enter your votes: \n");
                        var votes = Console.ReadLine();

                        var questionnaire = new DBModel { User = name, Question = question, Answer = answer, QuestionTags = questiontags, Votes = votes };
                        db.DBModel.Add(questionnaire);
                        db.SaveChanges();
                    }
                    else 
                    {


                        string dir = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                        string file = dir + @"\FILE\questionnaire.csv";
                        using (var reader = new StreamReader(file))
                        {
                            List<string> listA = new List<string>();
                            int headers = 1;
                            while (!reader.EndOfStream)
                            {
                                if (headers != 1)
                                {
                                    var line = reader.ReadLine();
                                    var values = line.Split(',');
                                    var questionnaire = new DBModel { User = values[0], Question = values[1], Answer = values[2], QuestionTags = values[3], Votes = values[4] };
                                    db.DBModel.Add(questionnaire);
                                    db.SaveChanges();
                                }
                                headers = 0;
                            }
                        }
                    }


                    var dataset = db.DBModel.Select(x => new { x.User, x.Question, x.Answer, x.QuestionTags, x.Votes }).ToList();
                    Console.WriteLine("All data in the database:");
                    foreach (var item in dataset)
                    {
                        Console.WriteLine(item.User);
                        Console.WriteLine(item.Question);
                        Console.WriteLine(item.Answer);
                        Console.WriteLine(item.QuestionTags);
                        Console.WriteLine(item.Votes);
                        Console.WriteLine("\n");
                    }

                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
        }

    }
}
