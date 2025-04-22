using UnityEngine;

[CreateAssetMenu(fileName ="Gun" , menuName ="GunType")]
public class GunType : ScriptableObject
{
    public float FireRate = 0.5f;
    public float FireDistance = 50f;
    public int Maxammo = 45;
    public bool IsShotGun;
    public bool IsSniper;
    public int Damage=4;
    public AudioClip gunSfx;
}
