using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float moveSpeed = 0.1f;
    private int _currentWaypoint;

    // Start is called before the first frame update
    private void Start()
    {
        if (waypoints.Count <= 0) return;
        _currentWaypoint = 0;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
    
        transform.position = Vector3.MoveTowards(transform.position, waypoints[_currentWaypoint].transform.position,
            (moveSpeed * Time.deltaTime));

        if (Vector3.Distance(waypoints[_currentWaypoint].transform.position, transform.position) <= 0)
        {
            _currentWaypoint++;
        }

        if (_currentWaypoint != waypoints.Count) return;
        waypoints.Reverse();
        _currentWaypoint = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("On platform!");
        other.transform.parent = transform;
        Debug.Log("Player local scale before: " + other.transform.localScale);
        other.transform.localScale = new Vector3 (0.36f, 1.06f, 1.00f);
        Debug.Log("Player local scale after: " + other.transform.localScale);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        other.transform.parent = null;
        other.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
