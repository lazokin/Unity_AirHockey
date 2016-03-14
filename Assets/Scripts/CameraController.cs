using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public SceneController sceneController;

	void Start ()
    {
        GetComponent<Camera>().orthographicSize = sceneController.SceneHeight / 2;
    }

	void Update () 
	{

	}
}
