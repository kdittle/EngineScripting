using System;
using UnityEngine;
using UnityEditor;

internal class EffectShaderGUI : ShaderGUI
{
    private enum WorkflowMode
    {
        Specular,
        Metallic,
        Dielectric
    }

    public enum BlendMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent,
        Dissolve
    }

    public enum SmoothnessMapChannel
    {
        SpecularMetallicAlpha,
        AlbedoAlpha,
    }

    private static class Styles
    {
        public static GUIContent uvSetLabel = new GUIContent("UV Set");

        public static GUIContent albedoText = new GUIContent("Albedo", "Albedo (RGB) and Transparency (A).");
        public static GUIContent alphaCutoffText = new GUIContent("Alpha Cutoff", "Threshold for alpha cutoff");
        public static GUIContent specularMapText = new GUIContent("Specular", "Specular (RGB) and Smoothness (A).");
        public static GUIContent metallicMapText = new GUIContent("Metallic", "Metallic (R) and smoothness (A).");
        public static GUIContent smoothnessText = new GUIContent("Smoothness", "Smoothness Value.");
        public static GUIContent smoothnessScaleText = new GUIContent("Smoothness", "Smoothness cale factor.");
        public static GUIContent smoothnessMapChannelText = new GUIContent("Source", "Smoothness texture and channel.");
        public static GUIContent normalMapText = new GUIContent("Normal Map", "Normal Map.");
        public static GUIContent emissionMapText = new GUIContent("Emission Map", "Emission Map");
        public static GUIContent dissolveMapText = new GUIContent("Dissolve Map", "Dissolve Map.");
        public static GUIContent dissolveValueText = new GUIContent("Dissolve Value", "Dissolve Value.");
        public static GUIContent dissolveLineWidthtext = new GUIContent("Dissolve Line Width", "Dissolve Line Width");
        public static GUIContent detailAlbedoText  = new GUIContent("Detail Albedo x2", "Albed (RGB) multiplied by 2");
        public static GUIContent detailNormalMapText = new GUIContent("Normal Map", "Normal Map");
        public static GUIContent highlightText = new GUIContent("Highlights", "Highlights");
        public static GUIContent reflectionText = new GUIContent("Refelctions", "Refelctions");
        
        public static string primaryMapsText = "Main Maps";
        public static string secondaryMapsText = "Secondary Maps";
        public static string forwardText = "Forward Rendering Options";
        public static string renderingMode = "Rendering Mode";
        public static string advancedText = "Advanced Options";
        public static GUIContent emissiveWarning = new GUIContent("Emmisve Value is animated but the material has not been setup to handle emmission.");
        public static readonly string[] blendNames = Enum.GetNames(typeof(BlendMode));
    }

    private MaterialProperty blendMode = null;
    private MaterialProperty albedoMap = null;
    private MaterialProperty albedoColor = null;
    private MaterialProperty alphaCutoff = null;
    private MaterialProperty specularMap = null;
    private MaterialProperty specularColor = null;
    private MaterialProperty metallicMap = null;
    private MaterialProperty metallic = null;
    private MaterialProperty smoothness = null;
    private MaterialProperty smoothnessScale = null;
    private MaterialProperty smoothnessMapChannel = null;
    private MaterialProperty highlights = null;
    private MaterialProperty reflections = null;
    private MaterialProperty bumpScale = null;
    private MaterialProperty bumpMap = null;
    private MaterialProperty occlusionStrength = null;
    private MaterialProperty occlusionMap = null;
    private MaterialProperty emissionColorForRendering = null;
    private MaterialProperty emissionsMap = null;
    private MaterialProperty dissolveMap = null;
    private MaterialProperty dissolveValue = null;
    private MaterialProperty dissolveColor = null;
    private MaterialProperty dissolveLineWidth = null;
    private MaterialProperty detailAlbedoMap = null;
    private MaterialProperty detailNormalMap = null;
    private MaterialProperty detailNormalMapScale = null;
    private MaterialProperty uvSetSecondary = null;

    private MaterialEditor materialEditor;
    WorkflowMode workFlowMode = WorkflowMode.Specular;
    private ColorPickerHDRConfig colorPickedHDRconfig = new ColorPickerHDRConfig(0.0f, 99.0f, 1.0f/99.0f, 3.0f);

    private bool firstTimeApply = true;

    public void FindProperties(MaterialProperty[] properties)
    {
        blendMode = FindProperty("_Mode", properties);
        albedoMap = FindProperty("_MainTex", properties);
        albedoColor = FindProperty("_Color", properties);
        alphaCutoff = FindProperty("_Cutoff", properties);
        specularMap = FindProperty("_SpecGlossMap", properties, false);
        specularColor = FindProperty("_SpecColor", properties, false);
        metallicMap = FindProperty("_MetallicClossMap", properties, false);
        metallic = FindProperty("_Metallic", properties, false);

        if(specularMap != null && specularColor != null)
            workFlowMode = WorkflowMode.Specular;
        else if (metallicMap != null && metallic != null)
            workFlowMode = WorkflowMode.Metallic;
        else
            workFlowMode = WorkflowMode.Dielectric;

        smoothness = FindProperty("_Glossiness", properties);
        smoothnessScale = FindProperty("_ClossMapScale", properties, false);
        smoothnessMapChannel = FindProperty("_SmoothnessTextureChannel", properties, false);

        highlights = FindProperty("_SpecularHighlights", properties, false);
        reflections = FindProperty("_GlossyReflections", properties, false);

        bumpScale = FindProperty("_BumpScale", properties, false);
        bumpMap = FindProperty("_BumpMap", properties, false);

        //Heightmapscale
        //Heightmap

        occlusionMap = FindProperty("_OcclusionMap", properties);
        occlusionStrength = FindProperty("_OcclusionStrength", properties);

        emissionColorForRendering = FindProperty("_EmissionColor", properties);
        emissionsMap = FindProperty("_EmissionMap", properties);

        dissolveMap = FindProperty("_DissolveMap", properties, false);
        dissolveValue = FindProperty("_DissolveVal", properties, false);
        dissolveColor = FindProperty("_DissolveColor", properties, false);
        dissolveLineWidth = FindProperty("_LineWidth", properties, false);

        detailAlbedoMap = FindProperty("_DetailAlbedoMap", properties);
        detailNormalMap = FindProperty("_DetailNormalMap", properties);
        detailNormalMapScale = FindProperty("_DetailNormalMapScale", properties);

        uvSetSecondary = FindProperty("_UVSec", properties, false);
    }

    public override void OnGUI(MaterialEditor matEditor, MaterialProperty[] properties)
    {
        FindProperties(properties);
        materialEditor = matEditor;
        Material material = materialEditor.target as Material;

        //Ensure needed setup pieces are set up
        if (firstTimeApply)
        {
            MaterialChanged(material, workFlowMode);
            firstTimeApply = false;
        }

        ShaderPropertiesGUI(material);
    }

    public void ShaderPropertiesGUI(Material material)
    {
        //Default label width
        EditorGUIUtility.labelWidth = 0.0f;

        //Detect changes in materials
        EditorGUI.BeginChangeCheck();
        {
            BlendModePopup();

            //Primary Properties
            GUILayout.Label(Styles.primaryMapsText, EditorStyles.boldLabel);
            DoAlbedoArea(material);
            DoSpecularMetallicArea();
            materialEditor.TexturePropertySingleLine(Styles.normalMapText, bumpMap, bumpMap.textureValue != null ? bumpScale : null);
            DoEmissionArea(material);
            EditorGUI.BeginChangeCheck();
            materialEditor.TextureScaleOffsetProperty(albedoMap);

            if (EditorGUI.EndChangeCheck())
                emissionsMap.textureScaleAndOffset = albedoMap.textureScaleAndOffset;
                
            EditorGUILayout.Space();

            //Secondary Properties
            GUILayout.Label(Styles.secondaryMapsText, EditorStyles.boldLabel);
            materialEditor.TexturePropertySingleLine(Styles.detailAlbedoText, detailAlbedoMap);
            materialEditor.TexturePropertySingleLine(Styles.detailNormalMapText, detailNormalMap, detailNormalMap.textureValue != null ? detailNormalMapScale : null);
            materialEditor.TextureScaleOffsetProperty(detailAlbedoMap);
            materialEditor.ShaderProperty(uvSetSecondary, Styles.uvSetLabel.text);

            //Thid properties
            GUILayout.Label(Styles.forwardText, EditorStyles.boldLabel);
            if(highlights != null)
                materialEditor.ShaderProperty(highlights, Styles.highlightText);
            if(reflections != null)
                materialEditor.ShaderProperty(reflections, Styles.reflectionText);
        }

        if (EditorGUI.EndChangeCheck())
        {
            foreach(var obj in blendMode.targets)
                MaterialChanged((Material)obj, workFlowMode);
        }

        EditorGUILayout.Space();

        //Special Custom Properties
        GUILayout.Label(Styles.dissolveMapText, EditorStyles.boldLabel);
        materialEditor.TexturePropertySingleLine(Styles.dissolveMapText, dissolveMap);
        materialEditor.TextureScaleOffsetProperty(dissolveMap);
        materialEditor.ShaderProperty(dissolveValue, Styles.dissolveValueText);
        materialEditor.ShaderProperty(dissolveColor, "Dissolve Color");
        materialEditor.ShaderProperty(dissolveLineWidth, "Dissolve Line Width");

        EditorGUILayout.Space();

        //Advanced Objections
        GUILayout.Label(Styles.advancedText, EditorStyles.boldLabel);
        materialEditor.RenderQueueField();
        materialEditor.EnableInstancingField();

    }

    internal void DetermineWorkFlow(MaterialProperty[] properties)
    {
        if(FindProperty("_SecGlossMap", properties, false) != null && FindProperty("_SpecColor", properties, false) != null)
            workFlowMode = WorkflowMode.Specular;
        else if(FindProperty("_MetallicGlossMap", properties, false) != null && FindProperty("_Metallic", properties, false) != null)
            workFlowMode = WorkflowMode.Metallic;
        else
            workFlowMode = WorkflowMode.Dielectric;
    }

    public override void AssignNewShaderToMaterial(Material mat, Shader oldShader, Shader newShader)
    {
        if(mat.HasProperty("_Emission"))
            mat.SetColor("_EmissionColor", mat.GetColor("_Emission"));

        base.AssignNewShaderToMaterial(mat, oldShader, newShader);

        if (oldShader != null || !oldShader.name.Contains("Legacy Shaders/"))
        {
            SetupMaterialWithBlendMode(mat, (BlendMode)mat.GetFloat("_Mode"));
            return;
        }

        BlendMode bm = BlendMode.Opaque;
        if(oldShader.name.Contains("/Transparent/Cutout/"))
            bm = BlendMode.Cutout;
        else if (oldShader.name.Contains("/Transparent"))
            bm = BlendMode.Fade;

        mat.SetFloat("_Mode", (float)bm);

        DetermineWorkFlow(MaterialEditor.GetMaterialProperties(new Material[] {mat}));
        MaterialChanged(mat, workFlowMode);
    }

    void BlendModePopup()
    {
        EditorGUI.showMixedValue = blendMode.hasMixedValue;
        var mode = (BlendMode) blendMode.floatValue;

        EditorGUI.BeginChangeCheck();
        mode = (BlendMode)EditorGUILayout.Popup(Styles.renderingMode, (int) mode, Styles.blendNames);

        if (EditorGUI.EndChangeCheck())
        {
            materialEditor.RegisterPropertyChangeUndo("Rendering Mode");
            blendMode.floatValue = (float) mode;
        }

        EditorGUI.showMixedValue = false;

    }

    void DoAlbedoArea(Material material)
    {
        materialEditor.TexturePropertySingleLine(Styles.albedoText, albedoMap, albedoColor);

        if(((BlendMode)material.GetFloat("_Mode") == BlendMode.Cutout))
            materialEditor.ShaderProperty(alphaCutoff, Styles.alphaCutoffText.text, MaterialEditor.kMiniTextureFieldLabelIndentLevel + 1);      
    }

    void DoEmissionArea(Material material)
    {
        if (materialEditor.EmissionEnabledProperty())
        {
            bool hasEmissionTexture = emissionsMap.textureValue != null;

            materialEditor.TexturePropertyWithHDRColor(Styles.emissionMapText, emissionsMap, emissionColorForRendering, colorPickedHDRconfig, false);

            float brightness = emissionColorForRendering.colorValue.maxColorComponent;
            if (emissionsMap.textureValue != null && !hasEmissionTexture && brightness <= 0.0f)
                emissionColorForRendering.colorValue = Color.white;

            materialEditor.LightmapEmissionFlagsProperty(MaterialEditor.kMiniTextureFieldLabelIndentLevel, true);
        }
    }

    void DoSpecularMetallicArea()
    {
        bool hasGlossMap = false;
        if (workFlowMode == WorkflowMode.Specular)
        {
            hasGlossMap = specularMap.textureValue != null;
            materialEditor.TexturePropertySingleLine(Styles.specularMapText, specularMap, hasGlossMap ? null : specularColor);
        }
        else if (workFlowMode == WorkflowMode.Metallic)
        {
            hasGlossMap = metallicMap.textureValue != null;
            materialEditor.TexturePropertySingleLine(Styles.metallicMapText, metallicMap, hasGlossMap ? null : metallic);
        }

        bool showSmoothnessScale = hasGlossMap;
        if (smoothnessMapChannel != null)
        {
            int smoothnessChannel = (int) smoothnessMapChannel.floatValue;
            if (smoothnessChannel == (int) SmoothnessMapChannel.AlbedoAlpha)
                showSmoothnessScale = true;
        }

        int indentation = 2;
        materialEditor.ShaderProperty(showSmoothnessScale ? smoothnessScale : smoothness, showSmoothnessScale ? Styles.smoothnessScaleText : Styles.smoothnessText, indentation);

        ++indentation;
        if(smoothnessMapChannel != null)
            materialEditor.ShaderProperty(smoothnessMapChannel, Styles.smoothnessMapChannelText, indentation);
    }

    public static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode)
    {
        switch (blendMode)
        {
            case BlendMode.Opaque:
            {
                material.SetOverrideTag("RenderType", "");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            }
            case BlendMode.Cutout:
            {
                material.SetOverrideTag("RenderType", "TransparentCutout");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.AlphaTest;
                break;
            }
            case BlendMode.Fade:
            {
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
                break;
            }
            case BlendMode.Transparent:
            {
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
                break;
            }
            case BlendMode.Dissolve:
                {
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
                    break;
                }
        }
    }

    static SmoothnessMapChannel GetSmoothnessMapChannel(Material material)
    {
        int ch = (int) material.GetFloat("_SmoothnessTextureChannel");
        if(ch == (int)SmoothnessMapChannel.AlbedoAlpha)
            return SmoothnessMapChannel.AlbedoAlpha;
        else
            return SmoothnessMapChannel.SpecularMetallicAlpha;
    }

    static void SetMaterialKeywords(Material material, WorkflowMode workFlowMode)
    {
        //These must be based on the material value, not the material property
        SetKeyword(material, "_NORMALMAP", material.GetTexture("_BumpMap") || material.GetTexture("_DetailNormalMap"));

        if(workFlowMode == WorkflowMode.Specular)
            SetKeyword(material, "_SPECGLOSSMAP", material.GetTexture("_SpecGlossMap"));
        else if(workFlowMode == WorkflowMode.Metallic)
            SetKeyword(material, "_METALLICGLOSSMAP", material.GetTexture("_MetallicGlossMap"));
        
        SetKeyword(material, "_PARALLAXMAP", material.GetTexture("_ParallaxMap"));
        SetKeyword(material, "_DETAIL_MULX2", material.GetTexture("_DetailAlbedoMap") || material.GetTexture("_DetailNormalMap"));

        //SetKeyword(material, "_DISSOLVEMAP", material.GetTexture("_DissolveMap"));

        //Material's GI flag keeps track of emission being enabled or not.
        MaterialEditor.FixupEmissiveFlag(material);
        bool shouldEmissionBeEnabled = (material.globalIlluminationFlags & MaterialGlobalIlluminationFlags.EmissiveIsBlack) == 0;
        SetKeyword(material, "_EMISSION", shouldEmissionBeEnabled);

        if(material.HasProperty("SmoothnessTextureChannel"))
            SetKeyword(material, "_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A", GetSmoothnessMapChannel(material) == SmoothnessMapChannel.AlbedoAlpha);
    }

    static void MaterialChanged(Material material, WorkflowMode workFlowMode)
    {
        SetupMaterialWithBlendMode(material, (BlendMode)material.GetFloat("_Mode"));
        SetMaterialKeywords(material, workFlowMode);
    }

    static void SetKeyword(Material material, string keyword, bool state)
    {
        if(state)
            material.EnableKeyword(keyword);
        else
            material.DisableKeyword(keyword);
    }
}