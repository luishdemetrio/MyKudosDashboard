namespace MyKudosDashboard.Common
{
    public interface IImageHelper
    {
        string GetImageUrl(string imageName);
        Task UploadImage(string fileName, Stream stream);
    }
}