using BarberBoss.Domain.Enums;

namespace BarberBoss.Domain.Entities;

public class Billing
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public string BarberName { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public Status Status { get; set; }
    public string? Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long UserId { get; set; }
    public User User { get; set; } = default!;
}
