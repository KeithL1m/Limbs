using UnityEngine;

public class CameraFocus : MonoBehaviour
{

    public float _halfXBounds = 20.0f;
    public float _halfYBounds = 15.0f;
    public float _halfZBounds = 15.0f;

    public Bounds focusBounds;

    // Update is called once per frame
    void Update()
    {
        Vector3 position = gameObject.transform.position;
        Bounds bounds = new Bounds();
        bounds.Encapsulate(new Vector3(position.x - _halfXBounds, position.y - _halfYBounds, position.z - _halfZBounds));
        bounds.Encapsulate(new Vector3(position.x + _halfXBounds, position.y + _halfYBounds, position.z + _halfZBounds));
        focusBounds = bounds;
    }
}
