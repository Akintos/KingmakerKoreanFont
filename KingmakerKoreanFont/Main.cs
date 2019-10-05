using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityModManagerNet;
using TMPro;
using UnityEngine;

namespace KingmakerKoreanFont
{
    public static class Main
    {
        private static UnityModManager.ModEntry.ModLogger logger;
        private static string modPath;

        public static void Load(UnityModManager.ModEntry modEntry)
        {
            logger = modEntry.Logger;
            modPath = modEntry.Path;
            PatchFont();
        }

        private static bool PatchFont()
        {
            string bundlePath = Path.Combine(modPath, "koreanfont");
            if (!File.Exists(bundlePath))
            {
                logger.Log("ERROR: Korean font AssetBundle file is not found.");
                return false;
            }
            AssetBundle fontBundle = AssetBundle.LoadFromFile(bundlePath);
            if (!fontBundle)
            {
                logger.Log("ERROR: Faiiled to load Korean font AssetBundle");
                return false;
            }

            logger.Log("Loaded Korean font AssetBundle");
        
            TMP_FontAsset NotoSerif = fontBundle.LoadAsset<TMP_FontAsset>("NotoSerifCJKkr-Medium_SDF");
            if (!NotoSerif)
            {
                logger.Log("ERROR: Faiiled to load Korean TMP_FontAsset");
                return false;
            }
            logger.Log("Loaded Korean TMP_FontAsset");

            TMP_FontAsset NexusSerif = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().FirstOrDefault(asset => asset.name.StartsWith("NexusSerif"));

            if (NexusSerif)
            {
                NexusSerif.fallbackFontAssets.Add(NotoSerif);
                logger.Log($"Added {NotoSerif.name} to {NexusSerif.name} fallback font list.");
                return true;
            }
            else
            {
                logger.Log("ERROR: Failed to find NexusSerif font asset.");
                return false;
            }
        }
    }
}
