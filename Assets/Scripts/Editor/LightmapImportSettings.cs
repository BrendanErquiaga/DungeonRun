using UnityEngine;
using UnityEditor;
using System.Collections;

namespace RealDedicated_UnityEditorLibrary
{
    public class LightmapImportSettings : EditorWindow
    {
        [MenuItem("LightmapSize/AtlasSize_32")]
        static void Init_32()
        {
            LightmapEditorSettings.maxAtlasHeight = 32;
            LightmapEditorSettings.maxAtlasWidth = 32;
        }

        [MenuItem("LightmapSize/AtlasSize_64")]
        static void Init_64()
        {
            LightmapEditorSettings.maxAtlasHeight = 64;
            LightmapEditorSettings.maxAtlasWidth = 64;
        }

        [MenuItem("LightmapSize/AtlasSize_128")]
        static void Init_128()
        {
            LightmapEditorSettings.maxAtlasHeight = 128;
            LightmapEditorSettings.maxAtlasWidth = 128;
        }

        [MenuItem("LightmapSize/AtlasSize_256")]
        static void Init_256()
        {
            LightmapEditorSettings.maxAtlasHeight = 256;
            LightmapEditorSettings.maxAtlasWidth = 256;
        }

        [MenuItem("LightmapSize/AtlasSize_512")]
        static void Init_512()
        {
            LightmapEditorSettings.maxAtlasHeight = 512;
            LightmapEditorSettings.maxAtlasWidth = 512;
        }

        [MenuItem("LightmapSize/AtlasSize_1K")]
        static void Init_1024()
        {
            LightmapEditorSettings.maxAtlasHeight = 1024;
            LightmapEditorSettings.maxAtlasWidth = 1024;
        }

        [MenuItem("LightmapSize/AtlasSize_2K")]
        static void Init_2048()
        {
            LightmapEditorSettings.maxAtlasHeight = 2048;
            LightmapEditorSettings.maxAtlasWidth = 2048;
        }

        [MenuItem("LightmapSize/AtlasSize_4K")]
        static void Init_4096()
        {
            LightmapEditorSettings.maxAtlasHeight = 4096;
            LightmapEditorSettings.maxAtlasWidth = 4096;
        }
    }
}
