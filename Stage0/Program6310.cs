namespace Stage0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome6310();
            Welcome0821();
        }

        static partial void Welcome0821();
        private static void Welcome6310()
        {
            Console.Write("Enter your name: ");
            Console.WriteLine("{0}, welcome to my first console application", Console.ReadLine());
        }
    }
}