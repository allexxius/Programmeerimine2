using KooliProjekt.Data;

using KooliProjekt.Services;

using Microsoft.AspNetCore.Mvc;


namespace KooliProjekt.Controllers

{

    [Route("api/Times")]

    [ApiController]

    public class TimesApiController : ControllerBase

    {

        private readonly ITimeService _service;

        public TimesApiController(ITimeService service)

        {

            _service = service;

        }

        // GET: api/<TimesApiController>

        [HttpGet]

        public async Task<IEnumerable<Time>> Get()

        {

            var result = await _service.List(1, 10000);

            return result.Results;

        }

        // GET api/<TimesApiController>/5

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

        // POST api/<TimesApiController>

        [HttpPost]

        public async Task<object> Post([FromBody] Time list)

        {

            await _service.Save(list);

            return Ok(list);

        }

        // PUT api/<TimesApiController>/5

        [HttpPut("{id}")]

        public async Task<IActionResult> Put(int id, [FromBody] Time list)

        {

            if (id != list.Id)

            {

                return BadRequest();

            }

            await _service.Save(list);

            return Ok();

        }

        // DELETE api/<TimesApiController>/5

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

