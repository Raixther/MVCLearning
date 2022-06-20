using System.Collections.Concurrent;

using RazorPagesFreeCoding.Infrastructure;

using Serilog;

namespace RazorPagesFreeCoding.Domain
{
	

	public class Catalog
	{		

		private readonly IMailSender _mailSender;

		private readonly ConcurrentDictionary<Guid, Product> _products = new();

		public Catalog(IMailSender mailSender)
		{
			_mailSender = mailSender;
		}
		public int Count => _products.Count;
		public void Add(Product product)
		{
			_products.TryAdd(product.Id, product);
			_mailSender.SendMail(
			"Tester"
			, "Продукт добавлен"
			, "Добавление продукта"
			);

		}
		public void Remove(Product product) => _products.TryRemove(product.Id,  out product);
		public IReadOnlyList<Product> GetAll() => _products.Values.ToArray();
		
	}	
}
