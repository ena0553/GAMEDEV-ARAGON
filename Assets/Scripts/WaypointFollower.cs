using UnityEngine;

public class WaypointFollower : MonoBehaviour
{

    [SerializeField] GameObject[] waypoints;
    
    int currentWaypoint = 0;

    [SerializeField] float speed = 1.6f;

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, waypoints[currentWaypoint].transform.position) < 0.1f)
        {
            currentWaypoint++;

            if(currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, speed * Time.deltaTime);
    }
}
