namespace PaymentGateway.Api.Extensions
{
    public static class StringExtensions
    {
        public static string Mask(this string source, int start, int maskLength)
        {
            return source.Mask(start, maskLength, 'X');
        }
        public static string Mask(this string source, int start, int maskLength, char maskCharacter)
        {
            if (start > source.Length - 1)
            {
                throw new ArgumentException("Start position is greater than string length");
            }

            if (maskLength > source.Length)
            {
                throw new ArgumentException("Mask length is greater than string length");
            }

            if (start + maskLength > source.Length)
            {
                throw new ArgumentException("Start position and mask length imply more characters than are present");
            }

            var mask = new string(maskCharacter, maskLength);
            var unMaskStart = source[..start];
            var unMaskEnd = source.Substring(start + maskLength, source.Length - start - maskLength);

            return unMaskStart + mask + unMaskEnd;
        }
    }
}
