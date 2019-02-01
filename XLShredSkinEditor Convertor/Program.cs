using Pastel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

namespace XLShredSkinEditor_Convertor
{
    internal class Program
    {
        private static readonly string PathRootSkinFolder = $@"{System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\SkaterXL\Skin";
        private static readonly List<string> OrignalTexture = new List<string>();
        private static readonly List<string> FlipedTexture = new List<string>();
        private static readonly List<string> ErrorBuffer = new List<string>();
        private static readonly string ConsoleInfoText = "[-] ";
        public static readonly string ConsoleError = "[!] ";
        private static readonly string ConsoleValidateText = "[v] ".Pastel(Color.Green);

        private static void Main(string[] args)
        {
            Console.Title = "Skater XL Skin editor convertor - Omniscient";
            Console.WriteLine("");
            Console.WriteLine("Skater XL Skin editor convertor");
            Console.WriteLine($"A program by {"Omnscient".Pastel(Color.FromArgb(48, 227, 202))}");
            Console.WriteLine("https://github.com/Azahet \n".Pastel(Color.FromArgb(47, 137, 252)));

            Console.WriteLine("{0}Index all skin :", ConsoleInfoText);
            IndexSkins();

            Console.WriteLine("\n{0}Application of vertical symmetry on image", ConsoleInfoText);
            VerticalSymmetry();

            Console.WriteLine("\nDXT1 Conversion :");
            Dxt1Conversion();
            Console.WriteLine("\nFinishing...");           
            CleanFolder();

            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
        }

        public static void IndexSkins()
        {
            foreach (string currentDirectory in Directory.GetDirectories($@"{PathRootSkinFolder}\"))
            {
                foreach (string CurrentSkinName in Directory.GetFiles(currentDirectory, "*.png"))
                {
                    if (!CurrentSkinName.Contains("temp") & !CurrentSkinName.Contains("-converted"))
                    { 
                        OrignalTexture.Add(CurrentSkinName);
                        Console.WriteLine("{0}{1}", ConsoleInfoText, CurrentSkinName);
                    }
                }
            }
            if(OrignalTexture.Count == 0)
            {
                Console.WriteLine("{0} No skin find".Pastel(Color.Red), ConsoleError);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }

            Console.WriteLine("{0}{1} skins indexed", ConsoleValidateText, OrignalTexture.Count);
        }

        public static void VerticalSymmetry()
        {
            using (ProgressBar progress = new ProgressBar())
            {
                for (int i = 0; i < OrignalTexture.Count; i++)
                {
                    progress.Report((double)i / OrignalTexture.Count);

                    using (FileStream fs = new FileStream(OrignalTexture[i], FileMode.Open, FileAccess.Read))
                    {
                        Image image = Image.FromStream(fs);
                        if (!image.RawFormat.Equals(ImageFormat.Png)) {
                            ErrorBuffer.Add($"Wrong file format on : {OrignalTexture[i]}" );
                            image.Dispose();
                            continue;   
                        }
                        image.RotateFlip(RotateFlipType.Rotate180FlipX);
                        image.Save(OrignalTexture[i].Replace(".png", "-temp.png"), ImageFormat.Png);
                        FlipedTexture.Add(OrignalTexture[i].Replace(".png", "-temp.png"));
                        image.Dispose();
                  }

                }
            }
            if (ErrorBuffer.Count > 0)
            {
                foreach (var Error in ErrorBuffer)
                {
                    Console.WriteLine(ConsoleError + Error.Pastel(Color.Red));
                }
                ErrorBuffer.Clear();
            }
            Console.WriteLine("{0}{1}/{2} images processed", ConsoleValidateText, FlipedTexture.Count, OrignalTexture.Count);

        }
        //
        public static void Dxt1Conversion()
        {
            using (ProgressBar progress = new ProgressBar())
            {
                for (int i = 0; i < FlipedTexture.Count; i++)
                {
                    progress.Report((double)i / FlipedTexture.Count);
                    Process nvcompress = new Process();
                    nvcompress.StartInfo.FileName = $@"bin\nvcompress.exe";
                    nvcompress.StartInfo.Arguments = $"\"{FlipedTexture[i]}\" ";
                    nvcompress.StartInfo.UseShellExecute = false;
                    nvcompress.StartInfo.CreateNoWindow = true;
                    nvcompress.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    nvcompress.Start();
                    nvcompress.WaitForExit();
                }
            }
            Console.WriteLine("{0}{1} images converted", ConsoleValidateText, FlipedTexture.Count);
        }

        public static void CleanFolder()
        {
            foreach (string currentDirectory in Directory.GetDirectories($@"{PathRootSkinFolder}\"))
            {
                foreach (string CurrentSkinName in Directory.GetFiles(currentDirectory, "*"))
                {
                    if (CurrentSkinName.Contains("temp.png")) { 
                    File.Delete(CurrentSkinName);
                    }
                    else if(CurrentSkinName.Contains("temp.dds"))
                    {
                       File.Move(CurrentSkinName, CurrentSkinName.Replace("-temp.dds",".dds"));
                    }
                    else if (!CurrentSkinName.Contains("-converted.png"))
                    {
                        File.Move(CurrentSkinName, CurrentSkinName.Replace(".png", "-converted.png"));
                    }
                }
            }
            }
        }

}
