namespace RazorPagesFreeCoding.Domain
{
	public class Product
	{
		public int Price{ get; set; }

		public string Name{ get; set; }

		public Product(int price, string name)
		{
			Price = price; Name = name;
		}
	}
}
