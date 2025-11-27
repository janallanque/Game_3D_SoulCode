using UnityEngine;
using UnityEngine.SceneManagement;

public class HitBoxController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    { 
    
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player atingido! Carregando Game Over...");
            SceneManager.LoadScene(2);
        }

    }   
}
