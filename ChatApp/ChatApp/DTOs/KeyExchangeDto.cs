namespace ChatApp.DTOs
{
    public class KeyExchangeDto
    {
        public Guid SenderId { get; set; }
        public string PublicKey { get; set; }
    }
}
