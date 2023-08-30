﻿using CarRental.BuildingBlocks.DDD;
using CarRental.BuildingBlocks.ErrorNotification;
using CarRental.BuildingBlocks.Validation;

namespace CarRental.ClientsCatalog.Model;

public class ClientAgreements: Entity
{
    public required bool MarketingEmailAgreement { get; init; }
    public required bool GDPR { get; init; }
    public bool AdditionalMarketingContacts { get; init; }
    public Notification<DomainError> Notification { get; } = new();
    
    public override bool Validate()
    {
        if(MarketingEmailAgreement is false || GDPR is false)
            Notification.AddError(new() {Message = "We cannot sign agreement without GDPR and MarketingEmailAgreement", Source = nameof(ClientAgreements)});

        return !Notification.HasErrors;
    }

    
}