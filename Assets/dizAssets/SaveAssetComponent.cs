using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

// TODO: editor拡張の方向を考える。
public class SaveAssetComponent : MonoBehaviour {


    [SerializeField] string saveName = "Mesh";

    [ContextMenu("SaveMesh")]
    public void SaveMesh()
    {
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        SaveAsset(mesh, saveName);
    }

    public void SaveAsset(Object obj, string assetName)
    {
        AssetDatabase.CreateAsset(obj, "Assets/Meshes/" + assetName+".asset");
        AssetDatabase.SaveAssets();
    }
}
