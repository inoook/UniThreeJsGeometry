using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UnityMeshUtils {

	public static int checkVertices (UnityEngine.Mesh mesh)
	{
		//var verticesMap = {}; // Hashmap for looking up vertice by position coordinates (and making sure they are unique)
		Dictionary<string, int> verticesMap = new Dictionary<string, int> ();
		List<Vector3> unique = new List<Vector3> ();
		List<int> changes = new List<int> ();
		
		float precisionPoints = 4; // number of decimal points, eg. 4 for epsilon of 0.0001
		float precision = Mathf.Pow (10, precisionPoints);

		Vector3[] vertices = mesh.vertices;
		Debug.Log("org: "+vertices.Length);
		for (int i = 0, il = vertices.Length; i < il; i ++) {
			
			Vector3 v = vertices [i];
			string key = Mathf.Round (v.x * precision).ToString () + '_' + Mathf.Round (v.y * precision).ToString () + '_' + Mathf.Round (v.z * precision).ToString ();
			
			int changeId;
			//if ( verticesMap[ key ] == null ) {
			if (!verticesMap.ContainsKey (key)) {
				verticesMap [key] = i;
				unique.Add (vertices [i]);
				//changes[ i ] = unique.Count - 1;
				changeId = unique.Count - 1;
				
			} else {
				//changes[ i ] = changes[ verticesMap[ key ] ];
				changeId = changes [verticesMap [key]];
			}
			changes.Add (changeId);
		}
		
		// Use unique set of vertices
		int diff = vertices.Length - unique.Count;

		Debug.Log("checkVertices: "+diff);
		
		return diff;
	}
    
	public static void MeshSmoothNormals (UnityEngine.Mesh mesh) {
		
		Vector3[] normals = mesh.normals;
		int[] trigs = mesh.triangles;
		
		for(int i = 0; i < trigs.Length; i+=3) {
			
			Vector3 avg = (normals[trigs[i]] + normals[trigs[i+1]] + normals[trigs[i+2]])/3;
			normals[trigs[i]] = avg;
			normals[trigs[i+1]] = avg;
			normals[trigs[i+2]] = avg;
		}
		mesh.normals = normals;
	}
	
	public static void MeshSmoothNormals2 (UnityEngine.Mesh mesh)
	{
		//In C#, you'd add `using System.Collections.Generic` to the top of your file for List<> to work
		Vector3[] normals = mesh.normals;
		List<Vector3>[] vertexNormals = new List<Vector3>[normals.Length]; //array of lists, so each element stores a list of normals for that vertex, to be averaged later
		for(int i = 0; i < vertexNormals.Length; i++){
			vertexNormals[i] = new List<Vector3>();
		}
		
		int[] trigs = mesh.triangles;
		
		//create list of normals for each vertex
		for (int i=0; i<trigs.Length; i+=3) {
			Vector3 currNormal = (normals[trigs[i]] + normals[trigs[i+1]] + normals[trigs[i+2]])/3;/*calculate current triangle's normal*/;
			vertexNormals[trigs[i]].Add(currNormal);
			vertexNormals[trigs[i+1]].Add(currNormal);
			vertexNormals[trigs[i+2]].Add(currNormal);
		}
		
		//now we have the lists, calculate average normal for each vertex from its list of normals
		for (int i=0; i<vertexNormals.Length; i++) {
			normals[i] = Vector3.zero; //ensure the normal starts as a zero vector
			//declared as float so we can divide as float with it later, might be ok as int though?
			float numNormals = vertexNormals[i].Count;
			for (int j=0; j<numNormals; j++) {
				normals[i] += vertexNormals[i][j];
			}
			//gets the average of the normals now they're added together
			normals[i].Scale(new Vector3( 1f/numNormals,1f/numNormals,1f/numNormals) );
		}
		mesh.normals = normals;
	}

    // http://norio.hatenadiary.jp/entry/2017/01/23/081937
    // スムージング角を用いて法線の再計算を行う
    public static void RecalculateNormals(Mesh mesh, float angle)
    {
        var triangles = mesh.triangles;
        var vertices = mesh.vertices;
        var triNormals = new Vector3[triangles.Length / 3];
        var normals = new Vector3[vertices.Length];

        angle = angle * Mathf.Deg2Rad;

        var dictionary = new Dictionary<VertexKey, VertexEntry>(vertices.Length);

        for (var i = 0; i < triangles.Length; i += 3)
        {
            var i1 = triangles[i];
            var i2 = triangles[i + 1];
            var i3 = triangles[i + 2];

            var p1 = vertices[i2] - vertices[i1];
            var p2 = vertices[i3] - vertices[i1];
            var normal = Vector3.Cross(p1, p2).normalized;
            int triIndex = i / 3;
            triNormals[triIndex] = normal;

            VertexEntry entry;
            VertexKey key;

            if (!dictionary.TryGetValue(key = new VertexKey(vertices[i1]), out entry))
            {
                entry = new VertexEntry();
                dictionary.Add(key, entry);
            }
            entry.Add(i1, triIndex);

            if (!dictionary.TryGetValue(key = new VertexKey(vertices[i2]), out entry))
            {
                entry = new VertexEntry();
                dictionary.Add(key, entry);
            }
            entry.Add(i2, triIndex);

            if (!dictionary.TryGetValue(key = new VertexKey(vertices[i3]), out entry))
            {
                entry = new VertexEntry();
                dictionary.Add(key, entry);
            }
            entry.Add(i3, triIndex);
        }
        foreach (var value in dictionary.Values)
        {
            for (var i = 0; i < value.Count; ++i)
            {
                var sum = new Vector3();
                for (var j = 0; j < value.Count; ++j)
                {
                    if (value.VertexIndex[i] == value.VertexIndex[j])
                    {
                        sum += triNormals[value.TriangleIndex[j]];
                    }
                    else
                    {
                        float dot = Vector3.Dot(
                            triNormals[value.TriangleIndex[i]],
                            triNormals[value.TriangleIndex[j]]);
                        dot = Mathf.Clamp(dot, -0.99999f, 0.99999f);
                        float acos = Mathf.Acos(dot);
                        if (acos <= angle)
                        {
                            sum += triNormals[value.TriangleIndex[j]];
                        }
                    }
                }

                normals[value.VertexIndex[i]] = sum.normalized;
            }
        }

        mesh.normals = normals;
    }

    private struct VertexKey
    {
        private readonly long x;
        private readonly long y;
        private readonly long z;

        private const int Tolerance = 100000;

        public VertexKey(Vector3 position)
        {
            x = (long)(Mathf.Round(position.x * Tolerance));
            y = (long)(Mathf.Round(position.y * Tolerance));
            z = (long)(Mathf.Round(position.z * Tolerance));
        }

        public override bool Equals(object obj)
        {
            var key = (VertexKey)obj;
            return x == key.x && y == key.y && z == key.z;
        }

        public override int GetHashCode()
        {
            return (x * 7 ^ y * 13 ^ z * 27).GetHashCode();
        }
    }

    private sealed class VertexEntry
    {
        public int[] TriangleIndex = new int[4];
        public int[] VertexIndex = new int[4];

        private int reserved = 4;

        public int Count { get; private set; }

        public void Add(int vertIndex, int triIndex)
        {
            if (reserved == Count)
            {
                reserved *= 2;
                System.Array.Resize(ref TriangleIndex, reserved);
                System.Array.Resize(ref VertexIndex, reserved);
            }
            TriangleIndex[Count] = triIndex;
            VertexIndex[Count] = vertIndex;
            ++Count;
        }
    }
}
