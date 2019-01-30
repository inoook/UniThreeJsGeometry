using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace THREE
{
    public class SegmentLine : Curve
    {
        List<Vector3> points;

        public SegmentLine(List<Vector3> points)
        {
            this.points = (points == null) ? new List<Vector3>() : points;
        }

        public override Vector3 getPoint(float t)
        {
            float point = (float)(points.Count - 1) * t;
            int index = Mathf.FloorToInt(point);
            Debug.Log(index);
            Vector3 v = points[index];

            return v;
        }

        //public override Vector3 getPointAt(float u)
        //{
        //    return this.getPoint(u);
        //}

        public override List<Vector3> getSpacedPoints(float divisions = 5, bool closedPath = false)
        {
            List<Vector3> pts = new List<Vector3> ();

            pts.Add(points[0]);

            for (int i = 0; i < points.Count; i ++) {
                pts.Add (points[i]);
            }

            //pts.Add(points[points.Count-1]);

            return pts; 
        }

        public override Vector3 getTangentAt(float t)
        {
            float point = (float)(points.Count - 1) * t;
            int index = Mathf.FloorToInt(point);
            Vector3 tan;
            if (index < points.Count - 1)
            {
                tan = points[index + 1] - points[index];
            }else{
                tan = points[index] - points[index-1];
            }
            return tan;

        }
    }
}