using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T9
{
    public static class StringHelper
    {
        public static string LastWord(this string str)
        {
            var last = str.Split(' ').LastOrDefault();

            if (last == null)
                return null;

            return last;
        }
    }
}
