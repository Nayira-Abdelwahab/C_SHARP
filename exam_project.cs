using System;
using System.Collections.Generic;
using System.Linq;

namespace project
{
    // ===================== Course =====================
    public class Course
    {
        private string title;
        private string description;
        private double maxDegree;

        public string Title
        {
            get => title;
            set => title = string.IsNullOrWhiteSpace(value) ? "Untitled Course" : value;


        }

        public string Description
        {
            get => description;
            set => description = string.IsNullOrWhiteSpace(value) ? "No description" : value;
        }

        public double MaxDegree
        {
            get => maxDegree;
            set => maxDegree = (value > 0) ? value : 100;

        }

        public Course(string title, string description, double maxDegree)
        {
            Title = title;
            Description = description;
            MaxDegree = maxDegree;
        }

        public override string ToString()
        {
            return $"Course: {Title}, Max Degree: {MaxDegree}";
        }
    }

    // ===================== Student =====================
    public class Student
    {
        private string name;
        private string email;

        public int Id { get; }
        public string Name
        {
            get => name;
            set => name = string.IsNullOrWhiteSpace(value) ? "Unknown" : value;

        }
        public string Email
        {
            get => email;
            set => email = value.Contains("@") ? value : "unknown@email.com";

        }
        public List<Course> EnrolledCourses { get; } = new List<Course>();

        public Student(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public override string ToString()
        {
            return $"Student: {Name} (ID: {Id})";
        }
    }

    // ===================== Instructor =====================
    public class Instructor
    {
        private string name;
        private string specialization;

        public int Id { get; }
        public string Name
        {
            get => name;
            set => name = string.IsNullOrWhiteSpace(value) ? "Unknown Instructor" : value;

        }
        public string Specialization
        {
            get => specialization;
            set => specialization = string.IsNullOrWhiteSpace(value) ? "General" : value;
        }

        public Instructor(int id, string name, string specialization)
        {
            Id = id;
            Name = name;
            Specialization = specialization;
        }

        public override string ToString()
        {
            return $"Instructor: {Name} (#{Id}), Spec: {Specialization}";
        }
    }

    // ===================== Question Base =====================
    public abstract class Question
    {
        public string Text { get; set; }
        public double Marks { get; set; }

        protected Question(string text, double marks)
        {
            Text = text;
            Marks = marks;
        }

        public abstract bool CheckAnswer(string answer);
    }

    public class MultipleChoiceQuestion : Question
    {
        public List<string> Options { get; }
        public string CorrectAnswer { get; }

        public MultipleChoiceQuestion(string text, double marks, List<string> options, string correctAnswer)
            : base(text, marks)
        {
            Options = options;
            CorrectAnswer = correctAnswer;
        }

        public override bool CheckAnswer(string answer)
        {
            return answer == CorrectAnswer;
        }
    }

    public class TrueFalseQuestion : Question
    {
        public bool CorrectAnswer { get; }

        public TrueFalseQuestion(string text, double marks, bool correctAnswer)
            : base(text, marks)
        {
            CorrectAnswer = correctAnswer;
        }

        public override bool CheckAnswer(string answer)
        {
            return bool.TryParse(answer, out bool ans) && ans == CorrectAnswer;
        }
    }

    public class EssayQuestion : Question
    {
        public EssayQuestion(string text, double marks) : base(text, marks) { }

        public override bool CheckAnswer(string answer)
        {
            return true; 
        }
    }

    // ===================== Exam =====================
    public class Exam
    {
        public string Title { get; set; }
        public Course Course { get; }
        public List<Question> Questions { get; } = new List<Question>();
        public bool Started { get; private set; }

        public Exam(string title, Course course)
        {
            Title = title;
            Course = course;
        }

        public bool AddQuestion(Question q)
        {
            if (Started)
            {
                Console.WriteLine("Exam has already started. Cannot add questions.");
                return false;
            }

            double totalMarks = Questions.Sum(x => x.Marks) + q.Marks;
            if (totalMarks > Course.MaxDegree)
            {
                Console.WriteLine("Cannot exceed the maximum degree of the course.");
                return false;
            }

            Questions.Add(q);
            Console.WriteLine("Question added successfully.");
            return true;
        }


        public void StartExam()
        {
            Started = true;
        }

        public double TakeExam(Student student, Dictionary<Question, string> answers)
        {
            if (!Started)
            {
                Console.WriteLine("Exam has not started yet.");
                return 0; 
            }

            double score = 0;
            foreach (var q in Questions)
            {
                if (answers.ContainsKey(q) && q.CheckAnswer(answers[q]))
                {
                    score += q.Marks;
                }
            }
            return score;
        }

    }

    // ===================== Report =====================
    public class Report
    {
        public static void ShowReport(Exam exam, Student student, double score)
        {
            Console.WriteLine($"Exam: {exam.Title}");
            Console.WriteLine($"Student: {student.Name}");
            Console.WriteLine($"Course: {exam.Course.Title}");
            Console.WriteLine($"Score: {score}/{exam.Course.MaxDegree}");
            Console.WriteLine(score >= exam.Course.MaxDegree / 2 ? "Status: Pass" : "Status: Fail");
        }

        public static void CompareStudents(Student s1, double score1, Student s2, double score2)
        {
            Console.WriteLine($"{s1.Name}: {score1} vs {s2.Name}: {score2}");
            Console.WriteLine(score1 > score2 ? $"{s1.Name} scored higher" :
                              score2 > score1 ? $"{s2.Name} scored higher" :
                              "Both students have the same score");
        }
    }

    // ===================== Main Program =====================
    internal class Program
    {
        static List<Course> courses = new List<Course>();
        static List<Student> students = new List<Student>();
        static List<Instructor> instructors = new List<Instructor>();
        static List<Exam> exams = new List<Exam>();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n==== Examination System ====");
                Console.WriteLine("1. Add Course");
                Console.WriteLine("2. Add Student");
                Console.WriteLine("3. Add Instructor");
                Console.WriteLine("4. Create Exam");
                Console.WriteLine("5. Start Exam for Student");
                Console.WriteLine("6. Show All Courses");
                Console.WriteLine("7. Show All Students");
                Console.WriteLine("8. Show All Instructors");
                Console.WriteLine("0. Exit");

                Console.Write("Choose option: ");
                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": AddCourse(); break;
                        case "2": AddStudent(); break;
                        case "3": AddInstructor(); break;
                        case "4": CreateExam(); break;
                        case "5": StartExamForStudent(); break;
                        case "6": ShowCourses(); break;
                        case "7": ShowStudents(); break;
                        case "8": ShowInstructors(); break;
                        case "0": return;
                        default: Console.WriteLine("Invalid choice!"); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        // ====== MENU METHODS ======
        static void AddCourse()
        {
            Console.Write("Enter Course Title: ");
            string title = Console.ReadLine();
            Console.Write("Enter Description: ");
            string desc = Console.ReadLine();
            Console.Write("Enter Max Degree: ");
            double max = double.Parse(Console.ReadLine());

            courses.Add(new Course(title, desc, max));
            Console.WriteLine("Course added successfully!");
        }

        static void AddStudent()
        {
            Console.Write("Enter Student ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Enter Student Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            students.Add(new Student(id, name, email));
            Console.WriteLine("Student added successfully!");
        }

        static void AddInstructor()
        {
            Console.Write("Enter Instructor ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Enter Instructor Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Specialization: ");
            string spec = Console.ReadLine();

            instructors.Add(new Instructor(id, name, spec));
            Console.WriteLine("Instructor added successfully!");
        }

        static void CreateExam()
        {
            if (courses.Count == 0)
            {
                Console.WriteLine("No courses available! Add a course first.");
                return;
            }

            Console.WriteLine("Available Courses:");
            for (int i = 0; i < courses.Count; i++)
                Console.WriteLine($"{i + 1}. {courses[i]}");

            Console.Write("Choose course index: ");
            int courseIndex = int.Parse(Console.ReadLine()) - 1;
            var course = courses[courseIndex];

            Console.Write("Enter Exam Title: ");
            string title = Console.ReadLine();

            var exam = new Exam(title, course);

            Console.Write("How many questions to add? ");
            int qCount = int.Parse(Console.ReadLine());

            for (int i = 0; i < qCount; i++)
            {
                Console.WriteLine("\nChoose Question Type: 1- MCQ, 2- True/False, 3- Essay");
                string qType = Console.ReadLine();
                Console.Write("Enter Question Text: ");
                string qText = Console.ReadLine();
                Console.Write("Enter Marks: ");
                double marks = double.Parse(Console.ReadLine());

                if (qType == "1")
                {
                    Console.Write("Enter options (comma separated): ");
                    var options = Console.ReadLine().Split(',').Select(x => x.Trim()).ToList();
                    Console.Write("Enter correct answer: ");
                    string correct = Console.ReadLine();
                    exam.AddQuestion(new MultipleChoiceQuestion(qText, marks, options, correct));
                }
                else if (qType == "2")
                {
                    Console.Write("Enter correct answer (true/false): ");
                    bool correct = bool.Parse(Console.ReadLine());
                    exam.AddQuestion(new TrueFalseQuestion(qText, marks, correct));
                }
                else
                {
                    exam.AddQuestion(new EssayQuestion(qText, marks));
                }
            }

            exams.Add(exam);
            Console.WriteLine("Exam created successfully!");
        }

        static void StartExamForStudent()
        {
            if (exams.Count == 0 || students.Count == 0)
            {
                Console.WriteLine("Need at least 1 exam and 1 student!");
                return;
            }

            Console.WriteLine("Available Students:");
            for (int i = 0; i < students.Count; i++)
                Console.WriteLine($"{i + 1}. {students[i]}");

            Console.Write("Choose student index: ");
            int sIndex = int.Parse(Console.ReadLine()) - 1;
            var student = students[sIndex];

            Console.WriteLine("Available Exams:");
            for (int i = 0; i < exams.Count; i++)
                Console.WriteLine($"{i + 1}. {exams[i].Title} ({exams[i].Course.Title})");

            Console.Write("Choose exam index: ");
            int eIndex = int.Parse(Console.ReadLine()) - 1;
            var exam = exams[eIndex];
            exam.StartExam();

            var answers = new Dictionary<Question, string>();
            foreach (var q in exam.Questions)
            {
                Console.WriteLine($"\nQ: {q.Text} (Marks: {q.Marks})");
                if (q is MultipleChoiceQuestion mcq)
                {
                    Console.WriteLine("Options: " + string.Join(", ", mcq.Options));
                }
                Console.Write("Your answer: ");
                answers[q] = Console.ReadLine();
            }

            double score = exam.TakeExam(student, answers);
            Report.ShowReport(exam, student, score);
        }

        static void ShowCourses()
        {
            Console.WriteLine("\n--- Courses ---");
            foreach (var c in courses)
                Console.WriteLine(c);
        }

        static void ShowStudents()
        {
            Console.WriteLine("\n--- Students ---");
            foreach (var s in students)
                Console.WriteLine(s);
        }

        static void ShowInstructors()
        {
            Console.WriteLine("\n--- Instructors ---");
            foreach (var i in instructors)
                Console.WriteLine(i);
        }
    }
}

