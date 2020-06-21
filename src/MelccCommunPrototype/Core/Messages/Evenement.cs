using MediatR;
using System;

namespace Env.Commun.Core
{
    public class Evenement : Message, INotification
    {
        public DateTime Timestamp { get; private set; }

        protected Evenement()
        {
            Timestamp = DateTime.Now;
        }
    }
}