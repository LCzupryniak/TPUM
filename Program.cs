using System;

namespace CalculatorApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Calculator calculator = new Calculator();

            Console.WriteLine("Dodawanie");
            Console.Write("Podaj pierwszą liczbę: ");
            int a = Convert.ToInt32(Console.ReadLine());

            Console.Write("Podaj drugą liczbę: ");
            int b = Convert.ToInt32(Console.ReadLine());

            int result = calculator.Add(a, b);

            Console.WriteLine($"Wynik dodawania {a} + {b} = {result}");
        }
    }

    public class Calculator
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
    }
}