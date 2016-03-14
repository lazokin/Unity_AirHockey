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

    private readonly float GOAL_WIDTH = 400f;

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
        // puck position and velocity
        var puckNextPosition = puck.NextPosition;
        var puckVector = puck.Vector;
        puckVector *= 0.98f;
        puckNextPosition += puckVector;

        // collision
        float squaredRadii = Mathf.Pow(player1.radius() + puck.radius(), 2);
        foreach (var player in players)
        {
            var playerNextPosition = player.NextPosition;
            var playerVector = player.Vector;
            float diffx = puckNextPosition.x - player.transform.position.x;
            float diffy = puckNextPosition.y - player.transform.position.y;
            float distance1 = Mathf.Pow(diffx, 2) + Mathf.Pow(diffy, 2);
            float distance2 = Mathf.Pow(puck.transform.position.x - playerNextPosition.x, 2) +
                Mathf.Pow(puck.transform.position.y - playerNextPosition.y, 2);
            if (distance1 <= squaredRadii || distance2 <= squaredRadii)
            {
                float magPuck = Mathf.Pow(puckVector.x, 2) + Mathf.Pow(puckVector.y, 2);
                float magPlayer = Mathf.Pow(playerVector.x, 2) + Mathf.Pow(playerVector.y, 2);
                float force = Mathf.Sqrt(magPuck + magPlayer);
                float angle = Mathf.Atan2(diffy, diffx);
                puckVector.x = force * Mathf.Cos(angle);
                puckVector.y = force * Mathf.Sin(angle);
                puckNextPosition.x = playerNextPosition.x + (player.radius() + puck.radius() + force) * Mathf.Cos(angle);
                puckNextPosition.y = playerNextPosition.y + (player.radius() + puck.radius() + force) * Mathf.Sin(angle);
                // play hit sound
            }
        }

        // puck and court left side
        if (puckNextPosition.x < -screenSize.x / 2 + puck.radius())
        {
            puckNextPosition.x = -screenSize.x / 2 + puck.radius();
            puckVector.x *= -0.8f;
            // play hit sound
        }

        // puck and court right side
        if (puckNextPosition.x > screenSize.x / 2 - puck.radius())
        {
            puckNextPosition.x = screenSize.x / 2 - puck.radius();
            puckVector.x *= -0.8f;
            // play hit sound
        }

        // puck and court top side
        if (puckNextPosition.y > screenSize.y / 2 - puck.radius())
        {
            if (puck.transform.position.x < -GOAL_WIDTH / 2 || puck.transform.position.x > GOAL_WIDTH / 2)
            {
                puckNextPosition.y = screenSize.y / 2 - puck.radius();
                puckVector.y *= -0.8f;
                // play hit sound
            }
        }

        // puck and court bottom side
        if (puckNextPosition.y < -screenSize.y / 2 + puck.radius())
        {
            if (puck.transform.position.x < -GOAL_WIDTH / 2 || puck.transform.position.x > GOAL_WIDTH / 2)
            {
                puckNextPosition.y = -screenSize.y / 2 + puck.radius();
                puckVector.y *= -0.8f;
                // play hit sound
            }
        }

        // update puck information
        puck.Vector = puckVector;
        puck.NextPosition = puckNextPosition;

        // check for goals
        if (puckNextPosition.y < -screenSize.y / 2 - puck.radius() * 2)
        {
            playerScore(2);
        }
        if (puckNextPosition.y > screenSize.y / 2 + puck.radius() * 2)
        {
            playerScore(1);
        }

        // complete update 
        player1.setPosition(player1.NextPosition);
        player2.setPosition(player2.NextPosition);
        puck.setPosition(puck.NextPosition);
    }

    public void onTouchesBegan(List<Touch> touches)
    {
        foreach (var touch in touches)
        {
            var tap = Camera.main.ScreenToWorldPoint(touch.position);
            tap.z = 0.0f;
            foreach (var player in players)
            {
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
        // play score sound
        puck.Vector = Vector2.zero;

        // update score
        if (player == 1)
        {
            player1Score++;
            player1ScoreText.text = player1Score.ToString();
            puck.setPosition(new Vector2(0, 2 * puck.radius()));
        }
        if (player == 2)
        {
            player2Score++;
            player2ScoreText.text = player2Score.ToString();
            puck.setPosition(new Vector2(0, -2 * puck.radius()));
        }

        // move players to starting positions
        player1.setPosition(new Vector2(0, -800));
        player2.setPosition(new Vector2(0, +800));
        player1.IsTouched = false;
        player2.IsTouched = false;
    }
}
