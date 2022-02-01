using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuotePDFService.Models;

namespace QuotePDFService.Data
{
    public class QuotePDFRepo : IQuotePDFRepo
    {
        private readonly AppDbContext _context;

        public QuotePDFRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateQuotePDF(QuotePDF quotePDF)
        {
            if (quotePDF == null)
            {
                throw new ArgumentNullException(nameof(quotePDF));
            }

            _context.Add(quotePDF);
            _context.SaveChanges();
        }

        public void DeleteQuotePDFById(int id)
        {
            var quotePDF = _context.QuotePDF.FirstOrDefault(QuotePDF => QuotePDF.Id == id);

            if (quotePDF != null)
            {
                _context.QuotePDF.Remove(quotePDF);
            }
        }

        public IEnumerable<QuotePDF> GetAllQuotePDF()
        {
            _context.Project.ToList();
            _context.TodoTemplate.ToList();
            return _context.QuotePDF.ToList();
        }

        public QuotePDF GetQuotePDFById(int id)
        {
            _context.Project.ToList();
            _context.TodoTemplate.ToList();
            return _context.QuotePDF.FirstOrDefault(QuotePDF => QuotePDF.Id == id);
        }

        public TodoTemplate GetTodoTemplateById(int id)
        {
            return _context.TodoTemplate.FirstOrDefault(TodoTemplate => TodoTemplate.Id == id);
        }

        public IEnumerable<Project> GetQuotePDFByClientId(int id)
        {
            _context.Project.ToList();
            _context.TodoTemplate.ToList();
            return _context.Project.Where(Project => Project.ClientId == id).ToList();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >=0 );
        }

        public void UpdateQuotePDFById(int id)
        {
            var quotePDF = _context.QuotePDF.FirstOrDefault(QuotePDF => QuotePDF.Id == id);

            _context.Entry(quotePDF).State = EntityState.Modified;
        }

        public void CreateTodoTemplate(TodoTemplate todoTemplate)
        {
            if (todoTemplate == null)
            {
                throw new ArgumentNullException(nameof(todoTemplate));
            }

            _context.TodoTemplate.Add(todoTemplate);
            _context.SaveChanges();

        }
    }
}