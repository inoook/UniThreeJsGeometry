using UnityEngine;
using System.Collections;
using THREE;

public class CreateMeshCube : CreateBase {

    [SerializeField] float width = 10;
    [SerializeField] float height = 10;
    [SerializeField] float depth = 10;
    [SerializeField] int widthSegments = 3;
    [SerializeField] int heightSegments = 3;
    [SerializeField] int depthSegments = 3;

    public override void Create() {
        geo = new BoxGeometry(width, height, depth, widthSegments, heightSegments, depthSegments);
	}
}
