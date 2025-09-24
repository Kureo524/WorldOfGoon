using UnityEngine;

public class Bar : MonoBehaviour
{
    public float maxLenght;
    public Vector2 startPosition;
    public SpriteRenderer barSpriteRenderer;

    public void UpdateCreatingBar(Vector2 toPosition) {
        transform.position = (toPosition + startPosition) / 2;
        Vector2 dir = toPosition - startPosition;
        float angle = Vector2.SignedAngle(Vector2.right, dir);
        transform.rotation = Quaternion.Euler(0, 0, angle);
        
        float length = dir.magnitude;
        barSpriteRenderer.size = new Vector2(length, barSpriteRenderer.size.y);
    }
}
