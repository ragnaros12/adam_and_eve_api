namespace adam_and_eve_api.models;

public class Token
{
    public string token { get; set; }

    public Token(string token)
    {
        this.token = token;
    }
}