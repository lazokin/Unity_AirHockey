using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private bool touching;
    private int fingerId;

    void Start()
    {

    }

    void Update()
    {

        if (!touching && Input.touchCount > 0)
        {
            Touch[] touches = Input.touches;
            foreach (Touch touch in touches)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
                if (hit)
                {
                    if (hit.collider.gameObject == this.gameObject)
                    {
                        touching = true;
                        fingerId = touch.fingerId;
                    }
                }
            }
        }

        if (touching)
        {
            Touch touch = Input.GetTouch(fingerId);
            if (touch.phase == TouchPhase.Ended)
            {
                touching = false;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector3 screenPoint = new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane);
                transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
            }
        }

    }

}
