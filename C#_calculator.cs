namespace calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello!");

            Console.Write("Input the first number: ");
            double num1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Input the second number: ");
            double num2 = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("What do you want to do with those numbers?");
            Console.WriteLine("[A]dd");
            Console.WriteLine("[S]ubtract");
            Console.WriteLine("[M]ultiply");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(choice))
            {
                Console.WriteLine("Invalid option");
            }
            else
            {
                choice = choice.ToUpper();
                if (choice == "A")
                {
                    Console.WriteLine($"{num1} + {num2} = {num1 + num2}");
                }
                else if (choice == "S")
                {
                    Console.WriteLine($"{num1} - {num2} = {num1 - num2}");
                }
                else if (choice == "M")
                {
                    Console.WriteLine($"{num1} * {num2} = {num1 * num2}");
                }
                else
                {
                    Console.WriteLine("Invalid option");
                }
            }

            Console.WriteLine("Press any key to close");
            Console.ReadKey();
        }
    }
}
