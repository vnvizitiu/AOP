using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;

namespace LoggerAspect.Tests
{
    [TestFixture]
    public class TestExcludeProperties
    {
        [Test]
        public void ShouldLogProperty()
        {
            var logger = new Mock<ILogger>();
            LoggingAspect.Logger = logger.Object;
            
            var p=new Person();
            p.Website = "http://msprogrammer.serviciipeweb.ro/";
            //3 constructor  + 3 property
            logger.Verify(logger1 => logger1.Debug(It.IsAny<string>()),Times.Exactly(6));
        }
        [Test]
        public void ShouldNotLogProperty()
        {
            var logger = new Mock<ILogger>();
            LoggingAspect.Logger = logger.Object;

            var p = new PersonExcludeProperty();
            p.Website = "http://msprogrammer.serviciipeweb.ro/";
            //3 constructor  
            logger.Verify(logger1 => logger1.Debug(It.IsAny<string>()), Times.Exactly(3));
        }
    }

    [LoggingAspect]
    public class Person
    {
        public string Website { get; set; }
    }
    [LoggingAspect(ExcludeProperties = true)]
    public class PersonExcludeProperty
    {
        public string Website { get; set; }
    }
}
