namespace adam_and_eve_api.models
{
	public class UserRecord
	{
		public int Id { get; set; }
		public virtual User user { get; set; }
		public bool value { get; set; }
	}
}
