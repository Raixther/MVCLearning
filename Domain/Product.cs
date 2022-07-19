namespace MVCLearning.Domain
{
	public class Product
	{
		public Guid Id { get; set; } = new();

		public string Name{ get; set; }

		public decimal Price{ get; set; }

		
	}
}
