using UnityEngine;
using UnityEditor;
using System.Collections;


public class CreatePlane : ScriptableWizard
{
    
    public enum Orientation
    {
        Horizontal,
        Vertical
    }

    public enum AnchorPoint
    {
        TopLeft,
        TopHalf,
        TopRight,
        RightHalf,
        BottomRight,
        BottomHalf,
        BottomLeft,
        LeftHalf,
        Center
    }
    
    public int widthSegments = 1;
    public int lengthSegments = 1;
    public float width = 1.0f;
    public float length = 1.0f;
    public Orientation orientation = Orientation.Horizontal;
    public AnchorPoint anchor = AnchorPoint.Center;
    public bool addCollider = false;
    public bool createAtOrigin = true;
	public bool doubleSided = false;
    public string optionalName;

    static Camera cam;
    static Camera lastUsedCam;

    
    [MenuItem("GameObject/Create Other/Custom Plane...")]
    static void CreateWizard()
    {
        cam = Camera.current;
        // Hack because camera.current doesn't return editor camera if scene view doesn't have focus
        if (!cam)
            cam = lastUsedCam;
        else
            lastUsedCam = cam;
        ScriptableWizard.DisplayWizard("Create Plane",typeof(CreatePlane));
    }
    
    
    void OnWizardUpdate()
    {
        widthSegments = Mathf.Clamp(widthSegments, 1, 254);
        lengthSegments = Mathf.Clamp(lengthSegments, 1, 254);
    }
    
    
    void OnWizardCreate()
    {
        GameObject plane = new GameObject();
        
        if (!string.IsNullOrEmpty(optionalName))
            plane.name = optionalName;
        else
            plane.name = "Plane";
        
        if (!createAtOrigin && cam)
            plane.transform.position = cam.transform.position + cam.transform.forward*5.0f;
        else
            plane.transform.position = Vector3.zero;
        
        Vector2 anchorOffset;
        string anchorId;
        switch (anchor)
        {
        case AnchorPoint.TopLeft:
            anchorOffset = new Vector2(-width/2.0f,length/2.0f);
            anchorId = "TL";
            break;
        case AnchorPoint.TopHalf:
            anchorOffset = new Vector2(0.0f,length/2.0f);
            anchorId = "TH";
            break;
        case AnchorPoint.TopRight:
            anchorOffset = new Vector2(width/2.0f,length/2.0f);
            anchorId = "TR";
            break;
        case AnchorPoint.RightHalf:
            anchorOffset = new Vector2(width/2.0f,0.0f);
            anchorId = "RH";
            break;
        case AnchorPoint.BottomRight:
            anchorOffset = new Vector2(width/2.0f,-length/2.0f);
            anchorId = "BR";
            break;
        case AnchorPoint.BottomHalf:
            anchorOffset = new Vector2(0.0f,-length/2.0f);
            anchorId = "BH";
            break;
        case AnchorPoint.BottomLeft:
            anchorOffset = new Vector2(-width/2.0f,-length/2.0f);
            anchorId = "BL";
            break;      
        case AnchorPoint.LeftHalf:
            anchorOffset = new Vector2(-width/2.0f,0.0f);
            anchorId = "LH";
            break;      
        case AnchorPoint.Center:
        default:
            anchorOffset = Vector2.zero;
            anchorId = "C";
            break;
        }
                
        MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
        plane.AddComponent(typeof(MeshRenderer));

        string planeAssetName = plane.name + widthSegments + "x" + lengthSegments + "W" + width + "L" + length + (orientation == Orientation.Horizontal? "H" : "V") + anchorId + (doubleSided ? "_double" : "") + ".asset";
        Mesh m = (Mesh)AssetDatabase.LoadAssetAtPath("Assets/Meshes/" + planeAssetName,typeof(Mesh));
 
        if (m == null)
        {
            m = new Mesh();
            m.name = plane.name;
        
            int hCount2 = widthSegments+1;
            int vCount2 = lengthSegments+1;
            int numTriangles = widthSegments * lengthSegments * 6 * (doubleSided ? 2 : 1);
			int numVerticesSingle = hCount2 * vCount2;
            int numVertices = numVerticesSingle * (doubleSided ? 2 : 1);
        	//Debug.Log("numVertices: "+numVertices);
            Vector3[] vertices = new Vector3[numVertices];
			Vector3[] normals = new Vector3[numVertices];
            Vector2[] uvs = new Vector2[numVertices];
            int[] triangles = new int[numTriangles];
        
            int index = 0;
            float uvFactorX = 1.0f/widthSegments;
            float uvFactorY = 1.0f/lengthSegments;
            float scaleX = width/widthSegments;
            float scaleY = length/lengthSegments;
            for (float y = 0.0f; y < vCount2; y++)
            {
                for (float x = 0.0f; x < hCount2; x++)
                {
                    if (orientation == Orientation.Horizontal)
                    {
                        vertices[index] = new Vector3(x*scaleX - width/2f - anchorOffset.x, 0.0f, y*scaleY - length/2f - anchorOffset.y);
						normals[index] = new Vector3(0,1,0);
                    }
                    else
                    {
                        vertices[index] = new Vector3(x*scaleX - width/2f - anchorOffset.x, y*scaleY - length/2f - anchorOffset.y, 0.0f);
						normals[index] = -Vector3.forward;
                    }
                    uvs[index++] = new Vector2(x*uvFactorX, y*uvFactorY);
                }
            }
			
			if(doubleSided){
				for (float y = 0.0f; y < vCount2; y++)
	            {
	                for (float x = 0.0f; x < hCount2; x++)
	                {
	                    if (orientation == Orientation.Horizontal)
	                    {
	                        vertices[index] = new Vector3(x*scaleX - width/2f - anchorOffset.x, 0.0f, y*scaleY - length/2f - anchorOffset.y);
							normals[index] = new Vector3(0,-1,0);
	                    }
	                    else
	                    {
	                        vertices[index] = new Vector3(x*scaleX - width/2f - anchorOffset.x, y*scaleY - length/2f - anchorOffset.y, 0.0f);
							normals[index] = Vector3.forward;
	                    }
	                    uvs[index++] = new Vector2(x*uvFactorX, y*uvFactorY);
	                }
	            }
			}
            
            index = 0;
            for (int y = 0; y < lengthSegments; y++)
            {
                for (int x = 0; x < widthSegments; x++)
                {
                    triangles[index]   = (y     * hCount2) + x;
                    triangles[index+1] = ((y+1) * hCount2) + x;
                    triangles[index+2] = (y     * hCount2) + x + 1;
        
                    triangles[index+3] = ((y+1) * hCount2) + x;
                    triangles[index+4] = ((y+1) * hCount2) + x + 1;
                    triangles[index+5] = (y     * hCount2) + x + 1;
                    index += 6;
                }
            }
			
			if(doubleSided){
				int startIndex = numVerticesSingle;
				for (int y = 0; y < lengthSegments; y++)
	            {
	                for (int x = 0; x < widthSegments; x++)
	                {
						Debug.Log("index "+index);
	                    triangles[index+5] = startIndex + (y     * hCount2) + x ;
	                    triangles[index+4] = startIndex + ((y+1) * hCount2) + x;
	                    triangles[index+3] = startIndex + (y     * hCount2) + x + 1;
	        
	                    triangles[index+2] = startIndex + ((y+1) * hCount2) + x;
	                    triangles[index+1] = startIndex + ((y+1) * hCount2) + x + 1;
	                    triangles[index]   = startIndex + (y     * hCount2) + x + 1;
	                    index += 6;
	                }
	            }
			}
        	m.Clear();
            m.vertices = vertices;
			m.normals = normals;
            m.uv = uvs;
            m.triangles = triangles;
			
			//m.RecalculateNormals();
			/*
			for(int i = 0; i < normals.Length; i++){
				Debug.Log(normals[i]);
			}
			*/
            AssetDatabase.CreateAsset(m, "Assets/Editor/" + planeAssetName);
            AssetDatabase.SaveAssets();
        }
        
        meshFilter.sharedMesh = m;
        m.RecalculateBounds();
        
        if (addCollider)
            plane.AddComponent(typeof(BoxCollider));
        
        Selection.activeObject = plane;
    }
}