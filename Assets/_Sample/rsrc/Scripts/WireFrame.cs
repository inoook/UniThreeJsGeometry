using UnityEngine;
using System.Collections;

public class WireFrame : MonoBehaviour {
	
	void Start()
	{
		
	}
	
	// attatch to camera
	void OnPreRender() {
        UnityEngine.GL.wireframe = true;
    }
    void OnPostRender() {
        UnityEngine.GL.wireframe = false;
    }
}
