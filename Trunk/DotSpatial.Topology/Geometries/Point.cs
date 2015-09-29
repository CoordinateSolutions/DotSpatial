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
using DotSpatial.Serialization;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Geometries
{
    /// <summary>
    /// Represents a single point.
    /// <para/>
    /// A <c>Point</c> is topologically valid if and only if:
    /// <list type="Bullet">
    /// <item>The coordinate which defines it if any) is a valid coordinate 
    /// (i.e. does not have an <c>NaN</c> X- or Y-ordinate</item>
    /// </list>
    /// </summary>
    [Serializable]
    public class Point : Geometry, IPoint
    {
        #region Fields

        private static readonly Coordinate EmptyCoordinate = null;

        /// <summary>
        /// Represents an empty <c>Point</c>.
        /// </summary>
        public static readonly IPoint Empty = new GeometryFactory().CreatePoint(EmptyCoordinate);

        /// <summary>  
        /// The <c>Coordinate</c> wrapped by this <c>Point</c>.
        /// </summary>
        private ICoordinateSequence _coordinates;
        private int _recordNumber;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a null point with X = 0, Y = 0, which can have its properties set later.
        /// </summary>
        public Point()
        {
            _coordinates = GeometryFactory.Default.CoordinateSequenceFactory.Create(new[] { new Coordinate() });
        }

        /// <summary>
        /// Creates a null point with X = 0, Y = 0 but using the specified factory for
        /// precision and SRID stuff.
        /// </summary>
        /// <param name="factory">The factory to use when creating this geometry.</param>
        public Point(IGeometryFactory factory)
            : base(factory)
        {
            _coordinates = factory.CoordinateSequenceFactory.Create(new[] { new Coordinate() });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> class.
        /// </summary>
        /// <param name="coordinate">The coordinate used for create this <see cref="Point" />.</param>
        /// <remarks>
        /// For create this <see cref="Geometry"/> is used a standard <see cref="GeometryFactory"/>
        /// with <see cref="PrecisionModel" /> <c> == </c> <see cref="PrecisionModelType.Floating"/>.
        /// </remarks>
        public Point(Coordinate coordinate) :
            this(GeometryFactory.Default.CoordinateSequenceFactory.Create(new Coordinate[] { coordinate }),
            GeometryFactory.Default) { }

        /// <summary>
        /// Initializes a new instance of the Point class.
        /// </summary>
        /// <param name="coordinate">The coordinate used for create this <see cref="Point" />.</param>
        /// <remarks>
        /// For create this <see cref="Geometry"/> is used a standard <see cref="GeometryFactory"/>
        /// with <see cref="PrecisionModel" /> <c> == </c> <see cref="PrecisionModelType.Floating"/>.
        /// </remarks>
        public Point(ICoordinate coordinate) : this(new Coordinate(coordinate), new GeometryFactory()) { }

        /// <summary>
        /// Constructs a <c>Point</c> with the given coordinate.
        /// </summary>
        /// <param name="coordinate">
        /// Contains the single coordinate on which to base this <c>Point</c>,
        /// or <c>null</c> to create the empty point.
        /// </param>
        /// <param name="factory"></param>
        public Point(Coordinate coordinate, IGeometryFactory factory)
            : base(factory)
        {
            _coordinates = factory.CoordinateSequenceFactory.Create(new[] { coordinate });
        }

        /// <summary>
        /// Constructs a <c>Point</c> with the given coordinate.
        /// </summary>
        /// <param name="coordinates">
        /// Contains the single coordinate on which to base this <c>Point</c>,
        /// or <c>null</c> to create the empty point.
        /// </param>
        /// <param name="factory"></param>
        public Point(ICoordinateSequence coordinates, IGeometryFactory factory)
            : base(factory)
        {
            if (coordinates == null)
                coordinates = factory.CoordinateSequenceFactory.Create(new Coordinate[] { });
            Assert.IsTrue(coordinates.Count <= 1);
            _coordinates = coordinates;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        /// /// <remarks>
        /// For create this <see cref="Geometry"/> is used a standard <see cref="GeometryFactory"/>
        /// with <see cref="PrecisionModel" /> <c> set to </c> <see cref="PrecisionModelType.Floating"/>.
        /// </remarks>
        public Point(double x, double y, double z) :
            this(DefaultFactory.CoordinateSequenceFactory.Create(new[] { new Coordinate(x, y, z) }), DefaultFactory) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// /// <remarks>
        /// For create this <see cref="Geometry"/> is used a standard <see cref="GeometryFactory"/>
        /// with <see cref="PrecisionModel" /> <c> set to </c> <see cref="PrecisionModelType.Floating"/>.
        /// </remarks>
        public Point(double x, double y)
            : this(DefaultFactory.CoordinateSequenceFactory.Create(new[] { new Coordinate(x, y) }), DefaultFactory) { }

        #endregion

        #region Properties

        /// <summary>
        ///Gets the boundary of this geometry.
        ///Zero-dimensional geometries have no boundary by definition,
        ///so an empty GeometryCollection is returned.
        /// </summary>
        public override IGeometry Boundary
        {
            get
            {
                return Factory.CreateGeometryCollection(null);
            }
        }

        /// <summary>
        /// Returns the dimension of this <c>Geometry</c>s inherent boundary.
        /// </summary>
        /// <returns>
        /// The dimension of the boundary of the class implementing this
        /// interface, whether or not this object is the empty point. Returns
        /// <c>Dimension.False</c> if the boundary is the empty point.
        /// </returns>
        public override Dimension BoundaryDimension
        {
            get
            {
                return Dimension.False;
            }
        }

        /// <summary>
        /// Returns a vertex of this Geometry
        /// </summary>
        public override Coordinate Coordinate
        {
            get
            {
                return _coordinates.Count != 0 ? _coordinates.GetCoordinate(0) : null;
            }
        }

        /// <summary>
        /// People might access "Coordinates".  If we spontaneously generate a list from
        /// our single coordinate, thne we will have problems.
        /// They cannot SET the coordinate like myPoint.Coordinates[0].X = 5.
        /// </summary>
        public override IList<Coordinate> Coordinates
        {
            get
            {
                return _coordinates.ToList();
            }
            set
            {
                _coordinates = DefaultFactory.CoordinateSequenceFactory.Create(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICoordinateSequence CoordinateSequence
        {
            get
            {
                return _coordinates;
            }
        }

        /// <summary>
        /// Gets or sets the DotSpatial.Geometries.Dimensions of this Geometry.
        /// </summary>
        public override Dimension Dimension
        {
            get
            {
                return Dimension.Point;
            }
        }

        /// <summary>
        /// Envelope containing this
        /// </summary>
        public new IEnvelope Envelope
        {
            get { return new Envelope(X, X, Y, Y); }
        }

        /// <summary>
        /// returns Point
        /// </summary>
        public override string GeometryType
        {
            get
            {
                return "Point";
            }
        }

        /// <summary>
        /// Returns whether or not the set of points in this geometry is empty
        /// </summary>
        public override bool IsEmpty
        {
            get
            {
                return _coordinates.Count == 0;
            }
        }

        /// <summary>
        /// The measure coordinate
        /// </summary>
        public virtual double M
        {
            get
            {
                if (Coordinate == null)
                    throw new ArgumentOutOfRangeException("M called on empty Point");
                return Coordinate.M;
            }
            set
            {
                Coordinate c = Coordinate;
                c.M = value;
            }
        }

        /// <summary>
        /// Gets the number of ordinates that are being used by the underlying coordinate for
        /// this point.
        /// </summary>
        public int NumOrdinates
        {
            get
            {
                return Coordinate.NumOrdinates;
            }
        }

        /// <summary>
        /// The integer number of points.  In this case it is either 1 or 0 if the point is empty.
        /// </summary>
        public override int NumPoints
        {
            get
            {
                return IsEmpty ? 0 : 1;
            }
        }

        /// <summary>
        /// Gets the OGC geometry type
        /// </summary>
        public override OgcGeometryType OgcGeometryType
        {
            get { return OgcGeometryType.Point; }
        }

        /// <summary>
        /// This is an optional recordnumber index, used specifically for Shapefile points.
        /// </summary>
        public int RecordNumber
        {
            get { return _recordNumber; }
            set { _recordNumber = value; }
        }

        /// <summary>
        /// Gets or sets the ordinates directly as an array of double values for this point.
        /// </summary>
        public double[] Values
        {
            get { return Coordinate.ToArray(); }
            set
            {
                SetCoordinate(new Coordinate(value));
            }
        }

        /// <summary>
        /// The X coordinate
        /// </summary>
        public virtual double X
        {
            get
            {
                if (Coordinate == null)
                    throw new ArgumentOutOfRangeException("X called on empty Point");
                return Coordinate.X;
            }
            set
            {
                Coordinate.X = value;
            }
        }

        /// <summary>
        /// The Y coordinate
        /// </summary>
        public virtual double Y
        {
            get
            {
                if (Coordinate == null)
                    throw new ArgumentOutOfRangeException("Y called on empty Point");
                return Coordinate.Y;
            }
            set
            {
                Coordinate.Y = value;
            }
        }

        /// <summary>
        /// The Z coordinate
        /// </summary>
        public virtual double Z
        {
            get
            {
                if (Coordinate == null)
                    throw new ArgumentOutOfRangeException("Z called on empty Point");
                return Coordinate.Z;
            }
            set
            {
                Coordinate.Z = value;
            }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets or sets the double value of the specific ordinate
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double this[int index]
        {
            get { return Coordinate[index]; }
            set { Coordinate[index] = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an operation with or on this <c>Geometry</c>'s
        /// coordinates. If you are using this method to modify the point, be sure
        /// to call GeometryChanged() afterwards. Notice that you cannot use this
        /// method to
        /// modify this Geometry if its underlying CoordinateSequence's Get method
        /// returns a copy of the Coordinate, rather than the actual Coordinate stored
        /// (if it even stores Coordinates at all).
        /// </summary>
        /// <param name="filter">The filter to apply to this <c>Geometry</c>'s coordinates</param>
        public override void Apply(ICoordinateFilter filter)
        {
            if (IsEmpty)
                return;
            filter.Filter(Coordinate);
        }

        public override void Apply(ICoordinateSequenceFilter filter)
        {
            if (IsEmpty)
                return;
            filter.Filter(_coordinates, 0);
            if (filter.GeometryChanged)
                GeometryChanged();
        }

        /// <summary>
        /// Performs an operation with or on this <c>Geometry</c> and its
        /// subelement <c>Geometry</c>s (if any).
        /// Only GeometryCollections and subclasses
        /// have subelement Geometry's.
        /// </summary>
        /// <param name="filter">
        /// The filter to apply to this <c>Geometry</c> (and
        /// its children, if it is a <c>GeometryCollection</c>).
        /// </param>
        public override void Apply(IGeometryFilter filter)
        {
            filter.Filter(this);
        }

        /// <summary>
        /// Performs an operation with or on this Geometry and its
        /// component Geometry's. Only GeometryCollections and
        /// Polygons have component Geometry's; for Polygons they are the LinearRings
        /// of the shell and holes.
        /// </summary>
        /// <param name="filter">The filter to apply to this <c>Geometry</c>.</param>
        public override void Apply(IGeometryComponentFilter filter)
        {
            filter.Filter(this);
        }

        /// <summary>
        /// Given the specified test point, this returns the closest point in this geometry.
        /// </summary>
        /// <param name="testPoint"></param>
        /// <returns></returns>
        public override Coordinate ClosestPoint(Coordinate testPoint)
        {
            return Coordinate;
        }

        /// <summary>
        /// Returns whether this <c>Geometry</c> is greater than, equal to,
        /// or less than another <c>Geometry</c> having the same class.
        /// </summary>
        /// <param name="other">A <c>Geometry</c> having the same class as this <c>Geometry</c>.</param>
        /// <returns>
        /// A positive number, 0, or a negative number, depending on whether
        /// this object is greater than, equal to, or less than <c>o</c>, as
        /// defined in "Normal Form For Geometry" in the NTS Technical
        /// Specifications.
        /// </returns>
        public override int CompareToSameClass(object other)
        {
            Point point = (Point)other;
            return Coordinate.CompareTo(point.Coordinate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        protected internal override int CompareToSameClass(object other, IComparer<ICoordinateSequence> comparer)
        {
            return comparer.Compare(CoordinateSequence, ((IPoint)other).CoordinateSequence);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override IEnvelope ComputeEnvelopeInternal()
        {
            if (IsEmpty)
                return new Envelope();
            return new Envelope(Coordinate.X, Coordinate.X, Coordinate.Y, Coordinate.Y);
        }

        /// <summary>
        /// Calculates the vector distance.  (This is a 2D operation)
        /// </summary>
        /// <param name="coord">Any valid implementation of the ICoordinate Interface</param>
        /// <returns>The Euclidean distance between two points {Sqrt((X2 - X1)^2 + (Y2 - Y1)^2)</returns>
        public double Distance(Coordinate coord)
        {
            double dX = X - coord.X;
            double dY = Y - coord.Y;
            return Math.Sqrt(dX * dX + dY * dY);
        }

        /// <summary>
        /// Tests to see if the X coordinate and Y coordinate are the same between this point and the
        /// specified Coordinate
        /// </summary>
        /// <param name="coord">Any valid implementation of the ICoordinate Interface</param>
        /// <returns>True if the coordinates are equal, false otherwise</returns>
        public bool Equals2D(Coordinate coord)
        {
            if (X == coord.X && Y == coord.Y) return true;
            return false;
        }

        /// <summary>
        /// Tests to see if the X, Y, and Z coordinate are the same between this point and the
        /// specified Coordinate
        /// </summary>
        /// <param name="coord">Any valid implementation of the ICoordinate Interface</param>
        /// <returns>True if the coordinates are equal, false otherwise</returns>
        public bool Equals3D(Coordinate coord)
        {
            if (X == coord.X && Y == coord.Y && Z == coord.Z) return true;
            return false;
        }

        /// EqualsExact
        /// <summary>
        /// Returns true if the two <c>Geometry</c>s are exactly equal,
        /// up to a specified tolerance.
        /// Two Geometries are exactly within a tolerance equal iff:
        /// they have the same class,
        /// they have the same values of Coordinates,
        /// within the given tolerance distance, in their internal
        /// Coordinate lists, in exactly the same order.
        /// If this and the other <c>Geometry</c>s are
        /// composites and any children are not <c>Geometry</c>s, returns
        /// false.
        /// </summary>
        /// <param name="other">The <c>Geometry</c> with which to compare this <c>Geometry</c></param>
        /// <param name="tolerance">Distance at or below which two Coordinates will be considered equal.</param>
        /// <returns>
        /// <c>true</c> if this and the other <c>Geometry</c>
        /// are of the same class and have equal internal data.
        /// </returns>
        public override bool EqualsExact(IGeometry other, double tolerance)
        {
            if (!IsEquivalentClass(other))
                return false;
            if (IsEmpty && other.IsEmpty)
                return true;
            if (IsEmpty != other.IsEmpty)
                return false;

            return Equal(other.Coordinate, Coordinate, tolerance);
        }

        public override double[] GetOrdinates(Ordinate ordinate)
        {

            if (IsEmpty)
                return new double[0];

            var ordinateFlag = OrdinatesUtility.ToOrdinatesFlag(ordinate);
            if ((_coordinates.Ordinates & ordinateFlag) != ordinateFlag)
                return new[] { Coordinate.NullOrdinate };

            return new[] { _coordinates.GetOrdinate(0, ordinate) };
        }

        /// <summary>
        /// Returns the distance that is appropriate for N dimensions.  In otherwords, if this point is
        /// three dimensional, then all three dimensions will be used for calculating the distance.
        /// </summary>
        /// <param name="coordinate">The coordinate to compare to this coordinate</param>
        /// <returns>A double valued distance measure that is invariant to the number of coordinates</returns>
        /// <exception cref="CoordinateMismatchException">The number of dimensions does not match between the points.</exception>
        public double HyperDistance(Coordinate coordinate)
        {
            if (coordinate.NumOrdinates != NumOrdinates) throw new CoordinateMismatchException();
            double sqrDist = 0;
            double[] vals = coordinate.ToArray();
            for (int i = 0; i < NumOrdinates; i++)
            {
                double diff = vals[i] - Coordinate[i];
                sqrDist += diff * diff;
            }
            return Math.Sqrt(sqrDist);
        }

        /// Normalize
        /// <summary>
        /// Converts this <c>Geometry</c> to normal form (or
        /// canonical form ). Normal form is a unique representation for <c>Geometry</c>
        /// s. It can be used to test whether two <c>Geometry</c>s are equal
        /// in a way that is independent of the ordering of the coordinates within
        /// them. Normal form equality is a stronger condition than topological
        /// equality, but weaker than pointwise equality. The definitions for normal
        /// form use the standard lexicographical ordering for coordinates. "Sorted in
        /// order of coordinates" means the obvious extension of this ordering to
        /// sequences of coordinates.  This does nothing for points.
        /// </summary>
        public override void Normalize() { }

        /// <summary>
        /// Creates a copy of this Point with the same factory
        /// specifications and values.
        /// </summary>
        protected override void OnCopy(Geometry copy)
        {
            base.OnCopy(copy);
            Point p = copy as Point;
            if (p != null)
            {
                p.SetCoordinate(Coordinate.Copy());
            }
        }

        public override IGeometry Reverse()
        {
            Point p = new Point();
            OnCopy(p);
            return p;
        }

        /// <summary>
        /// Rotates the point by the given radian angle around the Origin.
        /// </summary>
        /// <param name="origin">Coordinate the point gets rotated around.</param>
        /// <param name="radAngle">Rotation angle in radian.</param>
        public override void Rotate(Coordinate origin, double radAngle)
        {
            RotateCoordinateRad(origin, ref Coordinate.X, ref Coordinate.Y, radAngle);
        }

        /// <summary>
        /// Sets the coordinate.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetCoordinate(Coordinate value)
        {
            if (_coordinates == null || _coordinates.Count == 0)
            {
                _coordinates = DefaultFactory.CoordinateSequenceFactory.Create(value);
            }
            else
            {
                _coordinates[0] = value;
            }
        }

        /// <summary>
        /// Creates an array of ordinate values that is the size of NumDimensions.  This
        /// will not include an M value.
        /// </summary>
        /// <returns>An Array of double values, with one value for each ordinate.</returns>
        public double[] ToArray()
        {
            return Coordinate.ToArray();
        }

        #endregion
    }
}