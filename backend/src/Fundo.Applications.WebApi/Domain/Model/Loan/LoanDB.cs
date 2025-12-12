namespace Fundo.Applications.WebApi.Domain.Model.Loan
{
    public class LoanDB : ILoanDB
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal CurrentBalance { get; set; }
        public string AplicantName { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
