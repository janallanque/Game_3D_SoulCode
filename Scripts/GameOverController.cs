using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public float delay = 5f;

    void Start()
    {
        StartCoroutine(LoadAfterSeconds());
    }

 
    IEnumerator LoadAfterSeconds()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(2);
    }

}
