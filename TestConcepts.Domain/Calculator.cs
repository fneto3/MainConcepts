using System;
using System.Collections.Generic;
using System.Text;
using TestConcepts.Domain.Context.Interfaces;
using TestConcepts.Domain.Interfaces;

namespace TestConcepts.Domain
{
    public class Calculator : IBaseEntity, ICalculator
    {
        public long Id 
        { 
            get; 
            set; 
        }
        
        public decimal A 
        { 
            get; 
            set; 
        }

        public decimal B 
        { 
            get; 
            set; 
        }

        public decimal Result 
        { 
            get; 
            set; 
        }
    }
}
