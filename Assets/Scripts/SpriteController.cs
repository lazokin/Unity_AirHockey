using UnityEngine;
using System.Collections;

public class SpriteController : MonoBehaviour {

    public Vector2 NextPosition { get; set; }
    public Vector2 Vector { get; set; }
    public Touch Touch { get; set; }
    public bool IsTouched { get; set; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setPosition(Vector2 pos)
    {
        transform.position = new Vector3(pos.x, pos.y);
        if (NextPosition != pos)
        {
            NextPosition = pos;
        }
    }

    public float radius()
    {
        return gameObject.GetComponent<SpriteRenderer>().sprite.rect.size.x / 2;
    }

}
