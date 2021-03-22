using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace RhinoInside.Revit.External.DB.Extensions
{
  public static class GeometryObjectExtension
  {
    public static IEnumerable<GeometryObject> ToDirectShapeGeometry(this GeometryObject geometry)
    {
      switch (geometry)
      {
        case Point p: yield return p; yield break;
        case Curve c:
          foreach (var unbounded in c.ToBoundedCurves())
            yield return unbounded;
          yield break;
        case Solid s: yield return s; yield break;
        case Mesh m: yield return m; yield break;
        case GeometryInstance i: yield return i; yield break;
        default: throw new ArgumentException("DirectShape only supports Point, Curve, Solid, Mesh and GeometryInstance.");
      }
    }

    /// <summary>
    /// Computes an arbitrary object oriented coord system for <paramref name="geometry"/>.
    /// </summary>
    /// <param name="geometry"></param>
    /// <param name="origin"></param>
    /// <param name="basisX"></param>
    /// <param name="basisY"></param>
    /// <returns></returns>
    public static bool TryGetLocation(this GeometryObject geometry, out XYZ origin, out XYZ basisX, out XYZ basisY)
    {
      switch (geometry)
      {
        case GeometryElement element:
          foreach (var geo in element)
            if (TryGetLocation(geo, out origin, out basisX, out basisY)) return true;
          break;

        case GeometryInstance instance:
          origin = instance.Transform.Origin;
          basisX = instance.Transform.BasisX;
          basisY = instance.Transform.BasisY;
          return true;

        case Point point:
          origin = point.Coord;
          basisX = XYZ.BasisX;
          basisY = XYZ.BasisY;
          return true;

        case PolyLine polyline:
          return polyline.TryGetLocation(out origin, out basisX, out basisY);

        case Curve curve:
          return curve.TryGetLocation(out origin, out basisX, out basisY);

        case Edge edge:
          return edge.AsCurve().TryGetLocation(out origin, out basisX, out basisY);

        case Face face:
          using (var derivatives = face.ComputeDerivatives(new UV(0.5, 0.5), true))
          {
            origin = derivatives.Origin;
            basisX = derivatives.BasisX;
            basisY = derivatives.BasisY;
          }
          return true;

        case Solid solid:
          if(!solid.Faces.IsEmpty)
            return TryGetLocation(solid.Faces.get_Item(0), out origin, out basisX, out basisY);
          break;

        case Mesh mesh:
          return mesh.TryGetLocation(out origin, out basisX, out basisY);
      }

      origin = basisX = basisY = default;
      return false;
    }

    /// <summary>
    /// Retrieves a box that circumscribes the geometry object.
    /// </summary>
    /// <param name="mesh"></param>
    /// <returns></returns>
    public static bool TryGetBoundingBox(this GeometryObject geometry, out BoundingBoxXYZ bbox)
    {
      switch (geometry)
      {
        case GeometryElement element:
          bbox = element.GetBoundingBox();
          return true;

        case GeometryInstance instance:
          if (instance.SymbolGeometry.TryGetBoundingBox(out bbox))
          {
            bbox.Transform = instance.Transform;
            return true;
          }
          break;

        case Point point:
          bbox = new BoundingBoxXYZ() { Min = point.Coord, Max = point.Coord };
          return true;

        case PolyLine polyline:
          return XYZExtension.TryGetBoundingBox(polyline.GetCoordinates(), out bbox);

        case Curve curve:
          return XYZExtension.TryGetBoundingBox(curve.Tessellate(), out bbox);

        case Edge edge:
          return XYZExtension.TryGetBoundingBox(edge.Tessellate(), out bbox);

        case Face face:
          using (var mesh = face.Triangulate())
            return XYZExtension.TryGetBoundingBox(mesh.Vertices, out bbox);

        case Solid solid:
          if (!solid.Faces.IsEmpty)
          {
            bbox = solid.GetBoundingBox();
            bbox.Min = bbox.Transform.OfPoint(bbox.Min);
            bbox.Max = bbox.Transform.OfPoint(bbox.Max);
            bbox.Transform = Transform.Identity;
            return true;
          }
          break;

        case Mesh mesh:
          return XYZExtension.TryGetBoundingBox(mesh.Vertices, out bbox);
      }

      bbox = default;
      return false;
    }
  }
}
