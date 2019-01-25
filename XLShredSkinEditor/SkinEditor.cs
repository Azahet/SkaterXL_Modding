using System.Collections.Generic;
using UnityEngine;

namespace XLShredSkinEditor
{
    internal class SkinEditor : MonoBehaviour
    {
        public static bool show;

        Skin Skateboard = new Skin("Skateboard", new List<string> { "GripTape", "Hanger", "Wheel1 Mesh", "Wheel2 Mesh", "Wheel3 Mesh", "Wheel4 Mesh" });
        Skin TeeShirt = new Skin("TeeShirt", new List<string> { "Cory_fixed_Karam:cory_001:shirt_geo" });
        Skin Pants = new Skin("Pants", new List<string> { "Cory_fixed_Karam:cory_001:pants_geo", "Cory_fixed_Karam:cory_001:lashes_geo" });
        Skin Shoes = new Skin("Shoes", new List<string> { "Cory_fixed_Karam:cory_001:shoes_geo" });
       // Skin Body = new Skin("Body", new List<string> { "Cory_fixed_Karam:cory_001:Body_geo", "Cory_fixed_Karam:cory_001:lacrima_geo", "Cory_fixed_Karam:cory_001:teethUp_geo", "Cory_fixed_Karam:cory_001:teethDn_geo", "Cory_fixed_Karam:cory_001:tear_geo" });
        Skin Hat = new Skin("Hat", new List<string> { "Cory_fixed_Karam:cory_001:hat_geo" });
        Rect SkinEditorWindowRect = new Rect(20, 10, 170, 0);
        SkinEditorWindowShow SelectedWindow = SkinEditorWindowShow.MainSelector; 
        private void Start()
        {
            Skateboard.initialization();
            TeeShirt.initialization();
            Pants.initialization();
            Shoes.initialization();
            Body.initialization();
            Hat.initialization();
            Cursor.visible = true;
        }

        private void OnGUI()
        {
            if (show)
            {
                GUI.backgroundColor = Color.black;                 
                SkinEditorWindowRect = GUI.Window(0, SkinEditorWindowRect, SkinEditorWindow, "Skin Editor");
            }  
            
         
        }
        private void SkinEditorWindow(int windowID)
        {
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            switch (SelectedWindow)
            {
                case SkinEditorWindowShow.MainSelector:
                    GUI.Label(new Rect(50, 20, 500, 30), "Select type");

                    if (GUI.Button(new Rect(10, 40, 150, 25), "Skateboard"))
                    {
                        SelectedWindow = SkinEditorWindowShow.Skateboard;
                    }
                    if (GUI.Button(new Rect(10, 70, 150, 25), "TeeShirt"))
                    {
                        SelectedWindow = SkinEditorWindowShow.TeeShirt;
                    }
                    if (GUI.Button(new Rect(10, 100, 150, 25), "Pants"))
                    {
                        SelectedWindow = SkinEditorWindowShow.Pants;
                    }
                    if (GUI.Button(new Rect(10, 130, 150, 25), "Shoes"))
                    {
                        SelectedWindow = SkinEditorWindowShow.Shoes;
                    }
                    if (GUI.Button(new Rect(10, 160, 150, 25), "Hat"))
                    {
                        SelectedWindow = SkinEditorWindowShow.Hat;
                    }
                    GUI.backgroundColor = Color.red;
                    if (GUI.Button(new Rect(10, 190, 150, 25), "Exit"))
                    {
                        show = false;
                    }



                    SkinEditorWindowRect.height = 230;
                    break;

                case SkinEditorWindowShow.Skateboard:
                    DrawSelectSkin(Skateboard);
                    break;

                case SkinEditorWindowShow.TeeShirt:
                    DrawSelectSkin(TeeShirt);
                    break;

                case SkinEditorWindowShow.Shoes:
                    DrawSelectSkin(Shoes);
                    break;

                case SkinEditorWindowShow.Pants:
                    DrawSelectSkin(Pants);
                    break;
                case SkinEditorWindowShow.Hat:
                    DrawSelectSkin(Hat);
                    break;
            }
         
        }
        void DrawSelectSkin(Skin skin_)
        {
            GUI.Label(new Rect(50, 20, 500, 30), skin_.Name);
            for (int i = 0; i < skin_.AllSkinName.Count; i++)
            {
                string CurrentSkinName = skin_.AllSkinName[i];
                if (GUI.Button(new Rect(10, 40 + (30 * i), 150, 25), CurrentSkinName))
                {

                    skin_.SetTexture(CurrentSkinName);
                }

            }
            GUI.backgroundColor = Color.green;
            if (GUI.Button(new Rect(10, 40 + (30 * skin_.AllSkinName.Count), 150, 25), "Reload"))
            {
                skin_.initialization();
            }
            GUI.backgroundColor = Color.red;
            if (GUI.Button(new Rect(10, 40 + (30 * (skin_.AllSkinName.Count+1)), 150, 25), "Exit"))
            {

                SelectedWindow = SkinEditorWindowShow.MainSelector;
            }
            SkinEditorWindowRect.height = 40 + (30 * (skin_.AllSkinName.Count + 2));
        }
        enum SkinEditorWindowShow
        {
            MainSelector,
            Skateboard,
            TeeShirt,
            Pants,
            Shoes,
            Hat
        }

    }
}




