using System.Net;

namespace ElasticSearch.API.DTOs;

public sealed record ResponseDto<T>()
{
    public T? Data { get; set; }
    public List<string>? Messages { get; set; } = new();
    public HttpStatusCode HttpStatusCode { get; set; }
    
    
    public static ResponseDto<T> Success(T data, HttpStatusCode code)
        => new() {Data = data, HttpStatusCode = code};
    
    public static ResponseDto<T> Fail(List<string> messages,HttpStatusCode httpStatusCode)
        => new() {Messages = messages, HttpStatusCode = httpStatusCode};
    
    public static ResponseDto<T> Fail(string messages,HttpStatusCode httpStatusCode)
        => new() {Messages = new List<string> {messages}, HttpStatusCode = httpStatusCode};
}