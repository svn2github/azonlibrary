using System;
using System.Collections;
using System.Collections.Generic;

using Azon.Helpers.Generators.ValueGenerators;
using Azon.Helpers.Generators.ValueGenerators.Constraints;
using Azon.Helpers.Tests.Internal.Asserts;

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

        private IEnumerable<IEnumerable> TypeWithBoundaries() {
            yield return new object[] { typeof(float),   -300.5f, -350.005f };
            yield return new object[] { typeof(double),  -300.5d, -350.005d };
            yield return new object[] { typeof(decimal), -300.5m, -350.005m };
        }

        [Test]
        [Factory("TypeWithBoundaries")]
        public void ShouldReturnValueFromNarrowInterval<T>(T maxValue, T minValue)
            where T : struct, IComparable<T>
        {
            var constraints = new IConstraint[] {
                new MaxValueConstraint<T>(maxValue),
                new MinValueConstraint<T>(minValue)
            };

            var value = this.Generator.GetRandomValue(typeof(T), constraints);

            Assert.Between(value, minValue, maxValue);
        }

        [Test]
        public void ShouldThrowIfGivenMaxValueIsLessThanMinValue() {
            var constraints = new IConstraint[] {
                new MinValueConstraint<float>(5),
                new MaxValueConstraint<float>(0)
            };

            ExceptionAssert.Throws<InvalidOperationException>(
                () => this.Generator.GetRandomValue(typeof(float), constraints),
                "minValue (5) should not be greater than maxValue (0)."
            );
        }
    }
}
