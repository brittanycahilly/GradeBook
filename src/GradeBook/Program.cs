// PROGRAM
// Brittany Cahill
// Course: C# Fundamentals by Scott Allen

using System;
using System.Collections.Generic;

namespace GradeBook {
    class Program {
        static void Main(string[] args)
        {

            IBook book = new DiskBook("Brit's Gradebook");
            book.GradeAdded += OnGradeAdded;

            EnterGrades(book);

            var stats = book.GetStatistics();

            Console.WriteLine($"Book Name: {book.Name}");
            Console.WriteLine($"Highest Grade: {stats.High:N1}");
            Console.WriteLine($"Lowest Grade: {stats.Low:N1}");
            Console.WriteLine($"Average Grade: {stats.Average:N1}");
            Console.WriteLine($"Letter Grade: {stats.Letter}");
        }

        private static void EnterGrades(IBook book)
        {
            while (true)
            {
                Console.WriteLine("Enter a grade or 'q' to quit");
                var input = Console.ReadLine();
                if (input == "q")
                {
                    // break command exits the while loop
                    break;
                }
                // must catch the exception thrown in book.cs
                try
                {
                    // parsing converts string input to a number
                    var grade = double.Parse(input);
                    book.AddGrade(grade);
                }
                catch (Exception ex)
                {
                    // catch exception allows program to return to the
                    // start of the loop and try again instead of crashing
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.WriteLine("**");
                }
            }
        }

        static void OnGradeAdded(object sender, EventArgs e)
        {
            Console.WriteLine("A grade was added");
        }
    }
}