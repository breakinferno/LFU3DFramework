namespace YFramework
{
    using UnityEngine;
    using System.Collections;
    using System;

    // define a line
    public class Line
    {
        public Vector3 point;//a point on this line
        public Vector3 dir;//the direction of this line

        public Line()
        {

        }

        public Line(Vector3 point, Vector3 dir)
        {
            this.point = point;
            dir = Vector3.Normalize(dir);
            this.dir = dir;
        }

        public override string ToString()
        {
            return "point:" + point + "   dir:" + dir;
        }
    }

    //define a lineSegment
    public class LineSegment
    {
        public Vector3 point1;//start point of this linesegment
        public Vector3 point2;//end point of this linesegment

        public LineSegment()
        {

        }

        public LineSegment(Vector3 point1, Vector3 point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }

        public override string ToString()
        {
            return "point1:" + point1 + "   point2:" + point2;
        }
    }

    // define a plane
    public class Plane
    {
        public Vector3 point;//a point on this plane
        public Vector3 normal;//normal of this plane

        public Plane()
        {

        }

        public Plane(Vector3 point, Vector3 normal)
        {
            this.point = point;
            normal = Vector3.Normalize(normal);
            this.normal = normal;
        }

        public Plane(Vector3 point1, Vector3 point2, Vector3 point3)
        {
            Plane plane;
            Math3dTool.PlaneFrom3Points(out plane, point1, point2, point3);
            this.point = plane.point;
            this.normal = plane.normal;
        }

        public override string ToString()
        {
            return "point:" + point + "   normal:" + normal;
        }
    }

    public class Math3dTool : MonoBehaviour
    {
        private static Transform tempChild = null;
        private static Transform tempParent = null;

        /// <summary>
        /// transform a line into a lineSegment from point defined by the line,and length equals 1
        /// </summary>
        /// <returns>the lineSegment calculated</returns>
        /// <param name="line">need calculated line</param>
        public static LineSegment LineToLineSegment(Line line)
        {
            LineSegment lineSegment = new LineSegment(line.point, line.point + line.dir);
            return lineSegment;
        }

        /// <summary>
        /// transform a lineSegment into a line.the point on the line equals the point1 of lineSegment
        /// </summary>
        /// <returns>line calculated</returns>
        /// <param name="lineSegment">need calculated lineSegment</param>
        public static Line LineSegmentToLine(LineSegment lineSegment)
        {
            Line line = new Line(lineSegment.point1, lineSegment.point2 - lineSegment.point1);
            return line;
        }

        /// <summary>
        /// used for TransformWithParent method to init
        /// </summary>
        private static void TransformWithParentInit()
        {

            tempChild = (new GameObject("Math3d_TempChild")).transform;
            tempParent = (new GameObject("Math3d_TempParent")).transform;

            tempChild.gameObject.hideFlags = HideFlags.HideAndDontSave;
            DontDestroyOnLoad(tempChild.gameObject);

            tempParent.gameObject.hideFlags = HideFlags.HideAndDontSave;
            DontDestroyOnLoad(tempParent.gameObject);

            //set the parent
            tempChild.parent = tempParent;
        }

        /// <summary>
        /// get distance between two points
        /// </summary>
        /// <returns>distance between two points</returns>
        /// <param name="point1">Point1.</param>
        /// <param name="point2">Point2.</param>
        public static float PointPointDistance(Vector3 point1, Vector3 point2)
        {
            float x = (point1.x - point2.x) * (point1.x - point2.x);
            float y = (point1.y - point2.y) * (point1.y - point2.y);
            float z = (point1.z - point2.z) * (point1.z - point2.z);
            return Mathf.Sqrt(x + y + z);
        }

        /// <summary>
        /// increase or decrease the length of vector by size
        /// </summary>
        /// <returns>the vector sized</returns>
        /// <param name="dir">vector direction</param>
        /// <param name="size">size</param>
        public static Vector3 AddVectorLength(Vector3 dir, float size)
        {
            //get the vector length
            float magnitude = Vector3.Magnitude(dir);

            //change the length
            magnitude += size;

            //normalize the vector
            Vector3 vectorNormalized = Vector3.Normalize(dir);

            //scale the vector
            return vectorNormalized *= magnitude;
        }

        /// <summary>
        /// create a vector of direction "vector" with length "size"
        /// </summary>
        /// <returns>The vector sized</returns>
        /// <param name="vector">Vector.</param>
        /// <param name="size">Size.</param>
        public static Vector3 SetVectorLength(Vector3 vector, float size)
        {
            //normalize the vector
            Vector3 vectorNormalized = Vector3.Normalize(vector);

            //scale the vector
            return vectorNormalized *= size;
        }

        //?
        //    //caclulate the rotational difference from A to B
        //    public static Quaternion SubtractRotation(Quaternion B, Quaternion A)
        //    {
        //
        //        Quaternion C = Quaternion.Inverse(A) * B;
        //        return C;
        //    }

        /// <summary>
        /// Find the line of intersection between two planes.
        /// </summary>
        /// <returns><c>true</c>, if two planes are not parallel, <c>false</c> otherwise.</returns>
        /// <param name="line">Line.</param>
        /// <param name="plane1">Plane1.</param>
        /// <param name="plane2">Plane2.</param>
        public static bool PlanePlaneIntersection(out Line line, Plane plane1, Plane plane2)
        {

            Vector3 linePoint = Vector3.zero;
            Vector3 lineVec = Vector3.zero;

            //We can get the direction of the line of intersection of the two planes by calculating the 
            //cross product of the normals of the two planes. Note that this is just a direction and the line
            //is not fixed in space yet. We need a point for that to go with the line vector.
            lineVec = Vector3.Cross(plane1.normal, plane2.normal);

            //Next is to calculate a point on the line to fix it's position in space. This is done by finding a vector from
            //the plane2 location, moving parallel to it's plane, and intersecting plane1. To prevent rounding
            //errors, this vector also has to be perpendicular to lineDirection. To get this vector, calculate
            //the cross product of the normal of plane2 and the lineDirection.      
            Vector3 ldir = Vector3.Cross(plane2.normal, lineVec);

            float denominator = Vector3.Dot(plane1.normal, ldir);

            //Prevent divide by zero and rounding errors by requiring about 5 degrees angle between the planes.
            if (Mathf.Abs(denominator) > 0.006f)
            {

                Vector3 plane1ToPlane2 = plane1.point - plane2.point;
                float t = Vector3.Dot(plane1.normal, plane1ToPlane2) / denominator;
                linePoint = plane2.point + t * ldir;
                line = new Line(linePoint, lineVec);
                return true;
            }

            //output not valid
            else
            {
                line = new Line(linePoint, lineVec);
                return false;
            }
        }

        /// <summary>
        /// Get the intersection between a line and a plane. 
        /// </summary>
        /// <returns><c>true</c>, the line and plane are not parallel, <c>false</c> otherwise.</returns>
        /// <param name="intersection">Intersection.</param>
        /// <param name="line">Line.</param>
        /// <param name="plane">Plane.</param>
        public static bool LinePlaneIntersection(out Vector3 intersection, Line line, Plane plane)
        {
            float length;
            Vector3 vector = new Vector3();
            intersection = Vector3.zero;

            //calculate the distance between the linePoint and the line-plane intersection point
            float dotNumerator = Vector3.Dot((plane.point - line.point), plane.normal);
            float dotDenominator = Vector3.Dot(line.dir, plane.normal);

            //line and plane are not parallel
            if (dotDenominator != 0.0f)
            {
                length = dotNumerator / dotDenominator;

                //create a vector from the linePoint to the intersection point
                vector = SetVectorLength(line.dir, length);

                //get the coordinates of the line-plane intersection point
                intersection = line.point + vector;

                return true;
            }

            //output not valid
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Two non-parallel lines which may or may not touch each other have a point on each line which are closest
        /// to each other. This function finds those two points.
        /// </summary>
        /// <returns><c>true</c>, if two lines are not parallel, <c>false</c> otherwise.</returns>
        /// <param name="closestPointLine1">Closest point on line1.</param>
        /// <param name="closestPointLine2">Closest point on line2.</param>
        /// <param name="line1">Line1.</param>
        /// <param name="line2">Line2.</param>
        public static bool ClosestPointsOnTwoLines(out Vector3 closestPointLine1, out Vector3 closestPointLine2, Line line1, Line line2)
        {
            closestPointLine1 = Vector3.zero;
            closestPointLine2 = Vector3.zero;

            float a = Vector3.Dot(line1.dir, line1.dir);
            float b = Vector3.Dot(line1.dir, line2.dir);
            float e = Vector3.Dot(line2.dir, line2.dir);

            float d = a * e - b * b;

            //lines are not parallel
            if (d != 0.0f)
            {

                Vector3 r = line1.point - line2.point;
                float c = Vector3.Dot(line1.dir, r);
                float f = Vector3.Dot(line2.dir, r);

                float s = (b * f - c * e) / d;
                float t = (a * f - c * b) / d;

                closestPointLine1 = line1.point + line1.dir * s;
                closestPointLine2 = line2.point + line2.dir * t;

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// This function returns a point which is a projection from a point to a line.
        /// </summary>
        /// <returns>The projected point on line.</returns>
        /// <param name="point">point need projected</param>
        /// <param name="line">Line.</param>
        public static Vector3 ProjectPointOnLine(Vector3 point, Line line)
        {
            //get vector from point on line to point in space
            Vector3 linePointToPoint = point - line.point;

            float t = Vector3.Dot(linePointToPoint, line.dir);
            return line.point + line.dir * t;
        }

        /// <summary>
        /// This function returns a point which is a projection from a point to a line segment.
        /// If the projected point lies outside of the line segment, the projected point will 
        /// be clamped to the appropriate line edge.
        /// </summary>
        /// <returns>The point on line segment.</returns>
        /// <param name="lineSegment">Line segment.</param>
        /// <param name="point">Point.</param>
        public static Vector3 ProjectPointOnLineSegment(LineSegment lineSegment, Vector3 point)
        {

            Vector3 vector = lineSegment.point2 - lineSegment.point1;

            Vector3 projectedPoint = ProjectPointOnLine(point, new Line(lineSegment.point1, vector));

            int side = PointOnWhichSideOfLineSegment(lineSegment, projectedPoint);

            //The projected point is on the line segment
            switch (side)
            {
                case 0:
                    return projectedPoint;

                case 1:
                    return lineSegment.point1;

                case 2:
                    return lineSegment.point2;
            }

            //output is invalid
            return Vector3.zero;
        }

        /// <summary>
        /// This function returns a point which is a projection from a point to a plane.
        /// </summary>
        /// <returns>The point projected on plane.</returns>
        /// <param name="plane">Plane.</param>
        /// <param name="point">Point.</param>
        public static Vector3 ProjectPointOnPlane(Plane plane, Vector3 point)
        {
            float distance;
            Vector3 translationVector;

            //First calculate the distance from the point to the plane:
            distance = SignedDistancePlanePoint(plane, point);

            //Reverse the sign of the distance
            distance *= -1;

            //Get a translation vector
            translationVector = SetVectorLength(plane.normal, distance);

            //Translate the point to form a projection
            return point + translationVector;
        }

        /// <summary>
        /// Projects a vector onto a plane. The output is not normalized.
        /// </summary>
        /// <returns>The vector on plane.</returns>
        /// <param name="planeNormal">Plane normal.</param>
        /// <param name="vector">Vector.</param>
        public static Vector3 ProjectVectorOnPlane(Vector3 planeNormal, Vector3 vector)
        {
            planeNormal = planeNormal.normalized;
            return vector - (Vector3.Dot(vector, planeNormal) * planeNormal);
        }

        /// <summary>
        /// Get the shortest distance between a point and a plane. The output is signed so it holds information
        /// as to which side of the plane normal the point is.
        /// </summary>
        /// <returns>The distance plane point.</returns>
        /// <param name="plane">Plane.</param>
        /// <param name="point">Point.</param>
        public static float SignedDistancePlanePoint(Plane plane, Vector3 point)
        {
            return Vector3.Dot(plane.normal, (point - plane.point));
        }

        //This function calculates a signed (+ or - sign instead of being ambiguous) dot product. It is basically used
        //to figure out whether a vector is positioned to the left or right of another vector. The way this is done is
        //by calculating a vector perpendicular to one of the vectors and using that as a reference. This is because
        //the result of a dot product only has signed information when an angle is transitioning between more or less
        //then 90 degrees.
        //  public static float SignedDotProduct(Vector3 vectorA, Vector3 vectorB, Vector3 normal)
        //  {
        //
        //      Vector3 perpVector;
        //      float dot;
        //
        //      //Use the geometry object normal and one of the input vectors to calculate the perpendicular vector
        //      perpVector = Vector3.Cross(normal, vectorA);
        //
        //      //Now calculate the dot product between the perpendicular vector (perpVector) and the other input vector
        //      dot = Vector3.Dot(perpVector, vectorB);
        //
        //      return dot;
        //  }

        //  public static float SignedVectorAngle(Vector3 referenceVector, Vector3 otherVector, Vector3 normal)
        //  {
        //      Vector3 perpVector;
        //      float angle;
        //
        //      //Use the geometry object normal and one of the input vectors to calculate the perpendicular vector
        //      perpVector = Vector3.Cross(normal, referenceVector);
        //
        //      //Now calculate the dot product between the perpendicular vector (perpVector) and the other input vector
        //      angle = Vector3.Angle(referenceVector, otherVector);
        //      angle *= Mathf.Sign(Vector3.Dot(perpVector, otherVector));
        //
        //      return angle;
        //  }

        /// <summary>
        /// Calculate the angle between a vector and a plane. The plane is made by a normal vector.
        /// Output is in radians.
        /// </summary>
        /// <returns>the angle between a vector and a plane</returns>
        /// <param name="vector">Vector.</param>
        /// <param name="normal">normal of the plane</param>
        public static float AngleVectorPlane(Vector3 vector, Vector3 normal)
        {

            float dot;
            float angle;

            //calculate the the dot product between the two input vectors. This gives the cosine between the two vectors
            dot = Vector3.Dot(vector, normal);

            //this is in radians
            angle = (float)Math.Acos(dot);

            return 1.570796326794897f - angle; //90 degrees - angle
        }

        /// <summary>
        /// Convert a plane defined by 3 points to a plane defined by a vector and a point. 
        /// The plane point is the middle of the triangle defined by the 3 points.
        /// </summary>
        /// <param name="plane">Plane.</param>
        /// <param name="pointA">Point a.</param>
        /// <param name="pointB">Point b.</param>
        /// <param name="pointC">Point c.</param>
        public static void PlaneFrom3Points(out Plane plane, Vector3 pointA, Vector3 pointB, Vector3 pointC)
        {
            plane = new Plane();
            plane.normal = Vector3.zero;
            plane.point = Vector3.zero;

            //Make two vectors from the 3 input points, originating from point A
            Vector3 AB = pointB - pointA;
            Vector3 AC = pointC - pointA;

            //Calculate the normal
            plane.normal = Vector3.Normalize(Vector3.Cross(AB, AC));

            //Get the points in the middle AB and AC
            Vector3 middleAB = pointA + (AB / 2.0f);
            Vector3 middleAC = pointA + (AC / 2.0f);

            //Get vectors from the middle of AB and AC to the point which is not on that line.
            Vector3 middleABtoC = pointC - middleAB;
            Vector3 middleACtoB = pointB - middleAC;

            //Calculate the intersection between the two lines. This will be the center 
            //of the triangle defined by the 3 points.
            //We could use LineLineIntersection instead of ClosestPointsOnTwoLines but due to rounding errors 
            //this sometimes doesn't work.
            Vector3 temp;
            ClosestPointsOnTwoLines(out plane.point, out temp, new Line(middleAB, middleABtoC), new Line(middleAC, middleACtoB));
        }

        //Returns the forward vector of a quaternion
        //  public static Vector3 GetForwardVector(Quaternion q)
        //  {
        //
        //      return q * Vector3.forward;
        //  }
        //
        //  //Returns the up vector of a quaternion
        //  public static Vector3 GetUpVector(Quaternion q)
        //  {
        //
        //      return q * Vector3.up;
        //  }
        //
        //  //Returns the right vector of a quaternion
        //  public static Vector3 GetRightVector(Quaternion q)
        //  {
        //
        //      return q * Vector3.right;
        //  }

        //?
        //Gets a quaternion from a matrix
        //  public static Quaternion QuaternionFromMatrix(Matrix4x4 m)
        //  {
        //
        //      return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
        //  }

        /// <summary>
        /// Gets a position from a matrix
        /// </summary>
        /// <returns>position</returns>
        /// <param name="m">M.</param>
        public static Vector3 PositionFromMatrix(Matrix4x4 m)
        {

            Vector4 vector4Position = m.GetColumn(3);
            return new Vector3(vector4Position.x, vector4Position.y, vector4Position.z);
        }

        /// <summary>
        /// This is an alternative for Quaternion.LookRotation. Instead of aligning the forward and up vector of the game 
        /// object with the input vectors, a custom direction can be used instead of the fixed forward and up vectors.
        /// This function will make customUp align with alignWithUp and customForward align with alignWithForward
        /// </summary>
        /// <param name="gameObjectInOut">Game object in out.</param>
        /// <param name="alignWithForward">forward needs to be aligned with in world space</param>
        /// <param name="alignWithUp">up needs to be aligned with in world space</param>
        /// <param name="customForward">custom forward in object space</param>
        /// <param name="customUp">custom up in object space</param>
        public static void LookRotationExtended(ref GameObject gameObjectInOut, Vector3 alignWithForward, Vector3 alignWithUp, Vector3 customForward, Vector3 customUp)
        {

            //Set the rotation of the destination
            Quaternion rotationA = Quaternion.LookRotation(alignWithForward, alignWithUp);

            //Set the rotation of the custom normal and up vectors. 
            //When using the default LookRotation function, this would be hard coded to the forward and up vector.
            Quaternion rotationB = Quaternion.LookRotation(customForward, customUp);

            //Calculate the rotation
            gameObjectInOut.transform.rotation = rotationA * Quaternion.Inverse(rotationB);
        }

        /// <summary>
        /// This function transforms one object as if it was parented to the other
        /// </summary>
        /// <param name="childRotation">calculated child rotation</param>
        /// <param name="childPosition">calculated child position</param>
        /// <param name="parentTransform">parent transform,used to init and calculate child's transform</param>
        /// <param name="childTransform">child transform,used to init</param>
        public static void TransformWithParent(out Quaternion childRotation, out Vector3 childPosition, Transform parentTransform, Transform childTransform)
        {
            if (tempChild == null || tempParent == null)
            {
                TransformWithParentInit();

                //set the parent start transform
                tempParent.rotation = parentTransform.rotation;
                tempParent.position = parentTransform.transform.position;
                tempParent.localScale = Vector3.one; //to prevent scale wandering

                //set the child start transform
                tempChild.rotation = childTransform.rotation;
                tempChild.position = childTransform.position;
                tempChild.localScale = Vector3.one; //to prevent scale wandering

            }

            //translate and rotate the child by moving the parent
            tempParent.rotation = parentTransform.rotation;
            tempParent.position = parentTransform.transform.position;

            //get the child transform
            childRotation = tempChild.rotation;
            childPosition = tempChild.position;
        }

        //With this function you can align a triangle of an object with any transform.
        //Usage: gameObjectInOut is the game object you want to transform.
        //alignWithVector, alignWithNormal, and alignWithPosition is the transform with which the triangle of the object should be aligned with.
        //triangleForward, triangleNormal, and trianglePosition is the transform of the triangle from the object.
        //alignWithVector, alignWithNormal, and alignWithPosition are in world space.
        //triangleForward, triangleNormal, and trianglePosition are in object space.
        //trianglePosition is the mesh position of the triangle. The effect of the scale of the object is handled automatically.
        //trianglePosition can be set at any position, it does not have to be at a vertex or in the middle of the triangle.
        //  public static void PreciseAlign(ref GameObject gameObjectInOut, Vector3 alignWithVector, Vector3 alignWithNormal, Vector3 alignWithPosition, Vector3 triangleForward, Vector3 triangleNormal, Vector3 trianglePosition)
        //  {
        //
        //      //Set the rotation.
        //      LookRotationExtended(ref gameObjectInOut, alignWithVector, alignWithNormal, triangleForward, triangleNormal);
        //
        //      //Get the world space position of trianglePosition
        //      Vector3 trianglePositionWorld = gameObjectInOut.transform.TransformPoint(trianglePosition);
        //
        //      //Get a vector from trianglePosition to alignWithPosition
        //      Vector3 translateVector = alignWithPosition - trianglePositionWorld;
        //
        //      //Now transform the object so the triangle lines up correctly.
        //      gameObjectInOut.transform.Translate(translateVector, Space.World);
        //  }

        /// <summary>
        /// Convert a position, direction, and normal vector to a transform,
        /// which makes the axis of normalVector towards directionVector
        /// all directions are in world space
        /// </summary>
        /// <param name="go">Go.</param>
        /// <param name="position">position</param>
        /// <param name="directionVector">Direction vector</param>
        /// <param name="normalVector">Direction the object facing when rotation set to 0</param>
        public static void VectorsToTransform(ref GameObject go, Vector3 position, Vector3 directionVector, Vector3 normalVector)
        {
            go.transform.position = position;
            go.transform.rotation = Quaternion.LookRotation(directionVector, normalVector);
        }

        /// <summary>
        /// This function finds out on which side of a line segment the point is located.
        /// The point is assumed to be on a line created by linePoint1 and linePoint2. If the point is not on
        /// the line segment, project it on the line using ProjectPointOnLine() first.
        /// </summary>
        /// <returns>
        /// Returns 0 if point is on the line segment.
        /// Returns 1 if point is outside of the line segment and located on the side of linePoint1.
        /// Returns 2 if point is outside of the line segment and located on the side of linePoint2.
        /// </returns>
        /// <param name="lineSegment">Line segment.</param>
        /// <param name="point">Point.</param>
        public static int PointOnWhichSideOfLineSegment(LineSegment lineSegment, Vector3 point)
        {
            Vector3 linePoint1 = lineSegment.point1;
            Vector3 linePoint2 = lineSegment.point2;

            Vector3 lineVec = linePoint2 - linePoint1;
            Vector3 pointVec = point - linePoint1;

            float dot = Vector3.Dot(pointVec, lineVec);

            //point is on side of linePoint2, compared to linePoint1
            if (dot > 0)
            {

                //point is on the line segment
                if (pointVec.magnitude <= lineVec.magnitude)
                {

                    return 0;
                }

                //point is not on the line segment and it is on the side of linePoint2
                else
                {

                    return 2;
                }
            }

            //Point is not on side of linePoint2, compared to linePoint1.
            //Point is not on the line segment and it is on the side of linePoint1.
            else
            {

                return 1;
            }
        }

        /// <summary>
        /// Returns the pixel distance from the mouse pointer to a line.
        /// Alternative for HandleUtility.DistanceToLine(). Works both in Editor mode and Play mode.
        /// Do not call this function from OnGUI() as the mouse position will be wrong.
        /// </summary>
        /// <returns>The pixel number between mousePosition and line.</returns>
        /// <param name="linePoint1">Line point1.</param>
        /// <param name="linePoint2">Line point2.</param>
        public static float MouseToLinePixelDistance(Vector3 linePoint1, Vector3 linePoint2)
        {

            Camera currentCamera;
            Vector3 mousePosition;

#if UNITY_EDITOR
            if (Camera.current != null)
            {

                currentCamera = Camera.current;
            }

            else
            {

                currentCamera = Camera.main;
            }

            //convert format because y is flipped
            mousePosition = new Vector3(Event.current.mousePosition.x, currentCamera.pixelHeight - Event.current.mousePosition.y, 0f);

#else
        currentCamera = Camera.main;
        mousePosition = Input.mousePosition;
#endif

            Vector3 screenPos1 = currentCamera.WorldToScreenPoint(linePoint1);
            Vector3 screenPos2 = currentCamera.WorldToScreenPoint(linePoint2);
            Vector3 projectedPoint = ProjectPointOnLineSegment(new LineSegment(linePoint1, linePoint2), mousePosition);

            //set z to zero
            projectedPoint = new Vector3(projectedPoint.x, projectedPoint.y, 0f);

            Vector3 vector = projectedPoint - mousePosition;
            return vector.magnitude;
        }

        /// <summary>
        /// Returns the pixel distance from the mouse pointer to a camera facing circle.
        /// Alternative for HandleUtility.DistanceToCircle(). Works both in Editor mode and Play mode.
        /// Do not call this function from OnGUI() as the mouse position will be wrong.
        /// If you want the distance to a point instead of a circle, set the radius to 0.
        /// </summary>
        /// <returns>the pixel distance from the mouse pointer to a circle.</returns>
        /// <param name="circleCenter">Center of a camera facing circle on the screen.</param>
        /// <param name="radius">Circle's radius.</param>
        public static float MouseDistanceToCircle(Vector3 circleCenter, float radius)
        {

            Camera currentCamera;
            Vector3 mousePosition;

#if UNITY_EDITOR
            if (Camera.current != null)
            {

                currentCamera = Camera.current;
            }

            else
            {

                currentCamera = Camera.main;
            }

            //convert format because y is flipped
            mousePosition = new Vector3(Event.current.mousePosition.x, currentCamera.pixelHeight - Event.current.mousePosition.y, 0f);
#else
        currentCamera = Camera.main;
        mousePosition = Input.mousePosition;
#endif

            Vector3 screenPos = currentCamera.WorldToScreenPoint(circleCenter);

            //set z to zero
            screenPos = new Vector3(screenPos.x, screenPos.y, 0f);

            Vector3 vector = screenPos - mousePosition;
            float fullDistance = vector.magnitude;
            float circleDistance = fullDistance - radius;

            return circleDistance;
        }

        /// <summary>
        /// Returns true if a line segment is fully or partially in a rectangle
        /// made up of RectA to RectD. The line segment is assumed to be on the same plane as the rectangle. 
        /// </summary>
        /// <returns><c>true</c> if line is fully or partially in rectangle, otherwise, <c>false</c>.</returns>
        /// <param name="lineSegment">Line segment.</param>
        /// <param name="rectA">Rect a.</param>
        /// <param name="rectB">Rect b.</param>
        /// <param name="rectC">Rect c.</param>
        /// <param name="rectD">Rect d.</param>
        public static bool IsLineInRectangle(LineSegment lineSegment, Vector3 rectA, Vector3 rectB, Vector3 rectC, Vector3 rectD)
        {
            Plane plane = new Plane(rectA, Vector3.Cross(rectA - rectD, rectB - rectC));
            //as long as one point is not on the plane,its not in rectangle
            if (!IsPointOnPlane(lineSegment.point1, plane) || !IsPointOnPlane(lineSegment.point2, plane))
            {
                return false;
            }

            bool pointAInside = false;
            bool pointBInside = false;

            pointAInside = IsPointInRectangle(lineSegment.point1, rectA, rectC, rectB, rectD);

            if (!pointAInside)
            {

                pointBInside = IsPointInRectangle(lineSegment.point2, rectA, rectC, rectB, rectD);
            }

            //none of the points are inside, so check if a line is crossing
            if (!pointAInside && !pointBInside)
            {

                bool lineACrossing = AreLineSegmentsCrossing(lineSegment, new LineSegment(rectA, rectB));
                bool lineBCrossing = AreLineSegmentsCrossing(lineSegment, new LineSegment(rectB, rectC));
                bool lineCCrossing = AreLineSegmentsCrossing(lineSegment, new LineSegment(rectC, rectD));
                bool lineDCrossing = AreLineSegmentsCrossing(lineSegment, new LineSegment(rectD, rectA));

                if (lineACrossing || lineBCrossing || lineCCrossing || lineDCrossing)
                {

                    return true;
                }

                else
                {

                    return false;
                }
            }

            else
            {

                return true;
            }
        }

        /// <summary>
        /// return true if the point is on the plane and false if not
        /// </summary>
        /// <returns><c>true</c> if is point on plane the specified point plane; otherwise, <c>false</c>.</returns>
        /// <param name="point">Point.</param>
        /// <param name="plane">Plane.</param>
        public static bool IsPointOnPlane(Vector3 point, Plane plane)
        {
            Vector3 vec = (point - plane.point).normalized;
            if (Vector3.Dot(vec, plane.normal) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if "point" is in a rectangle mad up of RectA to RectD. The line point is assumed to be on the same 
        /// plane as the rectangle. 
        /// </summary>
        /// <returns><c>true</c> if is point in rectangle the specified point rectA rectC rectB rectD; otherwise, <c>false</c>.</returns>
        /// <param name="point">Point.</param>
        /// <param name="rectA">Rect point1</param>
        /// <param name="rectC">Rect point2</param>
        /// <param name="rectB">Rect point3</param>
        /// <param name="rectD">Rect point4</param>
        public static bool IsPointInRectangle(Vector3 point, Vector3 rectA, Vector3 rectC, Vector3 rectB, Vector3 rectD)
        {
            Plane plane = new Plane(rectA, Vector3.Cross(rectA - rectD, rectB - rectC));
            //if the point is not on the plane,its not in rectangle
            if (IsPointOnPlane(point, plane))
            {
                return false;
            }

            Vector3 vector;
            Vector3 linePoint;

            //get the center of the rectangle
            vector = rectC - rectA;
            float size = -(vector.magnitude / 2f);
            vector = AddVectorLength(vector, size);
            Vector3 middle = rectA + vector;

            Vector3 xVector = rectB - rectA;
            float width = xVector.magnitude / 2f;

            Vector3 yVector = rectD - rectA;
            float height = yVector.magnitude / 2f;

            linePoint = ProjectPointOnLine(middle, new Line(point, xVector.normalized));
            vector = linePoint - point;
            float yDistance = vector.magnitude;

            linePoint = ProjectPointOnLine(middle, new Line(point, yVector.normalized));
            vector = linePoint - point;
            float xDistance = vector.magnitude;

            if ((xDistance <= width) && (yDistance <= height))
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if line segment made up of pointA1 and pointA2 is crossing line segment made up of
        /// pointB1 and pointB2. The two lines are assumed to be in the same plane.
        /// </summary>
        /// <returns><c>true</c>, if line segments crossing , <c>false</c> otherwise.</returns>
        /// <param name="lineSegment1">Line segment1.</param>
        /// <param name="lineSegment2">Line segment2.</param>
        public static bool AreLineSegmentsCrossing(LineSegment lineSegment1, LineSegment lineSegment2)
        {

            Vector3 closestPointA;
            Vector3 closestPointB;
            int sideA;
            int sideB;

            //find out if two lines are parallel
            bool valid = ClosestPointsOnTwoLines(out closestPointA, out closestPointB, LineSegmentToLine(lineSegment1), LineSegmentToLine(lineSegment2));

            if (valid)
            {

                sideA = PointOnWhichSideOfLineSegment(lineSegment1, closestPointA);
                sideB = PointOnWhichSideOfLineSegment(lineSegment2, closestPointB);

                if ((sideA == 0) && (sideB == 0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //lines are parallel
                return false;
            }
        }
    }
}
