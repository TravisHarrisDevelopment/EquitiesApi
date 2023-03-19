using System.Text;

namespace EquitiesApi.Services
{
    public static class Validation
    {
        public static (DateTime, DateTime) ValidateDates(string fromString, string toString)
        {
            if (fromString == null || fromString == "")
            {
                fromString = "2023-01-01";
            }
            if (toString == null || toString == "")
            {
                toString = DateTime.Now.ToString("yyyy-MM-dd");
            }
            var message = new StringBuilder();
            DateTime fromDate, toDate;
            bool failedValidation = false;
            if (!DateTime.TryParse(fromString, out fromDate))
            {
                message.Append($"The from date provided, \"{fromString}\", is not parseable as a date.\n");
                failedValidation = true;
            }
            if (!DateTime.TryParse(toString, out toDate))
            {
                message.Append($"The from date provided, \"{toString}\", is not parseable as a date.\n");
                failedValidation = true;
            }

            if (!(fromDate <= toDate))
            {
                message.Append($"The from date, \"{fromString}\", occurs on or after the to date, \"{toString}\".\n");
                failedValidation = true;
            }

            //validate that fromDate thru toDate is less than or equal to 366 days
            if (!((toDate - fromDate).Days <= 366))
            {
                message.Append($"The period you requested (\"{fromString}\" to \"{toString}\") is longer than 366 days. Please specify a shorter period.");
                failedValidation = true;
            }
         
            if (failedValidation)
            {
                throw new InvalidOperationException(message.ToString());
            }

            return (fromDate, toDate);
        }
    }
}
