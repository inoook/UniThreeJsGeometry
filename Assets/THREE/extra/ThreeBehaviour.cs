using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//
namespace THREE
{
	public class RenderableObject
	{
		public Geometry geo;
		public Vector3 position = Vector3.zero;
		public Vector3 scale = Vector3.one;
		public Vector3 eulerAngles = Vector3.zero;
		public Material mat;

		public virtual UnityEngine.Mesh GetMesh ()
		{
			return null;
		}
	}

	public class MeshThreeJs : RenderableObject
	{

		public MeshThreeJs (Geometry geo, Material mat)
		{
			this.geo = geo;
			this.mat = mat;
			this.geo.CreateMesh ();
		}

		public override UnityEngine.Mesh GetMesh ()
		{
			return this.geo.GetMesh();
		}
	}

	public class Line : RenderableObject
	{
		public Line (Geometry geo, Material mat)
		{
			this.geo = geo;
			this.mat = mat;
		}
		public override UnityEngine.Mesh GetMesh ()
		{
			return this.geo.GetLineMesh ();
		}

		public void Add(Geometry geo)
		{
			this.geo.vertices.AddRange(geo.vertices);
			this.geo.verticexColors.AddRange(geo.verticexColors);
		}
	}

}
