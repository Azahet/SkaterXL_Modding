using Harmony12;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;
using XLShredLib;
using XLShredLib.UI;

namespace XLShredSkinEditor
{
    static class Main
    {
        public static bool enabled;
        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            Main.modId = modEntry.Info.Id;
            HarmonyInstance harmonyInstance = HarmonyInstance.Create(modEntry.Info.Id);
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            modEntry.OnToggle = OnToggle;
            ModUIBox uiBoxOmniscient = ModMenu.Instance.RegisterModMaker("com.Omniscient", "Omniscient");

            uiBoxOmniscient.AddCustom(() => {
                if (GUILayout.Button("Skin Editor", GUILayout.Height(30f)))
                {
                    SkinEditor.show = true;
                }
            }, () => enabled);

            ModMenu.Instance.RegisterShowCursor(Main.modId, () => SkinEditor.show ? 1 : 0);

            ModMenu.Instance.gameObject.AddComponent<SkinEditor>();
            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }



        // Token: 0x04000001 RID: 1
        public static string modId;
    }
}

