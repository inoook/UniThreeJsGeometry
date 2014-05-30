using UnityEngine;
using System.Collections;

public class Note : MonoBehaviour {

	public GameObject geometries;
	public GameObject geometries2;


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

		if(GUILayout.Button("> next")){
			index ++;
			index = index % 2;

			bool isEnable = (index == 0);
			geometries.SetActive(isEnable);
			geometries2.SetActive(!isEnable);
		}

		GUILayout.EndArea();
	}

	int index = 0;
}
