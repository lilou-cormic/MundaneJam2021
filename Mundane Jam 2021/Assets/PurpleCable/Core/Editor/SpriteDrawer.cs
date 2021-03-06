using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Sprite))]
public class SpriteDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        if (prop.objectReferenceValue != null)
        {
            return _textureSize;
        }
        else
        {
            return base.GetPropertyHeight(prop, label);
        }
    }

    private const float _textureSize = 65;


    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, prop);

        if (prop.objectReferenceValue != null)
        {
            position.width = EditorGUIUtility.labelWidth;
            GUI.Label(position, prop.displayName);

            position.x += position.width;
            position.width = _textureSize;
            position.height = _textureSize;

            prop.objectReferenceValue = EditorGUI.ObjectField(position, prop.objectReferenceValue, typeof(Sprite), false);
        }
        else
        {
            EditorGUI.PropertyField(position, prop, true);
        }

        EditorGUI.EndProperty();
    }

    //private static GUIStyle s_TempStyle = new GUIStyle();

    //public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //{
    //    var ident = EditorGUI.indentLevel;
    //    EditorGUI.indentLevel = 0;

    //    Rect spriteRect;

    //    //create object field for the sprite
    //    spriteRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
    //    property.objectReferenceValue = EditorGUI.ObjectField(spriteRect, property.displayName, property.objectReferenceValue, typeof(Sprite), false);

    //    //if this is not a repain or the property is null exit now
    //    if (Event.current.type != EventType.Repaint || property.objectReferenceValue == null)
    //        return;

    //    //draw a sprite
    //    Sprite sp = property.objectReferenceValue as Sprite;

    //    float ratio = Mathf.Min(sp.bounds.size.x * sp.pixelsPerUnit, 64) / sp.bounds.size.x;

    //    spriteRect.x += EditorGUIUtility.labelWidth;
    //    spriteRect.y += EditorGUIUtility.singleLineHeight + 4;
    //    spriteRect.width = sp.bounds.size.x * ratio;
    //    spriteRect.height = sp.bounds.size.y * ratio;

    //    if (spriteRect.height > 64)
    //    {
    //        ratio = 64 / spriteRect.height;

    //        spriteRect.width *= ratio;
    //        spriteRect.height *= ratio;
    //    }

    //    s_TempStyle.normal.background = sp.texture;
    //    s_TempStyle.Draw(spriteRect, GUIContent.none, false, false, false, false);

    //    EditorGUI.indentLevel = ident;
    //}

    //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //{
    //    return base.GetPropertyHeight(property, label) + 70f;
    //}
}
