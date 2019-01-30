using UnityEngine;
using System.Collections;

public class Note : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public WireFrame wireFrameRender;

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(10,10,200,200));
		GUILayout.Label("Unity3d / three.js examples Geometries, Geometries2");
		if(GUILayout.Button("Wireframe: "+wireFrameRender.enabled)){
			wireFrameRender.enabled = !wireFrameRender.enabled;
		}

		GUILayout.Space(20);

		GUILayout.EndArea();
	}

}
