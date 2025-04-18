﻿using FluentAssertions;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Validators;
using Tests.EntityGenerators;

namespace Tests.Validators;

[Trait("Category", "Unit")]
public class ResourceFilterValidatorShould
{
    private readonly ResourceFilterDtoValidator _sut = new();

    [Theory]
    [ClassData(typeof(ResourceFilterDtoValidModelGenerator))]
    public void ReturnTrue_WhenResourceFilterIsValid(ResourceFilterDto resourceFilter)
    {
        var result = _sut.Validate(resourceFilter);
        
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(ResourceFilterDtoInvalidModelGenerator))]
    public void ReturnFalse_WhenResourceFilterIsNotValid(ResourceFilterDto resourceFilter)
    {
        var result = _sut.Validate(resourceFilter);
        
        result.IsValid.Should().BeFalse();
    }
}

