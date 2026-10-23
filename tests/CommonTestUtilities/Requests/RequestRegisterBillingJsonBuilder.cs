using BarberBoss.Communication.Enums;
using BarberBoss.Communication.Requests;
using Bogus;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterBillingJsonBuilder
{
    public static RequestBillingJson Build()
    {
        var serviceName = new List<string>
        {
            "Corte de cabelo",
            "Barba",
            "Corte e barba",
            "Sobrancelha",
            "Outros"
        };

        return new Faker<RequestBillingJson>()
            .RuleFor(r => r.Date, faker => faker.Date.Past())
            .RuleFor(r => r.BarberName, faker => faker.Name.FullName())
            .RuleFor(r => r.ClientName, faker => faker.Name.FullName())
            .RuleFor(r => r.ServiceName, faker => faker.PickRandom(serviceName))
            .RuleFor(r => r.PaymentMethod, faker => faker.PickRandom<PaymentMethod>())
            .RuleFor(r => r.Status, faker => faker.PickRandom<Status>())
            .RuleFor(r => r.Amount, (faker, billing) => billing.Status == Status.Cancelled ? 0 : faker.Random.Decimal(min: 1, max: 1000))
            .RuleFor(r => r.Notes, faker => faker.Random.Bool() ? faker.Lorem.Sentence() : null)
            .RuleFor(r => r.CreatedAt, faker => faker.Date.Past())
            .RuleFor(r => r.UpdatedAt, (faker, billing) => faker.Date.Between(billing.CreatedAt, DateTime.Now));
    }
}
