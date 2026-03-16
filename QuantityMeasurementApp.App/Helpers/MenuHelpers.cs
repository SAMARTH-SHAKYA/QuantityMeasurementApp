using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using QuantityMeasurementApp.Entity;

namespace QuantityMeasurementApp.App
{
    internal static class MenuHelpers
    {
        internal static readonly Dictionary<string, string[]> Units = new Dictionary<string, string[]>
        {
            ["Length"]      = new[] { "Feet", "Inch", "Yard", "Centimeter" },
            ["Volume"]      = new[] { "Litre", "Millilitre", "Gallon" },
            ["Weight"]      = new[] { "Kilogram", "Gram", "Tonne" },
            ["Temperature"] = new[] { "Celsius", "Fahrenheit", "Kelvin" }
        };

        internal static void PrintHeader()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║      Quantity Measurement App  v1.0          ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();
        }

        internal static int PickFromMenu(string title, params string[] options)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  {title}");
            Console.ResetColor();
            Console.WriteLine("  " + new string('─', 40));

            foreach (var option in options)
            {
                Console.WriteLine($"  {option}");
            }

            Console.WriteLine();
            while (true)
            {
                Console.Write("  Enter choice: ");
                if (int.TryParse(Console.ReadLine()?.Trim(), out int choice)
                    && choice >= 1 && choice <= options.Length)
                {
                    return choice;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  ✗ Please enter a number between 1 and {options.Length}.");
                Console.ResetColor();
            }
        }

        internal static string PickMeasurementType(IEnumerable<string> allowedTypes)
        {
            Console.WriteLine();
            string[] types = allowedTypes.ToArray();
            string[] menuItems = types.Select((t, i) => $"{i + 1}. {t}").ToArray();
            int choice = PickFromMenu("Select Measurement Type", menuItems);
            return types[choice - 1];
        }

        internal static string PickUnit(string prompt, string[] units)
        {
            Console.WriteLine();
            string[] menuItems = units.Select((u, i) => $"{i + 1}. {u}").ToArray();
            int choice = PickFromMenu(prompt, menuItems);
            return units[choice - 1];
        }

        internal static QuantityDTO ReadQuantity(string label, string measurementType, string[] units)
        {
            string unit = PickUnit(label + " – pick unit", units);
            double value = ReadDouble($"  Enter value in {unit}: ");
            return new QuantityDTO(value, unit, measurementType);
        }

        internal static double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine()?.Trim(),
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out double value))
                {
                    return value;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  ✗ Invalid number. Please try again.");
                Console.ResetColor();
            }
        }

        internal static void Bye()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n  👋  Thanks for using Quantity Measurement App. Goodbye!\n");
            Console.ResetColor();
        }
    }
}

