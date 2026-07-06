using System.Threading;
using UnityEngine;

public class restart : MonoBehaviour
{
    public void LoadCurrentScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gamescene");
        Time.timeScale = 1;
    }
}
