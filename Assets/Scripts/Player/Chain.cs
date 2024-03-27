using UnityEngine;

public class Chain : MonoBehaviour
{
    [SerializeField]
    private SpringJoint2D spring;
    [SerializeField]
    private LineRenderer line;
    [SerializeField]
    private Transform linePosition;

    void Start()
    {
        // Get the material of the Line Renderer
        Material material = line.material;

        // Set the render queue to a high value to render the Line Renderer in front
        material.renderQueue = 5000;
    }

    private void Update()
    {
        if (line.enabled == true)
            line.SetPosition(0, new Vector3(linePosition.position.x, linePosition.position.y, -3.9f));
    }

    public void EnableChain(Transform deathLocation)
    {
        spring.connectedAnchor = deathLocation.position;
        line.enabled = true;
        line.SetPosition(1, new Vector3(deathLocation.position.x, deathLocation.position.y, -3.9f));
        spring.enabled = true;
    }
    
    public void DisableChain()
    {
        line.enabled = false;
        spring.enabled = false;
    }
}
