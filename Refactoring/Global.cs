using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class Global
    {
       
        public bool loggedIn = false;      //User is logged in?
        public List<User> lUsers { get { return new List<User>(); } }
        public List<Product> lProds = new List<Product>();
    }
}
