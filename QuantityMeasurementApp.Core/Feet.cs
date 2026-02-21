using System;
namespace QuantityMeasurementApp.Core
{
    public class Feet
    {
        // making fields encapsulated
        private readonly double value;

        // field can only be initialised in constructor
        public Feet(double value)
        {
            this.value = value;
        }

        // overriding equals method for edge cases comparision
        public override bool Equals(object? obj)
        {
            // checking for same reference
            if (ReferenceEquals(this, obj))
            {
                return true;
            }


            // checking for if objec
            if (obj == null || obj.GetType() != typeof(Feet))
            {
                return false;
            }

            Feet other = (Feet)obj;

            // comparing the actual value 
            return this.value.CompareTo(other.value) == 0;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

    }

    
}