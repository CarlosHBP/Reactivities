using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activitiy = await _context.Activities.FindAsync(request.Id);

                if (activitiy == null)
                    throw new Exception("Could not find activity");

                _context.Remove(activitiy);

                //Save retorna a qtd alterada
                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value; //Retorna 200 (ok)

                throw new Exception("Problem saving changes");
            }
        }

    }
}