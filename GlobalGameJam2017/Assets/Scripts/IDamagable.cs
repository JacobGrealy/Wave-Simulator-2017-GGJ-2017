using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDamagable
{
   float GetHealth();
   void DoDamage(float damage);
}
