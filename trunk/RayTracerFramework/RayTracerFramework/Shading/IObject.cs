using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using System.Xml.Serialization;

namespace RayTracerFramework.Shading {
    [XmlInclude(typeof(DSphere))]
    [XmlInclude(typeof(DBox))]
    public interface IObject : IGeometricObject, IShadable { }
}
