using System.Collections;
using System.Collections.Generic;

namespace Test.Util
{
    public abstract class AbstractTestData : IEnumerable<object[]>
    {
        protected readonly List<object[]> testData = new List<object[]>();

        public IEnumerator<object[]> GetEnumerator()
        {
            return testData.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
