using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veikka_Honkanen_LibraryApi.Models;

namespace Veikka_Honkanen_LibraryApi
{
    // Bonus 3
    // The background task structure was based on Microsoft's example at:
    // https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/fundamentals/host/hosted-services/samples/3.x/BackgroundTasksSample/Services/MonitorLoop.cs
    public class LoanChecker
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger _logger;
        private readonly CancellationToken _cancellationToken;
        private LibraryContext _context;

        /// <summary>
        /// Checks if a customer's loan is about to end and sends reminders
        /// </summary>
        /// <param name="taskQueue">task queue</param>
        /// <param name="logger">logger</param>
        /// <param name="applicationLifetime">application lifetime</param>
        public LoanChecker(IBackgroundTaskQueue taskQueue, ILogger<LoanChecker> logger, IHostApplicationLifetime applicationLifetime)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _cancellationToken = applicationLifetime.ApplicationStopping;
        }

        /// <summary>
        /// Starts loan checker background task
        /// </summary>
        /// <param name="context">Library context</param>
        public async void StartLoanCheckerTask(LibraryContext context)
        {
            _context = context;

            while (!_cancellationToken.IsCancellationRequested)
            {
                await _taskQueue.QueueBackgroundWorkItemAsync(CheckLoansTask);
            }
        }

        /// <summary>
        /// Check loan task that is run in the background
        /// </summary>
        /// <param name="token">cancellation token</param>
        /// <returns>void</returns>
        private async ValueTask CheckLoansTask(CancellationToken token)
        {
            var loans = _context.Loans.ToList();

            while (!token.IsCancellationRequested)
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
                                _logger.LogInformation($"A reminder has been sent to customer {customer.Person.FirstName} {customer.Person.LastName}");
                            }

                            if (customer != null && days == 0)
                            {
                                _logger.LogInformation($"The loan span of customer {customer.Person.FirstName} {customer.Person.LastName} has expired.");
                            }
                        }
                    }
                }
            }
        }
    }
}
