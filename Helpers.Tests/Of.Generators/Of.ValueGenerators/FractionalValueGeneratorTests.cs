using System;

using Azon.Helpers.Generators.ValueGenerators;
using Azon.Helpers.Generators.ValueGenerators.Constraints;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Generators.Of.ValueGenerators {
    [TestFixture]
    public class FractionalValueGeneratorTests : ValueGeneratorTest<FractionalValueGenerator> {
        protected override Type[] SupportedTypes {
            get {
                return new[] {
                    typeof(float),
                    typeof(double),
                    typeof(decimal)
                }; 
            }
        }

        protected override Type[] SampleTypes {
            get { return this.SupportedTypes; }
        }

        [Test]
        [Factory("SampleTypes")]
        public void ShouldReturnValueIn0To1RangeByDefault<T>() 
            where T : struct
        {
            var value = this.Generator.GetRandomValue(typeof(T), new IConstraint[0]);
            var minValue = Convert.ChangeType(0.0, typeof(T));
            var maxValue = Convert.ChangeType(1.0, typeof(T));

            Assert.Between(value, minValue, maxValue);
        }

        [Test]
        [Row(typeof(float),   float.MinValue + 40,   float.MinValue + 50)]
        [Row(typeof(double),  double.MinValue + 40,  double.MinValue + 50)]
        //[Row(typeof(decimal), decimal.MinValue + 40, decimal.MinValue + 50)]
        public void ShouldReturnValueFromNarrowInterval<T>(T minValue, T maxValue)
            where T : struct {
            var constraints = new IConstraint[] {
                new MaxValueConstraint<T>(maxValue),
                new MinValueConstraint<T>(minValue)
            };

            var value = this.Generator.GetRandomValue(typeof(T), constraints);

            Assert.Between(value, minValue, maxValue);
        }
    }
}
