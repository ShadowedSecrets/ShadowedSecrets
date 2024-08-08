using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IBoss
{
    void TakeDamage(int damage);
    int GetCurrentHealth();
}

