using FluentValidation.Results;
using MediatR;
using System;

namespace Env.Commun.Core
{
    public abstract class Command : Message, IRequest<ValidationResult>
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        public virtual bool EstValide()
        {
            throw new NotImplementedException();
        }
    }
}