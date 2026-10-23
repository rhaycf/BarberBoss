namespace BarberBoss.Communication.Responses;

public class ResponseRegisteredBillingJson
{
    public string ServiceName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}
