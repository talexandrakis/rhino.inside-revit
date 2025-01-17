using System.Collections.Generic;
using Grasshopper.Kernel;
using ARDB = Autodesk.Revit.DB;

namespace RhinoInside.Revit.GH.Kernel
{
  /// <summary>
  /// Base interface for all Parameter types in RhinoInside.Revit.GH that reference Revit elements.
  /// </summary>
  /// <remarks>
  /// Do not implement this interface from scratch, derive from <see cref="RhinoInside.Revit.GH.Parameters.ElementIdParam"/> instead.
  /// </remarks>
  /// <seealso cref="RhinoInside.Revit.GH.Parameters.ElementIdParam"/>
  internal interface IGH_ElementIdParam : IGH_Param
  {
    bool NeedsToBeExpired
    (
      ARDB.Document doc,
      ICollection<ARDB.ElementId> added,
      ICollection<ARDB.ElementId> deleted,
      ICollection<ARDB.ElementId> modified
    );
  }

  /// <summary>
  /// Base interface for all Component types in RhinoInside.Revit.GH that reference Revit elements.
  /// </summary>
  /// <remarks>
  /// Do not implement this interface from scratch, derive from <see cref="RhinoInside.Revit.GH.Components.Component"/> instead.
  /// </remarks>
  /// <seealso cref="RhinoInside.Revit.GH.Components.Component"/>
  internal interface IGH_ElementIdComponent : IGH_Component
  {
    bool NeedsToBeExpired
    (
      ARDB.Document doc,
      ICollection<ARDB.ElementId> added,
      ICollection<ARDB.ElementId> deleted,
      ICollection<ARDB.ElementId> modified
    );
  }
}
