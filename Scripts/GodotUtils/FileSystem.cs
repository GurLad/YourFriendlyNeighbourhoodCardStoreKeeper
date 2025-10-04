using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class FileSystem
{
    // Consts
    public const char SEPERATOR = '/';

    public static string LoadTextFile(string path, string extension = ".json")
    {
        using var file = FileAccess.Open(path + extension, FileAccess.ModeFlags.Read);
        return file?.GetAsText();
    }

    public static Image LoadImageFile(string path, string extension = ".png")
    {
        if (FileAccess.FileExists(path + extension))
        {
            return Image.LoadFromFile(path + extension);
        }
        else
        {
            return null;
        }
    }

    public static Texture2D LoadTextureFile(string path, string extension = ".png")
    {
        Image img = LoadImageFile(path, extension);
        return img != null ? ImageTexture.CreateFromImage(img) : null;
    }

    public static void SaveTextFile(string path, string content, string extension = ".json")
    {
        using var dataFile = FileAccess.Open(path + extension, FileAccess.ModeFlags.Write);
        if (dataFile == null)
        {
            throw new Exception("Error creating file " + (path + extension) + "!");
        }
        dataFile.StoreString(content);
    }

    public static void SaveImageFile(string path, Image image, string extension = ".png")
    {
        image.SavePng(path + extension);
    }

    public static void Copy(string from, string to)
    {
        DirAccess.CopyAbsolute(from, to);
    }

    public static bool FileExists(string path)
    {
        return FileAccess.FileExists(path);
    }

    public static bool DirectoryExists(string path)
    {
        return DirAccess.DirExistsAbsolute(path);
    }

    public static void CreateDirectory(string path)
    {
        DirAccess.MakeDirRecursiveAbsolute(path);
    }

    public static List<string> GetFilesAtDirectory(string path, bool fullPath = true, string? ending = null)
    {
        List<string> res = DirAccess.GetFilesAt(path).ToList().FindAll(a => ending != null ? a.EndsWith(ending) : true);
        if (fullPath)
        {
            res = res.ConvertAll(a => path + SEPERATOR + a);
        }
        return res;
    }

    public static List<string> GetDirectories(string path, bool fullPath = true)
    {
        List<string> res = DirAccess.GetDirectoriesAt(path).ToList();
        if (fullPath)
        {
            res = res.ConvertAll(a => path + SEPERATOR + a);
        }
        return res;
    }

    private static void DeleteRecursive(string path)
    {
        DirAccess.GetFilesAt(path).ToList().ForEach(a => DirAccess.RemoveAbsolute(path + SEPERATOR + a));
        DirAccess.GetDirectoriesAt(path).ToList().ForEach(a => { DeleteRecursive(path + SEPERATOR + a); DirAccess.RemoveAbsolute(a); });
        DirAccess.RemoveAbsolute(path);
    }
}
