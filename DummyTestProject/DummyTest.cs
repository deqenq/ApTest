using ApTest;
using System;
using Xunit;

namespace DummyTestProject
{
    public class UnitTest1
    {
        [Fact]
        public void DummyTest()
        {
            Assert.Equal(3, DummyClass.SumNumbers(2, 2));
        }
    }
}
