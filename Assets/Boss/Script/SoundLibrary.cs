using UnityEngine;

[System.Serializable]
public struct SoundEffect
{
    public string nameID;
    public AudioClip clips;
}

public class SoundLibrary : MonoBehaviour
{
    public SoundEffect[] soundEffects;

    public AudioClip GetClipFromName(string name)
    {
        foreach (var soundEffect in soundEffects)
        {
            if (soundEffect.nameID == name)
            {
                return soundEffect.clips;
            }
        }
        return null;
    }
}