using DinkToPdf;
using DinkToPdf.Contracts;

namespace API.Services
{
    public class GeneratorPDFService : IPdfService
    {
        private readonly IConverter _converter;
        public GeneratorPDFService(IConverter converter){
            _converter = converter;
        }
        public byte[] GeneratePdf(string htmlContent)
        {
            var globalSettings = new GlobalSettings
            {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait,
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };
            return _converter.Convert(pdf);
        }

       public byte[] GeneratePdfs(List<string> htmlContents)
        {
            var pdfAllStudents = new HtmlToPdfDocument()
            {   
                GlobalSettings = new GlobalSettings
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                }
            };

            foreach(var htmlContent in htmlContents)
            {
                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = htmlContent,
                };
                pdfAllStudents.Objects.Add(objectSettings);
            }

            return _converter.Convert(pdfAllStudents);
        }
    
    }
}