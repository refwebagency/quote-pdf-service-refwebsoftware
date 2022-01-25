using System.Collections.Generic;
using QuotePDFService.Models;

namespace QuotePDFService.Data
{
    public interface IQuotePDFRepo
    {
        bool SaveChanges();

        void CreateQuotePDF(QuotePDF quotePDF);

        IEnumerable<QuotePDF> GetAllQuotePDF();

        QuotePDF GetQuotePDFById(int id);

        IEnumerable<Project> GetQuotePDFByClientId(int id);

        void UpdateQuotePDFById(int id);

        void DeleteQuotePDFById(int id);

        TodoTemplate GetTodoTemplateById(int id);
    }
}