using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class AgentPosition : MonoBehaviour
{
    GameObject ARcamera;
    NavMeshAgent agent;
    MeshRenderer mesh;

    public TextMeshProUGUI debugText; // drag any TMP label here temporarily

    void Awake()
    {
        ARcamera = Camera.main.gameObject;
        agent = GetComponent<NavMeshAgent>();
        mesh = GetComponent<MeshRenderer>();
        mesh.enabled = false;
    }

    void Update()
    {
        bool foundNavMesh = NavMesh.SamplePosition(ARcamera.transform.position, out NavMeshHit hit, 4f, NavMesh.AllAreas);

        if (debugText != null)
        {
            debugText.text = $"Cam: {ARcamera.transform.position}\n" +
                              $"NavMeshFound: {foundNavMesh}\n" +
                              $"HitPos: {(foundNavMesh ? hit.position.ToString() : "N/A")}\n" +
                              $"AgentOnMesh: {agent.isOnNavMesh}\n" +
                              $"AgentPos: {agent.transform.position}" +
                              $"MapSpace Scale: {GameObject.Find("Map Space").transform.lossyScale}\n" +
                              $"MapSpace Pos: {GameObject.Find("Map Space").transform.position}";
        }

        if (!foundNavMesh)
        {
            return;
        }

        agent.Warp(new Vector3(ARcamera.transform.position.x, agent.gameObject.transform.position.y, ARcamera.transform.position.z));

        if (agent.gameObject.transform.localPosition.y > 0 && agent.gameObject.transform.localPosition.y > 1.5)
        {
            agent.Warp(new Vector3(ARcamera.transform.position.x, ARcamera.transform.position.y - 1.5f, ARcamera.transform.position.z));
        }
        else if (agent.gameObject.transform.localPosition.y < 0 && agent.gameObject.transform.localPosition.y < -3.5)
        {
            agent.Warp(new Vector3(ARcamera.transform.position.x, ARcamera.transform.position.y + 3.5f, ARcamera.transform.position.z));
        }
    }
}