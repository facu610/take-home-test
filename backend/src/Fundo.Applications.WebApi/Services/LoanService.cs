using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fundo.Applications.WebApi.Data;
using Fundo.Applications.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Fundo.Applications.WebApi.Services
{
    public class LoanService
    {
        private readonly AppDbContext dbContext;
        public LoanService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Loan>> GetAllLoansAsync()
        {
            return await dbContext.Loans
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<Loan> GetLoanByIdAsync(int id)
        {
            return await dbContext.Loans
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Loan> RegisterPaymentAsync(int loanId, decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Payment amount must be greater than zero.");
            }

            Loan loan = await dbContext.Loans
                .FirstOrDefaultAsync(x => x.Id == loanId);

            if (loan == null)
            {
                throw new KeyNotFoundException("Loan not found.");
            }

            if (loan.Status == LoanStatus.Paid)
            {
                throw new InvalidOperationException("Loan is already fully paid.");
            }

            if (amount > loan.CurrentBalance)
            {
                throw new InvalidOperationException("Payment amount exceeds current balance.");
            }

            loan.CurrentBalance -= amount;

            if (loan.CurrentBalance == 0)
            {
                loan.CurrentBalance = 0;
                loan.Status = LoanStatus.Paid;
            }

            await dbContext.SaveChangesAsync();
            return loan;
        }

        public async Task<Loan> CreateLoanAsync(decimal amount, string applicantName)
        {
            if(amount <= 0)
            {
                throw new ArgumentException("Loan amount must be greater than zero.");
            }

            if(string.IsNullOrWhiteSpace(applicantName))
            {
                throw new ArgumentException("Applicant name is required.");
            }
            Loan loan = new()
            {
                Amount = amount,
                CurrentBalance = amount,
                ApplicantName = applicantName,
                Status = LoanStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            dbContext.Loans.Add(loan);
            await dbContext.SaveChangesAsync();
            
            return loan;
        }
    }
}