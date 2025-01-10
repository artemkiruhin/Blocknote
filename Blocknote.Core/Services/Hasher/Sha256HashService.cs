using System.Security.Cryptography;
using System.Text;

namespace Blocknote.Core.Services.Hasher;

public class Sha256HashService : IHashService
{
    public string Compute(string message)
    {
        return Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(message)));
    }
}