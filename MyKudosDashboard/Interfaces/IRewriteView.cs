namespace MyKudosDashboard.Interfaces;

public interface IRewriteView
{
    Task<string> Rewrite(string message);
}
