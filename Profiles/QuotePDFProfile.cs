using AutoMapper;
using QuotePDFService.Dtos;
using QuotePDFService.Models;

namespace QuotePDFService.Profiles
{
    public class QuotePDFProfile : Profile
    {
        public QuotePDFProfile()
        {
            CreateMap<QuotePDF, ReadQuotePDFDTO>();
            CreateMap<CreateQuotePDFDTO, QuotePDF>();
            CreateMap<UpdateQuotePDFDTO, QuotePDF>();

            CreateMap<TodoTemplateCreateDto, TodoTemplate>();

            //RabbitMQ
            CreateMap<Project, PublishedProjectAsyncDTO>();
            CreateMap<PublishedProjectAsyncDTO, Project>();
        }
    }
}