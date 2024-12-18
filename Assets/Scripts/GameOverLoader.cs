using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverLoader : MonoBehaviour
{

    private AudioSource enterAudio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Inicializando o som
        enterAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            enterAudio.Play();
            //mudar de cena
            StartCoroutine(CarregarFase("TelaInicial"));


        }

        //Corrotina  - Coroutine 
        IEnumerator CarregarFase(string nomeFase)
        {


            //Esperar o tempo de animacao
            yield return new WaitForSeconds(0.5f);

            //Carregar a cena 
            SceneManager.LoadScene(nomeFase);
        }
    }
}
