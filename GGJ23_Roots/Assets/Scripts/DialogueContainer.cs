using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DialogueContainer", order = 2)]

public class DialogueContainer : ScriptableObject
{
    public DialoguePieces[] DialogueArray;
}
[System.Serializable]
public struct DialoguePieces
{
    public AudioClip dialogueAudio;
    public string dialogueText;
}
