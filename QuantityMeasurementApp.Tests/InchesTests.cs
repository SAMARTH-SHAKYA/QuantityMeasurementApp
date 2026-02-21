using NUnit.Framework;
using QuantityMeasurementApp.Core;

namespace QuantityMeasurementApp.Tests
{
    public class InchesTests
    {
        private Inches inches1;
        private Inches inches2;


        [SetUp]
        public void SetUp()
        {
            inches1 = new Inches(1.0);
            inches2 = new Inches(1.0);
        }

        [Test]
        public void GivenSameValue_WhenCompared_ShouldReturnTrue()
        {
            Assert.IsTrue(inches1.Equals(inches2));
        }

        [Test]
        public void GivenDifferentType_WhenCompared_ShouldReturnFalse()
        {
            inches2 = new Inches(2.0);
            Assert.IsFalse(inches1.Equals(inches2));
        }

        [Test]
        public void GivenNull_WhenCompared_ShouldReturnFalse()
        {
            Assert.IsFalse(inches1.Equals(null));
        }

        [Test]
        public void GivenSameReference_WhenCompared_ShouldReturnTrue()
        {
            Assert.IsTrue(inches1.Equals(inches1));
        }

        [Test]
        public void GivenDifferentValue_WhenCompared_ShouldReturnFalse()
        {
            Assert.IsFalse(inches1.Equals("1.0"));
        }
    }


}