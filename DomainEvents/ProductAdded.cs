using MVCLearning.Domain;

namespace MVCLearning.DomainEvents
{
	public class ProductAdded:IDomainEvent
	{
        public Product Product { get; }

        public ProductAdded(Product product)
        {
            Product = product;
        }
    }
}
