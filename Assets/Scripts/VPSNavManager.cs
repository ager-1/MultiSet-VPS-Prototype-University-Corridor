using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class VPSNavManager : MonoBehaviour
{
    [Header("References")]
    public NavMeshSurface navSurface;
    public NavMeshAgent agent;
    public Transform arCamera;

    public void PlaceAgentOnNavMesh()
    {
        // 1. Bake the mesh at the newly shifted MapSpace position
        navSurface.BuildNavMesh();

        // 2. Look for the NavMesh near the camera's current position.
        Vector3 searchPos = new Vector3(arCamera.position.x, arCamera.position.y, arCamera.position.z);
        NavMeshHit hit;

        if (NavMesh.SamplePosition(searchPos, out hit, 3.0f, NavMesh.AllAreas))
        {
            // 3. Teleport the agent exactly to the found floor
            agent.transform.position = hit.position;

            // 4. Enable the agent
            agent.enabled = true;
            Debug.Log("Agent successfully placed on runtime NavMesh at: " + hit.position);
        }
        else
        {
            Debug.LogError("NavMesh baked, but no surface found within 3 meters.");
        }
    }

    void Update()
    {
        // Only run if the agent has been successfully enabled and placed on the mesh
        if (agent != null && agent.enabled && agent.isOnNavMesh && arCamera != null)
        {
            // Track the phone's current X, Y, Z
            Vector3 currentCamPos = new Vector3(arCamera.position.x, arCamera.position.y, arCamera.position.z);
            NavMeshHit hit;

            // Project straight down to the floor
            if (NavMesh.SamplePosition(currentCamPos, out hit, 3.0f, NavMesh.AllAreas))
            {
                // Warp forces the agent to move to the new spot instantly 
                // while keeping its NavMesh logic intact
                agent.Warp(hit.position);
            }
        }
    }
}