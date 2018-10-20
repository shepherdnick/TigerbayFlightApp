namespace TigerbayFlightApp
{
    public interface IRestClient
    {
        T Get<T>(string url);
        TO Post<TI, TO>(string url, TI data);
    }
}