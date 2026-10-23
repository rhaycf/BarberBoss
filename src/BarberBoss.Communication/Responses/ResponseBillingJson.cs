using BarberBoss.Communication.Enums;

namespace BarberBoss.Communication.Responses;

public class ResponseBillingJson
{
    public int Id { get; set; }
    public string Date { get; set; } = string.Empty;
    public string BarberName { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public Status Status { get; set; }
    public string? Notes { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
    public string UpdatedAt { get; set; } = string.Empty;
}
