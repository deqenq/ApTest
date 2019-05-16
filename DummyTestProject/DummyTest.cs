using ApTest;
using System;
using Xunit;

namespace DummyTestProject
{
    public class UnitTest1
    {
        [Fact]
        public void DummySumTest()
        {
            Assert.Equal(3, DummyClass.SumNumbers(2, 1));
        }

        //[Fact]
        //public void DummySubtractTest()
        //{
        //    Assert.Equal(0, DummyClass.SubtractNumbers(2, 2));
        //}
    }
}
