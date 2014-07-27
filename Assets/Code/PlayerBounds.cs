using UnityEngine;

public class PlayerBounds: MonoBehaviour
{
    public enum BoundsBehavoiur
    {
        Nothing,
        Constrain,
        Kill
    }

    public BoxCollider2D Bounds;
    public BoundsBehavoiur Above;
    public BoundsBehavoiur Left;
    public BoundsBehavoiur Right;
    public BoundsBehavoiur Bellow;

    private Player _player;
    private BoxCollider2D _boxCollider;

    public void Start()
    {
        _player = GetComponent<Player>();
        _boxCollider = GetComponent<BoxCollider2D>();

    }

    public void Update()
    {
        if (_player.IsDead)
            return;
        var colliderSize = new Vector2(_boxCollider.size.x*Mathf.Abs(transform.localScale.x),
            _boxCollider.size.y*Mathf.Abs(transform.localScale.y))/2;
        if (Above != BoundsBehavoiur.Nothing && transform.position.y + colliderSize.y > Bounds.bounds.max.y)
            ApplyBoundsBehaviour(Above, new Vector2(transform.position.x, Bounds.bounds.max.y - colliderSize.y));
        if (Bellow != BoundsBehavoiur.Nothing && transform.position.y - colliderSize.y < Bounds.bounds.min.y)
            ApplyBoundsBehaviour(Bellow, new Vector2(transform.position.x, Bounds.bounds.min.y + colliderSize.y));
        if (Right != BoundsBehavoiur.Nothing && transform.position.x + colliderSize.x > Bounds.bounds.max.x)
            ApplyBoundsBehaviour(Right, new Vector2(Bounds.bounds.max.x - colliderSize.x, transform.position.y));
        if (Left != BoundsBehavoiur.Nothing && transform.position.x - colliderSize.x < Bounds.bounds.min.x)
            ApplyBoundsBehaviour(Left, new Vector2(Bounds.bounds.min.x + colliderSize.x, transform.position.y));


    }

    private void ApplyBoundsBehaviour(BoundsBehavoiur behavoiur, Vector2 constrainedPosition)
    {
        if (behavoiur == BoundsBehavoiur.Kill)
        {
            LevelManager.Instance.KillPlayer();
            return;
        }

        transform.position = constrainedPosition;

    }
}