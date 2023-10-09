# generatePDFTaller
Proyecto webApi de cuatro capas usando NetCore7.0 para generar informes en archivos PDF del boletín final de entrega de notas, haciendo uso de la biblioteca DinkToPdf y Microsoft.AspNet.Mvc,  para los estudiantes del colegio La Estrellita de Bucaramanga empleando como gestor de base de datos MySQL. 

### ¿Qué se va obtener?
  * Informe en PDF del boletín de notas final de un estudiante.
  * Informe en PDF del boletín de notas de cada uno de los estudiantes.
  * Informe en PDF de los 3 primeros estudiantes con mayor promedio de las notas por materia.

### Pre-requisitos 📋
MySQL<br>
NetCore 7.0
### Base de datos
![image](https://github.com/yllensc/generatePDFCsharp/assets/131481951/9afbcdb0-d825-4b54-993e-a9b73b610393)

### Ejecutar proyecto 🔧
1. Clone el repositorio en la carpeta que desee abriendo la terminal y ejecute el siguiente code
   ```
   https://github.com/Marsh1100/generatePDFTaller.git
   ```
2. Acceda al la carpeta que se acaba de generar
   ```cd  ```
3. Ahora ejecute el comando ```. code``` para abrir el proyecto en Visual Studio Code
4. En la carpeta API diríjase al archivo appsettings.Development.json
     Llene los campos según sea su caso en los valores server, user y password reemplazando las comillas simples.

     <b>Nota:</b> Puede cambiar el nombre de la base de datos (database) si así lo prefiere.
5. Ahora abra una nueva terminal en Visual Studio Code
6. Ejecute las siguientes líneas de código para migrar la Base de Datos a su servidor. <br>
     ```dotnet ef migrations add FirstMigration --project ./Persistence/ --startup-project ./API/ --output-dir ./Data/Migrations ```<br><br>
     ```dotnet ef database update --project ./Persistence --startup-project ./API```
7. Cree la carpeta Services junto con su respectiva interfaz e implementación:
   ```
   public interface IPdfService{
    byte[] GeneratePdf(string htmlContent);
    byte[] GeneratePdfs (List<string> htmlContents);}
   ```
   <br>
   Clase del servicio:
   
         public class PdfService : IPdfService{
         private readonly IConverter _converter;
         public PdfService(IConverter converter){
             _converter = converter;}

        public byte[] GeneratePdf(string htmlContent)
        {
            var globalSettings = new GlobalSettings
            {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
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
8. En su archivo de extenciones, inyecte el servicio, debería tener algo así:
   ```
   public static void AddAplicacionServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IStudent, StudentRepository>();
        services.AddScoped<IPdfService, PdfService>();
        services.AddControllersWithViews(); 
    }
   ```
9. Crea un método en la clase StudentController para generar los html:
    ```
    public string GenerateHtml (StudentDto student){
        var sw = new StringWriter();
        var viewContext = new ViewContext
        {
            HttpContext = HttpContext,
            RouteData = RouteData,
            ActionDescriptor = new ActionDescriptor(),
            ViewData = new ViewDataDictionary<StudentDto>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {Model = student }, // Asigna el modelo a la vista.
            Writer = sw
        };
        var viewName = "report";
        var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);
        if(viewResult == null){
            return null;
        }
        var viewEngineResult = viewResult.View.RenderAsync(viewContext);
        viewEngineResult.Wait();
        var html = sw.ToString();
        return html;
    }
    ```
10. Ahora puede crear allí mismo las solicitudes HTTP: <br>
    Método para el reporte de un estudiante, filtrado por el id:
    ```
    [HttpGet("generate-reportStudent/{studentId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GeneratePDF(int studentId)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(studentId);
        if (student == null)
        {
            return BadRequest("Estudiante no encontrado");
        }
        try
        {            
            var studentData = _mapper.Map<StudentDto>(student);
            var html = GenerateHtml(studentData);
            var pdfBytes = _pdfService.GeneratePdf(html);
            return File(pdfBytes, "application/pdf", $"reportStudent-{student.NameStudent}.pdf");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error al generar el informe: {ex.Message}");
        }
    }
    ```
    Método para el reporte de todos los estudiantes:
    <br>
    ```
    [HttpGet("generate-reportStudents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GeneratePDFs()
    {
        var students = await _unitOfWork.Students.GetAllAsync();

        try
        {            
            var studentsData = _mapper.Map<List<StudentDto>>(students);
            
            var htmlContents = new List<string>();
            foreach (var studentData in studentsData)
            {
                var html = GenerateHtml(studentData);
                htmlContents.Add(html);
            }
            var pdfBytes = _pdfService.GeneratePdfs(htmlContents);
            return File(pdfBytes, "application/pdf", "informe.pdf");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error al generar el informe: {ex.Message}");
        }
    }
    ```
11. Accede a la carpeta API ```cd API ``` y ejecuta el comando    ```dotnet run ```<br>
  Le aparecerá algo como esto:<br>
  ![image](https://github.com/yllensc/generatePDFCsharp/assets/131481951/9eede092-b899-4c32-9c14-a78721f05b2b)
<br>Nota:<br> Tenga en cuenta que el servidor es local y el puerto puede cambiar. <br>

¡Listo! Ahora podrá ejecutar los endpoints sin problema.<br>

## Ejecutando los Endpoints ⚙️📚
1. Informe en PDF del boletín de notas final de un estudiante<br>
   <b>Nota:</b> Reemplazar {id} por el id del estudiante.
   * ```http://localhost:5062/generate-reportStudent/{id} ```<br>
   ![image](https://github.com/yllensc/generatePDFCsharp/assets/131481951/3510195b-506c-465e-9ec7-c01c0ee96140)
3. Informe en PDF del boletín de notas de cada uno de los estudiantes<br>
    * ```http://localhost:5062/generate-reportStudents ```<br>
    ![image](https://github.com/yllensc/generatePDFCsharp/assets/131481951/e098f72a-3c47-449e-933e-63e87e66e1d8)


## Autores ✒️

* **Margie Bocanegra** - [Marsh1100](https://github.com/Marsh1100)
* **Yllen Santamaría** - [Yllensc](https://github.com/yllensc)
