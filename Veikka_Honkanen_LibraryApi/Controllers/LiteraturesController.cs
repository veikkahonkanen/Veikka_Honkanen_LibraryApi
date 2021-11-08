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
    public class LiteraturesController : ControllerBase
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;

        public LiteraturesController(LibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Literatures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LiteratureDtoOut>>> GetLiteratures()
        {
            return await _context.Literatures
                .Include(literature => literature.Authors).ThenInclude(author => author.Person)
                .Include(literature => literature.Publisher)
                .Include(literature => literature.Subjects)
                .ProjectTo<LiteratureDtoOut>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // GET: api/Literatures/SearchForLiterature
        [HttpGet("/SearchForLiterature")]
        // Params alignment was done using Visual Studio's tools, as Microsoft instructed here:
        // https://docs.microsoft.com/en-us/visualstudio/ide/reference/wrap-indent-align-refactorings?view=vs-2019#wrap-indent-and-align-parameters-or-arguments
        public async Task<ActionResult<IEnumerable<LiteratureDtoOut>>> SearchForLiterature(string? title,
                                                                                           string? subjectName,
                                                                                           string? publisherName,
                                                                                           string? manufacturerName,
                                                                                           long? yearOfRelease)
        {
            // The dbcontext query filtering implementation was inspired by looking at Tim's answer, https://stackoverflow.com/a/32412831
            var literatures = await _context.Literatures
                .Where(literature =>
                (string.IsNullOrEmpty(title) || literature.Title == title)
                && (string.IsNullOrEmpty(subjectName) || literature.Subjects.Any(subject => subject.Name == subjectName))
                && (string.IsNullOrEmpty(publisherName) || literature.Publisher.Name == publisherName)
                && (string.IsNullOrEmpty(manufacturerName) || literature.Manufacturer == manufacturerName)
                && (yearOfRelease == null || literature.YearOfRelease == yearOfRelease))
                .Include(literature => literature.Authors).ThenInclude(author => author.Person)
                .Include(literature => literature.Publisher)
                .Include(literature => literature.Subjects)
                .ProjectTo<LiteratureDtoOut>(_mapper.ConfigurationProvider)
                .ToListAsync();

            if (literatures == null)
            {
                return NotFound();
            }

            return literatures;
        }

        // GET: api/Literatures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LiteratureDtoOut>> GetLiterature(long id)
        {
            var literature = await _context.Literatures
                .Include(literature => literature.Authors).ThenInclude(author => author.Person)
                .Include(literature => literature.Publisher)
                .Include(literature => literature.Subjects)
                .ProjectTo<LiteratureDtoOut>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(literature => literature.Id == id);

            if (literature == null)
            {
                return NotFound();
            }

            return literature;
        }

        // PUT: api/Literatures/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLiterature(long id, LiteratureDtoIn literature)
        {
            if (id != literature.Id)
            {
                return BadRequest();
            }

            try
            {
                var literatureEntity = await _context.Literatures.FindAsync(id);

                literatureEntity = _mapper.Map(literature, literatureEntity);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiteratureExists(id))
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

        // Bonus 1
        // POST: api/Literatures/BorrowLiterature/{literatureId}/{customerId}
        [HttpPost("/BorrowLiterature/{literatureId}/{customerId}")]
        public async Task<IActionResult> BorrowLiterature(long literatureId, long customerId)
        {
            var literatureEntity = await _context.Literatures.SingleOrDefaultAsync(literature => literature.Id == literatureId);

            var customerEntity = await _context.Customers
                .Include(customer => customer.Person)?
                .Include(customer => customer.Loans)?
                .SingleOrDefaultAsync(customer => customer.Id == customerId);

            if (literatureEntity == null || customerEntity == null)
            {
                return StatusCode(404, "Customer or literature not found.");
            }

            if (customerEntity.IsLoanBanned)
            {
                
                return StatusCode(403, "Customer is banned from borrowing.");
            }

            if (literatureEntity.AvailableCopies == 0)
            {

                return StatusCode(403, "There are no available copies of this literature left.");
            }

            if (customerEntity.Loans.Any(loan => DateTime.Compare(loan.DateDue, DateTime.UtcNow) > 0))
            {

                return StatusCode(403, "Customer has loans due.");
            }

            var loan = new Loan()
            {
                LiteratureId = literatureId,
                CustomerId = customerId,
                DateBorrowed = DateTime.UtcNow,
                DateDue = DateTime.UtcNow.AddMonths(1)
            };

            literatureEntity.AvailableCopies--;

            customerEntity.Loans.Add(loan);

            _context.Loans.Add(loan);

            await _context.SaveChangesAsync();

            return StatusCode(200, $"{customerEntity.Person.FirstName} {customerEntity.Person.LastName} has successfully borrowed {literatureEntity.Title}. " +
                $"The due date of the loan is {loan.DateDue} UTC."); ;
        }

        // Bonus 1
        // POST: api/Literatures/ReturnLoanOfLiterature/{literatureId}/{customerId}
        [HttpPost("/ReturnLoanOfLiterature/{loanId}")]
        public async Task<IActionResult> ReturnLoanOfLiterature(long loanId)
        {
            var loan = await _context.Loans.SingleOrDefaultAsync(loan => loan.Id == loanId);

            if (loan == null)
            {
                return StatusCode(404, "Loan not found.");
            }

            var literatureEntity = await _context.Literatures.SingleOrDefaultAsync(literature => literature.Id == loan.LiteratureId);

            var customerEntity = await _context.Customers
                .Include(customer => customer.Person)?
                .Include(customer => customer.Loans)?
                .SingleOrDefaultAsync(customer => customer.Id == loan.CustomerId);

            if (literatureEntity == null || customerEntity == null)
            {
                return StatusCode(404, "Customer or literature not found.");
            }

            literatureEntity.AvailableCopies++;

            customerEntity.Loans.Remove(loan);

            _context.Loans.Remove(loan);

            await _context.SaveChangesAsync();

            return StatusCode(200, "Loan of literature has been succesfully returned.");
        }

        // POST: api/Literatures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Literature>> PostLiterature(LiteratureDtoIn literature)
        {
            var entityLiterature = _mapper.Map<Literature>(literature);

            _context.Literatures.Add(entityLiterature);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLiterature", new { id = entityLiterature.Id }, literature);
        }

        // DELETE: api/Literatures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLiterature(long id)
        {
            var literature = await _context.Literatures.FindAsync(id);
            if (literature == null)
            {
                return NotFound();
            }

            _context.Literatures.Remove(literature);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LiteratureExists(long id)
        {
            return _context.Literatures.Any(e => e.Id == id);
        }
    }
}
