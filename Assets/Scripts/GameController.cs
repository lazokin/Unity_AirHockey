using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public SceneController sceneController;

    public SpriteController player1;
    public SpriteController player2;
    public SpriteController puck;
    public Text player1ScoreText;
    public Text player2ScoreText;

    private List<SpriteController> players;
    private Vector2 screenSize;
    private int player1Score;
    private int player2Score;


	void Start ()
    {
        players = new List<SpriteController>(2);
        players.Add(player1);
        players.Add(player2);
        player1.setPosition(player1.transform.position);
        player2.setPosition(player2.transform.position);
        player1Score = 0;
        player2Score = 0;
        screenSize = new Vector2(sceneController.SceneWidth, sceneController.SceneHeight);
	}
	
	void Update ()
    {
        player1.setPosition(player1.NextPosition);
        player2.setPosition(player2.NextPosition);
    }

    public void onTouchesBegan(List<Touch> touches)
    {
        foreach (var touch in touches)
        {
            var tap = Camera.main.ScreenToWorldPoint(touch.position);
            tap.z = 0.0f;
            foreach (var player in players)
            {
                Debug.Log(tap.ToString());
                Debug.Log(player.gameObject.GetComponent<CircleCollider2D>().bounds.Contains(tap).ToString());
                if (player.gameObject.GetComponent<CircleCollider2D>().bounds.Contains(tap))
                {
                    player.IsTouched = true;
                    player.Touch = touch;
                }
            }
        }
    }

    public void onTouchesMoved(List<Touch> touches)
    {
        foreach (var touch in touches)
        {
            var tapInScreen = new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane);
            var tapInWorld = Camera.main.ScreenToWorldPoint(tapInScreen);
            foreach (var player in players)
            {
                if (player.IsTouched && player.Touch.fingerId == touch.fingerId)
                {
                    Vector2 nextPosition = tapInWorld;

                    // keep player on table
                    if (nextPosition.x < -screenSize.x / 2 + player.radius())
                    {
                        nextPosition.x = -screenSize.x / 2 + player.radius();
                    }
                    if (nextPosition.x > screenSize.x / 2 - player.radius())
                    {
                        nextPosition.x = screenSize.x / 2 - player.radius();
                    }
                    if (nextPosition.y < -screenSize.y / 2 + player.radius())
                    {
                        nextPosition.y = -screenSize.y / 2 + player.radius();
                    }
                    if (nextPosition.y > screenSize.y / 2 - player.radius())
                    {
                        nextPosition.y = screenSize.y / 2 - player.radius();
                    }

                    // keep player in half court
                    if (player.transform.position.y > 0.0)
                    {
                        if (nextPosition.y < player.radius())
                        {
                            nextPosition.y = player.radius();
                        }
                    }
                    else
                    {
                        if (nextPosition.y > -player.radius())
                        {
                            nextPosition.y = -player.radius();
                        }
                    }

                    // set player next position and velocity
                    player.NextPosition = nextPosition;
                    player.Vector = new Vector2(tapInWorld.x - player.transform.position.x, tapInWorld.y - player.transform.position.y);
                    
                }
            }
        }
    }

    public void onTouchesEnded(List<Touch> touches)
    {
        foreach (var touch in touches)
        {
            foreach (var player in players)
            {
                if (player.IsTouched && player.Touch.fingerId == touch.fingerId)
                {
                    player.IsTouched = false;
                    player.Vector = Vector2.zero;
                }
            }
        }
    }

    private void playerScore(int player)
    {

    }
}
