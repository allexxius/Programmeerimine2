using KooliProjekt.Data;

using KooliProjekt.Services;

using Microsoft.AspNetCore.Mvc;


namespace KooliProjekt.Controllers

{

    [Route("api/Invoices")]

    [ApiController]

    public class InvoicesApiController : ControllerBase

    {

        private readonly IInvoiceService _service;

        public InvoicesApiController(IInvoiceService service)

        {

            _service = service;

        }

        // GET: api/<InvoicesApiController>

        [HttpGet]

        public async Task<IEnumerable<Invoice>> Get()

        {

            var result = await _service.List(1, 10000);

            return result.Results;

        }

        // GET api/<InvoicesApiController>/5

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

        // POST api/<InvoicesApiController>

        [HttpPost]

        public async Task<object> Post([FromBody] Invoice list)

        {

            await _service.Save(list);

            return Ok(list);

        }

        // PUT api/<InvoicesApiController>/5

        [HttpPut("{id}")]

        public async Task<IActionResult> Put(int id, [FromBody] Invoice list)

        {

            if (id != list.Id)

            {

                return BadRequest();

            }

            await _service.Save(list);

            return Ok();

        }

        // DELETE api/<InvoicesApiController>/5

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

