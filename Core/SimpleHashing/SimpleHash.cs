// Decompiled with JetBrains decompiler
// Type: SimpleHashing.SimpleHash
// Assembly: SimpleHashing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 639922A4-EA5B-4859-AB83-4294D70FAA03
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\SimpleHashing.dll

using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;

namespace SimpleHashing
{
  public class SimpleHash : ISimpleHash
  {
    private int m_SaltSize = 16;
    private int m_HashSize = 32;
    private int m_Iterations = 50000;

    public string Compute(string password) => this.Compute(password, this.m_Iterations);

    public string Compute(string password, int iterations)
    {
      using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, this.m_SaltSize, iterations))
        return this.CreateHashString(rfc2898DeriveBytes.GetBytes(this.m_HashSize), rfc2898DeriveBytes.Salt, iterations);
    }

    private string ComputeHash(string password, string salt, int iterations, int hashSize)
    {
      byte[] salt1 = Convert.FromBase64String(salt);
      using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt1, iterations))
        return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(hashSize));
    }

    public bool Verify(string password, string passwordHashString)
    {
      SimpleHashParameters simpleHashParameters = new SimpleHashParameters(passwordHashString);
      int length = Convert.FromBase64String(simpleHashParameters.PasswordHash).Length;
      string hash = this.ComputeHash(password, simpleHashParameters.Salt, simpleHashParameters.Iterations, length);
      return simpleHashParameters.PasswordHash == hash;
    }

    private string CreateHashString(byte[] hash, byte[] salt, int iterations)
    {
      string base64String1 = Convert.ToBase64String(salt);
      string base64String2 = Convert.ToBase64String(hash);
      return string.Join('$'.ToString(), new string[4]
      {
        "Rfc2898DeriveBytes",
        iterations.ToString((IFormatProvider) CultureInfo.InvariantCulture),
        base64String1,
        base64String2
      });
    }

    public TimeSpan Estimate(string password, int iterations)
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      this.Compute(password, iterations);
      stopwatch.Stop();
      return stopwatch.Elapsed;
    }
  }
}
