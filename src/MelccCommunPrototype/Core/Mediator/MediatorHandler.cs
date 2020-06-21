using FluentValidation.Results;
using MediatR;
using System.Threading.Tasks;

namespace Env.Commun.Core
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ValidationResult> SoumettreCommand<T>(T comando) where T : Command
        {
            return await _mediator.Send(comando);
        }

        public async Task PublierEvenement<T>(T evento) where T : Evenement
        {
            await _mediator.Publish(evento);
        }
    }
}