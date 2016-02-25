using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using LoggerAspect.Enums;

namespace LoggerAspect.Tests.Dummies
{
    [LoggingAspect(LogParameters = true, LogExecutionTime = true, LogReturnValue = true)]
    public class FullTestClass
    {
        public string Value { get; set; }

        public void EmbeddedMethods()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            InnerMethod();
        }

        private void InnerMethod()
        {
            Thread.Sleep(TimeSpan.FromSeconds(.5));
        }
    }

    [LoggingAspect]
    public class Generic<T>
    {
        public T MyValue { get; set; }
    }

    [LoggingAspect]
    public class Person
    {
        public string Name { get; set; }
    }

    [LoggingAspect(Exclude = ExclusionFlags.Properties)]
    public class PersonExcludeProperty
    {
        public string Name { get; set; }
    }

    [LoggingAspect(Exclude = ExclusionFlags.InstanceConstructors)]
    public class PersonExcludeInstanceConstructor
    {
        public PersonExcludeInstanceConstructor()
        {
        }

        public string Name { get; set; }
    }

    [LoggingAspect(Exclude = ExclusionFlags.StaticConstructor)]
    public class PersonExcludeStaticConstructor
    {
        static PersonExcludeStaticConstructor()
        {

        }


        public string Name { get; set; }
    }

    [LoggingAspect(Exclude = ExclusionFlags.PropertySetters)]
    public class PersonExcludePropertySetters
    {
        static PersonExcludePropertySetters()
        {

        }

        public PersonExcludePropertySetters()
        {
        }

        public PersonExcludePropertySetters(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    [LoggingAspect(Exclude = ExclusionFlags.PropertyGetters)]
    public class PersonExcludePropertyGetters
    {
        static PersonExcludePropertyGetters()
        {

        }

        public PersonExcludePropertyGetters()
        {
        }

        public PersonExcludePropertyGetters(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    [LoggingAspect(Exclude = ExclusionFlags.Constructors)]
    public class PersonExcludeConstructors
    {
        static PersonExcludeConstructors()
        {

        }

        public PersonExcludeConstructors()
        {
        }

        public PersonExcludeConstructors(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    [LoggingAspect(Exclude = ExclusionFlags.Properties | ExclusionFlags.Constructors)]
    public class PersonExcludePropertyConstructors
    {
        static PersonExcludePropertyConstructors()
        {

        }

        public PersonExcludePropertyConstructors()
        {
        }

        public PersonExcludePropertyConstructors(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

}