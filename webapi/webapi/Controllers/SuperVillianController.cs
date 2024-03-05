using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data.Entities;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperVillianController : ControllerBase
    {
        private readonly ReactJSDemoContext _reactJSDemoContext;
        public SuperVillianController(ReactJSDemoContext reactJSDemoContext)
        {
            _reactJSDemoContext = reactJSDemoContext;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var villains = await _reactJSDemoContext.SuperVillain.ToListAsync();
            return Ok(villains);
        }
        [HttpPost]
        public async Task<IActionResult> Post(SuperVillain newSuperVillain)
        {
            _reactJSDemoContext.SuperVillain.Add(newSuperVillain);
            await _reactJSDemoContext.SaveChangesAsync();
            return Ok(newSuperVillain);
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var villainById = await _reactJSDemoContext
            .SuperVillain.Where(_ => _.Id == id)
            .FirstOrDefaultAsync();
            return Ok(villainById);
        }
        [HttpPut]
        public async Task<IActionResult> Put(SuperVillain villainToUpdate)
        {
            _reactJSDemoContext.SuperVillain.Update(villainToUpdate);
            await _reactJSDemoContext.SaveChangesAsync();
            return Ok(villainToUpdate);
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var villainToDelete = await _reactJSDemoContext.SuperVillain.FindAsync(id);
            if (villainToDelete == null)
            {
                return NotFound();
            }
            _reactJSDemoContext.SuperVillain.Remove(villainToDelete);
            await _reactJSDemoContext.SaveChangesAsync();
            return Ok();
        }
    }
}
