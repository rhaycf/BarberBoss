using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Security.Cryptography;
using BarberBoss.Domain.Security.Tokens;
using BarberBoss.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public BillingIdentityManager Billing_Barber { get; private set; } = default!;
    public BillingIdentityManager Billing_Admin { get; private set; } = default!;
    public UserIdentityManager User_Barber { get; private set; } = default!;
    public UserIdentityManager User_Admin { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<BarberBossDbContext>(config => 
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(provider);
                });

                var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<BarberBossDbContext>();
                var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                StartDatabase(dbContext, passwordEncripter, accessTokenGenerator);
            });
    }

    private void StartDatabase(BarberBossDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
    {
        var userBarber = AddUserBarber(dbContext, passwordEncripter, accessTokenGenerator);
        var billingBarber = AddBillings(dbContext, userBarber, billingId: 1);
        Billing_Barber = new BillingIdentityManager(billingBarber);

        var userAdmin = AddUserAdmin(dbContext, passwordEncripter, accessTokenGenerator);
        var billingAdmin = AddBillings(dbContext, userAdmin, billingId: 2);
        Billing_Admin = new BillingIdentityManager(billingAdmin);

        dbContext.SaveChanges();
    }

    private User AddUserBarber(BarberBossDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build();
        user.Id = 1;

        var password = user.Password;
        user.Password = passwordEncripter.Encrypt(user.Password);

        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);

        User_Barber = new UserIdentityManager(user, password, token);

        return user;
    }

    private User AddUserAdmin(BarberBossDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build(Roles.ADMIN);
        user.Id = 2;

        var password = user.Password;
        user.Password = passwordEncripter.Encrypt(user.Password);

        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);

        User_Admin = new UserIdentityManager(user, password, token);

        return user;
    }

    private Billing AddBillings(BarberBossDbContext dbContext, User user, long billingId)
    {
        var billing = BillingBuilder.Build(user);
        billing.Id = billingId;

        dbContext.Billings.Add(billing);

        return billing;
    }
}