using UnityEngine;
using System.Collections;

public class Notice : MonoBehaviour {

	public string str = "Unity3d / three.js examples";
	public string link = "http://threejs.org/examples/#webgl_geometry_extrude_shapes";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	private Rect rect;
	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(10,10,500,200));
		GUILayout.Label(str);

		GUI.color = Color.cyan;
		GUILayout.Label(link);
		rect = GUILayoutUtility.GetLastRect();
		if (Event.current.type == EventType.MouseUp && rect.Contains(Event.current.mousePosition)){
			Application.OpenURL(link);
		}
//		if (Event.current.type == EventType.MouseUp && yourLabelRect.Contains(Event.current.mousePosition)){
//			Application.OpenUrl(yourUrl);
//		}
//		GUI.Label(yourLabelRect, yourUrl);

		GUI.color = Color.white;
		GUILayout.Label("w: wireframe");
		GUILayout.EndArea();
	}
}
