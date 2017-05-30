using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public enum FileSizeUnitType
{
	Type_b,
	Type_Kb,
	Type_Mb,
	Type_Gb
}

public class FileManager {
	
	public static void CreateDirectory(string directory)
	{
		if (!IsDirectoryExist(directory))
		{
			Directory.CreateDirectory(directory);
		}
	}
	
	public static bool IsFileExist(string fullPath)
	{
		return File.Exists(fullPath);
	}
	
	public static bool IsDirectoryExist(string fullPath)
	{
		return Directory.Exists(fullPath);
	}
	
	/**
    * path：文件创建目录
    *  info：写入的内容
    */
	public static void CreateFileWithString(string path, string info)
	{
        //文件流信息
        StreamWriter sw;
        FileInfo t = new FileInfo(path);

        CreateDirectory(System.IO.Path.GetDirectoryName(path));

        if (!t.Exists)
        {
            //如果此文件不存在则创建
            sw = t.CreateText();
        }
        else
        {
            //如果此文件存在则删除
            File.Delete(path);
            sw = t.CreateText();
        }
        //以行的形式写入信息
        sw.Write(info);
        //关闭流
        sw.Close();
        //销毁流
        sw.Dispose();
    }
	
	/**
    * path：文件创建目录
    *  info：写入的内容
    */
	public static void CreateFileWithBytes(string path, byte[] info, int length)
	{
        //文件流信息
        Stream sw;
        FileInfo t = new FileInfo(path);
        CreateDirectory(System.IO.Path.GetDirectoryName(path));

        if (!t.Exists)
        {
            //如果此文件不存在则创建
            sw = t.Create();
        }
        else
        {
            //如果此文件存在则删除
            File.Delete(path);
            sw = t.Create();
        }
        //以行的形式写入信息
        //sw.WriteLine(info);
        sw.Write(info, 0, length);
        //关闭流
        sw.Close();
        //销毁流
        sw.Dispose();
    }
	
	/**
   * path：删除文件的路径
   */
	public static void DeleteFile(string path)
	{
		if (IsFileExist(path))
		{
			File.Delete(path);
		}
	}
	
	public static void DeleteDirectory(string directoryPath)
	{
        if (IsDirectoryExist(directoryPath))
        {
            Directory.Delete(directoryPath, true);
        }
    }
	
	public static float GetFileSize(FileSizeUnitType sizeType, long size)
	{
		long baseValue = 1;
		switch (sizeType)
		{
		case FileSizeUnitType.Type_b:
			break;
		case FileSizeUnitType.Type_Kb:
			baseValue = 1024;
			break;
		case FileSizeUnitType.Type_Mb:
			baseValue = 1024 * 1024;
			break;
		case FileSizeUnitType.Type_Gb:
			baseValue = 1024 * 1024 * 1024;
			break;
		default:
			break;
		}
		
		return (float)size / baseValue;
	}
}
