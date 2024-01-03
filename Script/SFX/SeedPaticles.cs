using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(ParticleSystem))]
public class SeedPaticles : MonoBehaviour
{
    public static Action<Vector3[]> OnSeedsCollided;
    private void OnParticleCollision(GameObject other)
    {
        if (PlayerSowAbility.Instance.CurrentSowAmount > 0)
        {
            ParticleSystem ps = GetComponent<ParticleSystem>();
            List<ParticleCollisionEvent> collisonEvents = new List<ParticleCollisionEvent>();

            int collisonAmount = ps.GetCollisionEvents(other, collisonEvents);
            Vector3[] collisonPositions = new Vector3[collisonAmount];

            for (int i = 0; i < collisonAmount; i++)
                collisonPositions[i] = collisonEvents[i].intersection;
            if (collisonPositions.Length > 0)
            OnSeedsCollided?.Invoke(collisonPositions);
        }
    }
}
