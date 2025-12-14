using Fundo.Applications.WebApi.Data;
using Fundo.Applications.WebApi.Domain;
using Fundo.Applications.WebApi.Dtos;
using Fundo.Applications.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fundo.Applications.WebApi.Controllers
{
    [ApiController]
    [Route("loans")]
    public class LoanManagementController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly LoanService loanService;

        public LoanManagementController(AppDbContext dbContext, LoanService loanService)
        {
            this.dbContext = dbContext;
            this.loanService = loanService;
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

            if (loan == null) return NotFound();

            return Ok(loan);
        }

        [HttpPost("{id:int}/payment")]
        public async Task<ActionResult> RegisterPayment(int id, [FromBody] RegisterPaymentRequest request)
        {
            try
            {
                var loan = await loanService.RegisterPaymentAsync(id, request.Amount);
                return Ok(loan);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
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

                return CreatedAtAction(nameof(GetLoanById), new { id = loan.Id }, loan);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}