﻿using EventBus.Messages.Common;

namespace EventBus.Messages.E_vents
{
    public class BasketCheckoutEventV2 : BaseIntegrationEvent
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
