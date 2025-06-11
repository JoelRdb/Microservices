using MediatR;
using Ordering.Application.Responses;

namespace Ordering.Application.Queries
{
    public class GetOrderListQuery : IRequest<List<OrderResponse>>
    {
        public string _userName { get; set; }

        public GetOrderListQuery(string userName)
        {
            _userName = userName;
        }
    }
}
