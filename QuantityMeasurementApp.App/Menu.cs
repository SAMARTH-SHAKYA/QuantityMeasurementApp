using System;
using System.Collections.Generic;
using System.Linq;
using QuantityMeasurementApp.Controller;
using QuantityMeasurementApp.Entity;

namespace QuantityMeasurementApp.App
{

    public class Menu : IMenu
    {
        private readonly QuantityMeasurementController _controller;

        public Menu(QuantityMeasurementController controller)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        // ─── IMenu ────────────────────────────────────────────────────────────

        public void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                MenuHelpers.PrintHeader();
                int opChoice = MenuHelpers.PickFromMenu("Select Operation",
                    "1. Compare two quantities",
                    "2. Convert a quantity",
                    "3. Add two quantities",
                    "4. Subtract two quantities",
                    "5. Divide two quantities",
                    "6. Exit");

                if (opChoice == 6)
                {
                    MenuHelpers.Bye();
                    return;
                }

                string   measurementType = MenuHelpers.PickMeasurementType();
                string[] available       = MenuHelpers.Units[measurementType];

                Console.WriteLine();
                switch (opChoice)
                {
                    // ── 1. COMPARE ───────────────────────────────────────────
                    case 1:
                    {
                        var q1 = MenuHelpers.ReadQuantity("First  quantity", measurementType, available);
                        var q2 = MenuHelpers.ReadQuantity("Second quantity", measurementType, available);
                        Console.WriteLine();
                        _controller.PerformComparison(q1, q2);
                        break;
                    }

                    // ── 2. CONVERT ───────────────────────────────────────────
                    case 2:
                    {
                        var    src    = MenuHelpers.ReadQuantity("Source quantity", measurementType, available);
                        string target = MenuHelpers.PickUnit("Convert TO", available);
                        Console.WriteLine();
                        _controller.PerformConversion(src, target);
                        break;
                    }

                    // ── 3. ADD ────────────────────────────────────────────────
                    case 3:
                    {
                        var    q1     = MenuHelpers.ReadQuantity("First  quantity", measurementType, available);
                        var    q2     = MenuHelpers.ReadQuantity("Second quantity", measurementType, available);
                        string target = MenuHelpers.PickUnit("Result unit", available);
                        Console.WriteLine();
                        _controller.PerformAddition(q1, q2, target);
                        break;
                    }

                    // ── 4. SUBTRACT ───────────────────────────────────────────
                    case 4:
                    {
                        var    q1     = MenuHelpers.ReadQuantity("First  quantity", measurementType, available);
                        var    q2     = MenuHelpers.ReadQuantity("Second quantity", measurementType, available);
                        string target = MenuHelpers.PickUnit("Result unit", available);
                        Console.WriteLine();
                        _controller.PerformSubtraction(q1, q2, target);
                        break;
                    }

                    // ── 5. DIVIDE ─────────────────────────────────────────────
                    case 5:
                    {
                        var q1 = MenuHelpers.ReadQuantity("Numerator   quantity", measurementType, available);
                        var q2 = MenuHelpers.ReadQuantity("Denominator quantity", measurementType, available);
                        Console.WriteLine();
                        _controller.PerformDivision(q1, q2);
                        break;
                    }
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\nPress any key to return to main menu...");
                Console.ResetColor();
                Console.ReadKey(true);
            }
        }
    }
}
