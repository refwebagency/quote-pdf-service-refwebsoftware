using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QuotePDFService.Data;
using QuotePDFService.Dtos;
using QuotePDFService.Models;
using IronPdf;
using System.IO;
using System.Diagnostics;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using Microsoft.EntityFrameworkCore;
using QuotePDFService.AsyncDataClient;
using System.Text;

namespace QuotePDFService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class QuotePDFController : ControllerBase
    {
        private readonly IQuotePDFRepo _repository;
        private readonly IMapper _mapper;
        private readonly HttpClient _HttpClient;
        private readonly IConfiguration _configuration;
        private readonly IMessageBusClient _messageBusClient;

        public QuotePDFController(IMapper mapper, IQuotePDFRepo repository, HttpClient HttpClient, IConfiguration configuration, IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _HttpClient = HttpClient;
            _configuration = configuration;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReadQuotePDFDTO>> GetAllQuotePDF()
        {
            var quotePDFItems = _repository.GetAllQuotePDF();

            return Ok(_mapper.Map<IEnumerable<ReadQuotePDFDTO>>(quotePDFItems));
        }

        [HttpGet("Devis", Name = "AccepteQuotePDF")]
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

        [HttpGet("todo/{id}", Name = "GetTodoTemplateById")]
        public ActionResult<TodoTemplate> GetTodoTemplateById(int id)
        {
            var quotePDFItem = _repository.GetTodoTemplateById(id);

            if (quotePDFItem == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TodoTemplate>(quotePDFItem));
        }

        [HttpPost]
        public async Task<ActionResult<CreateQuotePDFDTO>> CreateQuotePDF(CreateQuotePDFDTO quotePDFDTO)
        {
            var quotePDFModel = _mapper.Map<QuotePDF>(quotePDFDTO);
            // nouvelle liste de TodoTemplate
            var items = new List<TodoTemplate>();
            
            // pour chaque liste de TodoTemplate dans un quotePDF
            foreach (var TodoItem in quotePDFModel.TodoTemplates)
            {
                //nouvel objet TodoTemplate
                var newTodo = new TodoTemplate();
                var TodoTemplateDTO = _mapper.Map<TodoTemplate>(TodoItem);

                // requete GET en async d'un todoTemplate par son ID dans toDoTemplateService
                var getTodoTemplate = await _HttpClient.GetAsync($"{_configuration["TodoTemplateService"]}" + TodoItem.Id);

                // deserialization de l'objet reçu 
                var deserializedTodoTemplate = JsonConvert.DeserializeObject<TodoTemplateCreateDto>(
                    await getTodoTemplate.Content.ReadAsStringAsync());

                var todoTemplateModel = _mapper.Map<TodoTemplate>(deserializedTodoTemplate);
                
                // recupere l'objet stocké avec l'id de l'objet reçu
                var todoTemplate = _repository.GetTodoTemplateById(TodoItem.Id);
                
                // Si aucun objet n'est retoruné 
                if (todoTemplate != null)
                {   
                        // Set des valeurs 
                        newTodo.ExternalToDoId = todoTemplateModel.Id;
                        newTodo.Name = todoTemplateModel.Name;
                        newTodo.Experience = todoTemplateModel.Experience;
                        newTodo.Description = todoTemplateModel.Description;
                        newTodo.Time = todoTemplateModel.Time;
                        newTodo.SpecializationId = todoTemplateModel.SpecializationId;
                        newTodo.ProjectId = quotePDFModel.Project.Id;
                        newTodo.ProjectTypeId = todoTemplateModel.ProjectTypeId;
                    
                } 
                else
                { 
                        newTodo = todoTemplateModel;
                        newTodo.ExternalToDoId = todoTemplateModel.Id;
                }
                // Rajout de l'objet nouvellement crée à la liste
                items.Add(newTodo);
                
            }        
            // set de la liste à quotePDF
            quotePDFModel.TodoTemplates = items;

            _repository.CreateQuotePDF(quotePDFModel);
            _repository.SaveChanges();
            
            var readQuotePDF = _mapper.Map<ReadQuotePDFDTO>(quotePDFModel);

            return CreatedAtRoute(nameof(GetQuotePDFById), new {id = readQuotePDF.Id }, readQuotePDF);
        }

        /** Quand le client accepte le devis, je veux que l'objet project 
            soit envoyé à projectService en rabbitMQ, afin d'obtenir en réponse son Id,
            pour que je puisse le SET dans l'attribut projectId des objets Todo de la 
            liste todoTemplate*/
        [HttpGet("startproject/{id}", Name = "StartNewProjectWithHisTasks")]
        public async Task<ActionResult<ReadQuotePDFDTO>> StartNewProjectWithHisTasks(int id)
        {
            var quotePDFItem = _repository.GetQuotePDFById(id);

            // seialization du projet
            var httpProjectContent = new StringContent( System.Text.Json.JsonSerializer.Serialize(quotePDFItem.Project),
                        Encoding.UTF8,
                        "application/json"
                        );

            foreach(var toDo in quotePDFItem.TodoTemplates)
            {
                toDo.ProjectId = quotePDFItem.Project.Id;

                var httpTodoContent = new StringContent( System.Text.Json.JsonSerializer.Serialize(toDo),
                        Encoding.UTF8,
                        "application/json"
                        );
                Console.WriteLine("test");

                // post en async de ToDo vers todoService
                await _HttpClient.PostAsync($"{_configuration["TodoService"]}", httpTodoContent);
                
            }
            
            // post en async de project vers projectService
            await _HttpClient.PostAsync($"{_configuration["ProjectService"]}", httpProjectContent);

            // partie RabbitMQ
            try
            {
                var createProjectAsyncDTO = _mapper.Map<PublishedProjectAsyncDTO>(quotePDFItem.Project);
                createProjectAsyncDTO.Event = "Project_Published";
                Console.WriteLine("Error :" + quotePDFItem.Project.Name);
                Console.WriteLine("Error :" + quotePDFItem.Project.ProjectTypeId);

                _messageBusClient.SendAnyProject(createProjectAsyncDTO);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error :" + ex.Message);
            }
            
            return Ok();
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