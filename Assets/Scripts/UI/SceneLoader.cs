﻿namespace GGJ2021
{
  using UnityEngine;
  using UnityEngine.SceneManagement;

  public class SceneLoader : MonoBehaviour {

    public void ChangeScene(string sceneName) {
      SceneManager.LoadScene(sceneName);
    }

    public void QuitGame() {
      Application.Quit();
    }

    public void OpenStarcubeBlog() {
      Application.OpenURL("https://starcubelabs.github.io/");
    }
  }
}
