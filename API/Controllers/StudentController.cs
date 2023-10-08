using API.Dtos;
using API.Services;
using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
namespace API.Controllers;
public class StudentController : Controller
{
    private readonly ICompositeViewEngine _viewEngine;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPdfService _pdfService;
    private readonly IMapper _mapper;

    public StudentController(ICompositeViewEngine viewEngine, IUnitOfWork unitOfWork, IPdfService pdfService, IMapper mapper)
    {
        _mapper = mapper;
        _pdfService = pdfService;
        _unitOfWork = unitOfWork;
        _viewEngine = viewEngine;
    }
    
}

