using UnityEngine;
using System.Collections;
using UnityEngine;

public class GasLimb : Limb
{
    private ParticleManager _particleManager;
    [SerializeField] Collider2D _collider;
    [SerializeField] float _delayTimer = 0.2f;

    public GameObject gasCloudPrefab;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Initialize()
    {
        base.Initialize();
        _particleManager = ServiceLocator.Get<ParticleManager>();
        _specialLimbs = true;
    }

    public override void ThrowLimb(int direction)
    {
        base.ThrowLimb(direction);

        StartCoroutine(EnableAfterDelay());
    }

    private IEnumerator EnableAfterDelay()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(_delayTimer);
        _collider.enabled = true;
        yield break;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (State != LimbState.Throwing)
            return;

        GasExplode();
    }


    void GasExplode()
    {
        Debug.Log("FART");
        _particleManager.PlayGas(transform.position);
        Destroy(gameObject);
    }
}