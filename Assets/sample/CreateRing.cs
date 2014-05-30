using UnityEngine;
using System.Collections;
using THREE;

public class CreateRing : MonoBehaviour {

	public RingGeometry geo;
	public MeshFilter meshFilter;

	// Use this for initialization
	void Start () {
		geo = new RingGeometry(2, 8, 20, 8);
		meshFilter.mesh = geo.CreateMesh();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
