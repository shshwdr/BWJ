using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SerializedHint : CSSerializedObject {

    public List<int> actionList;
    public int level;

}


public class HintSaveLoad
{
    static public void Save(List<int> actionList, int level)
    {
        SerializedHint hint = new SerializedHint();
        hint.actionList = actionList;
        hint.level = level;
        SaveLoadUtil.Save(hint, Application.dataPath + "/Resources/hint" + $"/hint_{level}.save");
    }

    static public SerializedHint Load(int level)
    {

        var save = SaveLoadUtil.Load(Application.dataPath + "/Resources/hint" + $"/hint_{level}.save");
        if (save != null)
        {
            if (((SerializedHint)save).actionList.Count == 0)
            {
                Debug.Log("action list is 0");
            }
            return (SerializedHint)save;
        }
        return null;
    }
}