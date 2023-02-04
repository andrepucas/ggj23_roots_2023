using System;
using UnityEngine;

public class Narrator : MonoBehaviour
{
    public static event Action<AudioClip> OnPlayAudio;
    [SerializeField]
    public static DialogueContainer[] _allDialogues;
    static public void ZigZagSpawn()
    {
        Debug.Log("ZigZag Spawned");
        int temp = _allDialogues[0].DialogueArray.Length;
        OnPlayAudio.Invoke(_allDialogues[0].DialogueArray[UnityEngine.Random.Range(0, temp)].dialogueAudio);
    }
    static public void ZigZagDie()
    {
        Debug.Log("ZigZag Died");
        int temp = _allDialogues[0].DialogueArray.Length;
        OnPlayAudio.Invoke(_allDialogues[1].DialogueArray[UnityEngine.Random.Range(0, temp)].dialogueAudio);
    }
    static public void FlapperSpawn()
    {
        Debug.Log("Flapper Spawned");
        int temp = _allDialogues[0].DialogueArray.Length;
        OnPlayAudio.Invoke(_allDialogues[1].DialogueArray[UnityEngine.Random.Range(0, temp)].dialogueAudio);
    }
    static public void FlapperDie()
    {
        Debug.Log("Flapper Died");
        int temp = _allDialogues[0].DialogueArray.Length;
        OnPlayAudio.Invoke(_allDialogues[1].DialogueArray[UnityEngine.Random.Range(0, temp)].dialogueAudio);
    }
}