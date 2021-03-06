﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commander
{
    public class FileSizeFormatProvider : IFormatProvider, ICustomFormatter
    {
        private const string defaultFileSizeFormat = "fs";
        private const string fileSizeFormat = "fsB";
        private const Decimal OneKiloByte = 1024M;
        private const Decimal OneMegaByte = OneKiloByte * 1024M;
        private const Decimal OneGigaByte = OneMegaByte * 1024M;


        object IFormatProvider.GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        string ICustomFormatter.Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (format == null || !format.StartsWith(defaultFileSizeFormat))
            {
                return DefaultFormat(format, arg, formatProvider);
            }

            if (arg is string)
            {
                return DefaultFormat(format, arg, formatProvider);
            }

            Decimal size;

            try
            {
                size = Convert.ToDecimal(arg);
            }
            catch (InvalidCastException)
            {
                return DefaultFormat(format, arg, formatProvider);
            }

            string suffix = String.Empty;
            if (format.StartsWith(fileSizeFormat))
            {
                if (size > OneGigaByte)
                {
                    size /= OneGigaByte;
                    suffix = "GB";
                }
                else if (size > OneMegaByte)
                {
                    size /= OneMegaByte;
                    suffix = "MB";
                }
                else if (size > OneKiloByte)
                {
                    size /= OneKiloByte;
                    suffix = "kB";
                }
                else
                {
                    suffix = " B";
                }
            }

            string precision = format.Substring(2);
            if (String.IsNullOrEmpty(precision))
            {
                precision = "2";
            }
            return String.Format("{0:N" + precision + "}{1}", size, suffix);
        }


        private static string DefaultFormat(string format, object arg, IFormatProvider formatProvider)
        {
            IFormattable formattableArg = arg as IFormattable;
            if (formattableArg != null)
            {
                return formattableArg.ToString(format, formatProvider);
            }

            return arg.ToString();
        }
    }
}
