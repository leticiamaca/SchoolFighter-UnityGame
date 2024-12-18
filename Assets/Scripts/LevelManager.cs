using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        public static CinemachineConfiner2D currentConfiner;
        private CinemachineBrain brain;
        private CinemachineCamera cam;

        static BoxCollider2D currentSection;
        void Start()
        {
            //Controlador de cameras que esta ativo, as vezes podem existir gerenciadores de cameras diferentes
            brain = CinemachineBrain.GetActiveBrain(0);

            //Achando o confiner atual por meio do objeto cinemachine onde está o objeto section 1 dentro do cinemachineconfiner2D 
            currentConfiner = GameObject.Find("CM").GetComponent<CinemachineConfiner2D>();
        }

        // Método para mudar o confiner da camera
        public static void ChangeSection(string sectionName)
        {
            //Procura o objeto que contem o nome sectionName e pega o colisor dele para ser o novo confiner 2d
            currentSection = GameObject.Find(sectionName).GetComponent<BoxCollider2D>();

            //Se o objeto for encontrado e tiver o colisor
            if (currentSection != null)
            {
                currentConfiner.InvalidateBoundingShapeCache();
                currentConfiner.BoundingShape2D = currentSection;

                //Reposicionar o Right Limiter, alinhado ao max X do confiner (Direita do confiner)
                GameObject rightLimiter = GameObject.Find("Right");
                // Vector3(x, y, z / porem como o jogo é 2D só se usa o x e y )
                //currentConfiner.BoundingShape2D.bounds.max.x significa que voce quer pegar apenas o lado direito 
                //rightLimiter.transform.position.y manter o y intacto, nao é preciso mexer nele visto que o confiner nao vai para cima ou para baixo
                rightLimiter.transform.position = new Vector3(currentConfiner.BoundingShape2D.bounds.max.x, rightLimiter.transform.position.y);

            };
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}