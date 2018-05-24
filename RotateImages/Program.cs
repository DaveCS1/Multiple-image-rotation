using System;
using System.Drawing;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace RotateImages
{
    class Program
    {
        static Program()
        {
            Resolver.RegisterDependencyResolver();
        }


        public static void RotateImageFile(string filepath, System.Drawing.RotateFlipType rft)
        {
            Image img = null;
            using (FileStream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                try { img = Image.FromStream(stream); }
                catch { }
                finally { stream.Close(); }
            }
            if (img != null)
            {
                try
                {
                    img.RotateFlip(rft);
                    img.Save(filepath);
                }
                catch { }
                finally { img.Dispose(); }
            }
        }

        [STAThreadAttribute]
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.WriteLine("Rotate Image script");

            Console.WriteLine("Schoose rotate angle [1:90  2:-90  3:+180]");
            var input = Console.ReadKey(true).Key;

            RotateFlipType rotateAngle = RotateFlipType.Rotate180FlipNone;
            if (input == ConsoleKey.D1) { rotateAngle = RotateFlipType.Rotate90FlipNone; }
            else if (input == ConsoleKey.D2) { rotateAngle = RotateFlipType.Rotate270FlipNone; }
            else if (input == ConsoleKey.D3) { rotateAngle = RotateFlipType.Rotate180FlipNone; }
            else { Console.WriteLine("Error input...Enter to exit"); Console.ReadLine(); return; }

            var folderDialog = new CommonOpenFileDialog()
            {
                Multiselect = true,
                IsFolderPicker = true,
                Title = "Select need folder with images"
            };

            var dialogResult = folderDialog.ShowDialog();
            if (dialogResult == CommonFileDialogResult.Cancel || dialogResult == CommonFileDialogResult.None) { Console.WriteLine("Error schoose folder...Enter to exit"); Console.ReadLine(); }

            foreach (var folderItem in folderDialog.FileNames)
            {
                var imgFiles = Directory.GetFiles(folderItem, "*", SearchOption.AllDirectories);
                Console.Write(String.Format("Rotating {0} | ", folderItem));
                var startXPos = Console.CursorLeft;

                var imgRotateIter = 1;
                foreach (var imgItem in imgFiles)
                {
                    RotateImageFile(imgItem, rotateAngle);
                    Console.Write(String.Format("[{0}/{1}] {2}", imgRotateIter, imgFiles.Length, Path.GetFileName(imgItem)));
                    imgRotateIter++;
                    Console.CursorLeft = startXPos;
                }
                Console.WriteLine();
            }
            Console.WriteLine("Rotate is complite. Enter to exit");
            Console.ReadLine();
        }
    }
}
