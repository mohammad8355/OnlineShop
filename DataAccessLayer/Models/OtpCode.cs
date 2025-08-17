namespace DataAccessLayer.Models;

public class OtpCode
{
    public int Id { get; set; }
    public string PhoneNumber { get; set; }
    public string Code { get; set; }
    public DateTime Expiry { get; set; }
}