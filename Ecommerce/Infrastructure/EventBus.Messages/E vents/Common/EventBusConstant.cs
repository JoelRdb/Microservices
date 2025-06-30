namespace EventBus.Messages.E_vents.Common
{
    // Use in Program.cs for consumer (in our case, ordering.API)
    public class EventBusConstant
    {
        public const string BasketCheckoutQueue = "basketcheckout-queue";
        public const string BasketCheckoutQueueV2 = "basketcheckout-queue-v2";
    }
}
