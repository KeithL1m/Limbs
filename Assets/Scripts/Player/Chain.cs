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
            line.SetPosition(0, linePosition.position);
    }

    public void EnableChain(Transform deathLocation)
    {
        spring.connectedAnchor = deathLocation.position;
        line.enabled = true;
        line.SetPosition(1, deathLocation.position);
        spring.enabled = true;
    }
    
    public void DisableChain()
    {
        line.enabled = false;
        spring.enabled = false;
    }
}
