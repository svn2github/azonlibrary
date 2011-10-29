using Azon.Helpers.Generators.ValueGenerators;

namespace Azon.Helpers.Tests.Of.Generators.Of.ValueGenerators {
    public class ValueGeneratorTest<T> 
        where T : IValueGenerator, new() 
    {
        public T Generator {
            get { return new T(); }
        }
    }
}