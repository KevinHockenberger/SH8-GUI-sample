﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SH8_Sample.Models
{
  public static class StaticGlobals
  {
    // backend class
    internal static string decimalformat = "#,##0.000";
    internal static string intformat = "#,##0";
    internal static readonly Dictionary<int, string> MyDictionary = new Dictionary<int, string> { { 0, "zero" }, { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" } };

  }
}
