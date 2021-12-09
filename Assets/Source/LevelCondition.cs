using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source
{
    public class LevelCondition : MonoBehaviour
    {
        public void RestartGame()
        {
            SceneManager.LoadScene("Scenes/SampleScene");
        }
    }
}
