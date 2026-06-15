using UnityEngine;

namespace MegaRaketa.Marketing
{
    internal static class MarketingSerializedConfigUtility
    {
        public static void SetFloat(ScriptableObject config, string propertyName, float value)
        {
            if (config == null)
            {
                return;
            }

            SerializedConfigAccess.SetFloat(config, propertyName, value);
        }

        public static void SetVector3(ScriptableObject config, string propertyName, Vector3 value)
        {
            if (config == null)
            {
                return;
            }

            SerializedConfigAccess.SetVector3(config, propertyName, value);
        }

        public static void SetObjectReference(ScriptableObject config, string propertyName, Object value)
        {
            if (config == null)
            {
                return;
            }

            SerializedConfigAccess.SetObjectReference(config, propertyName, value);
        }

        public static float GetFloat(ScriptableObject config, string propertyName)
        {
            return config == null ? 0f : SerializedConfigAccess.GetFloat(config, propertyName);
        }

        public static Vector3 GetVector3(ScriptableObject config, string propertyName)
        {
            return config == null ? Vector3.zero : SerializedConfigAccess.GetVector3(config, propertyName);
        }

        public static Object GetObjectReference(ScriptableObject config, string propertyName)
        {
            return config == null ? null : SerializedConfigAccess.GetObjectReference(config, propertyName);
        }
    }

#if UNITY_EDITOR
    internal static class SerializedConfigAccess
    {
        public static void SetFloat(ScriptableObject config, string propertyName, float value)
        {
            var serializedObject = new UnityEditor.SerializedObject(config);
            UnityEditor.SerializedProperty property = serializedObject.FindProperty(propertyName);
            if (property == null)
            {
                return;
            }

            property.floatValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        public static void SetVector3(ScriptableObject config, string propertyName, Vector3 value)
        {
            var serializedObject = new UnityEditor.SerializedObject(config);
            UnityEditor.SerializedProperty property = serializedObject.FindProperty(propertyName);
            if (property == null)
            {
                return;
            }

            property.vector3Value = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        public static void SetObjectReference(ScriptableObject config, string propertyName, Object value)
        {
            var serializedObject = new UnityEditor.SerializedObject(config);
            UnityEditor.SerializedProperty property = serializedObject.FindProperty(propertyName);
            if (property == null)
            {
                return;
            }

            property.objectReferenceValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        public static float GetFloat(ScriptableObject config, string propertyName)
        {
            var serializedObject = new UnityEditor.SerializedObject(config);
            UnityEditor.SerializedProperty property = serializedObject.FindProperty(propertyName);
            return property != null ? property.floatValue : 0f;
        }

        public static Vector3 GetVector3(ScriptableObject config, string propertyName)
        {
            var serializedObject = new UnityEditor.SerializedObject(config);
            UnityEditor.SerializedProperty property = serializedObject.FindProperty(propertyName);
            return property != null ? property.vector3Value : Vector3.zero;
        }

        public static Object GetObjectReference(ScriptableObject config, string propertyName)
        {
            var serializedObject = new UnityEditor.SerializedObject(config);
            UnityEditor.SerializedProperty property = serializedObject.FindProperty(propertyName);
            return property != null ? property.objectReferenceValue : null;
        }
    }
#else
    internal static class SerializedConfigAccess
    {
        public static void SetFloat(ScriptableObject config, string propertyName, float value) { }
        public static void SetVector3(ScriptableObject config, string propertyName, Vector3 value) { }
        public static void SetObjectReference(ScriptableObject config, string propertyName, Object value) { }
        public static float GetFloat(ScriptableObject config, string propertyName) => 0f;
        public static Vector3 GetVector3(ScriptableObject config, string propertyName) => Vector3.zero;
        public static Object GetObjectReference(ScriptableObject config, string propertyName) => null;
    }
#endif
}
