using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veikka_Honkanen_LibraryApi.Models;

namespace Veikka_Honkanen_LibraryApi
{
    // Bonus 3
    /// <summary>
    /// Checks if a customer's loan is about to end and sends reminders
    /// </summary>
    public class LoanChecker
    {
        private readonly LibraryContext _context;
        public LoanChecker(LibraryContext context)
        {
            _context = context;
        }
        public void StartLoanCheckerTask()
        {
            // Multithreading the task to run in the background. Made by looking at LVBen's answer on https://stackoverflow.com/a/33713442
            var ts = new ThreadStart(CheckLoans);
            var thread = new Thread(ts);
            thread.Start();
        }

        public void CheckLoans()
        {
            var loans = _context.Loans.ToList();

            while (loans.Count > 0)
            {
                // The reminders start from a 7-day countdown, and those days need to be substracted from the due date
                var days = -7;

                for (var i = 0; days < i; days++)
                {
                    // If the time is equal (day, hour, minute, seconds...), a customer needs to be reminded. This will not repeat itself more than once per day
                    // due to DateTime.Compare returning a non-zero 
                    var dueInWeek = loans.FindAll(loan => DateTime.Compare(loan.DateDue.AddDays(days), DateTime.UtcNow) == 0);

                    if (dueInWeek.Count > 0)
                    {
                        foreach (var loan in dueInWeek)
                        {
                            var customer = _context.Customers
                                .Include(customer => customer.Person)?
                                .SingleOrDefault(customer => customer.Id == loan.CustomerId);

                            if (customer != null)
                            {
                                Console.WriteLine($"A reminder has been sent to customer {customer.Person.FirstName} {customer.Person.LastName}");
                            }
                        }
                    }

                    
                }
            }
        }
    }
}
