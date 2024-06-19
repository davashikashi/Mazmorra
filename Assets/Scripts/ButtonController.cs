using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject Inicio;
    public GameObject Running;
    public GameObject Pause;
    // Start is called before the first frame update

    public void StartGame()
    {
        Running.SetActive(true);
        Inicio.SetActive(false);  
    }
    public void RestartStartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void CloseGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                // Si estamos en una compilación, cerramos la aplicación
                Application.Quit();
        #endif
    }

    public void PauseGame(){
        Time.timeScale = 0f; // Pausar el juego
        Pause.SetActive(true);   
        Running.SetActive(false);
    }

    public void ResumeGame(){
        Time.timeScale = 1f;
        Running.SetActive(true);
        Pause.SetActive(false);
    }

    public void MenuGame(){
        Inicio.SetActive(true);
        Running.SetActive(false);
        Pause.SetActive(false);
    }
}
