namespace Blocknote.Core.Services.Hasher;

public interface IHashService
{
    string Compute(string message);
}