namespace RazorPagesFreeCoding.Domain
{
	public class Product
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public decimal Price{ get; set; }

		public Product(string name, decimal price)
		{
			Name = name;
			Price = price;
		}
	}



}

