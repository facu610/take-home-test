using Fundo.Applications.WebApi.Domain;
using Fundo.Applications.WebApi.Dtos;
using Fundo.Applications.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fundo.Applications.WebApi.Controllers
{
    [ApiController]
    [Route("loans")]
    public class LoanManagementController : ControllerBase
    {
        private readonly LoanService loanService;
        private readonly ILogger<LoanManagementController> logger;

        public LoanManagementController(LoanService loanService, ILogger<LoanManagementController> logger)
        {
            this.loanService = loanService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllLoans()
        {
            var loans = await loanService.GetAllLoansAsync();
            return Ok(loans);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetLoanById(int id)
        {
            var loan = await loanService.GetLoanByIdAsync(id);

            if (loan == null)
            {
                logger.LogInformation("Loan {LoanId} not found", id);
                return NotFound();
            }

            return Ok(loan);
        }

        [HttpPost("{id:int}/payment")]
        public async Task<ActionResult> RegisterPayment(int id, [FromBody] RegisterPaymentRequest request)
        {
            try
            {
                var loan = await loanService.RegisterPaymentAsync(id, request.Amount);
                logger.LogInformation("Registered payment of {Amount} for loan {LoanId}", request.Amount, id);
                return Ok(loan);
            }
            catch (KeyNotFoundException)
            {
                logger.LogWarning("Attempted payment for non-existent loan {LoanId}", id);
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, "Invalid payment request for loan {LoanId}", id);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                logger.LogWarning(ex, "Payment rejected for loan {LoanId}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateLoan([FromBody] CreateLoanRequest request)
        {
            try
            {
                Loan loan = await loanService.CreateLoanAsync(
                    request.Amount,
                    request.ApplicantName);

                logger.LogInformation("Created loan {LoanId} for applicant {ApplicantName}", loan.Id, request.ApplicantName);
                return CreatedAtAction(nameof(GetLoanById), new { id = loan.Id }, loan);

            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, "Invalid loan creation request for applicant {ApplicantName}", request.ApplicantName);
                return BadRequest(ex.Message);
            }
        }
    }
}
