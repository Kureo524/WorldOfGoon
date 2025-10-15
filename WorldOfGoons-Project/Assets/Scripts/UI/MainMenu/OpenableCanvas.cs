using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class OpenableCanvas :  MonoBehaviour {
    public Animator animator;
    public string openAnimationName;
    public string closeAnimationName;
    
    public UnityEvent<OpenableCanvas> onAnimationFinished;
    public void OpenCanvas() { 
        animator.Play(openAnimationName);
    }
    public void CloseCanvas() {
        animator.Play(closeAnimationName);
    }
    
    public void OnAnimationFinished() {
        onAnimationFinished.Invoke(this);
    }

    public void OpenScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
