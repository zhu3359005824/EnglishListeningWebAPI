namespace ZHZ.JWT
{
    public class JWTSettings
    {
        public required string Key { get; set; }

        public int ExpireMinutes { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }


}
