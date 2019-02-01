using Pastel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace XLShredSkinEditor_Convertor
{
    class Program_
    {
        private static string PathRootSkinFolder = $@"{System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\SkaterXL\Skin";
        static List<string> OrignalTexture = new List<string>();
        static List<string> FlipedTexture = new List<string>();


        static void Maddin(string[] args)
        {
            Console.Title = "Skater XL Skin editor convertor - Omniscient";
            Console.WriteLine("");
            Console.WriteLine("Skater XL Skin editor convertor");
            Console.WriteLine($"A program by {"Omnscient".Pastel(Color.FromArgb(48, 227, 202))}");
            Console.WriteLine("https://github.com/Azahet".Pastel(Color.FromArgb(47,137,252)));
            Console.WriteLine("\n");
            Console.WriteLine("Index all skin :");
            OrignalTexture = GetAllSkinName();
            try
            {
                foreach (var currentFile in OrignalTexture)
                    Console.WriteLine(currentFile);
            }
            catch (Exception)
            {

                Console.WriteLine("Error whend find skin (*.png)".Pastel(Color.OrangeRed));
            }
            Console.WriteLine($"{OrignalTexture.Count} skins indexed".Pastel(Color.SpringGreen));
          
            Console.WriteLine("\nProcess all skin :");
            processimg();
            Console.WriteLine($"{OrignalTexture.Count} processed image ".Pastel(Color.SpringGreen));
            Console.WriteLine("\nDXT1 Conversion :\n");

            for (int i = 0; i < FlipedTexture.Count; i++)
            {
             Console.SetCursorPosition(0, Console.CursorTop - 1);
             Console.Write(new string(' ', Console.BufferWidth));
             Console.WriteLine($"Processed file : {i + 1}/{FlipedTexture.Count} { drawTextProgressBar(i + 1, FlipedTexture.Count)}");             
              Process nvcompress = new Process();
              nvcompress.StartInfo.FileName = $@"{System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\bin\nvcompress.exe";
              nvcompress.StartInfo.Arguments = $"{FlipedTexture[i]} {OrignalTexture[i]}";
              nvcompress.StartInfo.UseShellExecute = true;
              nvcompress.StartInfo.CreateNoWindow = true;
              
            }
            Console.WriteLine("\nDXT1 Conversion :\n");
            foreach (var currentFile in FlipedTexture)
                File.Delete(currentFile);

            foreach (var currentFile in OrignalTexture)
                File.Move(currentFile, currentFile.Replace(".png","-Converted.png"));

            Console.WriteLine("\nConvertion finished...\nPress any key to exit");
            Console.ReadLine();
        }

        private static string drawTextProgressBar(int progress, int total)
        {
            Console.CursorLeft = 0;
            Console.Write("["); 
            Console.CursorLeft = 32;
            Console.Write("]"); 
            Console.CursorLeft = 1;
            float onechunk = 31.0f / total;            
            int position = 1;
            for (int i = 0; i < onechunk * progress; i++)
            {
                Console.CursorLeft = position++;
                Console.Write("#".Pastel(Color.SpringGreen));
            }            
            for (int i = position; i <= 31; i++)
            {
                Console.CursorLeft = position++;
                Console.Write("-");
            }

            Console.CursorLeft = 35;
            return $"{progress *100/total}%"; 
        }
        public static List<string> GetAllSkinName()
        {
            List<string> SkinName = new List<string>();
            foreach (var currentDirectory in Directory.GetDirectories($@"{PathRootSkinFolder}\"))
                { 
                foreach (var CurrentSkinName in Directory.GetFiles(currentDirectory, "*.png"))
              
                    if (!CurrentSkinName.Contains("temp") && !CurrentSkinName.Contains("converted"))
                    SkinName.Add(CurrentSkinName);                
            }
            return SkinName;
        }

        public static void processimg()
        {
            foreach (var currentDirectory in GetAllSkinName())
            {
                try
                {
                    var tempbmp = (Bitmap)Bitmap.FromFile(currentDirectory);
                    tempbmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                    tempbmp.Save($"{currentDirectory.Replace(".png","")}-temp.png");
                    FlipedTexture.Add($"{currentDirectory.Replace(".png", "")}-temp.png");
                    Console.WriteLine(currentDirectory);
                    Console.WriteLine(currentDirectory);
                   
               }
                catch (Exception Ex)
                {
                    Console.WriteLine($"{Ex}:  {currentDirectory.Pastel(Color.OrangeRed)}");
             
                }
                }
        }
        
      
    }



}
