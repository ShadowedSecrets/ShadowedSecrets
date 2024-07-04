using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemManager : MonoBehaviour
{
    [SerializeField] GameObject player;

    public bool isPickedUp;
    private Vector2 vel;
    public float smoothTime;

    void Start()
    {

    }

    
    void Update()
    {
        if (isPickedUp)
        {
            Vector3 offset = new Vector3(-1f, 0, 0);
            transform.position = Vector2.SmoothDamp(transform.position, player.transform.position + offset, ref vel, smoothTime);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isPickedUp)
        {
            isPickedUp = true;
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayKeyGrabSound();
            }
        }

    }

    public void Initialize(GameObject playerObject)
    {
        player = playerObject;
    }

}
