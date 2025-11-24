using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class DialogueControl : MonoBehaviour
{
    [Header("Components")]
    public GameObject dialogueObj;
    public Text actorNameText;
    public Text speechtText;

    [Header("Variables")]
    public float typingSpeed;
    private string[] sentences;
    private int index;
    private Coroutine typingCoroutine;


    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            NextSentence();
        }

    }

    public void Speech(string[] txt, string actorName)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        dialogueObj.SetActive(true);
        speechtText.text = "";
        actorNameText.text = actorName;
        sentences = txt;
        index = 0;
        typingCoroutine = StartCoroutine(TypeSentence(sentences[index]));
    }

    IEnumerator TypeSentence(string sentence)
    {
        speechtText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            speechtText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null;
    }


    public void NextSentence()
    {
        if (speechtText.text == sentences[index])
        {
            if (index < sentences.Length - 1)
            {
                index++;
                speechtText.text = "";
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }
               
                typingCoroutine = StartCoroutine(TypeSentence(sentences[index]));
            }
            else
            {
                EndDialogue();
            }
        }
    }

    public void PrevSentence(InputAction.CallbackContext context)
    {

    }

    public void HidePanel()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        speechtText.text = "";
        actorNameText.text = "";
        index = 0;
        dialogueObj.SetActive(false);
    }

    public void EndDialogue()
    {
        speechtText.text = "";
        index = 0;
        dialogueObj.SetActive(false);
    }
}