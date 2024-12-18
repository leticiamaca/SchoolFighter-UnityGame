using UnityEngine;

public class MusicController : MonoBehaviour
{
    //Classe respons�vel por controlar qualquer tipo de �udio
    //Cada audio soruce s� pode tocar/ executar um �udio por vez, entao se precisar de mais execucoes, mais audioSource, assim conseguimos ter v�rios audioSource tocando ao mesmo tempo
    //Por isso � importante criar um audioController para cada finalidade
    private AudioSource audioSource;


    //AudioClip � o arquivo de �udio que ser� executado
    public AudioClip levelMusic;
    void Start()
    {
        //Inicializando o audio source
        audioSource = GetComponent<AudioSource>();

        //Ao iniciar o MusicController, inicia a m�sica da fase
        PlayMusic(levelMusic);
    }
    public void PlayMusic(AudioClip music)
    {
        //Define o som que ir� ser reproduzido
        audioSource.clip = music;


        //Reproduz o som
        audioSource.Play();
    }

}
