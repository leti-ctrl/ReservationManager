﻿using FluentAssertions;
using NSubstitute;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.Core.Validators;
using Tests.EntityGenerators;

namespace Tests.Validators;

[Trait("Category", "Unit")]
public class ClosingCalendarFilterValidatorShould
{
    private readonly ClosingCalendarFilterDtoValidator _sut = new();

    [Theory]
    [ClassData(typeof(ClosingCalendarFilterDtoValidModelGenerator))]
    public void ReturnTrue_IsLegalDateRange_WhenDateOnlyIsValid(ClosingCalendarFilterDto filter)
    {
        var result = _sut.Validate(filter);
        
        result.IsValid.Should().BeTrue();
    }
    
    
    [Theory]
    [ClassData(typeof(ClosingCalendarFilterDtoInvalidModelGenerator))]
    public void ReturnFalse_IsLegalDateRange_WhenDateOnlyIsNotValid(ClosingCalendarFilterDto filter)
    {
        var result = _sut.Validate(filter);
        
        result.IsValid.Should().BeFalse();
    }
}



