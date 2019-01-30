using UnityEngine;
using System.Collections;

public class GizmosNormal : MonoBehaviour {

	[SerializeField] float amp = 1.5f;
    [SerializeField] Color color = Color.red;

    private void OnDrawGizmosSelected()
    {
        MeshFilter mf = this.gameObject.GetComponent<MeshFilter>();
        if(mf == null) { return; }

        Mesh mesh = mf.mesh;
        Vector3[] normals = mesh.normals;
        Vector3[] vertices = mesh.vertices;

        Gizmos.color = color;
        for (int i = 0; i < normals.Length; i++)
        {
            Vector3 pos = this.transform.TransformPoint(vertices[i]);
            Vector3 dir = this.transform.TransformDirection(normals[i]) * amp;
            Gizmos.DrawRay(pos, dir);
        }
    }
}
