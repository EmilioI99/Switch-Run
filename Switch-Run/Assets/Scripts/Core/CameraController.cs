using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;

    private Health health; 

    public float yOffset = 2;
    private float lookAhead;

    private void Awake()
    {
        health = player.GetComponent<Health>();
    }
    private void Update()
    {
        if (!health.dead)
        {
            //Follow Player
            transform.position = new Vector3(player.position.x + lookAhead, player.position.y + lookAhead + yOffset, transform.position.z);
            lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
        }
       
    }
}

