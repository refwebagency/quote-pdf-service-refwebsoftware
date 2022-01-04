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
        }
    }
}