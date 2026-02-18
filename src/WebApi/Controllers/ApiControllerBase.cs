using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
public abstract class ApiControllerBase(IMediator mediator) : ControllerBase
{
    protected readonly IMediator Mediator = mediator;
}

