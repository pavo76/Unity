using UnityEngine;

public class SimpleEnemyAI: MonoBehaviour,ITakeDamage, IPlayerRespawnListener
{
    public float Speed;
    public float FireRate = 1;
    public Projectile Projectile;
    public GameObject DestroyedEffect;
    public int PointsToGivePlayer;
    public AudioClip ShootSound;
    
    private CharacterController2D _controler;
    private Vector2 _direction;
    private Vector2 _startPosition;
    private float _canFireIn;

    public void Start()
    {
        _controler = GetComponent<CharacterController2D>();
        _direction=new Vector2(-1,0);
        _startPosition = transform.position;

    }

    public void Update()
    {
        _controler.SetHorizontalForce(_direction.x*Speed);

        if ((_direction.x < 0 && _controler.State.IsCollidingLeft) ||
            (_direction.x > 0 && _controler.State.IsCollidingRight))
        {
            _direction = -_direction;
            transform.localScale=new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if ((_canFireIn -= Time.deltaTime) > 0)
            return;
        var raycast = Physics2D.Raycast(transform.position, _direction, 10, 1 << LayerMask.NameToLayer("Player"));
        if (!raycast)
            return;
        var projectile = (Projectile) Instantiate(Projectile, transform.position, transform.rotation);
        projectile.Initialize(gameObject, _direction, _controler.Velocity);
        _canFireIn = FireRate;

        if(ShootSound!=null)
            AudioSource.PlayClipAtPoint(ShootSound, transform.position);
    }

    public void OnPlayerRespawnInThisCheckpoint(Checkpoint checkpoint, Player player)
    {
        _direction=new Vector2(-1,0);
        transform.localScale=new Vector3(1,1,1);
        transform.position = _startPosition;
        gameObject.SetActive(true);
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        if (PointsToGivePlayer != 0)
        {
            GameManager.Instance.AddPoints(PointsToGivePlayer);
            FloatingText.Show(string.Format("+{0}!", PointsToGivePlayer), "PointStarText", new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));
        }
        
        Instantiate(DestroyedEffect, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
}