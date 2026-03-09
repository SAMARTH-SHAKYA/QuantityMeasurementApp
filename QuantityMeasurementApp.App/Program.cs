using System;
using QuantityMeasurementApp.Controller;
using QuantityMeasurementApp.Entity;
using QuantityMeasurementApp.Repository;
using QuantityMeasurementApp.Service;

namespace QuantityMeasurementApp.App
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Initialize Repository (Singleton)
            IQuantityMeasurementRepository repository = QuantityMeasurementCacheRepository.GetInstance();

            // 2. Initialize Service Layer (Dependency Injection)
            IQuantityMeasurementService service = new QuantityMeasurementService(repository);

            // 3. Initialize Controller Layer (Facade & Dependency Injection)
            QuantityMeasurementController controller = new QuantityMeasurementController(service);

            Console.WriteLine("--- Quantity Measurement App N-Tier Demo ---\n");

            // --- Equality Demonstrations ---
            Console.WriteLine("--- Equality Demonstrations ---");
            QuantityDTO q1 = new QuantityDTO(1.0, "Feet", "Length");
            QuantityDTO q2 = new QuantityDTO(12.0, "Inch", "Length");
            controller.PerformComparison(q1, q2);
            Console.WriteLine();

            // --- Conversion Demonstrations ---
            Console.WriteLine("--- Conversion Demonstrations ---");
            QuantityDTO convertFirst = new QuantityDTO(1.0, "Gallon", "Volume");
            controller.PerformConversion(convertFirst, "Litre");
            Console.WriteLine();

            // --- Addition Demonstrations ---
            Console.WriteLine("--- Addition Demonstrations ---");
            QuantityDTO add1 = new QuantityDTO(1.0, "Kilogram", "Weight");
            QuantityDTO add2 = new QuantityDTO(1000.0, "Gram", "Weight");
            controller.PerformAddition(add1, add2, "Kilogram");
            Console.WriteLine();

            // --- Temperature Operations (Expecting Error for Addition) ---
            Console.WriteLine("--- Temperature Demonstrations ---");
            QuantityDTO temp1 = new QuantityDTO(100.0, "Celsius", "Temperature");
            QuantityDTO temp2 = new QuantityDTO(212.0, "Fahrenheit", "Temperature");
            controller.PerformComparison(temp1, temp2);

            controller.PerformConversion(temp1, "Kelvin");

            Console.WriteLine("Attempting unsupported addition (100 Celsius + 50 Celsius):");
            QuantityDTO temp3 = new QuantityDTO(50.0, "Celsius", "Temperature");
            controller.PerformAddition(temp1, temp3, "Celsius");
            
            Console.WriteLine();
        }
    }
}