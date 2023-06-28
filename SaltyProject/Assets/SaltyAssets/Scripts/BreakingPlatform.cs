using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BreakingPlatform : MonoBehaviour
{
    [SerializeField] public Collider2D BoxCollider { get; private set; }
    [SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }

    [SerializeField] public float HoldUpTime { get; private set; } = 1.5f;
    [SerializeField] public float RespawnTime { get; private set; } = 2.35f;

    private IPlatformState _platformState = new BasicState();

    public bool Hit { get; private set; }

    private void Start()
    {
        BoxCollider = GetComponent<Collider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        _platformState = _platformState.UpdateState(this);
        Hit = false;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null) 
            Hit = true;
        Debug.Log("hit");
    }
}

public interface IPlatformState
{
    IPlatformState UpdateState(BreakingPlatform platform);
}

public class BasicState : IPlatformState
{
    public IPlatformState UpdateState(BreakingPlatform platform)
    {
        if (platform.Hit) return new BreakingState(platform.HoldUpTime);

        return this;
    }
}

public class BreakingState : IPlatformState
{
    private float holdUpTimer;
    public IPlatformState UpdateState(BreakingPlatform platform)
    {
        holdUpTimer -= Time.deltaTime;
        if (holdUpTimer <= 0)
        {
            platform.BoxCollider.enabled = false;
            platform.SpriteRenderer.enabled = false;

            Debug.Log("platform breaking");

            return new RespawningState(platform.RespawnTime);
        }
        return new BreakingState(holdUpTimer);
    }

    public BreakingState(float holdUpTimer)
    {
        this.holdUpTimer = holdUpTimer;
    }
}

public class RespawningState : IPlatformState
{
    private float respawnTimer;
    public IPlatformState UpdateState(BreakingPlatform platform)
    {
        respawnTimer -= Time.deltaTime;
        if (respawnTimer <= 0) 
        {
            platform.BoxCollider.enabled = true;
            platform.SpriteRenderer.enabled = true;

            Debug.Log("platform respawning");

            return new BasicState(); 
        }
        return new RespawningState(respawnTimer);
    }

    public RespawningState(float respawnTimer)
    {
        this.respawnTimer = respawnTimer;
    }
}