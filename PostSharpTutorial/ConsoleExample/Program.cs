using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleExample
{
    [LoggingAspect]
    class Program
    {
        static void Main(string[] args)
        {
            LoggingAspect.Logger = new ConsoleLogger();

            try
            {
                int testIntValue = 5;
                DateTime testDateTimeValue = DateTime.Now;

                string[] testValues = {null, "value1", "value2", "value3", "value4"};
                InitializeApp(testValues);
                ResultModel methodResult = RunMethodWithParamteres(testIntValue, testDateTimeValue);
                RunWithComplexObject(methodResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void RunWithComplexObject(ResultModel methodResult)
        {
            throw new NotImplementedException();
        }

        private static ResultModel RunMethodWithParamteres(int testIntValue, DateTime testDateTimeValue)
        {
            return new ResultModel() {IntValue = testIntValue, TimeValue = testDateTimeValue};
        }

        private static void InitializeApp(string[] args)
        {
        }
    }

    [LoggingAspect]
    class ResultModel
    {
        public int IntValue { get; set; }
        public DateTime TimeValue { get; set; }

        public List<string> List { get; set; }

        public ResultModel()
        {
            List = new List<string>() {"listItem1", "listItem2" , "listItem3" };
        }
    }
}
