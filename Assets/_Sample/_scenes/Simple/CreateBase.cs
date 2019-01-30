using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using THREE;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CreateBase : MonoBehaviour {

    [SerializeField] MeshFilter meshFilter;

    [SerializeField] bool isFlatNormals = false;
    protected Geometry geo;
    
    void OnValidate() {
        Create();

        if(geo == null) { return; }

        if (isFlatNormals) {
            geo.SetFaceNormals();
        }

        // set mesh
        if (meshFilter == null){
            meshFilter = this.gameObject.GetComponent<MeshFilter>();
        }

        if (meshFilter != null){
            meshFilter.mesh = geo.CreateAndGetMesh();
        }
    }

    public virtual void Create() { }
}
