using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void HandleStartForGameMenu()
    {
        SceneManager.LoadScene("Select");
    }
}
