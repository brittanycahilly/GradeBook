// GRADEBOOK
// Brittany Cahill
// Course: C# Fundamentals by Scott Allen

namespace GradeBook
{
    public delegate void GradeAddedDelegate(object sender, EventArgs args);
    public class NamedObject
    {
        public NamedObject(string name)
        {
            Name = name;
        }
        // auto property uses 'get' to read and 'set' to write
        public string Name
        {
            get;
            set;
        }
    }
    // an interface only describes the members on a specific type
    // interface names typically begin with 'I'
    public interface IBook
    {
        // define a pure abstraction with no implementation details
        void AddGrade(double grade);
        Statistics GetStatistics();
        string Name { get; }
        event GradeAddedDelegate GradeAdded;
    }
    // inheritance - Book is a NamedObject and derives from NamedObject
    // if you don't specify a base class, it will derive from System.Object
    // everything specified in NamedObject and IBook will be implemented in Book
    public abstract class Book : NamedObject, IBook
    {
        public Book(string name) : base(name)
        {
        }
        // virtual - derived class can override the implementation details
        // abstract is a virtual method with no implementation
        public abstract event GradeAddedDelegate GradeAdded;
        public abstract void AddGrade(double grade);
        public abstract Statistics GetStatistics();
    }
    public class DiskBook : Book
    {
        public DiskBook(string name) : base(name)
        {
        }
        public override event GradeAddedDelegate GradeAdded;
        public override void AddGrade(double grade)
        {
            using(var writer = File.AppendText($"{Name}.txt"))
            {
                writer.WriteLine(grade);
                if(GradeAdded != null)
                {
                    GradeAdded(this, new EventArgs());
                }
            }
        }
        public override Statistics GetStatistics()
        {
            var result = new Statistics();
            using(var reader = File.OpenText($"{Name}.txt"))
            {
                // loop through file and read each grade into memory
                var line = reader.ReadLine();
                while(line != null)
                {
                    var number = double.Parse(line);
                    result.Add(number);
                    line = reader.ReadLine();
                }
            }
            return result;
        }
    }
    public class InMemoryBook : Book
    {
        public InMemoryBook(string name) : base(name)
        {
            grades = new List<double>();
            // implicit variable this.object refers to object currently operated on
            Name = name;
        }
        public void AddLetterGrade(char letter)
        {
            switch(letter)
            {
                case 'A':
                    AddGrade(90);
                    break;
                case 'B':
                    AddGrade(80);
                    break;
                case 'C':
                    AddGrade(70);
                    break;
                case 'D':
                    AddGrade(60);
                    break;
                default:
                    AddGrade(0);
                    break;
            }
        }
        // overrides the method inherited from the base class
        public override void AddGrade(double grade)
        {
            if (grade <= 100 && grade >= 0)
            {
                grades.Add(grade);
                if(GradeAdded != null)
                {
                    GradeAdded(this, new EventArgs());
                }
            }
            else
            {
                // create exception for invalid user inputs
                throw new ArgumentException($"Invalid {nameof(grade)}");
            }
        }
        // override implementation in the base class
        public override event GradeAddedDelegate GradeAdded;
        public override Statistics GetStatistics()
        {
            var result = new Statistics();
            for(var i = 0; i < grades.Count; i++)
            {
                result.Add(grades[i]);
            }
            return result;
        }
        private List<double> grades;
        // readonly only allows writing to an object in the constructor
        // const does not allow writing from anywhere
        // const are often labeled in all uppercase
        public const string CATEGORY = "Science";
    }
}