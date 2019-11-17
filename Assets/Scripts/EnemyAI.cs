using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface EnemyAI
{
    void StartFeeding();

    void StopFeeding();

    void DestroyEnemy();

    bool SeesPlayer();

}
