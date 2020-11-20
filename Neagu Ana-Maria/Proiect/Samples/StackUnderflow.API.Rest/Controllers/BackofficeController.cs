using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access.Primitives.Extensions.ObjectExtensions;
using Access.Primitives.IO;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Domain.Core;
using StackUnderflow.Domain.Core.Contexts;
using StackUnderflow.Domain.Schema.Backoffice.CreateTenantOp;
using StackUnderflow.EF.Models;
using Access.Primitives.EFCore;
using StackUnderflow.Domain.Schema.Backoffice.InviteTenantAdminOp;
using StackUnderflow.Domain.Schema.Backoffice;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Orleans;
using GrainInterfaces;

namespace StackUnderflow.API.Rest.Controllers
{
    [ApiController]
    [Route("backoffice")]
    public class BackofficeController : ControllerBase
    {
        private readonly IInterpreterAsync _interpreter;
        private readonly StackUnderflowContext _dbContext;
        private readonly IClusterClient _client;

        public BackofficeController(IInterpreterAsync interpreter, StackUnderflowContext dbContext, IClusterClient client)
        {
            _interpreter = interpreter;
            _dbContext = dbContext;
            _client = client;
        }

        [HttpPost("tenant")]
        public async Task<IActionResult> CreateTenantAsyncAndInviteAdmin([FromBody] CreateTenantCmd createTenantCmd)
        {
            BackofficeWriteContext ctx = new BackofficeWriteContext(
                new EFList<Tenant>(_dbContext.Tenant),
                new EFList<TenantUser>(_dbContext.TenantUser),
                new EFList<User>(_dbContext.User));

            var dependencies = new BackofficeDependencies();
            dependencies.GenerateInvitationToken = () => Guid.NewGuid().ToString();
            dependencies.SendInvitationEmail = SendEmail;

            var expr = from createTenantResult in BackofficeDomain.CreateTenant(createTenantCmd)
                       let adminUser = createTenantResult.SafeCast<CreateTenantResult.TenantCreated>().Select(p => p.AdminUser)
                       let inviteAdminCmd = new InviteTenantAdminCmd(adminUser)
                       from inviteAdminResult in BackofficeDomain.InviteTenantAdmin(inviteAdminCmd)
                       select new { createTenantResult, inviteAdminResult };

            var r = await _interpreter.Interpret(expr, ctx, dependencies);
            _dbContext.SaveChanges();
            return r.createTenantResult.Match(
                created => (IActionResult)Ok(created.Tenant.TenantId),
                notCreated => StatusCode(StatusCodes.Status500InternalServerError, "Tenant could not be created."),//todo return 500 (),
            invalidRequest => BadRequest("Invalid request."));
        }

        private TryAsync<InvitationAcknowledgement> SendEmail(InvitationLetter letter)
        => async () =>
        {
            var emialSender = _client.GetGrain<IEmailSender>(0);
            await emialSender.SendEmailAsync(letter.Letter);
            return new InvitationAcknowledgement(Guid.NewGuid().ToString());
        };
    }
}
