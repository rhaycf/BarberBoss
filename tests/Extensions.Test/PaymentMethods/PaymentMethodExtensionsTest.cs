using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Extensions;
using Shouldly;

namespace Extensions.Test.PaymentMethods;

public class PaymentMethodExtensionsTest
{
    [Theory]
    [InlineData(PaymentMethod.Card)]
    [InlineData(PaymentMethod.Cash)]
    [InlineData(PaymentMethod.Pix)]
    [InlineData(PaymentMethod.Other)]
    public void Success(PaymentMethod paymentMethod)
    {
        // Arrange

        // Act
        var result = paymentMethod.PaymentMethodToString();

        // Assert
        result.ShouldNotBeEmpty();
    }

    [Fact]
    public void Error_Invalid_PaymentMethod()
    {
        // Arrange
        var paymentMethod = (PaymentMethod)99;

        // Act
        var result = paymentMethod.PaymentMethodToString();

        // Assert
        result.ShouldBeEmpty();
    }
}
