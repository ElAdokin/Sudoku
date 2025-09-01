using UnityEngine;
using System;
using System.IO;

public static class SaveLoad<T>
{
    private static string _jsonData;

    private static T _returnedData;

    private static string _dataPath;

    private static byte[] _byteData;

    public static void Save(T data, string folder, string file)
    {

#if UNITY_WEBGL

        _jsonData = JsonUtility.ToJson(data, true);
        
        PlayerPrefs.SetString(file, _jsonData);

#else
        
        _dataPath = GetFilePath(folder, file);

        _jsonData = JsonUtility.ToJson(data, true);
        
        _byteData = Encoding.ASCII.GetBytes(jsonData);

        if (!Directory.Exists(Path.GetDirectoryName(_dataPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_dataPath));
        }

        try
        {
            File.WriteAllBytes(_dataPath,_byteData);
            Debug.Log("Save data to: " + _dataPath);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save data to: " + _dataPath);
            Debug.LogError("Error " + e.Message);
        }
#endif

    }

    public static T Load(string folder, string file)
    {
#if UNITY_WEBGL

        _jsonData = PlayerPrefs.GetString(file);

        _returnedData = JsonUtility.FromJson<T>(_jsonData);

#else
        _dataPath = GetFilePath(folder, file);

        if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
        {
            Debug.LogWarning("File or path does not exist! " + _dataPath);
            return default(T);
        }

        _byteData = null;

        try
        {
            _byteData = File.ReadAllBytes(_dataPath);
            Debug.Log("<color=green>Loaded all data from: </color>" + _dataPath);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to load data from: " + _dataPath);
            Debug.LogWarning("Error: " + e.Message);
            return default(T);
        }

        if (_byteData == null)
            return default(T);

        _jsonData = Encoding.ASCII.GetString(_byteData);

        _returnedData = JsonUtility.FromJson<T>(jsonData);

#endif

        return (T)Convert.ChangeType(_returnedData, typeof(T));
    }

    public static void DeleteSaveData(string folder, string file) 
    {
#if UNITY_WEBGL

        PlayerPrefs.DeleteKey(file);

#else
        _dataPath = GetFilePath(folder, file);

        if (!Directory.Exists(Path.GetDirectoryName(_dataPath)))
        {
            Debug.LogWarning("File or path does not exist! " + _dataPath);
            return;
        }

        if (File.Exists(_dataPath))
        {
            File.Delete(_dataPath);
            Debug.Log("File delete at path: " + _dataPath);
        }
        else 
        {
            Debug.LogWarning("File or path does not exist! " + _dataPath);
        }
#endif

    }

    public static bool CheckSaveDataExits(string folder, string file)
    {
#if UNITY_WEBGL

        return PlayerPrefs.HasKey(file);

#else

        _dataPath = GetFilePath(folder, file);

        if (!Directory.Exists(Path.GetDirectoryName(_dataPath)))
        {
            Debug.LogWarning("File or path does not exist! " + _dataPath);
            return false;
        }

        if (File.Exists(_dataPath))
        {
            Debug.Log("File exits at path: " + _dataPath);
            return true;
        }
        else
        {
            Debug.LogWarning("File or path does not exist! " + _dataPath);
            return false;
        }

#endif
    
    }

    private static string GetFilePath(string FolderName, string FileName = "")
    {
        string filePath;

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        // mac
        filePath = Path.Combine(Application.streamingAssetsPath, ("data/" + FolderName));

        if (FileName != "")
            filePath = Path.Combine(filePath, (FileName + ".txt"));
#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        // windows
        filePath = Path.Combine(Application.persistentDataPath, ("data/" + FolderName));

        if (FileName != "")
            filePath = Path.Combine(filePath, (FileName + ".txt"));
#elif UNITY_ANDROID
        // android
        filePath = Path.Combine(Application.persistentDataPath, ("data/" + FolderName));

        if(FileName != "")
            filePath = Path.Combine(filePath, (FileName + ".txt"));
#elif UNITY_IOS
        // ios
        filePath = Path.Combine(Application.persistentDataPath, ("data/" + FolderName));

        if(FileName != "")
            filePath = Path.Combine(filePath, (FileName + ".txt"));
#elif UNITY_WEBGL
        filePath = string.Empty;
#endif
        return filePath;
    }
}
