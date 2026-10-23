namespace BarberBoss.Communication.Responses;

public class ResponseShortBillingJson
{
    public long Id { get; set; }
    public string Date { get; set; } = string.Empty;
    public string BarberName { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}