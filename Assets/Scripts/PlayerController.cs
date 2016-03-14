using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public SceneController sceneController;

    private bool touching;
    private int fingerId;
    private float radius;
    private bool isTopPlayer;
    private bool isBtmPlayer;

    private float lft;
    private float rgt;
    private float top;
    private float btm;

    void Start()
    {
        radius = gameObject.GetComponent<CircleCollider2D>().radius;
        lft = sceneController.SceneLeft;
        rgt = sceneController.SceneRight;
        top = sceneController.SceneTop;
        btm = sceneController.SceneBottom;
        isTopPlayer = transform.position.y > 0 ? true : false;
        isBtmPlayer = transform.position.y < 0 ? true : false;
    }

    void Update()
    {
        // player movement
        if (Input.touchCount > 0)
        {
            Touch[] touches = Input.touches;
            foreach (Touch touch in touches)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
                if (hit && hit.collider.gameObject == this.gameObject)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        Vector3 screenPoint = new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane);
                        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
                    }
                }
            }
        }

        // keep player in court
        if (transform.position.x < lft + radius)
        {
            transform.position = new Vector3(lft + radius, transform.position.y, transform.position.z);
        }

        if (transform.position.x > rgt - radius)
        {
            transform.position = new Vector3(rgt - radius, transform.position.y, transform.position.z);
        }

        if (transform.position.y < btm + radius)
        {
            transform.position = new Vector3(transform.position.x, btm + radius, transform.position.z);
        }

        if (transform.position.y > top - radius)
        {
            transform.position = new Vector3(transform.position.x, top - radius, transform.position.z);
        }

        if (isTopPlayer)
        {
            if (transform.position.y < radius)
            {
                transform.position = new Vector3(transform.position.x, radius, transform.position.z);
            }
        }

        if (isBtmPlayer)
        {
            if (transform.position.y > -radius)
            {
                transform.position = new Vector3(transform.position.x, -radius, transform.position.z);
            }
        }

    }

}
