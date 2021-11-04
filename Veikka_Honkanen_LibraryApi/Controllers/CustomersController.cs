using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veikka_Honkanen_LibraryApi.DataTransferObjects.Incoming;
using Veikka_Honkanen_LibraryApi.DataTransferObjects.Outgoing;
using Veikka_Honkanen_LibraryApi.Models;

namespace Veikka_Honkanen_LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;
        public CustomersController(LibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDtoOut>>> GetCustomers()
        {
            return await _context.Customers
                .Include(customer => customer.Person)?
                .ProjectTo<CustomerDtoOut>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDtoOut>> GetCustomer(long id)
        {
            var customer = await _context.Customers
                .Include(customer => customer.Person)?
                .Include(customer => customer.Loans)?
                .ProjectTo<CustomerDtoOut>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(customer => customer.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(long id, CustomerDtoIn customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            try
            {
                var customerEntity = await _context.Customers.FindAsync(id);

                customerEntity = _mapper.Map(customer, customerEntity);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(CustomerDtoIn customer)
        {
            var entityCustomer = _mapper.Map<Customer>(customer);

            _context.Customers.Add(entityCustomer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = entityCustomer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(long id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(long id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
