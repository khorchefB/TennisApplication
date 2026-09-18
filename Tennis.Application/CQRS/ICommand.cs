using MediatR;

namespace Tennis.Application.CQRS;

public interface ICommand : ICommand<Unit>
{

}

public interface ICommand<out Response> : IRequest<Response>
{
}