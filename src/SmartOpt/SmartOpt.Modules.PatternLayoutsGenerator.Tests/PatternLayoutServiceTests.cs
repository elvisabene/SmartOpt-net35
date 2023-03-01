using System.Collections.Generic;
using System.Linq;
//using NUnit.Framework;
using SmartOpt.Modules.PatternLayoutsGenerator.Services;
using SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Models;
using SmartOpt.Modules.PatternLayoutsGenerator.Services.Implementation;
using SmartOpt.Modules.PatternLayoutsGenerator.Tests.Extensions;

namespace SmartOpt.Modules.PatternLayoutsGenerator.Tests
{
    //[TestFixture]
    public class PatternLayoutServiceTests
    {
        // Format: MethodName_StateUnderTest_ExpectedBehavior
        
        //[Test]
        public void CreatePatternLayoutsFromOrders_WhenNothingSpecialHappens_ReturnsResult()
        {
            // Allocate
            const int maxWidth = 6000;
            const double maxWaste = 3.0;
            const int groupSize = 5;
            
            var patternLayoutService = new PatternLayoutGenerator();

            var orders = new List<OrderInfo>
            {
                new("2773/18", 1300, 5.882352941),
                new("2773/18", 650, 5.882352941),
                new("2773/18", 1200, 5.882352941),
                new("2773/18", 800, 5.882352941),
                new("2773/18", 550, 5.882352941),
                new("2773/18", 600, 4.901960784),
                new("546/19-257", 1000, 3.529411765),
                new("546/19-257", 1100, 2.139037433),
                new("546/19-257", 1160, 2.535496957),
                new("546/19-257", 1200, 2.941176471),
                new("546/19-257", 1220, 2.892960463),
                new("546/19-259", 840, 3.008403361),
                new("2769/18-12", 320, 3.063725490),
                new("2769/18-12", 340, 1.153402537)
            };

            var expectedReport = new Report(
                new PatternLayout[]
                {
                    new(new OrderInfo[]
                    {
                        new("546/19-257", 1160, 2.5354969569999999),
                        new("546/19-257", 1000, 3.5294117649999999),
                        new("2773/18, 546/19-257", 1200, 8.8235294119999992),
                        new("546/19-257", 1220, 2.8929604630000001),
                        new("2773/18", 1300, 5.8823529409999997)
                    }, 2.5354969569999999, 2.0000000000000018),
                    new(new OrderInfo[]
                    {
                        new("2773/18, 546/19-257", 1200, 1.5720081137499999),
                        new("2773/18", 1300, 1.6734279919999999),
                        new("2773/18, 546/19-257", 1200, 1.5720081137499999),
                        new("2773/18, 546/19-257", 1200, 1.5720081137499999),
                        new("546/19-257", 1100, 2.1390374329999999)
                    }, 1.5720081137499999, 0)
                },
                new OrderInfo[]
                {
                    new("2773/18", 1300, 1.7748478702499999),
                    new("2773/18, 546/19-257", 1200, 1.5720081137499999),
                    new("2769/18-12", 320, 3.0637254899999999),
                    new("546/19-259", 840, 3.0084033610000001),
                    new("2773/18", 550, 5.8823529409999997),
                    new("2773/18", 800, 5.8823529409999997),
                    new("2773/18", 650, 5.8823529409999997),
                    new("2773/18", 600, 4.9019607839999999),
                    new("2769/18-12", 340, 1.1534025370000001),
                    new("546/19-257", 1000, 0.99391480799999998),
                    new("546/19-257", 1100, 0.56702931925),
                    new("546/19-257", 1220, 0.35746350600000021),
                    new("546/19-257", 1160, 0)
                }
            );

            // Act
            var report = patternLayoutService.GeneratePatternLayoutsFromOrders(orders, maxWidth, maxWaste, groupSize);

            // Assert
            AssertReportsAreEqual(expectedReport, report);
        }

        private static void AssertReportsAreEqual(Report first, Report second)
        {
            var firstPatternLayouts = first.GetPatternLayouts();
            var firstUngroupedOrders = first.GetUngroupedOrders();      
            var secondPatternLayouts = second.GetPatternLayouts();
            var secondUngroupedOrders = second.GetUngroupedOrders();

            var patternLayouts = firstPatternLayouts as PatternLayout[] ?? firstPatternLayouts.ToArray();
            var enumerable = secondPatternLayouts as PatternLayout[] ?? secondPatternLayouts.ToArray();
            
            // Assert.True(patternLayouts.Count() == enumerable.Count(), "expectedReportPatternLayouts.Count != reportPatternLayouts.Count");
            // Assert.True(patternLayouts.SelectMany(x => x.Orders).Count() == enumerable.SelectMany(x => x.Orders).Count(), "expectedReportPatternLayouts.SelectMany(x => x.Orders).Count() != reportPatternLayouts.SelectMany(x => x.Orders).Count()");
            //Assert.True(firstUngroupedOrders.Count() == secondUngroupedOrders.Count(), "expectedReportUngroupedOrders.Count != reportUngroupedOrders.Count");

            
            for (var i = 0; i < patternLayouts.Count(); i++)
            {
                var firstPatternLayout = patternLayouts.ElementAt(i);
                var secondPatternLayout = enumerable.ElementAt(i);
                
                // Assert.True(firstPatternLayout.Waste.Equals7DigitPrecision(secondPatternLayout.Waste));
                // Assert.True(firstPatternLayout.RollsCount.Equals7DigitPrecision(secondPatternLayout.RollsCount));

                for (var j = 0; j < firstPatternLayout.Orders.Count(); j++)
                {
                    var firstOrder = firstPatternLayout.Orders.ElementAt(j);
                    var secondOrder = secondPatternLayout.Orders.ElementAt(j);
                    
                    // Assert.True(firstOrder.Name == secondOrder.Name);
                    // Assert.True(firstOrder.Width == secondOrder.Width);
                    // Assert.True(firstOrder.RollsCount.Equals7DigitPrecision(secondOrder.RollsCount));
                }
            }
        }
    }
}