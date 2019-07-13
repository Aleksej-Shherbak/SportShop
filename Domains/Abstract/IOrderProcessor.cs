using System.Threading.Tasks;
using Domains.Entities;

namespace Domains.Abstract
{
    public interface IOrderProcessor
    {
        Task ProcessOrder(Cart cart, ShippingDetails shippingDetails);
    }
}