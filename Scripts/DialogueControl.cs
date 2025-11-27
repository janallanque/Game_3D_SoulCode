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
    public float typingSpeed = 0.03f;
    private string[] sentences;
    private int index;
    private Coroutine typingCoroutine;

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            // Se estiver digitando, completar instantaneamente; senão ir para próxima
            if (typingCoroutine != null)
            {
                // completa a frase atual
                CompleteCurrentSentence();
            }
            else
            {
                NextSentence();
            }
        }
    }

    public void Speech(string[] txt, string actorName)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        dialogueObj.SetActive(true);
        speechtText.text = "";
        actorNameText.text = actorName;
        sentences = txt;
        index = 0;
        typingCoroutine = StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        // pega a frase atual do array (aqui 'sentence' é declarada e conhecida)
        string sentence = sentences[index];
        speechtText.text = "";

        foreach (char letter in sentence)
        {
            speechtText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // terminou de digitar esta frase
        typingCoroutine = null;
    }

    // completa imediatamente a frase que está sendo digitada
    private void CompleteCurrentSentence()
    {
        if (sentences == null || sentences.Length == 0) return;
        speechtText.text = sentences[index];
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }

    public void NextSentence()
    {
        // evita crash se sentences for nulo
        if (sentences == null || sentences.Length == 0) return;

        // só avança se o texto visível é igual à frase completa atual
        if (speechtText.text == sentences[index])
        {
            if (index < sentences.Length - 1)
            {
                index++;
                speechtText.text = "";
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                    typingCoroutine = null;
                }
                typingCoroutine = StartCoroutine(TypeSentence());
            }
            else
            {
                EndDialogue();
            }
        }
        else
        {
            // se o texto não está completo, completamos imediatamente
            CompleteCurrentSentence();
        }
    }

    public void HidePanel()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
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
