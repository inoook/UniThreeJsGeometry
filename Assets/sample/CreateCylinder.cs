using UnityEngine;
using System.Collections;
using THREE;

public class CreateCylinder : MonoBehaviour {

	public CylinderGeometry geo;
	public MeshFilter meshFilter;

	// Use this for initialization
	void Start () {
		//geo = new CylinderGeometry(10,10,10, 12,1);
		geo = new CylinderGeometry( 25, 75, 100, 40, 5, false );
		meshFilter.mesh = geo.CreateMesh();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
