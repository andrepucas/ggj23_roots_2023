using System;
using UnityEngine;

public class Narrator : MonoBehaviour
{
    public static Narrator Instance { get { return Narrator.instance; } }

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    internal static readonly Narrator instance = new Narrator();
    public AudioSource _aS;
    public static event Action<AudioClip> OnPlayAudio;
    [SerializeField]
    public DialogueContainer[] _allDialogues;
    static public void ZigZagSpawn()
    {
        Debug.Log("ZigZag Spawned");
        int temp = Instance._allDialogues[0].DialogueArray.Length;
        OnPlayAudio.Invoke(Instance._allDialogues[0].DialogueArray[UnityEngine.Random.Range(0, temp)].dialogueAudio);
    }
    static public void ZigZagDie()
    {
        Debug.Log("ZigZag Died");
        int temp = Instance._allDialogues[0].DialogueArray.Length;
        OnPlayAudio.Invoke(Instance._allDialogues[1].DialogueArray[UnityEngine.Random.Range(0, temp)].dialogueAudio);
    }
    static public void FlapperSpawn()
    {
        Debug.Log("Flapper Spawned");
        int temp = Instance._allDialogues[0].DialogueArray.Length;
        OnPlayAudio.Invoke(Instance._allDialogues[1].DialogueArray[UnityEngine.Random.Range(0, temp)].dialogueAudio);
    }
    static public void FlapperDie()
    {
        Debug.Log("Flapper Died");
        int temp = Instance._allDialogues[0].DialogueArray.Length;
        OnPlayAudio.Invoke(Instance._allDialogues[1].DialogueArray[UnityEngine.Random.Range(0, temp)].dialogueAudio);
    }

}
