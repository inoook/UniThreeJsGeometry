using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThreeBehaviour : MonoBehaviour
{

	public THREE.Scene scene;
	public Camera viewCamera;
	
	protected virtual void Init ()
	{
		scene = new THREE.Scene ();
	}

	protected virtual void Render ()
	{
		
	}

	void Start ()
	{
		if(viewCamera == null){
			viewCamera = Camera.main;
		}

		Init ();
	}

	void Update ()
	{
		Render ();

		viewCamera.aspect = (float)Screen.width / (float)Screen.height;
		
		for (int i = 0; i < scene.children.Count; i++) {
			THREE.RenderableObject threeMesh = scene.children [i];
			Quaternion rot = Quaternion.Euler (threeMesh.eulerAngles);
			Matrix4x4 matrix = Matrix4x4.TRS (threeMesh.position, rot, threeMesh.scale);
			//Graphics.DrawMesh(threeMesh.GetMesh(), threeMesh.position, rot, threeMesh.mat, 0);
			Graphics.DrawMesh (threeMesh.GetMesh(), matrix, threeMesh.mat, 0); // TODO:  threeMesh.GetMesh()...
		}
	}


}

//
namespace THREE
{
	public class RenderableObject
	{
		public Geometry geo;
		public Vector3 position = Vector3.zero;
		public Vector3 scale = Vector3.one;
		//public Quaternion rotation = Quaternion.identity;
		public Vector3 eulerAngles = Vector3.zero;
		public Material mat;

		public virtual UnityEngine.Mesh GetMesh ()
		{
			return null;
		}
	}
	public class Mesh : RenderableObject
	{
//		Geometry geo;
//		public Vector3 position = Vector3.zero;
//		public Vector3 scale = Vector3.one;
//		//public Quaternion rotation = Quaternion.identity;
//		public Vector3 eulerAngles = Vector3.zero;
//		public Material mat;
	
		public Mesh (Geometry geo, Material mat)
		{
			this.geo = geo;
			this.mat = mat;
			//this.rotation = Quaternion.identity;

			this.geo.CreateMesh ();
		}

		public override UnityEngine.Mesh GetMesh ()
		{
			return this.geo.GetMesh ();
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

	public class Scene
	{
		public List<RenderableObject> children;
	
		public Scene ()
		{
			children = new List<RenderableObject> ();
		}
	
		public void Add (RenderableObject mesh)
		{
			children.Add (mesh);
		}
	}
}
