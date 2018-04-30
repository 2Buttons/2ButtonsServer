namespace AccountServer.Claims
{
    //public class ModeratorAuthorizationHandler : AuthorizationHandler<ModeratorRequirement, Question>
    //{
    //    private readonly ILogger<ModeratorAuthorizationHandler> _logger;
    //    private readonly IOptions<AuthOptions> _settings;

    //    public ModeratorAuthorizationHandler(ILogger<ModeratorAuthorizationHandler> logger, IOptions<AuthOptions> settings)
    //    {
    //        _logger = logger;
    //        _settings = settings;
    //    }

    //    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ModeratorRequirement requirement, Question resource)
    //    {
    //        if (!context.User.HasClaim(c => c.Type == "UserId" &&
    //                                        c.Issuer == _settings.Value.Issuer))
    //        {
    //            return Task.CompletedTask;
    //        }

    //        if (resource.OwnerId == context.User.FindFirst("UserId").Value)
    //        {
    //            context.Succeed(requirement);
    //        }

    //        return Task.CompletedTask;
    //    }
    //}
}
