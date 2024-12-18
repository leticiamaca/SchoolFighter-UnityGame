using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public Animator transition;
    public float transitionTime = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        //se pressionar qualquer tecla 
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
           //mudar de cena
            StartCoroutine(CarregarFase("Fase1"));

        }

        //Corrotina  - Coroutine 
        IEnumerator CarregarFase(string nomeFase)
        {
            //Iniciar a animacao
            transition.SetTrigger("Start");

            //Esperar o tempo de animacao
            yield return new WaitForSeconds(1f);

            //Carregar a cena 
            SceneManager.LoadScene(nomeFase);
        }
    }
}
