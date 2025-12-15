namespace Fundo.Applications.WebApi.Dtos
{
    public class CreateLoanRequest
    {
        public decimal Amount { get; set; }
        public string ApplicantName { get; set; }
    }
}