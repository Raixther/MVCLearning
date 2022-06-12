using System.Collections.Concurrent;
namespace RazorPagesFreeCoding.Domain
{
	public class Catalog
	{
		private readonly ConcurrentDictionary<Guid, Product> _products = new();
		public int Count => _products.Count;
		public void Add(Product product) => _products.TryAdd(product.Id, product);

		public void Remove(Product product) => _products.TryRemove(product.Id, out product);

		public IReadOnlyList<Product> GetAll() => _products.Values.ToArray();
	}
}
