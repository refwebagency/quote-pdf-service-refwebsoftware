using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QuotePDFService.Data;
using QuotePDFService.Dtos;
using QuotePDFService.Models;
using IronPdf;
using System.IO;
using System.Diagnostics;

namespace QuotePDFService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class QuotePDFController : ControllerBase
    {
        private readonly IQuotePDFRepo _repository;
        private readonly IMapper _mapper;

        public QuotePDFController(IMapper mapper, IQuotePDFRepo repository)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReadQuotePDFDTO>> GetAllQuotePDF()
        {
            var quotePDFItems = _repository.GetAllQuotePDF();

            return Ok(_mapper.Map<IEnumerable<ReadQuotePDFDTO>>(quotePDFItems));
        }

        [HttpGet("accepte", Name = "AccepteQuotePDF")]
        public ActionResult AccepteQuotePDF()
        {
           
            var htmltopdf = new HtmlToPdf();
            var pdfDoc = htmltopdf.RenderHtmlFileAsPdf("Template/quote.html");
            pdfDoc.SaveAs("Template/quote.pdf");
            string physicalPath = "Template/quote.pdf";
            byte[] pdfBytes = System.IO.File.ReadAllBytes(physicalPath);
            MemoryStream ms = new MemoryStream(pdfBytes);
            return new FileStreamResult(ms, "application/pdf");
        }

        [HttpGet("{id}", Name = "GetQuotePDFById")]
        public ActionResult<ReadQuotePDFDTO> GetQuotePDFById(int id)
        {
            var quotePDFItem = _repository.GetQuotePDFById(id);

            if (quotePDFItem == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ReadQuotePDFDTO>(quotePDFItem));
        }

        [HttpGet("client/{id}", Name = "GetQuotePDFByClientId")]
        public ActionResult<IEnumerable<ReadQuotePDFDTO>> GetQuotePDFByClientId(int id)
        {
            var quotePDFItem = _repository.GetQuotePDFByClientId(id);

            if (quotePDFItem == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<ReadQuotePDFDTO>>(quotePDFItem));
        }

        [HttpPost]
        public ActionResult<CreateQuotePDFDTO> CreateQuotePDF(CreateQuotePDFDTO quotePDFDTO)
        {
            var quotePDFModel = _mapper.Map<QuotePDF>(quotePDFDTO);

            _repository.CreateQuotePDF(quotePDFModel);
            _repository.SaveChanges();

            var readQuotePDF = _mapper.Map<ReadQuotePDFDTO>(quotePDFModel);

            return CreatedAtRoute(nameof(GetQuotePDFById), new {id = readQuotePDF.Id }, readQuotePDF);
        }

        [HttpPut("{id}", Name = "UpdateQuotePDFById")]
        public ActionResult<UpdateQuotePDFDTO> UpdateQuotePDFById(int id, UpdateQuotePDFDTO quotePDFDTO)
        {
            var quotePDFItem = _repository.GetQuotePDFById(id);

            _mapper.Map(quotePDFDTO, quotePDFItem);

            if (quotePDFItem == null)
            {
                return NotFound();
            }

            _repository.UpdateQuotePDFById(id);
            _repository.SaveChanges();

            return CreatedAtRoute(nameof(GetQuotePDFById), new {id = quotePDFDTO.Id }, quotePDFDTO);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteQuotePDFById(int id)
        {
            var quotePDFItem = _repository.GetQuotePDFById(id);

            if (quotePDFItem == null)
            {
                return NotFound();
            }

            _repository.DeleteQuotePDFById(quotePDFItem.Id);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}