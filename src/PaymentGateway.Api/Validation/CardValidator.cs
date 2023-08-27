using System.Text.RegularExpressions;

namespace PaymentGateway.Api.Validation
{
    public class CardValidator
    {
        public static bool IsValid(string pan, string expiryMonth, string expiryYear, string cvv)
        {
            {
                var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
                var yearCheck = new Regex(@"^[0-9]{2}$");
                var cvvCheck = new Regex(@"^\d{3,4}$");

                if (!Mod10Check(pan))
                {
                    return false;
                }

                if (!cvvCheck.IsMatch(cvv))
                {
                    return false;
                }

                if (!monthCheck.IsMatch(expiryMonth) || !yearCheck.IsMatch(expiryYear))
                    return false;

                var year = int.Parse("20" + expiryYear);
                var month = int.Parse(expiryMonth);
                var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month);
                var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

                return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6));
            }
        }

        private static bool Mod10Check(string pan)
        {
            if (string.IsNullOrEmpty(pan))
            {
                return false;
            }

            var sumOfDigits = pan.Where((e) => e >= '0' && e <= '9')
                .Reverse()
                .Select((e, i) => ((int)e - 48) * (i % 2 == 0 ? 1 : 2))
                .Sum((e) => e / 10 + e % 10);

            return sumOfDigits % 10 == 0;
        }
    }
}
