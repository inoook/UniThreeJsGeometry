using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{
    [System.Serializable]
    public class Geometry
    {
        public List<Face3> faces;
        public List<Vector3> vertices;
        public List<Vector3> normals;
        public List<Color> verticexColors;

        private UnityEngine.Mesh mesh;

        public List<Vector3> uni_vertices;
        public List<int> uni_triangle;
        public List<Vector2> uni_uvs;
        public List<Vector3> uni_normals;

        public Geometry() {
            faces = new List<Face3>();
            vertices = new List<Vector3>();
            normals = new List<Vector3>();

            verticexColors = new List<Color>();
        }

        //public UnityEngine.Mesh CreateMesh(float smoothAngle = 0)
        public UnityEngine.Mesh CreateMesh() {

            UpdateUnityMeshInfo();

            UnityEngine.Mesh mesh = new UnityEngine.Mesh();
            mesh.vertices = uni_vertices.ToArray();
            mesh.triangles = uni_triangle.ToArray();
            mesh.uv = uni_uvs.ToArray();
            mesh.normals = uni_normals.ToArray();

            return mesh;
        }


        public void UpdateUnityMeshInfo() {

            uni_vertices = new List<Vector3>();
            uni_triangle = new List<int>();
            uni_uvs = new List<Vector2>();
            uni_normals = new List<Vector3>();

            bool isUvsActive = true;
            int index = 0;
            for (int i = 0; i < faces.Count; i++) {
                Face3 face = faces[i];
                int[] indexes = face.GetVertexIndexList();
                Vector2[] uvs = face.GetUvs();
                Vector3[] normals = face.GetVertexNormals();

                isUvsActive = (uvs != null);
                for (int n = 0; n < indexes.Length; n++) {
                    int v_id = indexes[n]; // v_id: 頂点のid

                    // uvs
                    Vector2 uv;
                    if (isUvsActive) {
                        uv = uvs[n];
                        uv.x = 1.0f - uv.x; // flip x
                    } else {
                        uv = new Vector2();
                    }
                    uni_uvs.Add(uv);

                    // vertex
                    Vector3 vec = vertices[v_id];
                    uni_vertices.Add(vec);

                    // normal
                    Vector3 norm = normals[n];
                    Vector3 newNorm = norm;
                    uni_normals.Add(newNorm);
                    
                    // triangle
                    uni_triangle.Add(index);
                    index++;
                }
            }
            //
        }

        public UnityEngine.Mesh CreateAndGetMesh_Simple() {
            UnityEngine.Mesh mesh = new UnityEngine.Mesh();

            List<int> t_triangle = new List<int>();
            for (int i = 0; i < faces.Count; i++) {
                Face3 face = faces[i];
                t_triangle.AddRange(face.GetTriangles());
            }

            // uv
            List<Vector2> t_uv = new List<Vector2>(new Vector2[vertices.Count]);
            for (int i = 0; i < faces.Count; i++) {
                int[] indexes = faces[i].GetVertexIndexList();
                for (int n = 0; n < indexes.Length; n++) {
                    int v_id = indexes[n]; // v_id: 頂点のid
                    Vector2 uv = faces[i].uvs[n];//  i 番目のfaceのn番目のuv
                    uv.x = 1.0f - uv.x; // flip x
                    t_uv[v_id] = uv;
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = t_triangle.ToArray();
            mesh.uv = t_uv.ToArray();

            mesh.RecalculateBounds();
            //mesh.UploadMeshData(true);

            return mesh;
        }

        public UnityEngine.Mesh GetMesh() {
            if (mesh == null) {
                mesh = CreateMesh();
            }
            return mesh;
        }

        UnityEngine.Mesh CreateAndGetLineMesh() {
            UnityEngine.Mesh mesh = new UnityEngine.Mesh();

            mesh.vertices = vertices.ToArray();
            //int[] indices = new int[vertices.Count];
            Vector2[] uvs = new Vector2[vertices.Count];
            Vector3[] normals = new Vector3[vertices.Count];

            for (int i = 0; i < mesh.vertices.Length; i++) {
                //indices[i] = i;
                uvs[i] = new Vector2(0.0f, 0.0f);
                normals[i] = Vector3.one;
            }

            // or MeshTopology.LineStrip
            int[] indices = new int[mesh.vertices.Length * 2];
            for (int i = 1; i < mesh.vertices.Length; i++) {
                indices[i * 2] = i - 1;
                indices[i * 2 + 1] = i;
            }

            mesh.uv = uvs;
            mesh.colors = verticexColors.ToArray();
            mesh.normals = normals;
            mesh.SetIndices(indices, MeshTopology.Lines, 0);

            return mesh;
        }

        public UnityEngine.Mesh GetLineMesh() {
            if (mesh == null) {
                mesh = CreateAndGetLineMesh();
            }
            return mesh;
        }

        protected int IndexFromStr(string str) {
            int index = 0;
            if (str == "x") {
                index = 0;
            }
            else if (str == "y") {
                index = 1;
            }
            else {
                index = 2;
            }
            return index;
        }

        //
        public int mergeVertices() {
            //var verticesMap = {}; // Hashmap for looking up vertice by position coordinates (and making sure they are unique)
            Dictionary<string, int> verticesMap = new Dictionary<string, int>();
            List<Vector3> unique = new List<Vector3>();
            List<int> changes = new List<int>();

            float precisionPoints = 4; // number of decimal points, eg. 4 for epsilon of 0.0001
            float precision = Mathf.Pow(10, precisionPoints);

            for (int i = 0, il = this.vertices.Count; i < il; i++) {

                Vector3 v = this.vertices[i];
                string key = Mathf.Round(v.x * precision).ToString() + '_' + Mathf.Round(v.y * precision).ToString() + '_' + Mathf.Round(v.z * precision).ToString();

                int changeId;
                //if ( verticesMap[ key ] == null ) {
                if (!verticesMap.ContainsKey(key)) {
                    verticesMap[key] = i;
                    unique.Add(this.vertices[i]);
                    //changes[ i ] = unique.Count - 1;
                    changeId = unique.Count - 1;

                }
                else {
                    //console.log('Duplicate vertex found. ', i, ' could be using ', verticesMap[key]);
                    //changes[ i ] = changes[ verticesMap[ key ] ];
                    changeId = changes[verticesMap[key]];
                }
                changes.Add(changeId);
            }


            // if faces are completely degenerate after merging vertices, we
            // have to remove them from the geometry.
            List<int> faceIndicesToRemove = new List<int>();

            for (int i = 0, il = this.faces.Count; i < il; i++) {

                Face3 face = this.faces[i];

                face.a = changes[face.a];
                face.b = changes[face.b];
                face.c = changes[face.c];

                List<int> indices = new List<int>(new int[] { face.a, face.b, face.c });

                //int dupIndex = -1;

                // if any duplicate vertices are found in a Face3
                // we have to remove the face as nothing can be saved
                for (int n = 0; n < 3; n++) {
                    if (indices[n] == indices[(n + 1) % 3]) {
                        //dupIndex = n;
                        faceIndicesToRemove.Add(i);
                        break;
                    }
                }

            }

            for (int i = faceIndicesToRemove.Count - 1; i >= 0; i--) {
                int idx = faceIndicesToRemove[i];
                this.faces.RemoveAt(idx);
            }

            // Use unique set of vertices
            int diff = this.vertices.Count - unique.Count;
            this.vertices = unique;

            return diff;
        }

        // faceから面のnormal計算  faceに対して直行するnormalを求め、face.normalに設定
        void computeFaceNormals() {

            Vector3 cb = new Vector3();
            Vector3 ab = new Vector3();

            //for (int f = 0, fl = this.faces.Count; f < fl; f++){
            for (int f = 0; f < this.faces.Count; f++) {

                Face3 face = this.faces[f];

                Vector3 vA = this.vertices[face.a];
                Vector3 vB = this.vertices[face.b];
                Vector3 vC = this.vertices[face.c];

                cb = vC - vB;
                ab = vA - vB;
                cb = Vector3.Cross(cb, ab);

                cb.Normalize();

                face.normal = cb;
            }
        }

        //各頂点のnormalを再計算  スムージングされた見えになる
        void computeVertexNormals(bool areaWeighted = false) {

            int v, vl, f, fl;
            Face3 face;

            List<Vector3> computeNormals = new List<Vector3>(this.vertices.Count);

            for (v = 0, vl = this.vertices.Count; v < vl; v++) {
                //vertices[ v ] = new Vector3();
                computeNormals.Add(new Vector3());
            }

            if (areaWeighted) {
                // vertex normals weighted by triangle areas
                // http://www.iquilezles.org/www/articles/normals/normals.htm

                Vector3 vA, vB, vC;
                Vector3 cb = new Vector3(), ab = new Vector3();

                for (f = 0, fl = this.faces.Count; f < fl; f++) {
                    face = this.faces[f];

                    vA = this.vertices[face.a];
                    vB = this.vertices[face.b];
                    vC = this.vertices[face.c];

                    cb = (vC - vB);
                    ab = (vA - vB);

                    Vector3 no = Vector3.Cross(cb, ab);

                    computeNormals[face.a] += (no);
                    computeNormals[face.b] += (no);
                    computeNormals[face.c] += (no);
                }
            }
            else {

                for (f = 0, fl = this.faces.Count; f < fl; f++) {
                    face = this.faces[f];

                    computeNormals[face.a] += (face.normal);
                    computeNormals[face.b] += (face.normal);
                    computeNormals[face.c] += (face.normal);
                }
            }

            for (v = 0, vl = this.vertices.Count; v < vl; v++) {
                computeNormals[v].Normalize();
            }

            for (f = 0, fl = this.faces.Count; f < fl; f++) {

                face = this.faces[f];

                face.vertexNormals[0] = computeNormals[face.a].normalized;
                face.vertexNormals[1] = computeNormals[face.b].normalized;
                face.vertexNormals[2] = computeNormals[face.c].normalized;
            }
        }

        void copyFaceNormalToVertexNormals() {
            for (int f = 0; f < this.faces.Count; f++) {
                Face3 face = this.faces[f];
                face.SetFaceNormalToVertexNormals();
            }
        }

        /// <summary>
        /// 頂点ごとの法線ではなく面に対しての法線を計算し、vertexNormals に設定。
        /// </summary>
        public void SetFaceNormals() {
            computeFaceNormals();
            copyFaceNormalToVertexNormals();
        }

        public void SetFaceSmooth() {
            computeFaceNormals();
            computeVertexNormals();
        }

        public void SetFlipFace(bool enable = true) {
            foreach (Face3 f in faces) {
                f.flip = enable;
            }
        }
        public void SetDoubleSided(bool enable = true) {
            foreach (Face3 f in faces) {
                f.doubleSided = enable;
            }
        }
    }
}
