namespace MyKudos.Communication.Helper.Interfaces;

public interface IRestClientHelper
{
    Task<TResponse> GetApiData<TResponse>(string endpoint);

    Task<TResponse> GetApiData<TRequestBody, TResponse>(string endpoint, TRequestBody body);

    Task<TResponse> SendApiData<TRequestBody, TResponse>(string endpoint, HttpMethod httpMethod, TRequestBody? body, int timeoutInSeconds = 100);
}