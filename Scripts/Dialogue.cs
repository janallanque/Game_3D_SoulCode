using UnityEngine;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    [Header("Variables")]
    public string[] speechText;
    public string actorName;
    private DialogueControl dc;
    private bool onRadious;
    private bool isDialogueActive = false;
    public LayerMask playerLayer;
    public float radious;

    void Start()
    {
        
    }

    void Update()
    {
        if(Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame
            && onRadious && !isDialogueActive)
        {
            StartDialogue();
          
        }
    }

    private void FixedUpdate()
    {
     Interact();
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        dc.Speech(speechText, actorName);
        Debug.Log("Diálogo iniciado com " + actorName);
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dc.EndDialogue();
        Debug.Log("Diálogo encerrado!");

    }
    private void OnDrawGizmosSelected() //Opcional
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radious);
    }

    private void Interact()
    {
        Vector3 point1 = transform.position + Vector3.up * radious;
        Vector3 point2 = transform.position - Vector3.up * radious;

        Collider[] hits = Physics.OverlapCapsule(
            point1, point2, radious, playerLayer);

        if (hits.Length > 0)
        {
            if (!onRadious)
            {
                Debug.Log("Jogador saiu do raio de interação com o NPC");

            }
            onRadious = true;
        }
        else
        {
            if (onRadious)
            {
                Debug.Log("Jogador saiu do raio de interação com o NPC");
            }
            onRadious = false;
            if (isDialogueActive)
            {
                EndDialogue();
            }
        }

    }
}
