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
            Assert.AreEqual(6, logger.DebugCallCount);
        }

        [Test]
        public void ShouldNotLogProperty()
        {
            var logger = new MockLogger();
            LoggingAspect.Logger = logger;

            var p = new PersonExcludeProperty();
            p.Name = Guid.NewGuid().ToString();
            //3 constructor  
            Assert.AreEqual(3, logger.DebugCallCount);
        }
    }

    [LoggingAspect]
    public class Person
    {
        public string Name { get; set; }
    }

    [LoggingAspect(ExcludeProperties = true)]
    public class PersonExcludeProperty
    {
        public string Name { get; set; }
    }
}
