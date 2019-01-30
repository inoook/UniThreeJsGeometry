using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using THREE;

[ExecuteInEditMode]
public class Create : MonoBehaviour {

	[SerializeField] MeshFilter mf;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public float radius = 1;
	public float height = 1;
	public int rDivid = 24;
	public int hDivid = 1;

	public bool flipFace = false;
	public float per = 1;

	[ContextMenu ("Create Mesh")]
	public void CreateMesh()
	{
		CylinderGeometry geo = new CylinderGeometry (radius, radius, height, rDivid, hDivid, true, per);
		geo.SetFlipFace (flipFace);
		Mesh m = geo.GetMesh ();
		mf.mesh = m;
	}
}


#if UNITY_EDITOR
[CustomEditor(typeof(Create))]
class CreateEditor : Editor {
	override public void OnInspectorGUI () {
		Create _target = (Create)target;

		if (GUILayout.Button ("Create")) {
			_target.CreateMesh ();
		}

		if(_target != null){
			DrawDefaultInspector();
			EditorGUIUtility.labelWidth = 25;
			EditorGUIUtility.fieldWidth = 50;
		}
	}
}
#endif

