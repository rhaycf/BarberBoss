using BarberBoss.Application.UseCases.Users.Register;
using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using FluentValidation;
using Shouldly;

namespace Validators.Test.Users.Register;

public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        // arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();

        // act
        var result = validator.Validate(request);
        

        // assert
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Name_Empty(string name)
    {
        // arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = name;

        // act
        var result = validator.Validate(request);

        // assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceErrorMessages.NAME_EMPTY); ;
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Email_Empty(string email)
    {
        // arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = email;

        // act
        var result = validator.Validate(request);

        // assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceErrorMessages.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        // arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "email.com";

        // act
        var result = validator.Validate(request);

        // assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceErrorMessages.EMAIL_INVALID);
    }

    [Fact]
    public void Error_Password_empty()
    {
        // arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = string.Empty;

        // act
        var result = validator.Validate(request);

        // assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceErrorMessages.PASSWORD_INVALID);
    }
}
