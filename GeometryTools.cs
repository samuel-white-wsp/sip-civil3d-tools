using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System.ComponentModel;
using Autodesk.AutoCAD.Colors;
using Autodesk.Civil.Runtime;
using Autodesk.Civil.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil;

namespace SIP_Civil3D_Tools
{
    public static class GeometryTools
    {
        public static void NearestParallelLine()
        {
            var ed = Active.Editor;
            var db = Active.Database;

            var peo = new PromptEntityOptions("\n Select query line");
            peo.SetRejectMessage("\nRequires a line,");
            peo.AddAllowedClass(typeof(FeatureLine), false);
            var per = ed.GetEntity(peo);
            if (per.Status != PromptStatus.OK)
                return;

            
            Active.UsingTransaction(tr =>
            {
                var lineId = per.ObjectId;
                var block = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForRead);
                var selected = (FeatureLine)tr.GetObject(per.ObjectId, OpenMode.ForRead);
                var items = block.Cast<ObjectId>()
                    .Where(id => id.ObjectClass.IsDerivedFrom(featureLineClass) && id != lineId)
                    .Select(id => (FeatureLine)tr.GetObject(id, OpenMode.ForRead))
                    .Select(line => new { FeatureLine = line, Offset = GetClosestFeatureLineDistance(selected, line) })
                    .Where(item => item.Offset > 0.0);
                if (items.Any())
                {
                    var result = items.Aggregate((a, b) => a.Offset < b.Offset ? a : b);
                    ed.SetImpliedSelection(new[] { result.FeatureLine.ObjectId });
                    ed.WriteMessage("\nDistance to nearest line: {0}", Converter.DistanceToString(Math.Sqrt(result.Offset)));
                }
                else
                    ed.WriteMessage("\nNo lines matching query criteria were found.");

            });
            
            
            
        }

        static RXClass lineClass = RXObject.GetClass(typeof(Line));
        static RXClass featureLineClass = RXObject.GetClass(typeof(FeatureLine));


        static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source)
        {
            using (var e = source.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    for (var value = e.Current; e.MoveNext(); value = e.Current)
                    {
                        yield return value;
                    }
                }
            }
        }

        public static LineSegment3d[] GetLineSegmentsFromPoints(Point3dCollection points)
        {

            //var enum1 = points.GetEnumerator();
            //enum1.SkipLast

            Point3dCollection startPoints = points;
            startPoints.RemoveAt(startPoints.Count - 1);

            Point3dCollection endPoints = points;
            endPoints.RemoveAt(0);

            return null;

        }



        public static double GetClosestFeatureLineDistance(FeatureLine a, FeatureLine b)
        {
            double offset = -1;
            try
            {
                var curve_a = a.Spline.GetGeCurve();
                var curve_b = b.Spline.GetGeCurve();
                offset = curve_a.GetDistanceTo(curve_b);
                
                return offset;
            }
            catch
            {
                return offset;
            }

        }

        public static double GetLineOffset(Line a, Line b)
        {
            Point2d a1 = AsPoint2d(a.StartPoint);
            Point2d a2 = AsPoint2d(a.EndPoint);
            Point2d b1 = AsPoint2d(b.StartPoint);
            Point2d b2 = AsPoint2d(b.EndPoint);

            double result = -1.0;

            // reject lines with coincident endpoints
            if (a1.IsEqualTo(b1) || a1.IsEqualTo(b2) || a2.IsEqualTo(b1) || a2.IsEqualTo(b2))
                return result;

            var line1 = new LineSegment2d(a1, a2);
            var line2 = new LineSegment2d(b1, b2);

            // reject non-parallel lines
            if (!line1.IsParallelTo(line2))
                return result;

            // reject colinear lines
            if (line1.IsColinearTo(line2))
                return result;

            var cp = line1.GetClosestPointTo(line2);
            Point2d cp1 = cp[0].Point;
            Point2d cp2 = cp[1].Point;

            Vector2d vx = cp1.GetVectorTo(cp2);

            // reject non-adjacent lines:
            if (vx.IsPerpendicularTo(line1.Direction))
                result = vx.LengthSqrd;

            return result;
        }

        public static Point2d AsPoint2d(this Point3d p)
        {
            return new Point2d(p.X, p.Y);
        }

        public static Point3d GetOffsetPoint(Transaction tr, Alignment alignment, double chainage, double offset)
        {
            double alignmentLength = alignment.Length;
            double chainageRatio = chainage / alignmentLength;


            ObjectId alignmentCopyId = ObjectHelper.CopyObjects(tr, alignment.ObjectId);


            Alignment alignmentCopy = (Alignment)alignmentCopyId.GetObject(OpenMode.ForRead);

            ObjectId offsetAlignmentId = alignmentCopy.CreateOffsetAlignment(offset);
            Alignment offsetAlignment = (Alignment)offsetAlignmentId.GetObject(OpenMode.ForRead);
            Point3d offsetPoint = offsetAlignment.GetPointAtParameter(chainageRatio);

            alignmentCopy.Erase();
            offsetAlignment.Erase();
            return offsetPoint;
        }


    }

    

    }
