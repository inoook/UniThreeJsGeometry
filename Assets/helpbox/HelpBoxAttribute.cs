using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class HelpBoxAttribute : PropertyAttribute {

	public string label;
	public float height = 50;

	public HelpBoxAttribute (string label_, float height_ = 50) {
		this.label = label_;
		this.height = height_;
	}
}

#if UNITY_EDITOR

[CustomPropertyDrawer( typeof ( HelpBoxAttribute ) )]
public class HelpBoxAttributeDrawer : PropertyDrawer
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		HelpBoxAttribute myAttribute = (HelpBoxAttribute)attribute;
		position.height = myAttribute.height;

		EditorGUI.HelpBox(position, myAttribute.label, MessageType.None);
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		HelpBoxAttribute myAttribute = (HelpBoxAttribute)attribute;
		return myAttribute.height+5;
	}
}

#endif

