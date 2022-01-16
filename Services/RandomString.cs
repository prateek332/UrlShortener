namespace UrlShortner.Services
{
    public class RandomString : IRandomString
    {
        private readonly string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        private readonly Random random = null;

        public RandomString()
        {
            random = new Random();
        }

        public string GetRandomString(int stringLength)
        {
            return new string(Enumerable.Repeat(chars, stringLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
