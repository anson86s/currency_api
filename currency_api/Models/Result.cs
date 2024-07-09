namespace currency_api.Models;

public class Result<T> where T : class
{
    public bool IsSuccess { get; set; }
    public string ReturnCode { get; set; } = "";
    public string ReturnMessage { get; set; } = "";
    public T? Data { get; set; }
}

public class Result : Result<object>
{

}

