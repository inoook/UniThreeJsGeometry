using UnityEngine;
using System.Collections;
using THREE;

public class CreateCircle : CreateBase {

    [SerializeField] float radius = 5;
    [SerializeField] int segments = 32;

    public override void Create() {
		geo = new CircleGeometry(radius, segments, 0, Mathf.PI * 2);
    }
}
