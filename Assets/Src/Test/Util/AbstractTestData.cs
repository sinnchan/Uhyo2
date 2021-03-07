using System.Collections;
using System.Collections.Generic;

namespace Src.Test.Util
{
    public abstract class AbstractTestData : IEnumerable<object[]>
    {
        public List<object[]> _testData = new List<object[]>();

        public IEnumerator<object[]> GetEnumerator()
        {
            return _testData.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
