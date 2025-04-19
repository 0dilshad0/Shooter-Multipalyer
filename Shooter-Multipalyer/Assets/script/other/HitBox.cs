using UnityEngine;

public class HitBox : MonoBehaviour
{
    public enum HitBoxTypes { head, chest, Leg, arms };
    public HitBoxTypes hitBoxTypes;
    public float DamageMultiplier = 1f;
}
