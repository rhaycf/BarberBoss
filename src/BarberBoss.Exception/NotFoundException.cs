using BarberBoss.Exception.ExceptionsBase;
using System.Net;

namespace BarberBoss.Exception;

public class NotFoundException : BarberBossException
{
    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public NotFoundException(string message) : base(message) { }

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
