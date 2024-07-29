// Decompiled with JetBrains decompiler
// Type: SimpleHashing.ISimpleHash
// Assembly: SimpleHashing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 639922A4-EA5B-4859-AB83-4294D70FAA03
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\SimpleHashing.dll

using System;

namespace SimpleHashing
{
  public interface ISimpleHash
  {
    string Compute(string password);

    string Compute(string password, int iterations);

    bool Verify(string password, string passwordHashString);

    TimeSpan Estimate(string password, int iterations);
  }
}
