using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Enums;
using Bogus;

namespace CommonTestUtilities.Entities;

public class BillingBuilder
{
    public static List<Billing> Collection(User user, uint count = 2)
    {
        var list = new List<Billing>();

        if (count == 0)
            count = 1;

        var billingId = 1;

        for (int i = 0; i < count; i++)
        {
            var billing = Build(user);
            billing.Id = billingId++;

            list.Add(billing);
        }

        return list;
    }

    public static Billing Build(User user)
    {
        var services = new[]
        {
            "Corte masculino",
            "Degradê (Fade)",
            "Corte na tesoura",
            "Barba completa",
            "Pigmentação de barba",
            "Corte + Barba",
            "Design de sobrancelha",
            "Hidratação capilar",
            "Platinado",
            "Relaxamento capilar"
        };

        return new Faker<Billing>()
        .RuleFor(x => x.Id, _ => 1)
        .RuleFor(x => x.Date, faker => faker.Date.Past())
        .RuleFor(x => x.BarberName, faker => faker.Name.FirstName())
        .RuleFor(x => x.ClientName, faker => faker.Name.FirstName())
        .RuleFor(x => x.ServiceName, f => f.PickRandom(services))
        .RuleFor(x => x.Amount, faker => faker.Random.Decimal(min: 1, max: 1000))
        .RuleFor(x => x.PaymentMethod, faker => faker.PickRandom<PaymentMethod>())
        .RuleFor(x => x.Status, faker => faker.PickRandom(Status.Paid))
        .RuleFor(x => x.Notes, faker => faker.Lorem.Sentence())
        .RuleFor(x => x.CreatedAt, faker => faker.Date.Past())
        .RuleFor(x => x.UpdatedAt, faker => faker.Date.Past())
        .RuleFor(x => x.UserId, _ => user.Id);
    }
}
