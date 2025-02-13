using KooliProjekt.Data;

using KooliProjekt.Services;

using Microsoft.AspNetCore.Mvc;



namespace KooliProjekt.Controllers

{

    [Route("api/Doctors")]

    [ApiController]

    public class DoctorsApiController : ControllerBase

    {

        private readonly IDoctorService _service;

        public DoctorsApiController(IDoctorService service)

        {

            _service = service;

        }

        // GET: api/<DoctorsApiController>

        [HttpGet]

        public async Task<IEnumerable<Doctor>> Get()

        {

            var result = await _service.List(1, 10000);

            return result.Results;

        }

        // GET api/<DoctorsApiController>/5

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

        // POST api/<DoctorsApiController>

        [HttpPost]

        public async Task<object> Post([FromBody] Doctor list)

        {

            await _service.Save(list);

            return Ok(list);

        }

        // PUT api/<DoctorsApiController>/5

        [HttpPut("{id}")]

        public async Task<IActionResult> Put(int id, [FromBody] Doctor list)

        {

            if (id != list.Id)

            {

                return BadRequest();

            }

            await _service.Save(list);

            return Ok();

        }

        // DELETE api/<DoctorssApiController>/5

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

