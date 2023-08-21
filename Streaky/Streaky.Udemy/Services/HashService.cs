using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Streaky.Udemy.DTOs;

namespace Streaky.Udemy.Services;

public class HashService
{
    public ResultHash Hash(string textPlain)
    {
        var sal = new byte[16];
        using (var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(sal);
        }
        return Hash(textPlain, sal);
    }

    private ResultHash Hash(string textPlain, byte[] sal)
    {
        var derivatedKey = KeyDerivation.Pbkdf2(password: textPlain,
            salt: sal, prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 32);

        var hash = Convert.ToBase64String(derivatedKey);

        return new ResultHash()
        {
            Hash = hash,
            Sal = sal
        };
    }
}

