using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commander.Utility
{
    public static class StringHelper
    {
        public static string ToArguments(params string[] items)
        {
            StringBuilder result = new StringBuilder();

            foreach (string item in items)
            {
                if (result.Length > 0)
                {
                    result.Append(' ');
                }

                if (item.StartsWith("=\""))
                {
                    result.Append(item);
                }
                else
                {
                    result.Append('"');
                    result.Append(item);
                    result.Append('"');
                }
            }

            return result.ToString();
        }
    }
}
