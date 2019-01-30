using UnityEngine;
using System.Collections;
using THREE;

public class CreateMesh : CreateBase {

    [SerializeField] float width = 10;
    [SerializeField] float height = 10;
    [SerializeField] int widthSegments = 3;
    [SerializeField] int heightSegments = 3;

    public override void Create() {
        geo = new PlaneGeometry(width, height, widthSegments, heightSegments);
	}
}
