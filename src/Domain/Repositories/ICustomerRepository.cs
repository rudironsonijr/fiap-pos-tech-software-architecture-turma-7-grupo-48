using Domain.Entities.CustomerAggregate;

namespace Domain.Repositories;

public interface ICustomerRepository
{
	Task<Customer?> GetByCpf(
		string cpf,
		CancellationToken cancellationToken
	);
}