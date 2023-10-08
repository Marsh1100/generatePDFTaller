namespace API.Services;
public interface IPdfService
{
    byte[] GeneratePdf(string htmlContent);
    byte[] GeneratePdfs (List<string> htmlContents);

}
