// Decompiled with JetBrains decompiler
// Type: IEntityFrameWork.IUserGlobalFilter
// Assembly: IEntityFrameWork, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BB335633-8D1B-4245-9B53-3F25C5E463BD
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\IEntityFrameWork.dll

using System;

namespace IEntityFrameWork
{
  public interface IUserGlobalFilter
  {
    int? Creat_User_ID { get; set; }

    DateTime? Create_Date { get; set; }

    int? Last_Update_User_ID { get; set; }

    DateTime? Last_Update_Date { get; set; }
  }
}
