using UnityEngine;

public class MusicController : MonoBehaviour
{
    //Classe responsável por controlar qualquer tipo de áudio
    //Cada audio soruce só pode tocar/ executar um áudio por vez, entao se precisar de mais execucoes, mais audioSource, assim conseguimos ter vários audioSource tocando ao mesmo tempo
    //Por isso é importante criar um audioController para cada finalidade
    private AudioSource audioSource;


    //AudioClip é o arquivo de áudio que será executado
    public AudioClip levelMusic;
    void Start()
    {
        //Inicializando o audio source
        audioSource = GetComponent<AudioSource>();

        //Ao iniciar o MusicController, inicia a música da fase
        PlayMusic(levelMusic);
    }
    public void PlayMusic(AudioClip music)
    {
        //Define o som que irá ser reproduzido
        audioSource.clip = music;


        //Reproduz o som
        audioSource.Play();
    }

}
