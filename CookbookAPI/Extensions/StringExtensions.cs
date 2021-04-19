using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace CookbookAPI.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsIgnoreCase(this string source, string compared)
        {
            if (string.IsNullOrEmpty(source))
                return false;

            return source.Contains(compared, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
