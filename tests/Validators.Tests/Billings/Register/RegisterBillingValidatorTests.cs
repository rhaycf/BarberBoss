using BarberBoss.Application.UseCases.Billings;
using BarberBoss.Communication.Enums;
using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validators.Tests.Billings.Register;

public class RegisterBillingValidatorTests
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new BillingValidator();
        var request = RequestRegisterBillingJsonBuilder.Build();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("         ")]
    [InlineData(null)]
    [InlineData("A")]
    public void Error_BarberName_EmptyOrNull(string barberName)
    {
        // Arrange
        var validator = new BillingValidator();
        var request = RequestRegisterBillingJsonBuilder.Build();
        request.Status = Status.Paid;
        request.BarberName = barberName;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();

        var error = result.Errors.ShouldHaveSingleItem();
        error.ErrorMessage.ShouldBe(ResourceErrorMessages.BARBER_NAME_REQUIRED_AND_INVALID_LENGTH);
    }

    [Theory]
    [InlineData("")]
    [InlineData("         ")]
    [InlineData(null)]
    [InlineData("A")]
    public void Error_ClientName_EmptyOrNull(string clientName)
    {
        // Arrange
        var validator = new BillingValidator();
        var request = RequestRegisterBillingJsonBuilder.Build();
        request.Status = Status.Paid;
        request.ClientName = clientName;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();

        var error = result.Errors.ShouldHaveSingleItem();
        error.ErrorMessage.ShouldBe(ResourceErrorMessages.CLIENT_NAME_REQUIRED_AND_INVALID_LENGTH);
    }

    [Theory]
    [InlineData("")]
    [InlineData("         ")]
    [InlineData(null)]
    [InlineData("A")]
    public void Error_ServiceName_EmptyOrNull(string serviceName)
    {
        // Arrange
        var validator = new BillingValidator();
        var request = RequestRegisterBillingJsonBuilder.Build();
        request.ServiceName = serviceName;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();

        var error = result.Errors.ShouldHaveSingleItem();
        error.ErrorMessage.ShouldBe(ResourceErrorMessages.SERVICE_NAME_REQUIRED_AND_INVALID_LENGTH);
    }

    [Fact]
    public void Error_Payment_Method_Invalid()
    {
        // Arrange
        var validator = new BillingValidator();
        var request = RequestRegisterBillingJsonBuilder.Build();
        request.PaymentMethod = (PaymentMethod)700;
        request.Status = Status.Paid;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();

        var error = result.Errors.ShouldHaveSingleItem();
        error.ErrorMessage.ShouldBe(ResourceErrorMessages.PAYMENT_METHOD_INVALID);
    }

    [Fact]
    public void Error_Status_Invalid()
    {
        // Arrange
        var validator = new BillingValidator();
        var request = RequestRegisterBillingJsonBuilder.Build();
        request.Status = (Status)150;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();

        var error = result.Errors.ShouldHaveSingleItem();
        error.ErrorMessage.ShouldBe(ResourceErrorMessages.STATUS_INVALID);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-0.02)]
    [InlineData(-100)]
    public void Error_Amount_Invalid(decimal amount)
    {
        // Arrange
        var validator = new BillingValidator();
        var request = RequestRegisterBillingJsonBuilder.Build();
        request.Status = Status.Paid;
        request.Amount = amount;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();

        var error = result.Errors.ShouldHaveSingleItem();
        error.ErrorMessage.ShouldBe(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_OR_EQUAL_TO_ZERO);
    }

    [Fact]
    public void Success_Cancelled_With_Zero_Amount()
    {
        // Arrange
        var validator = new BillingValidator();
        var request = RequestRegisterBillingJsonBuilder.Build();
        request.Status = Status.Cancelled;
        request.Amount = 0;

        validator.Validate(request).IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Cancelled_With_Amount_Greater_Than_Zero()
    {
        // Arrange
        var validator = new BillingValidator();
        var request = RequestRegisterBillingJsonBuilder.Build();
        request.Status = Status.Cancelled;
        request.Amount = 50;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem()
                .ErrorMessage.ShouldBe(ResourceErrorMessages.CANCELLED_BILLING_AMOUNT_MUST_BE_ZERO);
    }

    [Fact]
    public void Error_Date_In_The_Future()
    {
        // Arrange
        var validator = new BillingValidator();
        var request = RequestRegisterBillingJsonBuilder.Build();
        request.Status = Status.Paid;
        request.Date = DateTime.UtcNow.AddDays(1);

        // Act
        var result = validator.Validate(request);
        
        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem()
                .ErrorMessage.ShouldBe(ResourceErrorMessages.DATE_CANNOT_BE_IN_THE_FUTURE);
    }

    [Fact]
    public void Error_CreatedAt_In_The_Future()
    {
        // Arrange
        var validator = new BillingValidator();
        var request = RequestRegisterBillingJsonBuilder.Build();
        request.Status = Status.Paid;
        request.CreatedAt = DateTime.UtcNow.AddDays(1);

        // Act
        var result = validator.Validate(request);
        
        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem()
                .ErrorMessage.ShouldBe(ResourceErrorMessages.CREATED_AT_CANNOT_BE_IN_THE_FUTURE);
    }

    [Fact]
    public void Error_UpdatedAt_In_The_Future()
    {
        // Arrange
        var validator = new BillingValidator();
        var request = RequestRegisterBillingJsonBuilder.Build();
        request.Status = Status.Paid;
        request.UpdatedAt = DateTime.UtcNow.AddDays(1);

        // Act
        var result = validator.Validate(request);
        
        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem()
                .ErrorMessage.ShouldBe(ResourceErrorMessages.UPDATED_AT_CANNOT_BE_IN_THE_FUTURE);
    }

}
