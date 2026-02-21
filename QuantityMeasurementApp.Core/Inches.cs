namespace QuantityMeasurementApp.Core
{
    public class Inches
    {
        // making field encapsulated
        private readonly double value;

        // constructor
        public Inches(double value)
        {
            this.value = value;
        }

        // overriding the Equals method to handle edge cases
        public override bool Equals(object? obj)
        {
            // checking for same reference
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            // checking for object matches type and not null
            if (obj == null || obj.GetType() != typeof(Inches))
            {
                return false;
            }

            Inches other = (Inches)obj;

            // comparing the actual value 
            return this.value.CompareTo(other.value) == 0;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}