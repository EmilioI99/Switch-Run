using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    public float yOffset = 2;
    private float lookAhead;


    private void Update()
    {



        //Follow Player
        transform.position = new Vector3(player.position.x + lookAhead, player.position.y + lookAhead + yOffset, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);



    }
}

