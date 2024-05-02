using Domain.Entities.Enums;
using Domain.Entities.OrderAggregate.Exceptions;
using Domain.Entities.ProductAggregate;

namespace Domain.Entities.OrderAggregate
{
	public class Order 
	{
		public Order() { }
		public Order(int id, OrderStatus status, List<OrderProduct> orderProducts) { 
			Id = id;
			Status = status;
			_orderProducts = orderProducts;
		}
		public int Id { get; init; }
		public int? CustomerId { get; init; }
		public OrderStatus Status { get; private set; } = OrderStatus.Creating;

		public List<OrderProduct> _orderProducts = [];
		public IReadOnlyCollection<OrderProduct> Products { get => _orderProducts.AsReadOnly(); }
		public decimal Price { 
			get 
			{
				decimal total = _orderProducts.Sum(x => x.Price);
				return Math.Round(total, 2);
			} 
		}

		public Order AddProduct(Product product, int quantity)
		{
			var item = _orderProducts.SingleOrDefault(x => x.Id == product.Id);
			if(item != null)
			{
				item.Quantity += quantity;
				return this;
			}

			OrderProduct orderProduct = new()
			{
				ProductId = product.Id,
				ProductPrice = product.Price,
				Quantity = quantity,
			};

			_orderProducts.Add(orderProduct);
			return this;
		}

		public Order RemoveProduct(int orderProductId)
		{
			var itemToRemove = _orderProducts.SingleOrDefault(x => x.Id == orderProductId);
			if(itemToRemove != null)
			{
				_orderProducts.Remove(itemToRemove);
			}
			return this;
		}

		public Order UpdateProductQuantity(int orderProductId, int quantity)
		{
			var itemToUpdate = _orderProducts.SingleOrDefault(x => x.Id == orderProductId);
			if (itemToUpdate == null)
			{
				return this;
			}

			if(quantity == 0)
			{
				_orderProducts.Remove(itemToUpdate);
				return this;
			}

			itemToUpdate.Quantity = quantity;
			return this;
		}

		public Order ChangeStatusToReceived()
		{
			ChangeOrderStatusToReceivedException.ThrowIfOrderProductsIsEmpty(_orderProducts);
			Status = OrderStatus.Received;
			return this;
		}

		private void ValidateStatusToUpdateOrderProduct()
		{
			if(Status == OrderStatus.Creating)
			{
				return;
			}
			throw new ChangeOrderProductNotAbleException();
		}
	}
}
