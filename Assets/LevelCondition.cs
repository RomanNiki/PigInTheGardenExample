using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCondition : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("Scenes/SampleScene");
    }
}
