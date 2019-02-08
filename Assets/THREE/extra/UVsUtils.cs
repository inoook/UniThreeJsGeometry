/*
 * @author gyuque / http://github.com/gyuque
 *
 * Cylinder Mapping for ExtrudeGeometry
 *
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE{

    #region UVGenerator
    /// <summary>
    /// UV Generator.
    /// UV Generator は ExtrudeGeometry, CylinderUVGenerator または ShapeGeometry でのみ使用する。
    /// </summary>

    public interface IUVGenerator
    {
        Vector2[] generateTopUV(THREE.Geometry geometry, List<Vector3> shapeVertices, int indexA, int indexB, int indexC);
        Vector2[] generateBottomUV(THREE.Geometry geometry, List<Vector3> shapeVertices, int indexA, int indexB, int indexC);
        Vector2[] generateSideWallUV(THREE.Geometry geometry, List<Vector3> shapeVertices, List<Vector3> wallContour,
                                         int indexA, int indexB, int indexC, int indexD, int stepIndex, int stepsLength,
                                         int contourIndex1, int contourIndex2);
    }

    //  ----------
    public class WorldUVGenerator : IUVGenerator
    {
        public Vector2[] generateTopUV(THREE.Geometry geometry, List<Vector3> vertices, int indexA, int indexB, int indexC) {
            float ax = geometry.vertices[indexA].x;
            float ay = geometry.vertices[indexA].y;

            float bx = geometry.vertices[indexB].x;
            float by = geometry.vertices[indexB].y;

            float cx = geometry.vertices[indexC].x;
            float cy = geometry.vertices[indexC].y;

            return new Vector2[]{
                        new Vector2( ax, ay ),
                        new Vector2( bx, by ),
                        new Vector2( cx, cy ) };
        }

        public Vector2[] generateBottomUV(THREE.Geometry geometry, List<Vector3> shapeVertices, int indexA, int indexB, int indexC) {
            return this.generateTopUV(geometry, shapeVertices, indexA, indexB, indexC);
        }

        public Vector2[] generateSideWallUV(THREE.Geometry geometry, List<Vector3> shapeVertices, List<Vector3> wallContour,
                                         int indexA, int indexB, int indexC, int indexD, int stepIndex, int stepsLength,
                                         int contourIndex1, int contourIndex2) {

            float ax = geometry.vertices[indexA].x;
            float ay = geometry.vertices[indexA].y;
            float az = geometry.vertices[indexA].z;

            float bx = geometry.vertices[indexB].x;
            float by = geometry.vertices[indexB].y;
            float bz = geometry.vertices[indexB].z;

            float cx = geometry.vertices[indexC].x;
            float cy = geometry.vertices[indexC].y;
            float cz = geometry.vertices[indexC].z;

            float dx = geometry.vertices[indexD].x;
            float dy = geometry.vertices[indexD].y;
            float dz = geometry.vertices[indexD].z;

            if (Mathf.Abs(ay - by) < 0.01f) {
                return new Vector2[]{
                            new Vector2( ax, 1 - az ),
                            new Vector2( bx, 1 - bz ),
                            new Vector2( cx, 1 - cz ),
                            new Vector2( dx, 1 - dz )
                    };
            }
            else {
                return new Vector2[]{
                            new Vector2( ay, 1 - az ),
                            new Vector2( by, 1 - bz ),
                            new Vector2( cy, 1 - cz ),
                            new Vector2( dy, 1 - dz )
                    };
            }
        }
    }

    // ----------
    public class UVGenerator : IUVGenerator
    {
        public float shapeWidth = 0;
        public float shapeHeight = 0;

        public UVGenerator(float shapeWidth = 1, float shapeHeight = 1) {
            this.shapeWidth = shapeWidth;
            this.shapeHeight = shapeHeight;
        }

        public Vector2[] generateTopUV(THREE.Geometry geometry, List<Vector3> shapeVertices, int indexA, int indexB, int indexC) {
            //Debug.Log("Top: " + indexA + ", " + indexB + ", " + indexC);
            float normalizeX = shapeWidth;
            float normalizeY = shapeHeight;

            float ax = shapeVertices[indexA].x / normalizeX + 0.5f;
            float ay = shapeVertices[indexA].y / normalizeY + 0.5f;

            float bx = shapeVertices[indexB].x / normalizeX + 0.5f;
            float by = shapeVertices[indexB].y / normalizeY + 0.5f;

            float cx = shapeVertices[indexC].x / normalizeX + 0.5f;
            float cy = shapeVertices[indexC].y / normalizeY + 0.5f;

            return new Vector2[]{
                        new Vector2( ax, ay ),
                        new Vector2( bx, by ),
                        new Vector2( cx, cy ) };
        }

        public Vector2[] generateBottomUV(THREE.Geometry geometry, List<Vector3> shapeVertices, int indexA, int indexB, int indexC) {
            int count = geometry.vertices.Count - 1;
            indexA = count - indexA;
            indexB = count - indexB;
            indexC = count - indexC;
            int shapeVCount = shapeVertices.Count - 1;
            return this.generateTopUV(geometry, shapeVertices, shapeVCount - indexA, shapeVCount - indexB, shapeVCount - indexC);

            //return new Vector2[]{
            //new Vector2( 0, 1 ),
            //new Vector2( 0, 1 ),
            //new Vector2( 0, 1 ) };
        }

        public Vector2[] generateSideWallUV(THREE.Geometry geometry, List<Vector3> shapeVertices, List<Vector3> wallContour,
                                         int indexA, int indexB, int indexC, int indexD, int stepIndex, int stepsLength,
                                         int contourIndex1, int contourIndex2) {
            //Debug.Log(indexA + ", " + indexB + ", " + indexC + ", " + indexD);
            //Debug.Log(stepIndex + " / " + stepsLength + " // " + contourIndex1 + " / " + contourIndex2 + " // " + wallContour.Count);

            float l = (float)(wallContour.Count - 1);
            float x0 = (float)stepIndex / ((float)stepsLength);
            float x1 = (float)(stepIndex + 1) / ((float)stepsLength);
            return new Vector2[]{
                           new Vector2( x0, (float)contourIndex1/l ),
                           new Vector2( x0, (float)contourIndex2/l ),
                           new Vector2( x1, (float)contourIndex2/l ),
                           new Vector2( x1, (float)contourIndex1/l )
                };

            //return new Vector2[]{
            //        new Vector2( u1, v1 ),
            //        new Vector2( u2, v1 ),
            //        new Vector2( u2, v2 ),
            //        new Vector2( u1, v2 )
            //};

            //return new Vector2[]{
            //           new Vector2( x0, 0.5f),
            //           new Vector2( x0, 0.5f),
            //           new Vector2( x1, 0.5f),
            //           new Vector2( x1, 0.5f)
            //};

            // face to UV
            //return new Vector2[]{
            //           new Vector2( 0, 0),
            //           new Vector2( 1, 0 ),
            //           new Vector2( 1, 1 ),
            //           new Vector2( 0, 1 )
            //};
        }
    }

    // ----------
    // TODO: 処理のチェック
    public class CylinderUVGenerator : IUVGenerator
    {
        int uRepeat = 1;
        Geometry targetGeometry = null;
        List<float> lengthCache = null;

        public Vector2[] generateTopUV(THREE.Geometry geometry, List<Vector3> shapeVertices, int indexA, int indexB, int indexC) {

            return new Vector2[]{
                new Vector2( 0, 1 ),
                new Vector2( 0, 1 ),
                new Vector2( 0, 1 ) };
        }

        public Vector2[] generateBottomUV(THREE.Geometry geometry, List<Vector3> shapeVertices, int indexA, int indexB, int indexC) {

            return new Vector2[]{
                new Vector2( 0, 1 ),
                new Vector2( 0, 1 ),
                new Vector2( 0, 1 ) };
        }

        public Vector2[] generateSideWallUV(Geometry geometry, List<Vector3> shapeVertices, List<Vector3> wallContour,
                                    int indexA, int indexB, int indexC, int indexD, int stepIndex, int stepsLength,
                                     int contourIndex1, int contourIndex2) {
            // first call
            if (this.targetGeometry != geometry) {
                this.prepare(geometry, wallContour);
            }

            // generate uv
            var u_list = this.lengthCache;
            var v1 = stepIndex / stepsLength;
            var v2 = (stepIndex + 1) / stepsLength;

            var u1 = u_list[contourIndex1];
            var u2 = u_list[contourIndex2];
            if (u1 < u2) { u1 += 1.0f; }

            u1 *= this.uRepeat;
            u2 *= this.uRepeat;
            return new Vector2[]{
                        new Vector2( u1, v1 ),
                        new Vector2( u2, v1 ),
                        new Vector2( u2, v2 ),
                        new Vector2( u1, v2 )
                };
        }

        void prepare(Geometry geometry, List<Vector3> wallContour) {
            Vector3 p1, p2;
            List<float> u_list = new List<float>();
            float lengthSum = 0;
            int len = wallContour.Count;
            for (var i = 0; i < len; i++) {
                p1 = wallContour[i];
                p2 = wallContour[(i + 1) % len];

                float dx = p1.x - p2.x;
                float dy = p1.y - p2.y;
                float segmentLength = Mathf.Sqrt(dx * dx + dy * dy);

                u_list.Add(lengthSum);
                lengthSum += segmentLength;
            }

            this.normalizeArray(u_list, lengthSum);
            this.targetGeometry = geometry;
            this.lengthCache = u_list;
        }

        void normalizeArray(List<float> ls, float v) {
            int len = ls.Count;
            for (int i = 0; i < len; i++) {
                ls[i] /= v;
            }
            //return ls;
        }
    }
    #endregion
}
