namespace APIGateway
{
    public interface IUserOrdersAggregator
    {
        Task<string> AggregateUserOrdersAsync(string userId);
    }
}
