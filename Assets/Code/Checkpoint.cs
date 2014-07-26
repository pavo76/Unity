﻿using System.Collections;
using System.Collections.Generic;
using  UnityEngine;
public class Checkpoint : MonoBehaviour
{
    private List<IPlayerRespawnListener> _listeners;
    public void Awake()
    {
        _listeners=new List<IPlayerRespawnListener>();
    }

    public void PlayerHitCheckpoint()
    {
        StartCoroutine(PlayerHitCeckpointCo(LevelManager.Instance.CurrentTimeBonus));
    }

    private IEnumerator PlayerHitCeckpointCo(int bonus)
    {
        FloatingText.Show("Checkpoint", "CheckpointText", new CenteredTextPositioner(.5f));
        yield return new WaitForSeconds(.5f);
        FloatingText.Show(string.Format("+{0} time bonus!", bonus), "CheckpointText", new CenteredTextPositioner(.5f));
    }

    public void PlayerLeftCheckpoint()
    {
        
    }

    public void SpawnPlayer(Player player)
    {
        if (transform != null) player.RespawnAt(transform);

        foreach (var listener in _listeners)
        {
            listener.OnPlayerRespawnInThisCheckpoint(this, player);
        }
    }

    public void AssignObjectToCheckpoint(IPlayerRespawnListener listener)
    {
        _listeners.Add(listener);
    }

}
