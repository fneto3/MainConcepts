using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestConcepts.Web.Options
{
    public class RegisterOptions
    {
        public const string Register = "Register";

        public bool MustIncludUpperCaseLetter { get; set; }
    }
}
