using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace APITests;
public class ResponseWrapper<T> where T : class
{
    public bool Success { get; set; }

    public string? ErrorMessage { get; set; }

    public int HttpStatusCode { get; set; }

    public List<T>? Results { get; set; }
}