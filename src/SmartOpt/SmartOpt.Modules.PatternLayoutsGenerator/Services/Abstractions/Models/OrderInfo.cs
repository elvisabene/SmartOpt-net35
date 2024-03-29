﻿using System;

namespace SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Models
{
    public class OrderInfo
    {
        private string name = string.Empty;
        private int width;
        private double rollsCount;

        public OrderInfo(string name, int width, double rollsCount)
        {
            Name = name;
            Width = width;
            RollsCount = rollsCount;
        }

        public string Name
        {
            get => name;
            set
            {
                if (value is null or " ")
                {
                    throw new ArgumentNullException(nameof(value), "Order name cannot be empty");
                }

                name = value;
            }
        }

        public int Width
        {
            get => width;
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentNullException(nameof(value), "Order width cannot be less than or equal to 0");
                }

                width = value;
            }
        }

        public double RollsCount
        {
            get => rollsCount;
            set
            {
                // todo: there are exceptions if count is 0 or negative
                // if (value <= 0)
                // {
                //     throw new ArgumentNullException(nameof(value), "The number of order rolls cannot be less than or equal to 0");
                // }

                rollsCount = value;
            }
        }

        public OrderInfo Clone()
        {
            return new OrderInfo(Name, Width, RollsCount);
        }

        public override string ToString()
        {
            return " Width: " + Width + "; RollsCount: " + RollsCount;
        }
    }
}
