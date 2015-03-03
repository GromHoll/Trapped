using System.Collections;
using UnityEditor;
using UnityEngine;

public class ResetBuildPreferences : MonoBehaviour {

    [MenuItem("Edit/Reset Player Preferences")]
    public static void DeletePlayerPreferences() {
        PlayerPrefs.DeleteAll();
    }

}
