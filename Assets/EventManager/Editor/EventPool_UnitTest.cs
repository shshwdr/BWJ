using NUnit.Framework;

namespace Pool
{
    [TestFixture]
    public class EventPool_UnitTest
    {
        private int talent0 = 0;
        private int talent1 = 0;
        private int talent2 = 0;
        private int talent3 = 0;

        private const string KEY = "key";
        private const string OTHER_KEY = "OtherKey";

        [OneTimeSetUp]
        public void BeforeTests()
        {
            EventPool.Reset();
        }

        [Test]
        public void TestNoArgs()
        {
            EventPool.OptIn(KEY, Callback);
            Assert.AreEqual(0, talent0, "Value has changed before trigger was called");
            EventPool.Trigger(KEY);
            Assert.AreEqual(1, talent0, "Event not received");

            DoSomeMessaging();

            EventPool.OptIn(KEY, Callback); // Should not do anything
            EventPool.OptIn(KEY, Callback2);
            EventPool.OptIn(OTHER_KEY, Callback2);
            EventPool.Trigger(KEY);
            Assert.AreEqual(3, talent0, "Triggered the wrong callbacks?");

            ResetTalents();
            EventPool.OptOut(KEY, Callback);
            EventPool.OptOut(KEY, Callback2);
            EventPool.OptOut(OTHER_KEY, Callback2);
        }

        private void Callback()
        {
            LevelUp(0, 0, 0);
        }

        private void Callback2()
        {
            LevelUp(0, 0, 0);
        }

        [Test]
        public void TestOneArgs()
        {
            EventPool.OptIn<int>(KEY, Callback);
            Assert.AreEqual(0, talent1, "Value has changed before trigger was called");
            EventPool.Trigger<int>(KEY, 1);
            Assert.AreEqual(1, talent0, "Event not received");
            Assert.AreEqual(1, talent1, "Event not received");

            DoSomeMessaging();

            EventPool.OptIn<int>(KEY, Callback); // Should not do anything
            EventPool.Trigger<int>(KEY, 1);
            Assert.AreEqual(2, talent1, "Triggered the wrong callbacks?");

            ResetTalents();
            EventPool.OptOut<int>(KEY, Callback);
            EventPool.OptOut<int>(KEY, Callback);
        }

        private void Callback(int t1)
        {
            LevelUp(t1, 0, 0);
        }

        [Test]
        public void TestTwoArgs()
        {
            EventPool.OptIn<int, int>(KEY, Callback);
            Assert.AreEqual(0, talent2, "Value has changed before trigger was called");

            DoSomeMessaging();

            EventPool.OptIn<int, int>(KEY, Callback); // Should not do anything
            EventPool.Trigger<int, int>(KEY, 1, 1);
            Assert.AreEqual(1, talent0, "Event not received");
            Assert.AreEqual(1, talent1, "Event not received");
            Assert.AreEqual(1, talent2, "Event not received");

            ResetTalents();
            EventPool.OptOut<int, int>(KEY, Callback);
        }

        private void Callback(int t1, int t2)
        {
            LevelUp(t1, t2, 0);
        }

        [Test]
        public void TestThreeArgs()
        {
            EventPool.OptIn<int, int, int>(KEY, Callback);
            Assert.AreEqual(0, talent3, "Value has changed before trigger was called");

            DoSomeMessaging();

            EventPool.OptIn<int, int, int>(KEY, Callback); // Should not do anything
            EventPool.Trigger<int, int, int>(KEY, 1, 1, 1);
            Assert.AreEqual(1, talent0, "Event not received");
            Assert.AreEqual(1, talent1, "Event not received");
            Assert.AreEqual(1, talent2, "Event not received");
            Assert.AreEqual(1, talent3, "Event not received");

            ResetTalents();
            EventPool.OptOut<int, int, int>(KEY, Callback);
        }

        private void Callback(int t1, int t2, int t3)
        {
            LevelUp(t1, t2, t3);
        }

        [Test]
        public void TestInterference()
        {
            string interfKey = "interf";

            EventPool.OptIn(interfKey, Callback);
            EventPool.OptIn<int>(interfKey, Callback);
            EventPool.OptIn<int, int>(interfKey, Callback);
            EventPool.OptIn<int, int, int>(interfKey, Callback);

            EventPool.Trigger(interfKey);
            Assert.AreEqual(1, talent0, "Event not received");
            Assert.AreEqual(0, talent1, "Event not received");
            Assert.AreEqual(0, talent2, "Event not received");
            Assert.AreEqual(0, talent3, "Event not received");

            EventPool.Trigger(interfKey, 1);
            Assert.AreEqual(2, talent0, "Event not received");
            Assert.AreEqual(1, talent1, "Event not received");
            Assert.AreEqual(0, talent2, "Event not received");
            Assert.AreEqual(0, talent3, "Event not received");

            EventPool.Trigger(interfKey, 1, 1);
            Assert.AreEqual(3, talent0, "Event not received");
            Assert.AreEqual(2, talent1, "Event not received");
            Assert.AreEqual(1, talent2, "Event not received");
            Assert.AreEqual(0, talent3, "Event not received");

            EventPool.Trigger(interfKey, 1, 1, 1);
            Assert.AreEqual(4, talent0, "Event not received");
            Assert.AreEqual(3, talent1, "Event not received");
            Assert.AreEqual(2, talent2, "Event not received");
            Assert.AreEqual(1, talent3, "Event not received");

            ResetTalents();
            EventPool.OptOut(interfKey, Callback);
            EventPool.OptOut<int>(interfKey, Callback);
            EventPool.OptOut<int, int>(interfKey, Callback);
            EventPool.OptOut<int, int, int>(interfKey, Callback);
        }

        [Test]
        public void TestOtherTypes()
        {
            string otherTypesKey = "otherT";
            EventPool.OptIn<int, string, MyType>(otherTypesKey, ChangeValues);

            MyType type = new MyType(0, 10, "abc");
            EventPool.Trigger<int, string, MyType>(otherTypesKey, 10, "xy", type);

            Assert.AreEqual(type.name, "xy", "Name was not changed");
            Assert.AreEqual(type.a, 20, "Int was not changed");

            EventPool.OptOut<int, string, MyType>(otherTypesKey, ChangeValues);
        }

        public class MyType
        {
            public int a, b;
            public string name;

            public MyType(int a, int b, string name)
            {
                this.a = a;
                this.b = b;
                this.name = name;
            }
        }

        private void ChangeValues(int i, string s, MyType t)
        {
            t.a = t.b + i;
            t.name = s;
        }

        private void LevelUp(int t1, int t2, int t3)
        {
            talent0 += 1;
            talent1 += t1;
            talent2 += t2;
            talent3 += t3;
        }

        private void ResetTalents()
        {
            talent0 = 0;
            talent1 = 0;
            talent2 = 0;
            talent3 = 0;
        }

        /// <summary>
        /// Registeres some listeners and triggers some events
        /// </summary>
        private void DoSomeMessaging()
        {
            EventPool.OptIn("XY", SomeMessaging);
            EventPool.Trigger("XY");
            EventPool.OptIn("UV", SomeMessaging);
            EventPool.Trigger("AB");
        }

        private void SomeMessaging() { }
    }
}