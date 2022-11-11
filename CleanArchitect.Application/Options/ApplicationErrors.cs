namespace CleanArchitect.Application.Options;

public class ApplicationErrors
{
    public string NoRowsWereAffected { get; set; }
    public string CustomerNotFound { get; set; }
    public string OrderAtLeastShouldHasOneOrderItem { get; set; }
    public string SomeOfOrderItemsDoesNotExist { get; set; }
    public string OrderItemsAreNotUnique { get; set; }
    public string OrderItemsQuantityShouldBeGreaterThanZero { get; set; }
    public string ItemDiscountShouldNotBeBiggerThanPrice { get; set; }



}
