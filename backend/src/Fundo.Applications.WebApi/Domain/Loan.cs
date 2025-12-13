using System;

namespace Fundo.Applications.WebApi.Domain
{
    public class Loan
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal CurrentBalance { get; set; }
        public string ApplicantName { get; set; }
        public LoanStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public enum LoanStatus
    {
        Active, 
        Paid
    }
}
