using UnityEngine;

public class Chain : MonoBehaviour
{
    [SerializeField]
    private SpringJoint2D spring;
    [SerializeField]
    private LineRenderer line;
    [SerializeField]
    private Transform linePosition;

    private void Update()
    {
        if (line.enabled == true)
            line.SetPosition(0, new Vector3(linePosition.position.x, linePosition.position.y, -10));
    }

    public void EnableChain(Transform deathLocation)
    {
        spring.connectedAnchor = deathLocation.position;
        line.enabled = true;
        line.SetPosition(1, new Vector3(deathLocation.position.x, deathLocation.position.y, -10));
        spring.enabled = true;
    }
    
    public void DisableChain()
    {
        line.enabled = false;
        spring.enabled = false;
    }
}
