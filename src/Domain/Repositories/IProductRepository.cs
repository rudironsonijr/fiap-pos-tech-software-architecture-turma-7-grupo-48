using Domain.Entities.Enums;
using Domain.Entities.ProductAggregate;
using Domain.Repositories.Base;

namespace Domain.Repositories;

public interface IProductRepository : IRepository<Product>
{
	Task<int> CreateAsync(Product product, CancellationToken cancellationToken);
	Task<IEnumerable<Product>> ListAsync(ProductType productType, int? page,
		int? limit, CancellationToken cancellationToken);

	Task<Product?> GetAsync(int id, CancellationToken cancellationToken);

	Task<Product> UpdateAsync(Product Product, CancellationToken cancellationToken);
}