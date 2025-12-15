using System.Linq;
using Fundo.Applications.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Fundo.Applications.WebApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext dbContext)
        {
            dbContext.Database.Migrate();

            if (dbContext.Loans.Any())
            {
                return;   
            }   

            var loans = new Loan[]
            {
                new Loan { ApplicantName = "Bob Marley Johnson", Amount = 15000, CurrentBalance = 1000, Status = LoanStatus.Active, CreatedAt = System.DateTime.Now },
                new Loan { ApplicantName = "Bob Smith", Amount = 20000, CurrentBalance = 0, Status = LoanStatus.Paid, CreatedAt = System.DateTime.Now },
                new Loan { ApplicantName = "Charlie Brown", Amount = 2000, CurrentBalance = 1980, Status = LoanStatus.Active, CreatedAt = System.DateTime.Now }
            };

            dbContext.Loans.AddRange(loans);
            dbContext.SaveChanges();
        }
    }
}