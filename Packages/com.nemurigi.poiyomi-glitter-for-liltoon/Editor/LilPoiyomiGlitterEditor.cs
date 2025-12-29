#if UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using lilToon;

namespace NmrgLibrary.PoiyomiGlitterForLiltoon.Editor
{
    public class LilPoiyomiGlitterEditor : lilToonInspector
    {
        private static bool isShowCustomProperties;
        private const string shaderName = "NmrgLibrary/lilPoiyomiGlitter";
        
        // Custom properties
        // MaterialProperty customVariable;
        // private bool isShowTexture;
        private bool isShowColorMap;
        private bool isShowGlitterMask;
        private bool isShowGlitterTexture;
        private bool isShowHueShift;
        private bool isShowRandomColors;

        // Glitter Properties
        MaterialProperty glitterUV;
        MaterialProperty glitterMode;
        MaterialProperty glitterShape;
        MaterialProperty glitterBlendType;
        MaterialProperty glitterColor;
        MaterialProperty glitterUseSurfaceColor;
        MaterialProperty glitterColorMap;
        MaterialProperty glitterColorMapPan;
        MaterialProperty glitterColorMapUV;
        MaterialProperty glitterPan;
        MaterialProperty glitterMask;
        MaterialProperty glitterMaskPan;
        MaterialProperty glitterMaskUV;
        MaterialProperty glitterTexture;
        MaterialProperty glitterTexturePan;
        MaterialProperty glitterTextureUV;
        MaterialProperty glitterUVPanning;
        MaterialProperty glitterTextureRotation;
        MaterialProperty glitterFrequency;
        MaterialProperty glitterJitter;
        MaterialProperty glitterSpeed;
        MaterialProperty glitterSize;
        MaterialProperty glitterContrast;
        MaterialProperty glitterAngleRange;
        MaterialProperty glitterMinBrightness;
        MaterialProperty glitterBrightness;
        MaterialProperty glitterBias;
        MaterialProperty glitterHideInShadow;
        MaterialProperty glitterCenterSize;
        MaterialProperty glitterFrequencyLinearEmissive;
        MaterialProperty glitterJaggyFix;
        MaterialProperty glitterHueShiftEnabled;
        MaterialProperty glitterHueShiftSpeed;
        MaterialProperty glitterHueShift;
        MaterialProperty glitterRandomColors;
        MaterialProperty glitterSaturationMin;
        MaterialProperty glitterSaturationMax;
        MaterialProperty glitterBrightnessMin;
        MaterialProperty glitterBrightnessMax;
        MaterialProperty glitterRandomSize;
        MaterialProperty glitterMinMaxSize;
        MaterialProperty glitterRandomRotation;

        protected override void LoadCustomProperties(MaterialProperty[] props, Material material)
        {
            isCustomShader = true;

            // If you want to change rendering modes in the editor, specify the shader here
            ReplaceToCustomShaders();
            isShowRenderMode = !material.shader.name.Contains("Optional");

            // If not, set isShowRenderMode to false
            // isShowRenderMode = false;

            LoadCustomLanguage("");

            // Load Glitter Properties
            glitterUV = FindProperty("_PoiGlitterUV", props);
            glitterMode = FindProperty("_PoiGlitterMode", props);
            glitterShape = FindProperty("_PoiGlitterShape", props);
            glitterBlendType = FindProperty("_PoiGlitterBlendType", props);
            glitterColor = FindProperty("_PoiGlitterColor", props);
            glitterUseSurfaceColor = FindProperty("_PoiGlitterUseSurfaceColor", props);
            glitterColorMap = FindProperty("_PoiGlitterColorMap", props);
            glitterColorMapPan = FindProperty("_PoiGlitterColorMapPan", props);
            glitterColorMapUV = FindProperty("_PoiGlitterColorMapUV", props);
            glitterPan = FindProperty("_PoiGlitterPan", props);
            glitterMask = FindProperty("_PoiGlitterMask", props);
            glitterMaskPan = FindProperty("_PoiGlitterMaskPan", props);
            glitterMaskUV = FindProperty("_PoiGlitterMaskUV", props);
            glitterTexture = FindProperty("_PoiGlitterTexture", props);
            glitterTexturePan = FindProperty("_PoiGlitterTexturePan", props);
            glitterTextureUV = FindProperty("_PoiGlitterTextureUV", props);
            glitterUVPanning = FindProperty("_PoiGlitterUVPanning", props);
            glitterTextureRotation = FindProperty("_PoiGlitterTextureRotation", props);
            glitterFrequency = FindProperty("_PoiGlitterFrequency", props);
            glitterJitter = FindProperty("_PoiGlitterJitter", props);
            glitterSpeed = FindProperty("_PoiGlitterSpeed", props);
            glitterSize = FindProperty("_PoiGlitterSize", props);
            glitterContrast = FindProperty("_PoiGlitterContrast", props);
            glitterAngleRange = FindProperty("_PoiGlitterAngleRange", props);
            glitterMinBrightness = FindProperty("_PoiGlitterMinBrightness", props);
            glitterBrightness = FindProperty("_PoiGlitterBrightness", props);
            glitterBias = FindProperty("_PoiGlitterBias", props);
            glitterHideInShadow = FindProperty("_PoiGlitterHideInShadow", props);
            glitterCenterSize = FindProperty("_PoiGlitterCenterSize", props);
            glitterFrequencyLinearEmissive = FindProperty("_PoiGlitterFrequencyLinearEmissive", props);
            glitterJaggyFix = FindProperty("_PoiGlitterJaggyFix", props);
            glitterHueShiftEnabled = FindProperty("_PoiGlitterHueShiftEnabled", props);
            glitterHueShiftSpeed = FindProperty("_PoiGlitterHueShiftSpeed", props);
            glitterHueShift = FindProperty("_PoiGlitterHueShift", props);
            glitterRandomColors = FindProperty("_PoiGlitterRandomColors", props);
            glitterSaturationMin = FindProperty("_PoiGlitterSaturationMin", props);
            glitterSaturationMax = FindProperty("_PoiGlitterSaturationMax", props);
            glitterBrightnessMin = FindProperty("_PoiGlitterBrightnessMin", props);
            glitterBrightnessMax = FindProperty("_PoiGlitterBrightnessMax", props);
            glitterRandomSize = FindProperty("_PoiGlitterRandomSize", props);
            glitterMinMaxSize = FindProperty("_PoiGlitterMinMaxSize", props);
            glitterRandomRotation = FindProperty("_PoiGlitterRandomRotation", props);
        }

        protected override void DrawCustomProperties(Material material)
        {
            // GUIStyles Name   Description
            // ---------------- ------------------------------------
            // boxOuter         outer box
            // boxInnerHalf     inner box
            // boxInner         inner box without label
            // customBox        box (similar to unity default box)
            // customToggleFont label for box

            isShowCustomProperties = Foldout("Poiyomi Glitter", "Poiyomi Style Glitter", isShowCustomProperties);
            if (isShowCustomProperties)
            {
                EditorGUILayout.BeginVertical(boxOuter);
                EditorGUILayout.LabelField("Glitter / Sparkle", customToggleFont);
                EditorGUILayout.BeginVertical(boxInnerHalf);
            
                // Basic Settings
                m_MaterialEditor.ShaderProperty(glitterUV, "UV");
                m_MaterialEditor.ShaderProperty(glitterMode, "Mode");
                m_MaterialEditor.ShaderProperty(glitterShape, "Shape");
                m_MaterialEditor.ShaderProperty(glitterBlendType, "Blend Mode");
            
                // Color Settings
                // m_MaterialEditor.ShaderProperty(glitterColor, "Color");
                // m_MaterialEditor.TexturePropertySingleLine(new GUIContent("Glitter Color Map"), glitterColorMap);
                // m_MaterialEditor.ShaderProperty(glitterColorMapPan, "Color Map Panning");
                // m_MaterialEditor.ShaderProperty(glitterColorMapUV, "Color Map UV");
                // Mask Settings
                // m_MaterialEditor.TexturePropertySingleLine(new GUIContent("Glitter Mask"), glitterMask);
                // m_MaterialEditor.ShaderProperty(glitterMaskPan, "Mask Panning");
                // m_MaterialEditor.ShaderProperty(glitterMaskUV, "Mask UV");
                // Texture Settings
                // m_MaterialEditor.TexturePropertySingleLine(new GUIContent("Glitter Texture"), glitterTexture);
                // m_MaterialEditor.ShaderProperty(glitterTexturePan, "Texture Panning");
                // m_MaterialEditor.ShaderProperty(glitterTextureUV, "Texture UV");
                
                m_MaterialEditor.ShaderProperty(glitterColor, "Color");
                m_MaterialEditor.ShaderProperty(glitterUseSurfaceColor, "Use Surface Color");
                
                
                lilEditorGUI.TextureGUI(
                    m_MaterialEditor,
                    false,
                    ref isShowColorMap,
                    new GUIContent("Glitter Color Map"),
                    glitterColorMap,
                    null,
                    glitterColorMapPan,
                    glitterColorMapUV,
                    true,
                    true
                );
            
                lilEditorGUI.TextureGUI(
                    m_MaterialEditor,
                    false,
                    ref isShowGlitterMask,
                    new GUIContent("Glitter Mask"),
                    glitterMask,
                    null,
                    glitterMaskPan,
                    glitterMaskUV,
                    true,
                    true);
            
                lilEditorGUI.TextureGUI(
                    m_MaterialEditor,
                    false,
                    ref isShowGlitterTexture,
                    new GUIContent("Glitter Texture"),
                    glitterTexture,
                    null,
                    glitterTexturePan,
                    glitterTextureUV,
                    true,
                    true);
            
                m_MaterialEditor.ShaderProperty(glitterUVPanning, "UV Panning Speed");
                m_MaterialEditor.ShaderProperty(glitterTextureRotation, "Texture Rotation Speed");
            
                // Main Parameters
                m_MaterialEditor.ShaderProperty(glitterFrequency, "Glitter Density");
                m_MaterialEditor.ShaderProperty(glitterJitter, "Glitter Jitter");
                m_MaterialEditor.ShaderProperty(glitterSpeed, "Glitter Speed");
                m_MaterialEditor.ShaderProperty(glitterSize, "Glitter Size");
                m_MaterialEditor.ShaderProperty(glitterContrast, "Glitter Contrast");
                m_MaterialEditor.ShaderProperty(glitterAngleRange, "Glitter Angle Range");
                m_MaterialEditor.ShaderProperty(glitterMinBrightness, "Glitter Min Brightness");
                m_MaterialEditor.ShaderProperty(glitterBrightness, "Glitter Max Brightness");
                m_MaterialEditor.ShaderProperty(glitterBias, "Glitter Bias");
                m_MaterialEditor.ShaderProperty(glitterHideInShadow, "Hide in Shadow");
                m_MaterialEditor.ShaderProperty(glitterCenterSize, "Dim Light");
                m_MaterialEditor.ShaderProperty(glitterFrequencyLinearEmissive, "Frequency (Linear Emissive)");
                m_MaterialEditor.ShaderProperty(glitterJaggyFix, "Jaggy Fix");
            
                lilEditorGUI.DrawLine();
            
                // Hue Shift
                m_MaterialEditor.ShaderProperty(glitterHueShiftEnabled, "Hue Shift");
                if (glitterHueShiftEnabled.floatValue > 0)
                {
                    EditorGUI.indentLevel++;
                    m_MaterialEditor.ShaderProperty(glitterHueShiftSpeed, "Shift Speed");
                    m_MaterialEditor.ShaderProperty(glitterHueShift, "Hue Shift");
                    EditorGUI.indentLevel--;
                }
            
                lilEditorGUI.DrawLine();
            
                // Random Colors
                m_MaterialEditor.ShaderProperty(glitterRandomColors, "Random Colors");
                if (glitterRandomColors.floatValue > 0)
                {
                    EditorGUI.indentLevel++;
                    m_MaterialEditor.ShaderProperty(glitterSaturationMin, "Saturation Min");
                    m_MaterialEditor.ShaderProperty(glitterSaturationMax, "Saturation Max");
                    m_MaterialEditor.ShaderProperty(glitterBrightnessMin, "Brightness Min");
                    m_MaterialEditor.ShaderProperty(glitterBrightnessMax, "Brightness Max");
            
                    m_MaterialEditor.ShaderProperty(glitterRandomSize, "Random Size");
                    if (glitterRandomSize.floatValue > 0)
                    {
                        EditorGUI.indentLevel++;
                        m_MaterialEditor.ShaderProperty(glitterMinMaxSize, "Size Range");
                        EditorGUI.indentLevel--;
                    }
            
                    m_MaterialEditor.ShaderProperty(glitterRandomRotation, "Random Texture Rotation");
                    EditorGUI.indentLevel--;
                }
            
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndVertical();
            }
        }

        protected override void ReplaceToCustomShaders()
        {
            lts = Shader.Find(shaderName + "/lilToon");
            ltsc = Shader.Find("Hidden/" + shaderName + "/Cutout");
            ltst = Shader.Find("Hidden/" + shaderName + "/Transparent");
            ltsot = Shader.Find("Hidden/" + shaderName + "/OnePassTransparent");
            ltstt = Shader.Find("Hidden/" + shaderName + "/TwoPassTransparent");

            ltso = Shader.Find("Hidden/" + shaderName + "/OpaqueOutline");
            ltsco = Shader.Find("Hidden/" + shaderName + "/CutoutOutline");
            ltsto = Shader.Find("Hidden/" + shaderName + "/TransparentOutline");
            ltsoto = Shader.Find("Hidden/" + shaderName + "/OnePassTransparentOutline");
            ltstto = Shader.Find("Hidden/" + shaderName + "/TwoPassTransparentOutline");

            ltsoo = Shader.Find(shaderName + "/[Optional] OutlineOnly/Opaque");
            ltscoo = Shader.Find(shaderName + "/[Optional] OutlineOnly/Cutout");
            ltstoo = Shader.Find(shaderName + "/[Optional] OutlineOnly/Transparent");

            ltstess = Shader.Find("Hidden/" + shaderName + "/Tessellation/Opaque");
            ltstessc = Shader.Find("Hidden/" + shaderName + "/Tessellation/Cutout");
            ltstesst = Shader.Find("Hidden/" + shaderName + "/Tessellation/Transparent");
            ltstessot = Shader.Find("Hidden/" + shaderName + "/Tessellation/OnePassTransparent");
            ltstesstt = Shader.Find("Hidden/" + shaderName + "/Tessellation/TwoPassTransparent");

            ltstesso = Shader.Find("Hidden/" + shaderName + "/Tessellation/OpaqueOutline");
            ltstessco = Shader.Find("Hidden/" + shaderName + "/Tessellation/CutoutOutline");
            ltstessto = Shader.Find("Hidden/" + shaderName + "/Tessellation/TransparentOutline");
            ltstessoto = Shader.Find("Hidden/" + shaderName + "/Tessellation/OnePassTransparentOutline");
            ltstesstto = Shader.Find("Hidden/" + shaderName + "/Tessellation/TwoPassTransparentOutline");

            ltsl = Shader.Find(shaderName + "/lilToonLite");
            ltslc = Shader.Find("Hidden/" + shaderName + "/Lite/Cutout");
            ltslt = Shader.Find("Hidden/" + shaderName + "/Lite/Transparent");
            ltslot = Shader.Find("Hidden/" + shaderName + "/Lite/OnePassTransparent");
            ltsltt = Shader.Find("Hidden/" + shaderName + "/Lite/TwoPassTransparent");

            ltslo = Shader.Find("Hidden/" + shaderName + "/Lite/OpaqueOutline");
            ltslco = Shader.Find("Hidden/" + shaderName + "/Lite/CutoutOutline");
            ltslto = Shader.Find("Hidden/" + shaderName + "/Lite/TransparentOutline");
            ltsloto = Shader.Find("Hidden/" + shaderName + "/Lite/OnePassTransparentOutline");
            ltsltto = Shader.Find("Hidden/" + shaderName + "/Lite/TwoPassTransparentOutline");

            ltsref = Shader.Find("Hidden/" + shaderName + "/Refraction");
            ltsrefb = Shader.Find("Hidden/" + shaderName + "/RefractionBlur");
            ltsfur = Shader.Find("Hidden/" + shaderName + "/Fur");
            ltsfurc = Shader.Find("Hidden/" + shaderName + "/FurCutout");
            ltsfurtwo = Shader.Find("Hidden/" + shaderName + "/FurTwoPass");
            ltsfuro = Shader.Find(shaderName + "/[Optional] FurOnly/Transparent");
            ltsfuroc = Shader.Find(shaderName + "/[Optional] FurOnly/Cutout");
            ltsfurotwo = Shader.Find(shaderName + "/[Optional] FurOnly/TwoPass");
            ltsgem = Shader.Find("Hidden/" + shaderName + "/Gem");
            ltsfs = Shader.Find(shaderName + "/[Optional] FakeShadow");

            ltsover = Shader.Find(shaderName + "/[Optional] Overlay");
            ltsoover = Shader.Find(shaderName + "/[Optional] OverlayOnePass");
            ltslover = Shader.Find(shaderName + "/[Optional] LiteOverlay");
            ltsloover = Shader.Find(shaderName + "/[Optional] LiteOverlayOnePass");

            ltsm = Shader.Find(shaderName + "/lilToonMulti");
            ltsmo = Shader.Find("Hidden/" + shaderName + "/MultiOutline");
            ltsmref = Shader.Find("Hidden/" + shaderName + "/MultiRefraction");
            ltsmfur = Shader.Find("Hidden/" + shaderName + "/MultiFur");
            ltsmgem = Shader.Find("Hidden/" + shaderName + "/MultiGem");
        }

        // You can create a menu like this
        /*
        [MenuItem("Assets/TemplateFull/Convert material to custom shader", false, 1100)]
        private static void ConvertMaterialToCustomShaderMenu()
        {
            if(Selection.objects.Length == 0) return;
            LiltoonPoiGlitterInspector inspector = new LiltoonPoiGlitterInspector();
            for(int i = 0; i < Selection.objects.Length; i++)
            {
                if(Selection.objects[i] is Material)
                {
                    inspector.ConvertMaterialToCustomShader((Material)Selection.objects[i]);
                }
            }
        }
        */

        /// <summary>
        /// Callback method for menu item which refreshes shader cache and reimport.
        /// </summary>
        [MenuItem("Assets/TemplateFull/Refresh shader cache", false, 2000)]
        private static void RefreshShaderCacheMenu()
        {
            var result = NativeMethods.Open("Library/ShaderCache.db", out var dbHandle);
            if (result != 0)
            {
                Debug.LogErrorFormat("Failed to open Library/ShaderCache.db [{0}]", result);
                return;
            }

            try
            {
                result = NativeMethods.Execute(dbHandle, "DELETE FROM shadererrors");
                if (result != 0)
                {
                    Debug.LogErrorFormat("SQL failed [{0}]", result);
                    return;
                }
            }
            finally
            {
                result = NativeMethods.Close(dbHandle);
                if (result != 0)
                {
                    Debug.LogErrorFormat("Failed to close database [{0}]", result);
                }
            }

            AssetDatabase.ImportAsset("Assets/TemplateFull/Shaders", ImportAssetOptions.ImportRecursive);
        }


        /// <summary>
        /// Provides some native methods of SQLite3.
        /// </summary>
        internal static class NativeMethods
        {
#if UNITY_EDITOR && !UNITY_EDITOR_WIN
            /// <summary>
            /// Native library name of SQLite3.
            /// </summary>
            private const string LibraryName = "sqlite3";
            /// <summary>
            /// Calling convention of library functions.
            /// </summary>
            private const CallingConvention CallConv = CallingConvention.Cdecl;
#else
            /// <summary>
            /// Native library name of SQLite3.
            /// </summary>
            private const string LibraryName = "winsqlite3";

            /// <summary>
            /// Calling convention of library functions.
            /// </summary>
            private const CallingConvention CallConv = CallingConvention.StdCall;
#endif
            /// <summary>
            /// Open database.
            /// </summary>
            /// <param name="filePath">SQLite3 database file path.</param>
            /// <param name="pDb">SQLite db handle.</param>
            /// <returns>Result code.</returns>
            /// <remarks>
            /// <seealso href="https://www.sqlite.org/c3ref/open.html"/>
            /// </remarks>
            [DllImport(LibraryName, EntryPoint = "sqlite3_open16", CallingConvention = CallConv,
                CharSet = CharSet.Unicode)]
            public static extern int Open([In] string filePath, out IntPtr pDb);

            /// <summary>
            /// Close database.
            /// </summary>
            /// <param name="pDb">SQLite db handle.</param>
            /// <returns>Result code.</returns>
            /// <remarks>
            /// <seealso href="https://www.sqlite.org/c3ref/close.html"/>
            /// </remarks>
            [DllImport(LibraryName, EntryPoint = "sqlite3_close", CallingConvention = CallConv)]
            public static extern int Close(IntPtr pDb);

            /// <summary>
            /// Execute specified SQL.
            /// </summary>
            /// <param name="pDb">SQLite db handle.</param>
            /// <param name="sql">SQL to be evaluated.</param>
            /// <param name="pCallback">Callback function.</param>
            /// <param name="pCallbackArg">1st argument to callback.</param>
            /// <param name="pErrMsg">Error message written here.</param>
            /// <returns>Result code.</returns>
            /// <remarks>
            /// <seealso href="https://www.sqlite.org/c3ref/exec.html"/>
            /// </remarks>
            [DllImport(LibraryName, EntryPoint = "sqlite3_exec", CallingConvention = CallConv)]
            public static extern int Execute(IntPtr pDb, [In] string sql, IntPtr pCallback = default(IntPtr), IntPtr pCallbackArg = default(IntPtr), IntPtr pErrMsg = default(IntPtr));
        }
    }
}
#endif