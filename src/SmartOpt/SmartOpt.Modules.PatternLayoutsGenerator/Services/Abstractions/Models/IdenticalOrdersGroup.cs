using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Models;

public class IdenticalOrdersGroup : OrderInfo
{
    public IEnumerable<OrderInfo> Orders { get; set; }

    public static IdenticalOrdersGroup FromOrders(IEnumerable<OrderInfo> orders)
    {
        var names = string.Join(", ", orders.Select(x => x.Name).ToArray());
        var width = orders.First().Width;
        var rollsCount = orders.Sum(x => x.RollsCount);

        return new IdenticalOrdersGroup(orders, names, width, rollsCount);
    }

    private IdenticalOrdersGroup(IEnumerable<OrderInfo> orders, string name, int width, double rollsCount)
        : base(name, width, rollsCount)
    {
        Orders = orders;
    }
}
