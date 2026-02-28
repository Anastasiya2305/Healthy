using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public static class UrpRendererConfigurator
{
    private const string RendererAssetPath = "Assets/_Project/Settings/URP/Renderer3D.asset";
    private const string SessionRunKey = "UrpRendererConfigurator.HasRun";

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        if (SessionState.GetBool(SessionRunKey, false))
        {
            return;
        }

        SessionState.SetBool(SessionRunKey, true);
        EditorApplication.delayCall += ConfigureRenderer;
    }

    private static void ConfigureRenderer()
    {
        var urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        if (urpAsset == null)
        {
            Debug.LogWarning("[URP Renderer Configurator] No active Universal Render Pipeline Asset found in Graphics settings.");
            return;
        }

        EnsureFolder("Assets/_Project");
        EnsureFolder("Assets/_Project/Editor");
        EnsureFolder("Assets/_Project/Settings");
        EnsureFolder("Assets/_Project/Settings/URP");

        var renderer3D = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(RendererAssetPath);
        var createdRendererAsset = false;

        if (renderer3D == null)
        {
            renderer3D = ScriptableObject.CreateInstance<UniversalRendererData>();
            AssetDatabase.CreateAsset(renderer3D, RendererAssetPath);
            EditorUtility.SetDirty(renderer3D);
            createdRendererAsset = true;
        }

        var urpSerializedObject = new SerializedObject(urpAsset);
        var rendererDataList = urpSerializedObject.FindProperty("m_RendererDataList");
        var defaultRendererIndex = urpSerializedObject.FindProperty("m_DefaultRendererIndex");

        if (rendererDataList == null || defaultRendererIndex == null)
        {
            Debug.LogError("[URP Renderer Configurator] Failed to locate URP serialized fields m_RendererDataList and m_DefaultRendererIndex.");
            return;
        }

        var rendererIndex = IndexOfRenderer(rendererDataList, renderer3D);
        var addedRendererToUrpAsset = false;

        if (rendererIndex < 0)
        {
            rendererIndex = rendererDataList.arraySize;
            rendererDataList.InsertArrayElementAtIndex(rendererIndex);
            rendererDataList.GetArrayElementAtIndex(rendererIndex).objectReferenceValue = renderer3D;
            addedRendererToUrpAsset = true;
        }

        var changedDefaultRenderer = false;
        if (defaultRendererIndex.intValue != rendererIndex)
        {
            defaultRendererIndex.intValue = rendererIndex;
            changedDefaultRenderer = true;
        }

        if (!createdRendererAsset && !addedRendererToUrpAsset && !changedDefaultRenderer)
        {
            return;
        }

        urpSerializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(urpAsset);
        AssetDatabase.SaveAssets();

        Debug.Log(
            $"[URP Renderer Configurator] Configured 3D renderer at '{RendererAssetPath}'. " +
            $"CreatedAsset={createdRendererAsset}, AddedToURP={addedRendererToUrpAsset}, " +
            $"SetDefaultRenderer={changedDefaultRenderer}, DefaultIndex={rendererIndex}."
        );
    }

    private static int IndexOfRenderer(SerializedProperty rendererDataList, UniversalRendererData target)
    {
        for (var i = 0; i < rendererDataList.arraySize; i++)
        {
            if (rendererDataList.GetArrayElementAtIndex(i).objectReferenceValue == target)
            {
                return i;
            }
        }

        return -1;
    }

    private static void EnsureFolder(string folderPath)
    {
        if (AssetDatabase.IsValidFolder(folderPath))
        {
            return;
        }

        var parent = Path.GetDirectoryName(folderPath)?.Replace('\\', '/');
        var folderName = Path.GetFileName(folderPath);

        if (!string.IsNullOrEmpty(parent) && !AssetDatabase.IsValidFolder(parent))
        {
            EnsureFolder(parent);
        }

        AssetDatabase.CreateFolder(parent ?? "Assets", folderName);
    }
}
