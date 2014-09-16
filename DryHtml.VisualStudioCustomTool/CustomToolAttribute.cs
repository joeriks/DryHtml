/*
 * Copyright (C) 2006 Chris Stefano
 *       cnjs@mweb.co.za
 */
namespace TransformCodeGenerator
{
  using System;
  using System.Runtime.InteropServices;

  /// <summary>
  /// 
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
  public class CustomToolAttribute: Attribute
  {

    /// <summary></summary>
    protected string _name;

    /// <summary></summary>
    protected string _description;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    public CustomToolAttribute(string name):
      this(name, "")
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    public CustomToolAttribute(string name, string description)
    {
      this._name = name;
      this._description = description;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Name
    {
      get
      {
        return this._name;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public string Description
    {
      get
      {
        return this._description;
      }
    }

  }
}
