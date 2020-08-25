using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.API.Model
{
    public class CalculatorType
    {
        public CalculatorType(string name)
        {
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; private set; }
    }
}
