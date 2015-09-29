// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using DotSpatial.Topology.Operation;

namespace DotSpatial.Topology.Geometries
{
    /// <summary>
    /// Basic implementation of <c>MultiLineString</c>.
    /// </summary>
    [Serializable]
    public class MultiLineString : GeometryCollection, IMultiLineString
    {
        #region Fields

        /// <summary>
        /// Represents an empty <c>MultiLineString</c>.
        /// </summary>
        public static new readonly IMultiLineString Empty = new GeometryFactory().CreateMultiLineString(null);

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a multiLineString from the list of IBasicLineStrings, creating new full geometries where necessary.
        /// </summary>
        /// <param name="lineStrings">An IBasicLineString that is either a full IGeometry itself, or will be cast into one by this step</param>
        public MultiLineString(IEnumerable<ILineString> lineStrings)
        {
            int count = lineStrings.Count();
            IGeometry[] g = new IGeometry[count];
            int index = 0;
            foreach (ILineString basicLineString in lineStrings)
            {
                g[index] = FromBasicGeometry(basicLineString);
                index = index + 1;
            }
            base.Geometries = g;
        }

        /// <summary>
        /// Constructs a <c>MultiLineString</c>.
        /// </summary>
        /// <param name="lineStrings">
        /// The <c>LineString</c>s for this <c>MultiLineString</c>,
        /// or <c>null</c> or an empty array to create the empty
        /// point. Elements may be empty <c>LineString</c>s,
        /// but not <c>null</c>s.
        /// </param>
        /// <param name="factory"></param>
        public MultiLineString(ILineString[] lineStrings, IGeometryFactory factory)
            : base(lineStrings, factory)
        {
        }

        /// <summary>
        /// This will attempt to create a new MultiLineString from the specified basic geometry.
        /// </summary>
        /// <param name="inBasicGeometry">A Basic geometry that shoule be a LineString or MultiLineString</param>
        public MultiLineString(IGeometry inBasicGeometry)
            : base(inBasicGeometry, DefaultFactory)
        {
        }

        /// <summary>
        /// This will attempt to create a new MultiLineString from the specified basic geometry.
        /// </summary>
        /// <param name="inBasicGeometry">A Basic geometry that shoule be a LineString or MultiLineString</param>
        /// <param name="inFactory">Any valid Geometry Factory</param>
        public MultiLineString(IGeometry inBasicGeometry, IGeometryFactory inFactory)
            : base(inBasicGeometry, inFactory)
        {
        }

        /// <summary>
        /// Constructs a <c>MultiLineString</c>.
        /// </summary>
        /// <param name="lineStrings">
        /// The <c>LineString</c>s for this <c>MultiLineString</c>,
        /// or <c>null</c> or an empty array to create the empty
        /// point. Elements may be empty <c>LineString</c>s,
        /// but not <c>null</c>s.
        /// </param>
        /// <remarks>
        /// For create this <see cref="Geometry"/> is used a standard <see cref="GeometryFactory"/>
        /// with <see cref="PrecisionModel" /> <c> == </c> <see cref="PrecisionModelType.Floating"/>.
        /// </remarks>
        public MultiLineString(ILineString[] lineStrings) : this(lineStrings, DefaultFactory) { }

        /// <summary>
        /// Constructor for a MultiLineString that is empty
        /// </summary>
        public MultiLineString()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public override IGeometry Boundary
        {
            get
            {
                return (new BoundaryOp(this)).GetBoundary();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public override Dimension BoundaryDimension
        {
            get
            {
                if (IsClosed)
                    return Dimension.False;                
                return Dimension.Point;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public override Dimension Dimension
        {
            get
            {
                return Dimension.Curve;
            }
        }

        /// <summary>
        /// Returns the name of this object's interface.
        /// </summary>
        /// <returns>"MultiLineString"</returns>
        public override string GeometryType
        {
            get
            {
                return "MultiLineString";
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is closed.
        /// </summary>
        /// <value><c>true</c> if this instance is closed; otherwise, <c>false</c>.</value>
        public bool IsClosed
        {
            get
            {
                if (IsEmpty) 
                    return false;
                for (int i = 0; i < Geometries.Length; i++)
                    if (!((ILineString) Geometries[i]).IsClosed)
                        return false;                
                return true;
            }
        }

        /// <summary>
        /// Gets the OGC geometry type
        /// </summary>
        public override OgcGeometryType OgcGeometryType
        {
            get { return OgcGeometryType.MultiLineString; }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets the ILineString that corresponds to the specified index
        /// </summary>
        /// <param name="index">The integer index to get a linestring</param>
        /// <returns>An ILineString</returns>
        public new ILineString this[int index]
        {
            get
            {
                return base[index] as ILineString;
            }
            set
            {
                base[index] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="other"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public override bool EqualsExact(IGeometry other, double tolerance)
        {
            if (!IsEquivalentClass(other))
                return false;
            return base.EqualsExact(other, tolerance);
        }

        /// <summary>
        /// Creates a <see cref="MultiLineString" /> in the reverse order to this object.
        /// Both the order of the component LineStrings
        /// and the order of their coordinate sequences are reversed.
        /// </summary>
        /// <returns>a <see cref="MultiLineString" /> in the reverse order.</returns>
        public override IGeometry Reverse()
        {
            int nLines = Geometries.Length;
            ILineString[] revLines = new ILineString[nLines];
            for (int i = 0; i < Geometries.Length; i++)
                revLines[nLines - 1 - i] = (ILineString) Geometries[i].Reverse();            
            return Factory.CreateMultiLineString(revLines);
        }

        #endregion
    }
}