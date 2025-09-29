using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class BarCreator : MonoBehaviour, IPointerDownHandler {
    private bool _barCreationStarted = false;

    [FormerlySerializedAs("currentBar")] public TutoBar currentTutoBar;
    public GameObject barToInstantiate;
    public Transform barParent;

    [FormerlySerializedAs("currentStartPoint")] public TutoPoint currentStartTutoPoint;
    [FormerlySerializedAs("currentEndPoint")] public TutoPoint currentEndTutoPoint;
    public GameObject pointToInstantiate;
    public Transform pointParent;

    void Update() {
        if (_barCreationStarted == true) {
            Vector2 endPosition= (Vector2)Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Vector2 dir = endPosition - currentTutoBar.startPosition;
            Vector2 clampedPosition = currentTutoBar.startPosition + Vector2.ClampMagnitude(dir, currentTutoBar.maxLenght);

            currentEndTutoPoint.transform.position = (Vector2)Vector2Int.FloorToInt(clampedPosition);
            currentEndTutoPoint.pointId = currentEndTutoPoint.transform.position;
            currentTutoBar.UpdateCreatingBar(currentEndTutoPoint.transform.position);
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
        currentTutoBar = Instantiate(barToInstantiate, barParent).GetComponent<TutoBar>();
        currentTutoBar.startPosition = startPosition;

        if (TutoGameManager.AllPoints.ContainsKey(startPosition)) {
            currentStartTutoPoint = TutoGameManager.AllPoints[startPosition];
        } else {
            currentStartTutoPoint = Instantiate(pointToInstantiate, startPosition, Quaternion.identity, pointParent).GetComponent<TutoPoint>();
            TutoGameManager.AllPoints.Add(startPosition, currentStartTutoPoint);
        }
        
        currentEndTutoPoint = Instantiate(pointToInstantiate, startPosition, Quaternion.identity, pointParent).GetComponent<TutoPoint>();
    }

    void FinishBarCreation() {
        if (TutoGameManager.AllPoints.ContainsKey(currentEndTutoPoint.transform.position)) {
            Destroy(currentEndTutoPoint.gameObject);
            currentEndTutoPoint = TutoGameManager.AllPoints[currentEndTutoPoint.transform.position];
        } else {
            TutoGameManager.AllPoints.Add(currentEndTutoPoint.transform.position, currentEndTutoPoint);
        }
        
        currentStartTutoPoint.connectedBars.Add(currentTutoBar);
        currentEndTutoPoint.connectedBars.Add(currentTutoBar);
        StartBarCreation(currentEndTutoPoint.transform.position);
    }

    void DeleteCurrentBar() {
        Destroy(currentTutoBar.gameObject);
        if (currentStartTutoPoint.connectedBars.Count == 0 && currentStartTutoPoint.runtime == true) {
            Destroy(currentStartTutoPoint.gameObject);
        }
        if (currentEndTutoPoint.connectedBars.Count == 0 && currentEndTutoPoint.runtime == true) {
            Debug.Log("currentEndPoint.connectedBars.Count");
            Destroy(currentEndTutoPoint.gameObject);
        }
    }
}
