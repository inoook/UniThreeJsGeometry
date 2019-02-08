﻿/**
 * @author zz85 / http://www.lab4games.net/zz85/blog
 *
 * Creates extruded geometry from a path shape.
 *
 * parameters = {
 *
 *  curveSegments: <int>, // number of points on the curves
 *  steps: <int>, // number of points for z-side extrusions / used for subdividing segements of extrude spline too
 *  amount: <int>, // Depth to extrude the shape
 *
 *  bevelEnabled: <bool>, // turn on bevel
 *  bevelThickness: <float>, // how deep into the original shape bevel goes
 *  bevelSize: <float>, // how far from shape outline is bevel
 *  bevelSegments: <int>, // number of bevel layers
 *
 *  extrudePath: <THREE.CurvePath> // 3d spline path to extrude shape along. (creates Frames if .frames aren't defined)
 *  frames: <THREE.TubeGeometry.FrenetFrames> // containing arrays of tangents, normals, binormals
 *
 *  material: <int> // material index for front and back faces
 *  extrudeMaterial: <int> // material index for extrusion and beveled faces
 *  uvGenerator: <Object> // object that provides UV generator functions
 *
 * }
 **/
// https://github.com/mrdoob/three.js/blob/master/src/extras/geometries/ExtrudeGeometry.js

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{

    public class ExtrudeGeometry : Geometry
    {
        public class Option
        {
            public int amount = 100;// pathを指定しない時の押し出し量

            public float bevelThickness = 6;
            public float bevelSize = 4;
            public int bevelSegments = 3;
            public bool bevelEnabled = false;

            public int steps = 1;// 押し出しの分割数

            public Curve extrudePath;
            public IUVGenerator uvGenerator;
            public FrenetFrames frames;

            public AnimationCurve thicknessCurve;

        }


        public FrenetFrames splineTube = null;
        public BoundingBox shapebb;

        public ExtrudeGeometry() {
            vertIndex = 0;
        }

        public ExtrudeGeometry(Shape shape, Option options) {
            vertIndex = 0;
            //this.shapebb = shape.getBoundingBox();
            this.addShape(shape, options);

            //// shapeにnormalが無いときは計算を行う。 shapeにnormalを設定できるように変更した。
            //this.computeFaceNormals();
            //this.copyFaceNormalToVertexNormals();
        }

        void ExtractShape(Shape shape) {
            List<Vector3> ahole;

            shape.extractPoints();

            List<Vector3> shapeVertices = shape.shapeVertices;
            List<List<Vector3>> holes = shape.holesList;

            bool reverse = !Shape.UtilsShape.isClockWise(shapeVertices);
            if (reverse) {
                shapeVertices.Reverse();

                for (int h = 0; h < holes.Count; h++) {
                    ahole = holes[h];

                    if (Shape.UtilsShape.isClockWise(ahole)) {
                        ahole.Reverse();
                        holes[h] = ahole;
                    }
                }
            }

            shape.holesList = holes;
            shape.reverse = reverse;
        }

        void addShape(Shape shape, Option options) {

            ExtractShape(shape);

            float bevelThickness = options.bevelThickness;
            float bevelSize = options.bevelSize;
            int bevelSegments = options.bevelSegments;
            bool bevelEnabled = options.bevelEnabled;

            int steps = options.steps;

            Curve extrudePath = options.extrudePath;
            bool extrudeByPath = (extrudePath != null);

            // Use default WorldUVGenerator if no UV generators are specified.
            IUVGenerator uvgen = options.uvGenerator;
            if (uvgen == null) {
                uvgen = new WorldUVGenerator();
            }

            List<Vector3> extrudePts = new List<Vector3>();
            Vector3 binormal, normal, position2;

            bool isClosePath = false;

            if (extrudeByPath) {
                extrudePts = extrudePath.getSpacedPoints(steps); // pathの分割点配列を取得
                if (bevelEnabled) {
                    Debug.LogWarning("bevels not supported for path extrusion");
                }
                bevelEnabled = false; // bevels not supported for path extrusion

                isClosePath = (extrudePath.GetType() == typeof(ClosedSplineCurve3)); // add inok

                // SETUP TNB variables
                // Reuse TNB from TubeGeomtry for now.
                splineTube = new FrenetFrames(extrudePath, steps, isClosePath);

                //binormal = new Vector3();
                //normal = new Vector3();
                //position2 = new Vector3();
            }
            else {
                Debug.Log("no extrudePath. automatic in z direction.");
            }

            // Variables initalization
            //var ahole, h, hl; // looping of holes
            List<Vector3> ahole;
            List<Vector3> shapeVertices = shape.shapeVertices; // shapeの頂点座標
            List<List<Vector3>> holes = shape.holesList;
            bool reverse = shape.reverse;

            // shapeVertices から face 用のindexリスト取得
            List<int[]> faces = Shape.UtilsShape.triangulateShape(shapeVertices, holes);

            /* Vertices */
            //List<Vector3> contour = vertices; // vertices has all points but contour has only points of circumference
            List<Vector3> contour = new List<Vector3>(); // 上記だとholeが上手くいかない。改良の余地あり
            contour.AddRange(shapeVertices);
            for (int h = 0, hl = holes.Count; h < hl; h++) {
                ahole = holes[h];
                shapeVertices.AddRange(ahole);
            }

            // normalが無いときに簡易的に計算
            shape.CalculateNormalTangents();

            float t;
            float z, bs;

            Vector3 vert;
            int vlen = shapeVertices.Count;
            int flen = faces.Count;

            List<Vector3> contourMovements = new List<Vector3>();
            for (int i = 0, il = contour.Count, j = il - 1, k = i + 1; i < il; i++, j++, k++) {
                if (j == il) j = 0;
                if (k == il) k = 0;

                //  (j)---(i)---(k)
                Vector3 bevelVec = getBevelVec(contour[i], contour[j], contour[k]);
                contourMovements.Add(bevelVec);
                //contourMovements[ i ] = bevelVec;
            }

            List<List<Vector3>> holesMovements = new List<List<Vector3>>();
            List<Vector3> oneHoleMovements;

            //verticesMovements = contourMovements.concat(); // TODO: Check /////////
            List<Vector3> verticesMovements = new List<Vector3>();
            verticesMovements.AddRange(contourMovements); // COPY????

            //List<Vector3> ahole;
            for (int h = 0, hl = holes.Count; h < hl; h++) {
                ahole = holes[h];
                oneHoleMovements = new List<Vector3>();

                for (int i = 0, il = ahole.Count, j = il - 1, k = i + 1; i < il; i++, j++, k++) {
                    if (j == il) j = 0;
                    if (k == il) k = 0;

                    //  (j)---(i)---(k)
                    //oneHoleMovements[ i ] = getBevelVec( ahole[ i ], ahole[ j ], ahole[ k ] );
                    Vector3 bevelVec = getBevelVec(ahole[i], ahole[j], ahole[k]);
                    oneHoleMovements.Add(bevelVec);
                }

                holesMovements.Add(oneHoleMovements);
                verticesMovements.AddRange(oneHoleMovements);
            }


            bool isUseThicknessCurve = options.thicknessCurve != null;

            // Safeguards if bevels are not enabled
            if (!bevelEnabled) {
                bevelSegments = 0;
                bevelThickness = 0;
                bevelSize = 0;
            }

            // Loop bevelSegments, 1 for the front, 1 for the back
            for (int b = 0; b < bevelSegments; b++) {
                t = (float)b / bevelSegments;
                z = bevelThickness * (1 - t);

                bs = bevelSize * (Mathf.Sin(t * Mathf.PI / 2)); // curved

                // contract shape
                for (int i = 0, il = contour.Count; i < il; i++) {
                    vert = scalePt2(contour[i], contourMovements[i], bs);
                    if (isUseThicknessCurve) {
                        vert *= options.thicknessCurve.Evaluate(0);
                    }
                    addVertex(vert.x, vert.y, -z);
                }

                // expand holes
                for (int h = 0, hl = holes.Count; h < hl; h++) {
                    ahole = holes[h];
                    oneHoleMovements = holesMovements[h];

                    for (int i = 0, il = ahole.Count; i < il; i++) {
                        vert = scalePt2(ahole[i], oneHoleMovements[i], bs);
                        if (isUseThicknessCurve) {
                            vert *= options.thicknessCurve.Evaluate(0);
                        }
                        addVertex(vert.x, vert.y, -z);
                    }
                }
            }
            bs = bevelSize;

            // Back facing vertices
            for (int i = 0; i < vlen; i++) {
                vert = bevelEnabled ? scalePt2(shapeVertices[i], verticesMovements[i], bs) : shapeVertices[i];
                if (isUseThicknessCurve) {
                    vert *= options.thicknessCurve.Evaluate(0);
                }
                if (!extrudeByPath) {
                    addVertex(vert.x, vert.y, 0);
                }
                else {
                    normal = splineTube.normals[0] * (vert.x);
                    binormal = splineTube.binormals[0] * (vert.y);
                    position2 = (extrudePts[0]) + (normal) + (binormal);
                    addVertex(position2.x, position2.y, position2.z);
                }
            }

            // Add stepped vertices...
            // Including front facing vertices
            int amount = options.amount;
            for (int s = 1; s <= steps; s++) { // extrudeの回数
                for (int i = 0; i < vlen; i++) { // shapeの頂点数
                    vert = bevelEnabled ? scalePt2(shapeVertices[i], verticesMovements[i], bs) : shapeVertices[i];
                    if (isUseThicknessCurve) {
                        vert *= options.thicknessCurve.Evaluate(s / (float)steps);
                    }
                    // パスに沿ったgeometryを追加していく ----------
                    if (!extrudeByPath) {
                        // Z方向に自動で extrude
                        addVertex(vert.x, vert.y, (float)amount / steps * s);
                    }
                    else {
                        normal = splineTube.normals[s] * (vert.x);
                        binormal = (splineTube.binormals[s]) * (vert.y);
                        position2 = (extrudePts[s]) + (normal) + (binormal);
                        addVertex(position2.x, position2.y, position2.z);
                    }
                }
            }

            // Add bevel segments planes
            for (int b = bevelSegments - 1; b >= 0; b--) {

                t = (float)b / bevelSegments;
                z = bevelThickness * (1.0f - t);
                bs = bevelSize * Mathf.Sin(t * Mathf.PI / 2.0f);

                // contract shape
                for (int i = 0, il = contour.Count; i < il; i++) {
                    vert = scalePt2(contour[i], contourMovements[i], bs);
                    if (isUseThicknessCurve) {
                        vert *= options.thicknessCurve.Evaluate(1);
                    }
                    addVertex(vert.x, vert.y, (float)amount + z);
                }

                // expand holes
                for (int h = 0, hl = holes.Count; h < hl; h++) {

                    ahole = holes[h];
                    oneHoleMovements = holesMovements[h];

                    for (int i = 0, il = ahole.Count; i < il; i++) {
                        vert = scalePt2(ahole[i], oneHoleMovements[i], bs);
                        if (isUseThicknessCurve) {
                            vert *= options.thicknessCurve.Evaluate(1);
                        }
                        if (!extrudeByPath) {
                            // Z方向に自動で extrude
                            addVertex(vert.x, vert.y, (float)amount + z);
                        }
                        else {
                            addVertex(vert.x, vert.y + extrudePts[steps - 1].y, extrudePts[steps - 1].x + z);
                        }
                    }
                }
            }


            /* Faces */
            // 頂点とfaceの割り当て（面の設定）
            faceIndex = 0;
            // Top and bottom faces
            if (!isClosePath) {
                buildLidFaces(vlen, faces, flen, steps, bevelSegments, bevelEnabled, shape, uvgen, options, reverse);
            }
            // Sides faces
            buildSideFaces(vlen, contour, holes, steps, bevelSegments, uvgen, shape, options, isClosePath, reverse);
        }

        Vector3 scalePt2(Vector3 pt, Vector3 vec, float size) {
            return (vec * size) + (pt);
        }

        Vector3 getBevelVec(Vector3 inPt, Vector3 inPrev, Vector3 inNext) {

            float EPSILON = THREE.Setting.EPSILON;

            // computes for inPt the corresponding point inPt' on a new contour
            //   shiftet by 1 unit (length of normalized vector) to the left
            // if we walk along contour clockwise, this new contour is outside the old one
            //
            // inPt' is the intersection of the two lines parallel to the two
            //  adjacent edges of inPt at a distance of 1 unit on the left side.

            float v_trans_x, v_trans_y, shrink_by = 1;      // resulting translation vector for inPt

            // good reading for geometry algorithms (here: line-line intersection)
            // http://geomalgorithms.com/a05-_intersect-1.html

            float v_prev_x = inPt.x - inPrev.x;
            float v_prev_y = inPt.y - inPrev.y;
            float v_next_x = inNext.x - inPt.x;
            float v_next_y = inNext.y - inPt.y;

            float v_prev_lensq = (v_prev_x * v_prev_x + v_prev_y * v_prev_y);
            float v_nextv_lensq = (v_next_x * v_next_x + v_next_y * v_next_y);// add inok

            // check for colinear edges
            float colinear0 = (v_prev_x * v_next_y - v_prev_y * v_next_x);

            if (Mathf.Abs(colinear0) > EPSILON) {       // not colinear

                // length of vectors for normalizing

                float v_prev_len = Mathf.Sqrt(v_prev_lensq);
                float v_next_len = Mathf.Sqrt(v_nextv_lensq);

                // shift adjacent points by unit vectors to the left

                float ptPrevShift_x = (inPrev.x - v_prev_y / v_prev_len);
                float ptPrevShift_y = (inPrev.y + v_prev_x / v_prev_len);

                float ptNextShift_x = (inNext.x - v_next_y / v_next_len);
                float ptNextShift_y = (inNext.y + v_next_x / v_next_len);

                // scaling factor for v_prev to intersection point

                float sf = ((ptNextShift_x - ptPrevShift_x) * v_next_y - (ptNextShift_y - ptPrevShift_y) * v_next_x) / (v_prev_x * v_next_y - v_prev_y * v_next_x);

                // vector from inPt to intersection point

                v_trans_x = (ptPrevShift_x + v_prev_x * sf - inPt.x);
                v_trans_y = (ptPrevShift_y + v_prev_y * sf - inPt.y);

                // Don't normalize!, otherwise sharp corners become ugly
                //  but prevent crazy spikes
                float v_trans_lensq = (v_trans_x * v_trans_x + v_trans_y * v_trans_y);
                if (v_trans_lensq <= 2) {
                    return new Vector2(v_trans_x, v_trans_y);
                }
                else {
                    shrink_by = Mathf.Sqrt(v_trans_lensq / 2.0f);
                }

            }
            else {       // handle special case of colinear edges

                bool direction_eq = false;      // assumes: opposite
                if (v_prev_x > EPSILON) {
                    if (v_next_x > EPSILON) { direction_eq = true; }
                }
                else {
                    if (v_prev_x < -EPSILON) {
                        if (v_next_x < -EPSILON) { direction_eq = true; }
                    }
                    else {
                        if (Mathf.Sign(v_prev_y) == Mathf.Sign(v_next_y)) { direction_eq = true; }
                    }
                }

                if (direction_eq) {
                    // console.log("Warning: lines are a straight sequence");
                    v_trans_x = -v_prev_y;
                    v_trans_y = v_prev_x;
                    shrink_by = Mathf.Sqrt(v_prev_lensq);
                }
                else {
                    // console.log("Warning: lines are a straight spike");
                    v_trans_x = v_prev_x;
                    v_trans_y = v_prev_y;
                    shrink_by = Mathf.Sqrt(v_prev_lensq / 2.0f);
                }

            }

            float t_x = v_trans_x / shrink_by;
            float t_y = v_trans_y / shrink_by;

            if (float.IsNaN(t_x)) { t_x = 0; }
            if (float.IsNaN(t_y)) { t_y = 0; }

            return new Vector3(t_x, t_y, 0);
        }

        /////  Internal functions
        void buildLidFaces(int vlen, List<int[]> faces, int flen, int steps, int bevelSegments, bool bevelEnabled, Shape shape, IUVGenerator uvgen, Option options, bool reverse = false) {

            if (bevelEnabled) {

                int layer = 0; // steps + 1
                int offset = vlen * layer;

                // Bottom faces
                for (int i = 0; i < flen; i++) {

                    int[] face = faces[i];
                    f3(face[2] + offset, face[1] + offset, face[0] + offset, shape, true, uvgen, options);
                }

                layer = steps + bevelSegments * 2;
                offset = vlen * layer;

                // Top faces
                for (int i = 0; i < flen; i++) {
                    int[] face = faces[i];
                    f3(face[0] + offset, face[1] + offset, face[2] + offset, shape, false, uvgen, options);
                }

            }
            else {
                // Bottom faces
                for (int i = 0; i < flen; i++) {
                    int[] face = faces[i];
                    f3(face[2], face[1], face[0], shape, true, uvgen, options);
                }

                // Top faces
                for (int i = 0; i < flen; i++) {
                    int[] face = faces[i];
                    f3(face[0] + vlen * steps, face[1] + vlen * steps, face[2] + vlen * steps, shape, false, uvgen, options);
                }
            }

        }

        // Create faces for the z-sides of the shape
        void buildSideFaces(int vlen, List<Vector3> contour, List<List<Vector3>> holes, int steps, int bevelSegments, IUVGenerator uvgen, Shape shape, Option options, bool closePath = false, bool reverse = true) {

            int layeroffset = 0;
            sidewalls(vlen, contour, layeroffset, steps, bevelSegments, uvgen, shape, options, closePath, reverse);
            layeroffset += contour.Count;

            // holes sideWall
            for (int h = 0, hl = holes.Count; h < hl; h++) {
                List<Vector3> ahole = holes[h];
                sidewalls(vlen, ahole, layeroffset, steps, bevelSegments, uvgen, shape, options, closePath, reverse);
                layeroffset += ahole.Count;
            }
        }

        void sidewalls(int vlen, List<Vector3> contour, int layeroffset, int steps, int bevelSegments, IUVGenerator uvgen, Shape shape, Option options, bool closePath = false, bool reverse = true) {

            int j, k;

            //TubeGeometry.FrenetFrames splineTube = options.frames;

            // pre compute Quat
            int sl = steps + bevelSegments * 2;
            Quaternion[] normalQuats = new Quaternion[sl + 1];
            if (splineTube != null) {
                // splineTube による描画
                for (int i = 0; i < normalQuats.Length; i++) {
                    normalQuats[i] = Quaternion.LookRotation(splineTube.tangents[i], splineTube.normals[i]) * Quaternion.AngleAxis(90, Vector3.forward);
                }
            }

            for (int i = 0; i < contour.Count; i++) {
                j = i;
                k = i - 1;
                if (k < 0) k = contour.Count - 1;

                int s = 0;
                //int sl = steps + bevelSegments * 2;
                for (s = 0; s < sl; s++) {

                    int slen1 = vlen * s;
                    int slen2 = vlen * (s + 1);

                    if (closePath && s == steps - 1) {
                        // close path end
                        slen1 = vlen * (sl - 1);
                        slen2 = 0;
                    }

                    int a = layeroffset + j + slen1;
                    int b = layeroffset + k + slen1;
                    int c = layeroffset + k + slen2;
                    int d = layeroffset + j + slen2;

                    //
                    int stepIndex = s;
                    int stepsLength = sl;
                    Quaternion tQ0;
                    Quaternion tQ1;
                    if (splineTube != null) {
                        tQ0 = normalQuats[stepIndex];
                        int id = stepIndex + 1;
                        if (id > stepsLength) {
                            id = 0;
                        }
                        tQ1 = normalQuats[id];
                    }
                    else {
                        tQ0 = Quaternion.identity;
                        tQ1 = Quaternion.identity;
                    }
                    f4(a, b, c, d, shape, contour, s, sl, j, k, uvgen, options, reverse, tQ0, tQ1);
                }
            }
        }
        //		

        int vertIndex = 0;
        void addVertex(float x, float y, float z) {

            Vector3 vertex = new Vector3(x, y, z);
            //			this.vertices.Add( vertex );

            if (vertIndex >= this.vertices.Count) {
                this.vertices.Add(vertex);
            }
            else {
                this.vertices[vertIndex] = vertex;
            }
            vertIndex++;
        }

        int faceIndex = 0;
        void f3(int a, int b, int c, Shape shape, bool isBottom, IUVGenerator uvgen, Option options) {

            // normal, color, material
            Face3 face;
            if (faceIndex >= faces.Count) {
                face = new Face3(a, b, c);
                this.faces.Add(face);
            }
            else {
                face = faces[faceIndex];
                face.a = a;
                face.b = b;
                face.c = c;
            }
            faceIndex++;

            //TubeGeometry.FrenetFrames splineTube = options.frames;
            Vector3 normal;
            if (splineTube != null) {
                int stepIndex = isBottom ? 0 : splineTube.tangents.Length - 1;
                normal = splineTube.tangents[stepIndex] * (isBottom ? -1 : 1);
            }
            else {
                normal = (isBottom ? -1 : 1) * Vector3.forward;
            }
            face.vertexNormals = new Vector3[] { normal, normal, normal };

            Vector2[] uvs = !isBottom ? uvgen.generateBottomUV(this, shape.shapeVertices, a, b, c) : uvgen.generateTopUV(this, shape.shapeVertices, face.a, b, c);
            face.uvs = uvs;
        }

        void f4(int a, int b, int c, int d, Shape shape, List<Vector3> wallContour, int stepIndex, int stepsLength, int contourIndex1, int contourIndex2, IUVGenerator uvgen, Option options, bool reverse, Quaternion tQ0, Quaternion tQ1) {

            Vector2[] uvs = uvgen.generateSideWallUV(this, shape.shapeVertices, wallContour, a, b, c, d,
                stepIndex, stepsLength, contourIndex1, contourIndex2);

            Face3 face0;
            if (faceIndex >= faces.Count) {
                face0 = new Face3(a, b, d);
                this.faces.Add(face0);
            }
            else {
                face0 = faces[faceIndex];
                face0.a = a;
                face0.b = b;
                face0.c = d;
            }

            face0.uvs = new Vector2[] { uvs[0], uvs[1], uvs[3] };
            faceIndex++;

            Face3 face1;
            if (faceIndex >= faces.Count) {
                face1 = new Face3(b, c, d);
                this.faces.Add(face1);
            }
            else {
                face1 = faces[faceIndex];
                face1.a = b;
                face1.b = c;
                face1.c = d;
            }

            face1.uvs = new Vector2[] { uvs[1], uvs[2], uvs[3] };
            faceIndex++;

            // 法線の調整。
            Vector3 normal0;
            Vector3 normal1;

            //TubeGeometry.FrenetFrames splineTube = options.frames;
            if (shape.normals != null) {
                if (splineTube != null) {
                    Vector3 n0 = shape.normals[contourIndex1];
                    if (reverse) {
                        n0.y *= -1;
                    }
                    normal0 = tQ0 * n0;
                    normal0.Normalize();

                    Vector3 n1 = shape.normals[contourIndex1];
                    if (reverse) {
                        n1.y *= -1;
                    }
                    normal1 = tQ1 * n1;
                    normal1.Normalize();

                }
                else {
                    Vector3 norm = shape.normals[contourIndex1];
                    if (reverse) {
                        norm.y *= -1;
                    }
                    normal0 = norm;
                    normal1 = norm;
                }

                face0.vertexNormals[0] = normal0;
                face0.vertexNormals[1] = normal0;
                face0.vertexNormals[2] = normal1;

                face1.vertexNormals[0] = normal0;
                face1.vertexNormals[1] = normal1;
                face1.vertexNormals[2] = normal1;
            }
        }

    }
}
