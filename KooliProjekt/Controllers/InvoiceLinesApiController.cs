using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;


namespace KooliProjekt.Controllers
{
    [Route("api/InvoiceLines")]
    [ApiController]
    public class InvoiceLinesApiController : ControllerBase
    {
        private readonly IInvoiceLineService _service;

        public InvoiceLinesApiController(IInvoiceLineService service)
        {
            _service = service;
        }

        // GET: api/<InvoiceLinesApiController>
        [HttpGet]
        public async Task<IEnumerable<InvoiceLine>> Get()
        {
            var result = await _service.List(1, 10000);
            return result.Results;
        }

        // GET api/<InvoiceLinesApiController>/5
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

        // POST api/<InvoiceLinesApiController>
        [HttpPost]
        public async Task<object> Post([FromBody] InvoiceLine list)
        {
            await _service.Save(list);

            return Ok(list);
        }

        // PUT api/<InvoiceLinesApiController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] InvoiceLine list)
        {
            if (id != list.Id)
            {
                return BadRequest();
            }

            await _service.Save(list);

            return Ok();
        }

        // DELETE api/<InvoiceLinesApiController>/5
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
