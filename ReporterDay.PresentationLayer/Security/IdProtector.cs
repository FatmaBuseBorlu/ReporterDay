namespace ReporterDay.PresentationLayer.Security
{
    public interface IdProtector
    {
        string Protect(int id);
        bool TryUnprotect(string protectedId, out int id);
    }
}
