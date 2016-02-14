using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace LoggerAspect.Tests
{
    [TestFixture]
    public class TestExcludeProperties
    {
        [Test]
        public void ShouldLogProperty()
        {
            var logger = new MockLogger();
            LoggingAspect.Logger = logger;
            
            var p=new Person();
            p.Name = Guid.NewGuid().ToString();
            //3 constructor  + 3 property
            Assert.AreEqual(6, logger.DebugCallCount, "the Debug method was not called 6 times only (Entry, Success, Exit for constructor and propert)");
        }

        [Test]
        public void ShouldNotLogProperty()
        {
            var logger = new MockLogger();
            LoggingAspect.Logger = logger;

            var p = new PersonExcludeProperty("123");
            p.Name = Guid.NewGuid().ToString();
            var a = p.Name;
            //3 constructor  
            Assert.AreEqual(3, logger.DebugCallCount, "the Debug method was not called 3 times only (Entry, Success, Exit for constructor only)");
        }
    }

    [LoggingAspect]
    public class Person
    {
        public string Name { get; set; }
    }

    [LoggingAspect(Exclude = ExclusionFlags.Properties | ExclusionFlags.StaticConstructor)]
    public class PersonExcludeProperty
    {
        static PersonExcludeProperty()
        {
            
        }

        public PersonExcludeProperty()
        {
        }

        public PersonExcludeProperty(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
