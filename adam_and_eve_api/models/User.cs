using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace adam_and_eve_api.models
{
	public class User
	{
		//[JsonIgnore]
		public string token { get; set; } = "a";
		public int Id { get; set; }
		public string name { get; set; } = "a";
		public DateTime age { get; set; } = DateTime.Now;
		public Gender gender { get; set; } = Gender.man;
		public Gender interestedIn { get; set; } = Gender.man;
		public string country { get; set; } = "a";
		public string city { get; set; } = "a";
		public string education { get; set; } = "a";
		public string job { get; set; } = "a"; 
		public string zodiac { get; set; } = "a";
		public string about { get; set; } = "a";
		public string location { get; set; } = "a";
		public Purpose purpose { get; set; } = Purpose.flirt;
		public string login { get; set; } = "a";
		public string password { get; set; } = "a";
		public bool isVerified { get; set; } = false;

		[JsonIgnore]
		public virtual List<UserRecord> usersRecords { get; set; }


		public User(string login, string password)
		{
			this.login = login;
			this.password = password;
			isVerified = false;
		}
		public User()
		{
		}
	}
}

public enum Gender
{
	[EnumMember(Value = "man")]
	man,
	[EnumMember(Value = "wooman")]
	wooman,
	[EnumMember(Value = "transgender")]
	transgender
}
public enum Purpose 
{
	[EnumMember(Value = "family")]
	family,
	[EnumMember(Value = "frien")]
	friendship,
	flirt 
}