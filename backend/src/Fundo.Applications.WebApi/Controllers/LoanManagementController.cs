using Fundo.Applications.WebApi.Data;
using Fundo.Applications.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public LoanManagementController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllLoans()
        {
            // TODO: Only for practical purposes, data access is handled directly here. 
            // This can be refactored into a service layer for better separation of concerns.
            var loans = await dbContext.Loans
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Ok(loans);
        }
    }
}