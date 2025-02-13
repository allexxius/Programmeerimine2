using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;


namespace KooliProjekt.Controllers
{
    [Route("api/Documents")]
    [ApiController]
    public class DocumentsApiController : ControllerBase
    {
        private readonly IDocumentService _service;

        public DocumentsApiController(IDocumentService service)
        {
            _service = service;
        }

        // GET: api/<DocumentsApiController>
        [HttpGet]
        public async Task<IEnumerable<Document>> Get()
        {
            var result = await _service.List(1, 10000);
            return result.Results;
        }

        // GET api/<DocumentsApiController>/5
        [HttpGet("{id}")]
        public async Task<object> Get(int id)
        {
            var list = await _service.Get(id);
            if (list == null)
            {
                return NotFound();
            }

            return list;
        }

        // POST api/<DocumentsApiController>
        [HttpPost]
        public async Task<object> Post([FromBody] Document list)
        {
            await _service.Save(list);

            return Ok(list);
        }

        // PUT api/<DocumentsApiController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Document list)
        {
            if (id != list.ID)
            {
                return BadRequest();
            }

            await _service.Save(list);

            return Ok();
        }

        // DELETE api/<DocumentsApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var list = await _service.Get(id);
            if (list == null)
            {
                return NotFound();
            }

            await _service.Delete(id);

            return Ok();
        }
    }
}
