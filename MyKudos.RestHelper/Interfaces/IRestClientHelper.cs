namespace MyKudos.Communication.Helper.Interfaces;

public interface IRestClientHelper
{
    Task<TResponse> GetApiData<TResponse>(string endpoint);

    Task<TResponse> GetApiData<TRequestBody, TResponse>(string endpoint, TRequestBody body);

    Task<TResponse> SendApiData<TRequestBody, TResponse>(string endpoint, RestSharp.Method httpMethod, TRequestBody? body);
}