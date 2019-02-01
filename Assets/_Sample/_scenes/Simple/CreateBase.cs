using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using THREE;

public interface IRecieveMessage : IEventSystemHandler
{
    void OnValidate();
}

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CreateBase : MonoBehaviour, IRecieveMessage
{
    [SerializeField] MeshFilter meshFilter;

    [SerializeField] bool isFlatNormals = false;
    [SerializeField] bool isDoubleSided = false;
    [SerializeField] bool isFlipFace = false;
    [SerializeField] bool isRecalculateNormals = false;
    protected Geometry geo;

    public void OnValidate() {
        Create();

        if(geo == null) { return; }

        if (isFlatNormals) {
            geo.SetFaceNormals();
        }
        if (isDoubleSided) {
            geo.SetDoubleSided();
        }
        if (isFlipFace){
            geo.SetFlipFace();
        }
        // set mesh
        if (meshFilter == null){
            meshFilter = this.gameObject.GetComponent<MeshFilter>();
        }

        if (meshFilter != null){
            Mesh mesh = geo.CreateMesh();
            if (isRecalculateNormals) {
                UnityMeshUtils.RecalculateNormals(mesh, 30f);
            }
            meshFilter.mesh = mesh;
        }
    }

    public virtual void Create() { }
}
