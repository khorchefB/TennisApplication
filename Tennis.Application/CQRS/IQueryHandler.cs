using MediatR;

namespace Tennis.Application.CQRS;

public interface IQueryHandler<in IQuery, TResponse> : IRequestHandler<IQuery, TResponse>
    where IQuery : IQuery<TResponse>
    where TResponse : notnull
{
}
