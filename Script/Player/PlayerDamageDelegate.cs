using UnityEngine;

public class PlayerDamageDelegate : MonoBehaviour, IDamageable
{
    public void TakeDamage(AttackInfo attack)
    {
        PlayerHP.instance.TakeDamage(attack);
    }
}
