using System;
using RhinoInside.Revit.Convert.System.Drawing;
using DB = Autodesk.Revit.DB;

namespace RhinoInside.Revit.GH.Types
{
  public class Material : Element
  {
    public override string TypeName => "Revit Material";
    public override string TypeDescription => "Represents a Revit material";
    protected override Type ScriptVariableType => typeof(DB.Material);
    public static explicit operator DB.Material(Material value) => value?.Value;
    public new DB.Material Value => value as DB.Material;

    public Material() { }
    public Material(DB.Material material) : base(material) { }

    #region Identity Data
    public string MaterialClass
    {
      get => Value?.MaterialClass;
      set
      {
        if (value is object && Value?.MaterialClass != value)
          Value.MaterialClass = value;
      }
    }

    public string MaterialCategory
    {
      get => Value?.MaterialCategory;
      set
      {
        if (value is object && Value?.MaterialCategory != value)
          Value.MaterialCategory = value;
      }
    }
    #endregion

    #region Materials and finshed

    public bool? UseRenderAppearanceForShading
    {
      get => Value?.UseRenderAppearanceForShading;
      set
      {
        if (value is object && Value?.UseRenderAppearanceForShading != value)
          Value.UseRenderAppearanceForShading = value.Value;
      }
    }

    public System.Drawing.Color? Color
    {
      get
      {
        if (Value is DB.Material material)
        {
          var color = material.Color.ToColor();
          return System.Drawing.Color.FromArgb(material.Transparency * 255 / 100, color);
        }

        return default;
      }
      set
      {
        if (value is object && Color != value)
        {
          var color = value.Value.ToColor();
          Value.Color = color;
          Value.Transparency = value.Value.A * 100 / 255;
        }
      }
    }

    public double? Smoothness
    {
      get => Value?.Smoothness / 100.0;
      set
      {
        if (value is object && Value is DB.Material material && material.Smoothness / 100.0 != value.Value)
          material.Smoothness = (int) Math.Round(value.Value * 100.0);
      }
    }

#if REVIT_2019
    public Element SurfaceForegroundPattern
    {
      get => Element.FromElementId(Document, SurfaceForegroundPatternId);
      set
      {
        if (value is object && Value is DB.Material material && value.Id != material.SurfaceForegroundPatternId)
        {
          AssertValidDocument(value.Document, nameof(SurfaceForegroundPattern));
          material.SurfaceForegroundPatternId = value.Id;
        }
      }
    }

    public DB.ElementId SurfaceForegroundPatternId
    {
      get => Value?.SurfaceForegroundPatternId;
      set
      {
        if(value is object && Value is DB.Material material && value != material.SurfaceForegroundPatternId)
          material.SurfaceForegroundPatternId = value;
      }
    }

    public System.Drawing.Color? SurfaceForegroundPatternColor
    {
      get => Value?.SurfaceForegroundPatternColor.ToColor();
      set
      {
        if (value is object && Value is DB.Material material)
        {
          using (var color = value.Value.ToColor())
          {
            if (color != material.SurfaceForegroundPatternColor)
              material.SurfaceForegroundPatternColor = color;
          }
        }
      }
    }

    public Element SurfaceBackgroundPattern
    {
      get => Element.FromElementId(Document, SurfaceBackgroundPatternId);
      set
      {
        if (value is object && Value is DB.Material material && value.Id != material.SurfaceBackgroundPatternId)
        {
          AssertValidDocument(value.Document, nameof(SurfaceBackgroundPattern));
          material.SurfaceBackgroundPatternId = value.Id;
        }
      }
    }

    public DB.ElementId SurfaceBackgroundPatternId
    {
      get => Value?.SurfaceBackgroundPatternId;
      set
      {
        if (value is object && Value is DB.Material material && value != material.SurfaceBackgroundPatternId)
          material.SurfaceBackgroundPatternId = value;
      }
    }

    public System.Drawing.Color? SurfaceBackgroundPatternColor
    {
      get => Value?.SurfaceBackgroundPatternColor.ToColor();
      set
      {
        if (value is object && Value is DB.Material material)
        {
          using (var color = value.Value.ToColor())
          {
            if (color != material.SurfaceBackgroundPatternColor)
              material.SurfaceBackgroundPatternColor = color;
          }
        }
      }
    }

    public Element CutForegroundPattern
    {
      get => Element.FromElementId(Document, CutForegroundPatternId);
      set
      {
        if (value is object && Value is DB.Material material && value.Id != material.CutForegroundPatternId)
        {
          AssertValidDocument(value.Document, nameof(CutForegroundPattern));
          material.CutForegroundPatternId = value.Id;
        }
      }
    }

    public DB.ElementId CutForegroundPatternId
    {
      get => Value?.CutForegroundPatternId;
      set
      {
        if (value is object && Value is DB.Material material && value != material.CutForegroundPatternId)
          material.CutForegroundPatternId = value;
      }
    }

    public System.Drawing.Color? CutForegroundPatternColor
    {
      get => Value?.CutForegroundPatternColor.ToColor();
      set
      {
        if (value is object && Value is DB.Material material)
        {
          using (var color = value.Value.ToColor())
          {
            if (color != material.CutForegroundPatternColor)
              material.CutForegroundPatternColor = color;
          }
        }
      }
    }

    public Element CutBackgroundPattern
    {
      get => Element.FromElementId(Document, CutForegroundPatternId);
      set
      {
        if (value is object && Value is DB.Material material && value.Id != material.CutBackgroundPatternId)
        {
          AssertValidDocument(value.Document, nameof(CutBackgroundPattern));
          material.CutBackgroundPatternId = value.Id;
        }
      }
    }

    public DB.ElementId CutBackgroundPatternId
    {
      get => Value?.CutBackgroundPatternId;
      set
      {
        if (value is object && Value is DB.Material material && value != material.CutBackgroundPatternId)
          material.CutBackgroundPatternId = value;
      }
    }

    public System.Drawing.Color? CutBackgroundPatternColor
    {
      get => Value?.CutBackgroundPatternColor.ToColor();
      set
      {
        if (value is object && Value is DB.Material material)
        {
          using (var color = value.Value.ToColor())
          {
            if (color != material.CutBackgroundPatternColor)
              material.CutBackgroundPatternColor = color;
          }
        }
      }
    }
#else
    public Element SurfaceForegroundPattern
    {
      get => Element.FromElementId(Document, SurfaceForegroundPatternId);
      set
      {
        if (value is object && Value is DB.Material material && value.Id != material.SurfacePatternId)
        {
          AssertValidDocument(value.Document, nameof(SurfaceForegroundPattern));
          material.SurfacePatternId = value.Id;
        }
      }
    }

    public DB.ElementId SurfaceForegroundPatternId
    {
      get => Value?.SurfacePatternId;
      set
      {
        if (value is object && Value is DB.Material material && value != material.SurfacePatternId)
          material.SurfacePatternId = value;
      }
    }

    public System.Drawing.Color? SurfaceForegroundPatternColor
    {
      get => Value?.SurfacePatternColor.ToColor();
      set
      {
        if (value is object && Value is DB.Material material)
        {
          using (var color = value.Value.ToColor())
          {
            if (color != material.SurfacePatternColor)
              material.SurfacePatternColor = color;
          }
        }
      }
    }

    public Element SurfaceBackgroundPattern
    {
      get => default;
      set { }
    }

    public DB.ElementId SurfaceBackgroundPatternId
    {
      get => default;
      set { }
    }

    public System.Drawing.Color? SurfaceBackgroundPatternColor
    {
      get => default;
      set { }
    }

    public Element CutForegroundPattern
    {
      get => Element.FromElementId(Document, CutForegroundPatternId);
      set
      {
        if (value is object && Value is DB.Material material && value.Id != material.CutPatternId)
        {
          AssertValidDocument(value.Document, nameof(CutForegroundPattern));
          material.CutPatternId = value.Id;
        }
      }
    }

    public DB.ElementId CutForegroundPatternId
    {
      get => Value?.CutPatternId;
      set
      {
        if (value is object && Value is DB.Material material && value != material.CutPatternId)
          material.CutPatternId = value;
      }
    }

    public System.Drawing.Color? CutForegroundPatternColor
    {
      get => Value?.CutPatternColor.ToColor();
      set
      {
        if (value is object && Value is DB.Material material)
        {
          using (var color = value.Value.ToColor())
          {
            if (color != material.CutPatternColor)
              material.CutPatternColor = color;
          }
        }
      }
    }

    public Element CutBackgroundPattern
    {
      get => default;
      set { }
    }

    public DB.ElementId CutBackgroundPatternId
    {
      get => default;
      set { }
    }

    public System.Drawing.Color? CutBackgroundPatternColor
    {
      get => default;
      set { }
    }
#endif
    #endregion
  }
}
