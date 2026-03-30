using System;

namespace QuantityMeasurementApp.Entity
{
    public class AreaUnit : IMeasurable
    {
        public static readonly AreaUnit SqFeet = new AreaUnit("SqFeet", 1.0);
        public static readonly AreaUnit SqInch = new AreaUnit("SqInch", 1.0 / 144.0);
        public static readonly AreaUnit SqCentimeter = new AreaUnit("SqCentimeter", 1.0 / 929.0304);

        private readonly string name;
        private readonly double conversionFactor;

        private AreaUnit(string name, double conversionFactor)
        {
            this.name = name;
            this.conversionFactor = conversionFactor;
        }

        public double GetConversionFactor() => conversionFactor;
        public double ConvertToBaseUnit(double value) => value * conversionFactor;
        public double ConvertFromBaseUnit(double baseValue) => conversionFactor == 0 ? 0 : baseValue / conversionFactor;
        public string GetUnitName() => name;
        public string GetMeasurementType() => "Area";

        public IMeasurable GetUnitInstance(string name)
        {
            switch (name.ToUpper())
            {
                case "SQFEET": return SqFeet;
                case "SQINCH": return SqInch;
                case "SQCENTIMETER": return SqCentimeter;
                default: throw new ArgumentException($"Invalid Area unit: {name}");
            }
        }

        public override string ToString() => name;
    }
}
