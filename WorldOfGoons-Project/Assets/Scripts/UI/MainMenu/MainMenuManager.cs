using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
	public List<OpenableCanvas> openedCanvas =  new();

	private bool _isBusy = false;
	
    public void OpenScene(string sceneName){
		SceneManager.LoadScene(sceneName);
	}

	public void QuitApplication() {
		Application.Quit();
	}

	public void Back() {
		if (_isBusy) return;
		if (openedCanvas.Count > 0) {
			OpenableCanvas canvas = openedCanvas[^1];
			canvas.CloseCanvas();
			canvas.onAnimationFinished.AddListener(StopBusy);
			_isBusy = true;
			openedCanvas.RemoveAt(openedCanvas.Count - 1);
		}
	}

	public void OpenCanvas(OpenableCanvas canvas) {
		if(_isBusy) return;
		canvas.OpenCanvas();
		canvas.onAnimationFinished.AddListener(StopBusy);
		openedCanvas.Add(canvas);
		_isBusy = true;
	}

	public void StopBusy(OpenableCanvas sender) {
		sender.onAnimationFinished.RemoveAllListeners();
		_isBusy = false;
	}
}
