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

        void UpdateQuotePDFById(int id);

        void DeleteQuotePDFById(int id);
    }
}