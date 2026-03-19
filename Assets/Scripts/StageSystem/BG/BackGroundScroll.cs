using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{
    public Transform player;
    public Vector2 parallax;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float x = player.position.x * parallax.x;
        float y = player.position.y * parallax.y;
        
        transform.localPosition = startPos + new Vector3(x, y, 0);
    }
}