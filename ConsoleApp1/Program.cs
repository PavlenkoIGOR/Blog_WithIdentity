namespace ConsoleApp1
{
    public class MyClass
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public MyClass(Guid id, string Name)
        {
            Id = id;
            this.Name = Name;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello, World!");

            MyClass myClass = new MyClass(id: new Guid(), Name: "John");
            Console.WriteLine($"{typeof(MyClass)}");

            Console.ReadLine();
        }
    }
}
