using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MainSystem.Audio.Editor
{
[CustomEditor(typeof(AudioSO))]
public class AudioSOEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Refresh Audio Clips"))
        {
            AudioSO audioSO = (AudioSO)target;
            RefreshClips(audioSO);
            EditorUtility.SetDirty(audioSO);
        }
    }
// 
    public static void RefreshClips(AudioSO audioSO)
    {
        var seClips = new List<SoundData>();
        var bgmClips = new List<SoundData>();
        var jingleClips = new List<SoundData>();

        string[] allSoundData = AssetDatabase.FindAssets("t:SoundData");
        foreach (string guid in allSoundData)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            SoundData soundData = AssetDatabase.LoadAssetAtPath<SoundData>(path);
            
            switch (soundData.Category)
            {
                case AudioCategory.SE:
                    seClips.Add(soundData);
                    break;
                case AudioCategory.BGM:
                    bgmClips.Add(soundData);
                    break;
                case AudioCategory.Jingle:
                    jingleClips.Add(soundData);
                    break;
            }
        }
        
        // リフレクションでプライベートフィールドを更新
        typeof(AudioSO).GetField("_seSounds", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(audioSO, seClips);
        typeof(AudioSO).GetField("_bgmSounds", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(audioSO, bgmClips);
        typeof(AudioSO).GetField("_jingleSounds", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(audioSO, jingleClips);
    }
}
}