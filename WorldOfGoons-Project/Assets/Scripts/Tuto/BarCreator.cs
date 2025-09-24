using UnityEngine;
using UnityEngine.EventSystems;

public class BarCreator : MonoBehaviour, IPointerDownHandler {
    private bool _barCreationStarted = false;

    public Bar currentBar;
    public GameObject barToInstantiate;
    public Transform barParent;

    public Point currentStartPoint;
    public Point currentEndPoint;
    public GameObject pointToInstantiate;
    public Transform pointParent;

    void Update() {
        if (_barCreationStarted == true) {
            Vector2 endPosition= (Vector2)Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Vector2 dir = endPosition - currentBar.startPosition;
            Vector2 clampedPosition = currentBar.startPosition + Vector2.ClampMagnitude(dir, currentBar.maxLenght);

            currentEndPoint.transform.position = (Vector2)Vector2Int.FloorToInt(clampedPosition);
            currentEndPoint.pointId = currentEndPoint.transform.position;
            currentBar.UpdateCreatingBar(currentEndPoint.transform.position);
        }
    }
    
    public void OnPointerDown(PointerEventData eventData) {
        if (_barCreationStarted == false) {
            _barCreationStarted = true;
            StartBarCreation(Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(eventData.position)));
        } else {
            if (eventData.button == PointerEventData.InputButton.Left) {
                FinishBarCreation();
            } else if (eventData.button == PointerEventData.InputButton.Right) {
                _barCreationStarted = false;
                DeleteCurrentBar();
            }
        }
    }

    void StartBarCreation(Vector2 startPosition) {
        currentBar = Instantiate(barToInstantiate, barParent).GetComponent<Bar>();
        currentBar.startPosition = startPosition;

        if (TutoGameManager.AllPoints.ContainsKey(startPosition)) {
            currentStartPoint = TutoGameManager.AllPoints[startPosition];
        } else {
            currentStartPoint = Instantiate(pointToInstantiate, startPosition, Quaternion.identity, pointParent).GetComponent<Point>();
            TutoGameManager.AllPoints.Add(startPosition, currentStartPoint);
        }
        
        currentEndPoint = Instantiate(pointToInstantiate, startPosition, Quaternion.identity, pointParent).GetComponent<Point>();
    }

    void FinishBarCreation() {
        if (TutoGameManager.AllPoints.ContainsKey(currentEndPoint.transform.position)) {
            Destroy(currentEndPoint.gameObject);
            currentEndPoint = TutoGameManager.AllPoints[currentEndPoint.transform.position];
        } else {
            TutoGameManager.AllPoints.Add(currentEndPoint.transform.position, currentEndPoint);
        }
        
        currentStartPoint.connectedBars.Add(currentBar);
        currentEndPoint.connectedBars.Add(currentBar);
        StartBarCreation(currentEndPoint.transform.position);
    }

    void DeleteCurrentBar() {
        Destroy(currentBar.gameObject);
        if (currentStartPoint.connectedBars.Count == 0 && currentStartPoint.runtime == true) {
            Destroy(currentStartPoint.gameObject);
        }
        if (currentEndPoint.connectedBars.Count == 0 && currentEndPoint.runtime == true) {
            Debug.Log("currentEndPoint.connectedBars.Count");
            Destroy(currentEndPoint.gameObject);
        }
    }
}
