using UnityEngine;

public class injuredguymoves : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
[SerializeField] GameObject[] waypoints;
int currentWaypointIndex = 0;
[SerializeField] float speed = 2f;
    // Update is called once per frame
    void Update()
    { if(Vector3.Distance(transform.position, waypoints[currentWaypointIndex].transform.position) < 0.1f)
        {
            currentWaypointIndex++;
            if(currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0; // Loop back to the first waypoint
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
    }
}
