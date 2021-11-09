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

        /// <summary>
        /// Starts loan checker background task
        /// </summary>
        public void StartLoanCheckerTask()
        {
            // Multithreading the task to run in the background. Made by looking at LVBen's answer on https://stackoverflow.com/a/33713442
            var ts = new ThreadStart(CheckLoans);
            var thread = new Thread(ts);
            thread.Start();
        }

        private void CheckLoans()
        {
            var loans = _context.Loans.ToList();

            while (loans != null) // Always true, therefore always running
            {
                // The reminders start from a 7-day countdown, and those days need to be substracted from the due date
                var days = -7;

                for (var i = 1; days < i; days++)
                {
                    // The reminder is not supposed to be sent later than the loan span expiration, so the days need to be reset, therefore never reaching number 1
                    if (days == 1)
                    {
                        days = -7;
                    }

                    // If the time is equal (day, hour, minute, seconds...), a customer needs to be reminded. This will not repeat itself more than once per day
                    // due to DateTime.Compare returning a non-zero value
                    var loansToRemindOf = loans.FindAll(loan => DateTime.Compare(loan.DateDue.AddDays(days), DateTime.UtcNow) == 0);

                    // Only executing if exact matches were found, otherwise looping from start
                    if (loansToRemindOf.Count > 0)
                    {
                        foreach (var loan in loansToRemindOf)
                        {
                            var customer = _context.Customers
                                .Include(customer => customer.Person)?
                                .SingleOrDefault(customer => customer.Id == loan.CustomerId);

                            if (customer != null && days < 0)
                            {
                                Console.WriteLine($"A reminder has been sent to customer {customer.Person.FirstName} {customer.Person.LastName}");
                            }

                            if (customer != null && days == 0)
                            {
                                Console.WriteLine($"The loan span of customer {customer.Person.FirstName} {customer.Person.LastName} has expired.");
                            }
                        }
                    }
                }
            }
        }
    }
}
