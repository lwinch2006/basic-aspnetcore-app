namespace Dka.AspNetCore.BasicWebApp.Models.ExceptionProcessing
{
    public interface IBasicWebAppException
    {
        string Message { get; }

        object StackTrace { get; }
    }
}