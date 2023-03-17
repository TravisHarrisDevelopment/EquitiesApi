/*namespace EquitiesApi
{
    public class Return
    {
        public DateTime Date { get; set; }

        private string _tickerSymbol;
        public string? TickerSymbol {
            get
            {
                return _tickerSymbol;
            }
            set
            {
                if (value == null)
                {
                    _tickerSymbol = GenerateTickerSymbol();
                }
                else
                {
                    _tickerSymbol = value;
                }
            }
        }

        public decimal Price { get; set; }

        private string GenerateTickerSymbol()
        {

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var stringChars = new char[4];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }
    }
    }
*/