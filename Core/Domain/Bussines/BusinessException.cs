// Decompiled with JetBrains decompiler
// Type: Akhbar.DBBusiness.BusinessException
// Assembly: AkhbarDBBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 447873D6-3586-48DC-A4C8-11855DFF0A7A
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBBusiness.dll

using System;

namespace Domain.Akhbar.DBBusiness
{
  [Serializable]
  public class BusinessException : Exception
  {
    public BusinessException()
    {
    }

    public BusinessException(string message)
      : base(message)
    {
    }

    public BusinessException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
