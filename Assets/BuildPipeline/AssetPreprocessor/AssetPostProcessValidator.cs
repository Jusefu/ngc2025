using System;
using UnityEditor;
using UnityEngine;

public class AssetPostprocessorValidation : AssetPostprocessor
{
    // Called before Unity imports a texture
    void OnPreprocessTexture()
    {
        TextureImporter textureImporter = assetImporter as TextureImporter;

        // Apply automatic settings based on folder path
        if (assetPath.Contains("UI/"))
        {
            // UI textures should use UI sprite mode
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.alphaIsTransparency = true;

            // Set compression based on platform
            SetupUITextureCompression(textureImporter);
        }
        else if (assetPath.Contains("Textures/Environment/"))
        {
            // Environment textures
            textureImporter.textureType = TextureImporterType.Default;

            // Enable normal map import for normal maps
            if (assetPath.Contains("_Normal") || assetPath.Contains("_N"))
            {
                textureImporter.textureType = TextureImporterType.NormalMap;
            }
        }

        ValidateTexture(assetPath);
    }

    // Called before Unity imports an audio clip
    void OnPreprocessAudio()
    {
        AudioImporter audioImporter = assetImporter as AudioImporter;

        // Apply different settings for sound effects vs music
        if (assetPath.Contains("SFX/"))
        {
            // Sound effects usually work well with compression
            AudioImporterSampleSettings settings = new AudioImporterSampleSettings
            {
                loadType = AudioClipLoadType.DecompressOnLoad,
                compressionFormat = AudioCompressionFormat.Vorbis,
                quality = 0.75f
            };

            audioImporter.defaultSampleSettings = settings;
        }
    }

    private void SetupUITextureCompression(TextureImporter textureImporter)
    {
        // Set compression settings for UI textures
        TextureImporterPlatformSettings platformSettings = new TextureImporterPlatformSettings
        {
            name = "Standalone",
            maxTextureSize = 1024,
            format = TextureImporterFormat.BC4,
            compressionQuality = 50,
            textureCompression = TextureImporterCompression.Uncompressed
        };
        textureImporter.SetPlatformTextureSettings(platformSettings);
    }

    static int ValidateTexture(string assetPath)
    {
        
        int issues = 0;
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);

        if (texture == null)
            return issues;
        
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

        // Check texture size
        if (texture.width > 2048 || texture.height > 2048)
        {
            if (assetPath.Contains("UI/"))
            {
                Debug.LogWarning($"UI texture is very large: {assetPath}");
                issues++;
            }
        }

        // Check power of two
        bool isPowerOfTwo = IsPowerOfTwo(texture.width) && IsPowerOfTwo(texture.height);
        if (!isPowerOfTwo && !assetPath.Contains("UI/"))
        {
            Debug.LogWarning($"Non-power-of-two texture: {assetPath}");
            issues++;
        }

        // Check compression settings
        if (importer != null && importer.textureCompression == TextureImporterCompression.Uncompressed)
        {
            Debug.LogWarning($"Uncompressed texture: {assetPath}");
            issues++;
        }

        return issues;
    }

    private static bool IsPowerOfTwo(int width)
    {
        return width > 0 && (width & (width - 1)) == 0;
    }
}