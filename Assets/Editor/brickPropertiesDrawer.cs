﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomPropertyDrawer(typeof(brickPropertiesClass))]
public class brickPropertiesDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {


			EditorGUI.PrefixLabel(position, label);
		Rect newposition = position;
	newposition.y += 18f;
		SerializedProperty data = property.FindPropertyRelative("rows");
		//data.rows[0][]
		for(int j=0;j<4;j++){
			SerializedProperty row = data.GetArrayElementAtIndex(j).FindPropertyRelative("row");
	newposition.height = 18f;
			if(row.arraySize != 4)
				row.arraySize = 4;
			newposition.width = position.width/4;
			for(int i=0;i<4;i++){
				EditorGUI.PropertyField(newposition, row.GetArrayElementAtIndex(i), GUIContent.none);
				newposition.x += newposition.width;
			}

newposition.x = position.x;
newposition.y += 18f;
		}
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
{
	return 18f * 8;
}
}
