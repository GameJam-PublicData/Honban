using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{
    public Transform player;
    public float parallax = 0.1f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float x = player.position.x * parallax;
        transform.localPosition = startPos + new Vector3(x, 0, 0);
    }
}