// https://github.com/mrdoob/three.js/blob/b7279adc60d366ff33a3e662576f349720139820/src/core/Face4.js
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{
	public class Face
	{
		protected Vector3 _normal = new Vector3();
		public virtual Vector3 normal
		{
			get{
				return _normal;
			}
			set{
				_normal = value;
			}
		}


		public virtual List<int> GetTriangles ()
		{
			return null;
		}

		public virtual List<int> GetVertexIndexList ()
		{
			return null;
		}
	}
	
	public class Face3 : Face
	{
	
		public int a;
		public int b;
		public int c;
		public List<Vector3> normals;
		public List<Vector3> vertexNormals;
		
		public Face3 (int a, int b, int c)
		{
			this.a = a;
			this.b = b;
			this.c = c;

			//this.normals = new List<Vector3>(new List<Vector3>( new Vector3[3]) );
			this.vertexNormals = new List<Vector3>(new List<Vector3>( new Vector3[3]) );

		}

		public Face3 (int a, int b, int c, List<Vector3> vertexNormals)
		{
			this.a = a;
			this.b = b;
			this.c = c;

			this.vertexNormals = vertexNormals;
		}

		public override List<int> GetTriangles ()
		{
			List<int> tris = new List<int> ();
			tris.Add (a);
			tris.Add (b);
			tris.Add (c);
		
			return tris;
		}

		public override List<int> GetVertexIndexList ()
		{
			return new List<int> (new int[]{a, b, c});
		}

		public override Vector3 normal
		{
			get{
				return _normal;
			}
			set{
				_normal = value;
			}
		}

		public void SetFaceNormalToVertexNormals()
		{
			this.vertexNormals = new List<Vector3>(new List<Vector3>( new Vector3[]{normal, normal, normal}) );
		}
	}
}
