using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// https://threejs.org/examples/#webgl_modifier_subdivision
public class SubdivisionUnity : MonoBehaviour
{

	public Material material;
	
	// Use this for initialization
	void Start() {

		THREE.Geometry boxGeo = new THREE.BoxGeometry( 200, 200, 200, 2, 2, 2 );

		int subdivisions = 2;
		THREE.SubdivisionModifier modifier = new THREE.SubdivisionModifier( subdivisions );

		modifier.modify(boxGeo);

        AddRenderObject(boxGeo, material, new Vector3(0, 0, 0));
    }

    THREE.Geometry AddRenderObject(THREE.Geometry geo, Material material, Vector3 position)
    {
        UnityEngine.Mesh mesh = geo.GetMesh();

        GameObject gObj = new GameObject();
        gObj.transform.SetParent(this.transform);
        gObj.transform.localPosition = position;

        MeshFilter mf = gObj.AddComponent<MeshFilter>();
        MeshRenderer mr = gObj.AddComponent<MeshRenderer>();

        mf.mesh = mesh;
        mr.material = material;

        return geo;
    }
}
