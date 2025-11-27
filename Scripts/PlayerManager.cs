using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public float vidaJogador = 100f;

    public float vidaMaxima = 100f;
    public Image barravida;

    private bool podeSalvar = false;

    private Vector3 ultimaPosicaoSalva;
    private float ultimaVidaSalva;
    private bool temDadosSalvos = false;

    void Start()
    {
        CarregarDadosJogador();
        AtualizarBarraVida();

    }

    void Update()
    {
        AtualizarBarraVida();
        TakeDamage();

        if (podeSalvar && Keyboard.current.bKey.wasPressedThisFrame)
        {
            SalvarDadosJogador();
        }

        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            LimparDadosJogador();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PontoDeSalvamento"))
        {
            Debug.Log("Colidiu com o ponto de salvamento! Pressione B para salvar!");
            podeSalvar = true;
           
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PontoDeSalvamento"))
        {
            Debug.Log("Saiu do ponto de salvamento!");
            podeSalvar = false;
        }
    }

    void SalvarDadosJogador()
    {
        ultimaPosicaoSalva = transform.position;
        ultimaVidaSalva = vidaJogador;

        PlayerPrefs.SetFloat("PosX", ultimaPosicaoSalva.x);
        PlayerPrefs.SetFloat("PosY", ultimaPosicaoSalva.y);
        PlayerPrefs.SetFloat("PosZ", ultimaPosicaoSalva.z);
        PlayerPrefs.SetFloat("Vida", ultimaVidaSalva);
        PlayerPrefs.SetInt("temDadosSalvos", 1);

        Debug.Log("Os dados foram salvos com sucesso!");
        podeSalvar = false;
    }

    void LimparDadosJogador()
    {
        PlayerPrefs.DeleteKey("PosX");
        PlayerPrefs.DeleteKey("PosY");
        PlayerPrefs.DeleteKey("PosZ");
        PlayerPrefs.DeleteKey("Vida");
        PlayerPrefs.DeleteKey("temDadosSalvos");

        ultimaPosicaoSalva = Vector3.zero;
        ultimaVidaSalva = 0f;
        temDadosSalvos = false;
        vidaJogador = vidaMaxima;
        AtualizarBarraVida();

        Debug.Log("Os dados salvos foram limpos!");
    }

    void CarregarDadosJogador()
    {
        if(PlayerPrefs.GetInt("temDadosSalvos", 0) == 1)
        {
            float posX = PlayerPrefs.GetFloat("PosX");
            float posY = PlayerPrefs.GetFloat("PosY");
            float posZ = PlayerPrefs.GetFloat("PosZ");
            ultimaPosicaoSalva = new Vector3(posX, posY, posZ);
            ultimaVidaSalva = PlayerPrefs.GetFloat("Vida");

            transform.position = ultimaPosicaoSalva;
            vidaJogador = ultimaVidaSalva;
            temDadosSalvos = true;

            Debug.Log("Os dados foram carregados com sucesso!");
        }
        else
        {
            Debug.Log("Nenhum dado salvo encontrado!");
        }
    }

    void AtualizarBarraVida()
    {
        if (barravida == null) return;

        float vidaNormalizada = Mathf.Clamp01(vidaJogador / vidaMaxima);
        barravida.fillAmount = vidaNormalizada;
    }

    public void TakeDamage()
    { 
    if(Keyboard.current.tKey.wasPressedThisFrame)
        {            
            vidaJogador -= 10;
            AtualizarBarraVida();
            
            if (vidaJogador <= 0)
            {
                SceneManager.LoadScene(2);
            }
        }
    }
}
