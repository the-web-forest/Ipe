using System;
namespace Ipe.Helpers
{
	public class DateHelper
	{
        public static DateTime BrazilDateTimeNow() => TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
    }
}

