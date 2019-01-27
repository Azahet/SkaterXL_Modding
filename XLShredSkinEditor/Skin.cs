using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XLShredSkinEditor
{
  internal  class Skin
    {
        private string PathRootSkinFolder = $@"{System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\SkaterXL\Skin";
        const string MainTextureName = "Texture2D_4128E5C7";
        public Skin(string name, List<String> GameObjects)
        {
            Name = name;
            GameObjects_ = GameObjects;
        }
        
       public string Name { get; }
        public List<string> GameObjects_ { get; }

        public List<string> AllSkinName;


        public void initialization()
        {
            AllSkinName = GetAllSkinName();
            Debug.Log($"Initialization finish : {AllSkinName.ToString()}");
        }
        public void SetTexture(string TextureName)
        {
            Texture2D Texture = LoadTextureDXT(File.ReadAllBytes($@"{PathRootSkinFolder}\{Name}\{TextureName}.dds"), TextureFormat.DXT1);
            foreach (var GameObject_ in GameObjects_)
            {
                foreach (var CurrentGameObject_ in Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == GameObject_))
                {
                   CurrentGameObject_.GetComponent<Renderer>().material.SetTexture(MainTextureName, Texture);
                }
            }           
        }

  

        public List<string> GetAllSkinName()
        {
            List<string> SkinName = new List<string>();

            foreach (var CurrentSkinName in Directory.GetFiles($@"{PathRootSkinFolder}\{Name}", "*.dds"))
            {
                SkinName.Add(Path.GetFileNameWithoutExtension(CurrentSkinName));
            }
            return SkinName;
        }
        
        private Texture2D LoadTextureDXT(byte[] ddsBytes, TextureFormat textureFormat)
        {
            if (textureFormat != TextureFormat.DXT1 && textureFormat != TextureFormat.DXT5)
                throw new Exception("Invalid TextureFormat. Only DXT1 and DXT5 formats are supported by this method.");

            byte ddsSizeCheck = ddsBytes[4];
            if (ddsSizeCheck != 124)
                throw new Exception("Invalid DDS DXTn texture. Unable to read");

            int height = ddsBytes[13] * 256 + ddsBytes[12];
            int width = ddsBytes[17] * 256 + ddsBytes[16];

            int DDS_HEADER_SIZE = 128;
            byte[] dxtBytes = new byte[ddsBytes.Length - DDS_HEADER_SIZE];
            Buffer.BlockCopy(ddsBytes, DDS_HEADER_SIZE, dxtBytes, 0, ddsBytes.Length - DDS_HEADER_SIZE);

            Texture2D texture = new Texture2D(width, height, textureFormat, false);
            texture.LoadRawTextureData(dxtBytes);
            texture.Apply();

            return (texture);
        }
    }
}
