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


		public virtual int[] GetTriangles ()
		{
			return null;
		}

		public virtual int[] GetVertexIndexList ()
		{
			return null;
		}
	}
	
	public class Face3 : Face
	{
		public int a;
		public int b;
		public int c;
		public Vector3[] vertexNormals;
		public Vector2[] uvs;

		public bool flip = false;
		public bool doubleSided = false;

		public Face3 ()
		{

		}
		public Face3 (int a, int b, int c)
		{
			this.a = a;
			this.b = b;
			this.c = c;

			this.vertexNormals = new Vector3[]{ Vector3.zero, Vector3.zero, Vector3.zero };
		}

		public Face3 (int a, int b, int c, Vector3[] vertexNormals)
		{
			this.a = a;
			this.b = b;
			this.c = c;

			this.vertexNormals = vertexNormals;
		}
		public Face3 (int a, int b, int c, Vector3[] vertexNormals, Vector2[] uvs)
		{
			this.a = a;
			this.b = b;
			this.c = c;

			this.vertexNormals = vertexNormals;
			this.uvs = uvs;
		}

		public override int[] GetTriangles ()
		{
			return GetVertexIndexList();
		}

		public override int[] GetVertexIndexList ()
		{
			if (doubleSided) {
				return new int[]{ a, b, c, c, b, a };
			}else{
				if (!flip) {
					return new int[]{ a, b, c };
				} else {
					return new int[]{ c, b, a };
				}
			}
		}

		public Vector2[] GetUvs ()
		{
			if (doubleSided) {
				return new Vector2[]{ uvs [0], uvs [1], uvs [2], uvs [2], uvs [1], uvs [0] };
			} else {
				if (!flip) {
					return uvs;
				} else {
					return new Vector2[]{ uvs [2], uvs [1], uvs [0] };
				}
			}
		}
		public Vector3[] GetVertexNormals ()
		{
			if (doubleSided) {
				return new Vector3[]{ vertexNormals [0], vertexNormals [1], vertexNormals [2],  -vertexNormals [2], -vertexNormals [1], -vertexNormals [0] };
			} else {
				if (!flip) {
					return vertexNormals;
				} else {
					return new Vector3[]{ -vertexNormals [2], -vertexNormals [1], -vertexNormals [0] };
				}
			}
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
			this.vertexNormals = new Vector3[]{normal, normal, normal};
		}
	}
}
