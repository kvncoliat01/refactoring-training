using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public static class LoggedInUser
    {
        //we might have a User identification that is unique for security purposes
        public static string sUser { get; set; }
        public static double dBalance { get; set; }
    }
}
