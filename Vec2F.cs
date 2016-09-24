using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Drawing;

/// <summary>
/// 2D-vector struct (originally from Sharp3D.Math library from Eran Kempf)
/// </summary>
public class Vec2F
{
   #region public fields
   /// <summary>x-component of vector</summary>
   public float x;
   /// <summary>y-component of vector</summary>
   public float y;
   #endregion

   #region Constructors
   /// <summary>
   /// Default constructor that inits all components to zero
   /// </summary>
   public Vec2F()
   {
      this.x = 0;
      this.y = 0;
   }
   /// <summary>
   /// Initializes a new instance with the specified coordinates.
   /// </summary>
   /// <param name="x">The vector's x coordinate.</param>
   /// <param name="y">The vector's y coordinate.</param>
   public Vec2F(float x, float y)
   {
      this.x = x;
      this.y = y;
   }
   /// <summary>
   /// Initializes a new instance with the specified coordinates.
   /// </summary>
   /// <param name="coordinates">An array containing the coordinate parameters.</param>
   public Vec2F(float[] coordinates)
   {
      Debug.Assert(coordinates != null);
      Debug.Assert(coordinates.Length >= 2);

      this.x = coordinates[0];
      this.y = coordinates[1];
   }
   /// <summary>
   /// Initializes a new instance using coordinates from a given <see cref="Vec2F"/> instance.
   /// </summary>
   /// <param name="vector">A <see cref="Vec2F"/> to get the coordinates from.</param>
   public Vec2F(Vec2F vector)
   {
      this.x = vector.x;
      this.y = vector.y;
   }
   /// <summary>
   /// Initializes a new instance with the specified PointF.
   /// </summary>
   /// <param name="p">PointF p</param>
   public Vec2F(PointF p)
   {
      this.x = p.X;
      this.y = p.Y;
   }
   #endregion

   #region Constants
   /// <summary>
   /// 2-Dimentional float-precision floating point zero vector.
   /// </summary>
   public static readonly Vec2F Zero	= new Vec2F(0.0f, 0.0f);
   /// <summary>
   /// 2-Dimentional float-precision floating point X-Axis vector.
   /// </summary>
   public static readonly Vec2F XAxis	= new Vec2F(1.0f, 0.0f);
   /// <summary>
   /// 2-Dimentional float-precision floating point Y-Axis vector.
   /// </summary>
   public static readonly Vec2F YAxis	= new Vec2F(0.0f, 1.0f);
   #endregion

   #region Public Static Vector Arithmetics
   /// <summary>
   /// Adds two vectors.
   /// </summary>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <param name="w">A <see cref="Vec2F"/> instance.</param>
   /// <returns>A new <see cref="Vec2F"/> instance containing the sum.</returns>
   public static Vec2F Add(Vec2F v, Vec2F w)
   {
      return new Vec2F(v.x + w.x, v.y + w.y);
   }
   /// <summary>
   /// Adds a vector and a scalar.
   /// </summary>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar.</param>
   /// <returns>A new <see cref="Vec2F"/> instance containing the sum.</returns>
   public static Vec2F Add(Vec2F v, float s)
   {
      return new Vec2F(v.x + s, v.y + s);
   }
   /// <summary>
   /// Adds two vectors and put the result in the third vector.
   /// </summary>
   /// <param name="u">A <see cref="Vec2F"/> instance.</param>
   /// <param name="v">A <see cref="Vec2F"/> instance</param>
   /// <param name="w">A <see cref="Vec2F"/> instance to hold the result.</param>
   public static void Add(Vec2F u, Vec2F v, Vec2F w)
   {
      w.x = u.x + v.x;
      w.y = u.y + v.y;
   }
   /// <summary>
   /// Adds a vector and a scalar and put the result into another vector.
   /// </summary>
   /// <param name="u">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar.</param>
   /// <param name="v">A <see cref="Vec2F"/> instance to hold the result.</param>
   public static void Add(Vec2F u, float s, Vec2F v)
   {
      v.x = u.x + s;
      v.y = u.y + s;
   }
   /// <summary>
   /// Subtracts a vector from a vector.
   /// </summary>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <param name="w">A <see cref="Vec2F"/> instance.</param>
   /// <returns>A new <see cref="Vec2F"/> instance containing the difference.</returns>
   /// <remarks>
   ///	result[i] = v[i] - w[i].
   /// </remarks>
   public static Vec2F Subtract(Vec2F v, Vec2F w)
   {
      return new Vec2F(v.x - w.x, v.y - w.y);
   }
   /// <summary>
   /// Subtracts a scalar from a vector.
   /// </summary>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar.</param>
   /// <returns>A new <see cref="Vec2F"/> instance containing the difference.</returns>
   /// <remarks>
   /// result[i] = v[i] - s
   /// </remarks>
   public static Vec2F Subtract(Vec2F v, float s)
   {
      return new Vec2F(v.x - s, v.y - s);
   }
   /// <summary>
   /// Subtracts a vector from a scalar.
   /// </summary>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar.</param>
   /// <returns>A new <see cref="Vec2F"/> instance containing the difference.</returns>
   /// <remarks>
   /// result[i] = s - v[i]
   /// </remarks>
   public static Vec2F Subtract(float s, Vec2F v)
   {
      return new Vec2F(s - v.x, s - v.y);
   }
   /// <summary>
   /// Subtracts a vector from a second vector and puts the result into a third vector.
   /// </summary>
   /// <param name="u">A <see cref="Vec2F"/> instance.</param>
   /// <param name="v">A <see cref="Vec2F"/> instance</param>
   /// <param name="w">A <see cref="Vec2F"/> instance to hold the result.</param>
   /// <remarks>
   ///	w[i] = v[i] - w[i].
   /// </remarks>
   public static void Subtract(Vec2F u, Vec2F v, Vec2F w)
   {
      w.x = u.x - v.x;
      w.y = u.y - v.y;
   }
   /// <summary>
   /// Subtracts a vector from a scalar and put the result into another vector.
   /// </summary>
   /// <param name="u">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar.</param>
   /// <param name="v">A <see cref="Vec2F"/> instance to hold the result.</param>
   /// <remarks>
   /// v[i] = u[i] - s
   /// </remarks>
   public static void Subtract(Vec2F u, float s, Vec2F v)
   {
      v.x = u.x - s;
      v.y = u.y - s;
   }
   /// <summary>
   /// Subtracts a scalar from a vector and put the result into another vector.
   /// </summary>
   /// <param name="u">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar.</param>
   /// <param name="v">A <see cref="Vec2F"/> instance to hold the result.</param>
   /// <remarks>
   /// v[i] = s - u[i]
   /// </remarks>
   public static void Subtract(float s, Vec2F u, Vec2F v)
   {
      v.x = s - u.x;
      v.y = s - u.y;
   }
   /// <summary>
   /// Divides a vector by another vector.
   /// </summary>
   /// <param name="u">A <see cref="Vec2F"/> instance.</param>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <returns>A new <see cref="Vec2F"/> containing the quotient.</returns>
   /// <remarks>
   ///	result[i] = u[i] / v[i].
   /// </remarks>
   public static Vec2F Divide(Vec2F u, Vec2F v)
   {
      return new Vec2F(u.x / v.x, u.y / v.y);
   }
   /// <summary>
   /// Divides a vector by a scalar.
   /// </summary>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar</param>
   /// <returns>A new <see cref="Vec2F"/> containing the quotient.</returns>
   /// <remarks>
   /// result[i] = v[i] / s;
   /// </remarks>
   public static Vec2F Divide(Vec2F v, float s)
   {
      return new Vec2F(v.x / s, v.y / s);
   }
   /// <summary>
   /// Divides a scalar by a vector.
   /// </summary>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar</param>
   /// <returns>A new <see cref="Vec2F"/> containing the quotient.</returns>
   /// <remarks>
   /// result[i] = s / v[i]
   /// </remarks>
   public static Vec2F Divide(float s, Vec2F v)
   {
      return new Vec2F(s / v.x, s/ v.y);
   }
   /// <summary>
   /// Divides a vector by another vector.
   /// </summary>
   /// <param name="u">A <see cref="Vec2F"/> instance.</param>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <param name="w">A <see cref="Vec2F"/> instance to hold the result.</param>
   /// <remarks>
   /// w[i] = u[i] / v[i]
   /// </remarks>
   public static void Divide(Vec2F u, Vec2F v, Vec2F w)
   {
      w.x = u.x / v.x;
      w.y = u.y / v.y;
   }
   /// <summary>
   /// Divides a vector by a scalar.
   /// </summary>
   /// <param name="u">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar</param>
   /// <param name="v">A <see cref="Vec2F"/> instance to hold the result.</param>
   /// <remarks>
   /// v[i] = u[i] / s
   /// </remarks>
   public static void Divide(Vec2F u, float s, Vec2F v)
   {
      v.x = u.x / s;
      v.y = u.y / s;
   }
   /// <summary>
   /// Divides a scalar by a vector.
   /// </summary>
   /// <param name="u">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar</param>
   /// <param name="v">A <see cref="Vec2F"/> instance to hold the result.</param>
   /// <remarks>
   /// v[i] = s / u[i]
   /// </remarks>
   public static void Divide(float s, Vec2F u, Vec2F v)
   {
      v.x = s / u.x;
      v.y = s / u.y;
   }
   /// <summary>
   /// Multiplies a vector by a scalar.
   /// </summary>
   /// <param name="u">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar.</param>
   /// <returns>A new <see cref="Vec2F"/> containing the result.</returns>
   public static Vec2F Multiply(Vec2F u, float s)
   {
      return new Vec2F(u.x * s, u.y * s);
   }
   /// <summary>
   /// Multiplies a vector by a scalar and put the result in another vector.
   /// </summary>
   /// <param name="u">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar.</param>
   /// <param name="v">A <see cref="Vec2F"/> instance to hold the result.</param>
   public static void Multiply(Vec2F u, float s, Vec2F v)
   {
      v.x = u.x * s;
      v.y = u.y * s;
   }
   /// <summary>
   /// Calculates the dot product of two vectors.
   /// </summary>
   /// <param name="u">A <see cref="Vec2F"/> instance.</param>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <returns>The dot product value.</returns>
   public static float DotProduct(Vec2F u, Vec2F v)
   {
      return (u.x * v.x) + (u.y * v.y);
   }
      /// <summary>
   /// Negates a vector.
   /// </summary>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <returns>A new <see cref="Vec2F"/> instance containing the negated values.</returns>
   public static Vec2F Negate(Vec2F v)
   {
      return new Vec2F(-v.x, -v.y);
   }
   /// <summary>
   /// Tests whether two vectors are approximately equal given a tolerance value.
   /// </summary>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <param name="u">A <see cref="Vec2F"/> instance.</param>
   /// <param name="tolerance">The tolerance value used to test approximate equality.</param>
   /// <returns>True if the two vectors are approximately equal; otherwise, False.</returns>
   public static bool ApproxEqual(Vec2F v, Vec2F u, float tolerance)
   {
      return
         (
         (System.Math.Abs(v.x - u.x) <= tolerance) &&
         (System.Math.Abs(v.y - u.y) <= tolerance)
         );
   }
   #endregion

   #region Public Methods
   /// <summary>
   /// Setter for all components at once
   /// </summary>
   public void Set(float X, float Y)
   {  x=X;
      y=Y;
   }
   /// <summary>
   /// Returns the dot product with the vector v.
   /// </summary>
   /// <returns>dot product: x*v.x + y*v.y</returns>
   public float Dot(Vec2F v)
   {
      return x*v.x + y*v.y;;
   }
   /// <summary>
   /// Scale the vector so that its length is 1.
   /// </summary>
   public void Normalize()
   {
      float length = Length();
      if (length == 0)
      {
         throw new DivideByZeroException("Trying to normalize a vector with length of zero.");
      }

      this.x /= length;
      this.y /= length;
   }
   /// <summary>
   /// Returns the length of the vector.
   /// </summary>
   /// <returns>The length of the vector. (Sqrt(X*X + Y*Y + Z*Z))</returns>
   public float Length()
   {
      return (float)System.Math.Sqrt(this.x*this.x + this.y*this.y);
   }
   /// <summary>
   /// Returns the squared length of the vector.
   /// </summary>
   /// <returns>The squared length of the vector. (X*X + Y*Y + Z*Z)</returns>
   public float LengthSquared()
   {
      return (this.x*this.x + this.y*this.y);
   }
   /// <summary>
   /// Sets the minimum values of this and the passed vector v
   /// </summary>
   public void SetMin(Vec2F v)
   {  if (v.x < x) x=v.x;
      if (v.y < y) y=v.y;
   }
   /// <summary>
   /// Sets the maximum values of this and the passed vector v
   /// </summary>
   public void SetMax(Vec2F v)
   {  if (v.x > x) x=v.x;
      if (v.y > y) y=v.y;
   }
   /// <summary>
   /// Returns the angle to the vector v in radians
   /// </summary>
   /// <param name="v">Vector v</param>
   public double AngleToRAD(Vec2F v)
   {  Vec2F n1 = new Vec2F(x, y);
      Vec2F n2 = new Vec2F(v);
      n1.Normalize();
      n2.Normalize();
      double ndn = n1.Dot(n2);
      if (ndn < -1) ndn = -1;
      if (ndn >  1) ndn =  1;
      return Math.Acos(ndn);
   }   
   /// <summary>
   /// Returns the angle to the vector v in degrees
   /// </summary>
   /// <param name="v">Vector v</param>
   public double AngleToDEG(Vec2F v)
   {  
      return AngleToRAD(v) * 180 / Math.PI;
   }
   #endregion

   #region Overrides
   /// <summary>
   /// Returns the hashcode for this instance.
   /// </summary>
   /// <returns>A 32-bit signed integer hash code.</returns>
   public override int GetHashCode()
   {
      return x.GetHashCode() ^ y.GetHashCode();
   }
   /// <summary>
   /// Returns a value indicating whether this instance is equal to
   /// the specified object.
   /// </summary>
   /// <param name="obj">An object to compare to this instance.</param>
   /// <returns>True if <paramref name="obj"/> is a <see cref="Vec2F"/> and has the same values as this instance; otherwise, False.</returns>
   public override bool Equals(object obj)
   {
      if (obj is Vec2F)
      {
         Vec2F v = (Vec2F)obj;
         return (this.x == v.x) && (this.y == v.y);
      }
      return false;
   }

   /// <summary>
   /// Returns a string representation of this object.
   /// </summary>
   /// <returns>A string representation of this object.</returns>
   public override string ToString()
   {
      return string.Format("({0}, {1})", this.x, this.y);
   }
   #endregion
   
   #region Comparison Operators
   /// <summary>
   /// Tests whether two specified vectors are equal.
   /// </summary>
   /// <param name="u">The left-hand vector.</param>
   /// <param name="v">The right-hand vector.</param>
   /// <returns>True if the two vectors are equal; otherwise, False.</returns>
   public static bool operator==(Vec2F u, Vec2F v)
   {
      if (Object.Equals(u, null))
      {
         return Object.Equals(v, null);
      }

      if (Object.Equals(v, null))
      {
         return Object.Equals(u, null);
      }

      return (u.x == v.x) && (u.y == v.y);
   }
   /// <summary>
   /// Tests whether two specified vectors are not equal.
   /// </summary>
   /// <param name="u">The left-hand vector.</param>
   /// <param name="v">The right-hand vector.</param>
   /// <returns>True if the two vectors are not equal; otherwise, False.</returns>
   public static bool operator!=(Vec2F u, Vec2F v)
   {
      if (Object.Equals(u, null))
      {
         return !Object.Equals(v, null);
      }

      if (Object.Equals(v, null))
      {
         return !Object.Equals(u, null);
      }

      return !((u.x == v.x) && (u.y == v.y));
   }
   /// <summary>
   /// Tests if a vector's components are greater than another vector's components.
   /// </summary>
   /// <param name="u">The left-hand vector.</param>
   /// <param name="v">The right-hand vector.</param>
   /// <returns>True if the left-hand vector's components are greater than the right-hand vector's component; otherwise, False.</returns>
   public static bool operator>(Vec2F u, Vec2F v)
   {
      return (
         (u.x > v.x) && 
         (u.y > v.y));
   }
   /// <summary>
   /// Tests if a vector's components are smaller than another vector's components.
   /// </summary>
   /// <param name="u">The left-hand vector.</param>
   /// <param name="v">The right-hand vector.</param>
   /// <returns>True if the left-hand vector's components are smaller than the right-hand vector's component; otherwise, False.</returns>
   public static bool operator<(Vec2F u, Vec2F v)
   {
      return (
         (u.x < v.x) && 
         (u.y < v.y));
   }
   /// <summary>
   /// Tests if a vector's components are greater or equal than another vector's components.
   /// </summary>
   /// <param name="u">The left-hand vector.</param>
   /// <param name="v">The right-hand vector.</param>
   /// <returns>True if the left-hand vector's components are greater or equal than the right-hand vector's component; otherwise, False.</returns>
   public static bool operator>=(Vec2F u, Vec2F v)
   {
      return (
         (u.x >= v.x) && 
         (u.y >= v.y));
   }
   /// <summary>
   /// Tests if a vector's components are smaller or equal than another vector's components.
   /// </summary>
   /// <param name="u">The left-hand vector.</param>
   /// <param name="v">The right-hand vector.</param>
   /// <returns>True if the left-hand vector's components are smaller or equal than the right-hand vector's component; otherwise, False.</returns>
   public static bool operator<=(Vec2F u, Vec2F v)
   {
      return (
         (u.x <= v.x) && 
         (u.y <= v.y));
   }
   #endregion

   #region Unary Operators
   /// <summary>
   /// Negates the values of the vector.
   /// </summary>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <returns>A new <see cref="Vec2F"/> instance containing the negated values.</returns>
   public static Vec2F operator-(Vec2F v)
   {
      return Vec2F.Negate(v);
   }
   #endregion

   #region Binary Operators
   /// <summary>
   /// Adds two vectors.
   /// </summary>
   /// <param name="u">A <see cref="Vec2F"/> instance.</param>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <returns>A new <see cref="Vec2F"/> instance containing the sum.</returns>
   public static Vec2F operator+(Vec2F u, Vec2F v)
   {
      return Vec2F.Add(u,v);
   }
   /// <summary>
   /// Adds a vector and a scalar.
   /// </summary>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar.</param>
   /// <returns>A new <see cref="Vec2F"/> instance containing the sum.</returns>
   public static Vec2F operator+(Vec2F v, float s)
   {
      return Vec2F.Add(v,s);
   }
   /// <summary>
   /// Adds a vector and a scalar.
   /// </summary>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar.</param>
   /// <returns>A new <see cref="Vec2F"/> instance containing the sum.</returns>
   public static Vec2F operator+(float s, Vec2F v)
   {
      return Vec2F.Add(v,s);
   }
   /// <summary>
   /// Subtracts a vector from a vector.
   /// </summary>
   /// <param name="u">A <see cref="Vec2F"/> instance.</param>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <returns>A new <see cref="Vec2F"/> instance containing the difference.</returns>
   /// <remarks>
   ///	result[i] = v[i] - w[i].
   /// </remarks>
   public static Vec2F operator-(Vec2F u, Vec2F v)
   {
      return Vec2F.Subtract(u,v);
   }
   /// <summary>
   /// Subtracts a scalar from a vector.
   /// </summary>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar.</param>
   /// <returns>A new <see cref="Vec2F"/> instance containing the difference.</returns>
   /// <remarks>
   /// result[i] = v[i] - s
   /// </remarks>
   public static Vec2F operator-(Vec2F v, float s)
   {
      return Vec2F.Subtract(v, s);
   }
   /// <summary>
   /// Subtracts a vector from a scalar.
   /// </summary>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar.</param>
   /// <returns>A new <see cref="Vec2F"/> instance containing the difference.</returns>
   /// <remarks>
   /// result[i] = s - v[i]
   /// </remarks>
   public static Vec2F operator-(float s, Vec2F v)
   {
      return Vec2F.Subtract(s, v);
   }

   /// <summary>
   /// Multiplies a vector by a scalar.
   /// </summary>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar.</param>
   /// <returns>A new <see cref="Vec2F"/> containing the result.</returns>
   public static Vec2F operator*(Vec2F v, float s)
   {
      return Vec2F.Multiply(v,s);
   }
   /// <summary>
   /// Multiplies a vector by a scalar.
   /// </summary>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar.</param>
   /// <returns>A new <see cref="Vec2F"/> containing the result.</returns>
   public static Vec2F operator*(float s, Vec2F v)
   {
      return Vec2F.Multiply(v,s);
   }
   /// <summary>
   /// Divides a vector by a scalar.
   /// </summary>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar</param>
   /// <returns>A new <see cref="Vec2F"/> containing the quotient.</returns>
   /// <remarks>
   /// result[i] = v[i] / s;
   /// </remarks>
   public static Vec2F operator/(Vec2F v, float s)
   {
      return Vec2F.Divide(v,s);
   }
   /// <summary>
   /// Divides a scalar by a vector.
   /// </summary>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <param name="s">A scalar</param>
   /// <returns>A new <see cref="Vec2F"/> containing the quotient.</returns>
   /// <remarks>
   /// result[i] = s / v[i]
   /// </remarks>
   public static Vec2F operator/(float s, Vec2F v)
   {
      return Vec2F.Divide(s,v);
   }
   #endregion

   #region Array Indexing Operator
   /// <summary>
   /// Indexer ( [x, y] ).
   /// </summary>
   public float this[int index]
   {
      get	
      {
         switch( index ) 
         {
            case 0:
               return x;
            case 1:
               return y;
            default:
               throw new IndexOutOfRangeException();
         }
      }
      set 
      {
         switch( index ) 
         {
            case 0:
               x = value;
               break;
            case 1:
               y = value;
               break;
            default:
               throw new IndexOutOfRangeException();
         }
      }

   }

   #endregion

   #region Conversion Operators
   /// <summary>
   /// Converts the vector to an array of float-precision floating point values.
   /// </summary>
   /// <param name="v">A <see cref="Vec2F"/> instance.</param>
   /// <returns>An array of float-precision floating point values.</returns>
   public static explicit operator float[](Vec2F v)
   {
      float[] array = new float[2];
      array[0] = v.x;
      array[1] = v.y;
      return array;
   }
   #endregion

}
