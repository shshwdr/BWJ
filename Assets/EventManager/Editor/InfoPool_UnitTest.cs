using System;
using NUnit.Framework;

namespace Pool
{
    public class InfoPool_UnitTest
    {
        private const string TEST_VALUE = "TESTVALUE";

        [Test]
        public void TestString()
        {
            InfoPool.Provide<string>("test", ReturnSomething);

            string x = InfoPool.Request<string>("test");
            Assert.AreEqual(x, TEST_VALUE);

            InfoPool.Unprovide<string>("test", ReturnSomething);
        }

        [Test]
        public void TestNotProvidedException()
        {
            Assert.That(() => InfoPool.Request<int>("test"),
                Throws.TypeOf<InfoPool.RequestedItemNotProvidedException>());
        }

        [Test]
        public void TestCastException()
        {
            InfoPool.Provide<string>("test", ReturnSomething);
            try
            {
                InfoPool.Request<int>("test");
                Assert.Fail();
            }
            catch (InvalidCastException e)
            {
                Assert.Pass("Expected: " + e.ToString());
            }
            finally
            {
                InfoPool.Unprovide<string>("test", ReturnSomething);
            }
        }

        [Test]
        public void TestUnprovide()
        {
            InfoPool.Provide<string>("test", ReturnSomething);
            string x = InfoPool.Request<string>("test");
            Assert.AreEqual(x, TEST_VALUE);

            InfoPool.Unprovide<string>("test", ReturnSomething);

            Assert.That(() => InfoPool.Request<string>("test"),
            Throws.TypeOf<InfoPool.RequestedItemNotProvidedException>());
        }

        public string ReturnSomething()
        {
            return TEST_VALUE;
        }
    }
}