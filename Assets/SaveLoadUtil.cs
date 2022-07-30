using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadUtil
{
    static public void Save(CSSerializedObject save, string path, bool saveJson = true)
    {
        FileStream file = null;
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            file = File.Create(path);
            bf.Serialize(file, save);


            if (saveJson)
            {
                string json = JsonUtility.ToJson(save, true);
                File.WriteAllText(path + ".json", json);
            }

            Debug.Log("Game Saved");
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);

        }
        finally
        {

            file.Close();
        }
    }

    static public CSSerializedObject Load(string path)
    {
        if (File.Exists(path))
        {
            FileStream file = null;
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                file = File.Open(path, FileMode.Open);
                CSSerializedObject save = (CSSerializedObject)bf.Deserialize(file);
                Debug.Log("Game Loaded");
                return save;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;

            }
            finally
            {

                file.Close();
            }
        }
        else
        {
            Debug.Log("No game saved!");
            return null;
        }
    }

    public static void clearSavedData(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static bool hasSavedData(string path)
    {
        return File.Exists(path);
    }
}




[System.Serializable]
public class CSSerializedObject
{
    public float serializationTime;
    public int version = 0;
    public bool isValid = false;
}

[System.Serializable]
public struct SerializedDictionary<T, U>
{
    public List<T> Keys;
    public List<U> Values;
    public Dictionary<T, U> getDictionary()
    {
        Dictionary<T, U> res = new Dictionary<T, U>();
        for (int i = 0; i < Keys.Count; i++)
        {
            res[Keys[i]] = Values[i];
        }
        return res;
    }
    public SerializedDictionary(Dictionary<T, U> dict)
    {
        Keys = dict.Keys.ToList();
        Values = dict.Values.ToList();
    }

    public static implicit operator Dictionary<T, U>(SerializedDictionary<T, U> test)
    {
        return test.getDictionary();
    }
    public static implicit operator SerializedDictionary<T, U>(Dictionary<T, U> test)
    {
        return new SerializedDictionary<T, U>(test);
    }
}


[System.Serializable]
public struct SerializedVector
{
    public float x, y, z;
    public Vector3 GetPos()
    {
        return new Vector3(x, y, z);
    }
    public SerializedVector(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }
    public static implicit operator Vector3(SerializedVector test)
    {
        return test.GetPos();
    }
    public static implicit operator SerializedVector(Vector3 test)
    {
        return new SerializedVector(test);
    }
}