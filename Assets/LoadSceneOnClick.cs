using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    // Название сцены, на которую нужно перейти
    public string sceneName;

    // Этот метод будет вызываться кнопкой
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
