using System;
using System.IO;

namespace PowerSokoBan.Scripts.Tools;

public class GenerateSkins
{
    public static void Generate()
    {
        string folderPath = "E:\\Dev\\Godot\\PowerSokoBan\\Assets\\Textures\\Skins";
        string outputPath = "E:\\Dev\\Godot\\PowerSokoBan\\Assets\\DataBase\\Skins.txt";

        try
        {
            // 获取文件夹中的所有 .png 文件
            string[] pngFiles = Directory.GetFiles(folderPath, "*.png");

            // 创建一个新的 .txt 文件并打开以写入
            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                // 遍历所有 .png 文件并写入文件名到 .txt 文件
                foreach (string filePath in pngFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    writer.WriteLine(fileName);
                }
            }

            Console.WriteLine("文件名已成功写入到 .txt 文件中。");
        }
        catch (Exception e)
        {
            Console.WriteLine("发生错误: " + e.Message);
        }

        Console.ReadLine();
    }
}