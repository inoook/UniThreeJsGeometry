using UnityEngine;
using System.Collections;
using THREE;

public class CreateMesh : MonoBehaviour {

	public PlaneGeometry planeGeo;
	public MeshFilter meshFilter;
	// Use this for initialization
	void Start () {
		planeGeo = new PlaneGeometry(10,10,3,3);
		meshFilter.mesh = planeGeo.CreateMesh();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
