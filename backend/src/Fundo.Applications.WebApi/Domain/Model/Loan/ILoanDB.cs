namespace Fundo.Applications.WebApi.Domain.Model.Loan
{
    public interface ILoanDB
    {
        int Id { get; set; }
        decimal Amount { get; set; }
        decimal CurrentBalance { get; set; }
        string AplicantName { get; set; }
        string Status { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
