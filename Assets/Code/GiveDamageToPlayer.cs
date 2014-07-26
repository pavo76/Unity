﻿using UnityEngine;

public class GiveDamageToPlayer:MonoBehaviour
{
    public int DamageToGive = 10;

    private Vector2
        _lastPostition,
        _velocity;

    public void LateUpdate()
    {
        _velocity = (_lastPostition - (Vector2) transform.position)/Time.deltaTime;
        _lastPostition = transform.position;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player == null)
            return;
        player.TakeDamage(DamageToGive);
        var controller = player.GetComponent<CharacterController2D>();
        var totalVelocity = controller.Velocity + _velocity;
        controller.SetForce(new Vector2(
            -1*Mathf.Sign(totalVelocity.x)*Mathf.Clamp(Mathf.Abs(totalVelocity.x)*6, 10, 40),
            -1 * Mathf.Sign(totalVelocity.y) * Mathf.Clamp(Mathf.Abs(totalVelocity.y) * 6, 10, 40)));
    }
}