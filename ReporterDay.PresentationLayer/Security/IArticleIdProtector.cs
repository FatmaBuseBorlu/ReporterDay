namespace ReporterDay.PresentationLayer.Security
{
    public interface IArticleIdProtector
    {
        string Protect(int id);
        int Unprotect(string protectedId);
        bool TryUnprotect(string protectedId, out int id);
    }
}
