using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LocalDataManager
{
    private static readonly string LOCAL_DATA_PATH = Application.dataPath + "/Editor/AssetProfiler/LocalResources/AssetData.bin";

    public static bool CheckExist()
    {
        return File.Exists(LOCAL_DATA_PATH);
    }

    public static void Serialize(object data)
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(LOCAL_DATA_PATH,  FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static object Deserialize()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(LOCAL_DATA_PATH, FileMode.Open, FileAccess.Read, FileShare.None);
        object data = formatter.Deserialize(stream);
        stream.Close();
        return data;
    }
}
