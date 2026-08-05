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
        navSurface.BuildNavMesh();
        Vector3 searchPos = new Vector3(arCamera.position.x, arCamera.position.y, arCamera.position.z);
        NavMeshHit hit;

        if (NavMesh.SamplePosition(searchPos, out hit, 3.0f, NavMesh.AllAreas))
        {
            
            agent.transform.position = hit.position;
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
        if (agent != null && agent.enabled && agent.isOnNavMesh && arCamera != null)
        {

            Vector3 currentCamPos = new Vector3(arCamera.position.x, arCamera.position.y, arCamera.position.z);
            NavMeshHit hit;

            if (NavMesh.SamplePosition(currentCamPos, out hit, 3.0f, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }
        }
    }
}