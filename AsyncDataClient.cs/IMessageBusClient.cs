using QuotePDFService.Dtos;

namespace QuotePDFService.AsyncDataClient
{
    public interface IMessageBusClient
    {
        void SendAnyProject(PublishedProjectAsyncDTO publishedProjectAsyncDTO);
    }
}