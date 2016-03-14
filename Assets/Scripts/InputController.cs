using UnityEngine;
using System.Collections.Generic;

public class InputController : MonoBehaviour {

    public GameController gameController;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            List<Touch> touchesBegan = new List<Touch>();
            List<Touch> touchesMoved = new List<Touch>();
            List<Touch> touchesEnded = new List<Touch>();

            Touch[] touches = Input.touches;
            foreach (Touch touch in touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    touchesBegan.Add(touch);
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    touchesMoved.Add(touch);
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    touchesEnded.Add(touch);
                }
            }

            if (touchesBegan.Count > 0)
            {
                gameController.onTouchesBegan(touchesBegan);
            }

            if (touchesMoved.Count > 0)
            {
                gameController.onTouchesMoved(touchesMoved);
            }

            if (touchesEnded.Count > 0)
            {
                gameController.onTouchesEnded(touchesEnded);
            }

        }
    }

}
