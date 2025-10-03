using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    private void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, player.position.y, -10);
        }
    }
}
