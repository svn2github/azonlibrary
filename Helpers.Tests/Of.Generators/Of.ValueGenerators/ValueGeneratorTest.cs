using System;

using Azon.Helpers.Generators.ValueGenerators;

using MbUnit.Framework.ContractVerifiers;

namespace Azon.Helpers.Tests.Of.Generators.Of.ValueGenerators {
    public abstract class ValueGeneratorTest<T> 
        where T : IValueGenerator, new() 
    {
        [VerifyContract]
        public readonly IContract generatorTests;

        protected ValueGeneratorTest() {
            this.generatorTests = new ValueGeneratorContract<T> {
                SupportedTypes = this.SupportedTypes,
                SampleTypes = this.SampleTypes,
                SkipUnsupportedTypeTest = this.SkipUnsupportedTypeTest
            };
        }

        protected abstract Type[] SupportedTypes { get; }

        protected virtual Type[] SampleTypes {
            get { return new Type[0]; }
        }

        protected virtual bool SkipUnsupportedTypeTest {
            get { return false; }
        }

        protected T Generator {
            get { return new T(); }
        }
    }
}