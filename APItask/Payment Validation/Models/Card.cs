namespace Payment_Validation.Models
{
    public class Card
    {
        public string Owner { get; set; } = "Not Set";
        public string CardNumber { get; set; } = "Not Set";
        public string CVC { get; set; } = "Not Set";
        public string ExpiryMonth { get; set; } = "Not Set";
        public string ExpiryYear { get; set; } = "Not Set";
        public string IssueMonth { get; set; }= "Not Set";
        public string IssueYear { get; set; }= "Not Set";
        public string CardType { get; set; } = "Not Set";
    }
}