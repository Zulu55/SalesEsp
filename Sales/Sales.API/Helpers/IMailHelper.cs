using Sales.Shared.Responses;

namespace Sales.API.Helpers
{
    public interface IMailHelper
    {
        Response<Exception> SendMail(string toName, string toEmail, string subject, string body);
    }
}
