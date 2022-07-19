using System.Collections.Concurrent;

using MVCLearning.DomainEvents;

namespace MVCLearning.Domain
{
	public class Catalog
	{
		public ConcurrentDictionary<Guid, Product> Products { get;private set; }=new();

		public int Count => Products.Count;

		public Task Add(Product product)
		{
			Products.TryAdd(product.Id, product);

		//    DomainEventsManager.Raise<ProductAdded>(new ProductAdded(product));

			return Task.CompletedTask;	
		}

		//public void Remove(Product product)
		//{
		//	Products.TryRemove(product.Id, out product);
		//}
	}
}
