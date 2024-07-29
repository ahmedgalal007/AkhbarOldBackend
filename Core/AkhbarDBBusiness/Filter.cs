// Decompiled with JetBrains decompiler
// Type: Akhbar.DBBusiness.Filter
// Assembly: AkhbarDBBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 447873D6-3586-48DC-A4C8-11855DFF0A7A
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBBusiness.dll

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Text;

namespace Akhbar.DBBusiness
{
  internal class Filter
  {
    public static string FilterProperties<T>(
      T entity,
      string prefix,
      ref List<ObjectParameter> lstObjectParameter)
    {
      if ((object) entity == null)
        return "";
      Type type = entity.GetType();
      int length = type.GetProperties().Length;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < length; ++index)
      {
        if (!type.GetProperties()[index].GetMethod.IsVirtual && type.GetProperties()[index].GetValue((object) entity) != null)
        {
          stringBuilder.Append(prefix + "." + type.GetProperties()[index].Name);
          if (type.GetProperties()[index].GetMethod.ReturnType.Name == "String")
          {
            ObjectParameter objectParameter = new ObjectParameter(prefix + "_" + type.GetProperties()[index].Name, type.GetProperties()[index].GetValue((object) entity));
            stringBuilder.Append(" like ");
            stringBuilder.Append("@" + prefix + "_" + type.GetProperties()[index].Name);
            stringBuilder.Append(" ");
            lstObjectParameter.Add(objectParameter);
          }
          else
          {
            ObjectParameter objectParameter = new ObjectParameter(prefix + "_" + type.GetProperties()[index].Name, type.GetProperties()[index].GetValue((object) entity));
            stringBuilder.Append(" = ");
            stringBuilder.Append("@" + prefix + "_" + type.GetProperties()[index].Name);
            stringBuilder.Append(" ");
            lstObjectParameter.Add(objectParameter);
          }
          stringBuilder.Append(" And ");
        }
      }
      return stringBuilder.ToString();
    }
  }
}
