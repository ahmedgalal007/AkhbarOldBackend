// Decompiled with JetBrains decompiler
// Type: SimpleHashing.SimpleHashParameters
// Assembly: SimpleHashing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 639922A4-EA5B-4859-AB83-4294D70FAA03
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\SimpleHashing.dll

using System;

namespace SimpleHashing
{
  public class SimpleHashParameters
  {
    public string Algorithm { get; private set; }

    public int Iterations { get; private set; }

    public string Salt { get; private set; }

    public string PasswordHash { get; private set; }

    public SimpleHashParameters(string passwordHashString) => this.ProcessParameters(SimpleHashParameters.ParseParameters(passwordHashString));

    private static string[] ParseParameters(string passwordHashString)
    {
      string[] strArray = passwordHashString.Split('$');
      return strArray.Length == 4 ? strArray : throw new ArgumentException("Invalid password hash string format", nameof (passwordHashString));
    }

    private void ProcessParameters(string[] parameters)
    {
      this.Algorithm = parameters[0];
      this.Iterations = int.Parse(parameters[1]);
      this.Salt = parameters[2];
      this.PasswordHash = parameters[3];
    }
  }
}
