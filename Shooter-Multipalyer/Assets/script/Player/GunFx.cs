using Photon.Pun;
using UnityEngine;

public class GunFx : MonoBehaviour
{
    public Transform muzzlEffenctPoint;
    public PhotonView photonView;


    [PunRPC]
    private void showFx(string name, Vector3 position, Quaternion rotation)
    {
        ObjectPool.Instance.spawn(name, position, rotation);
    }

    public void MuzzleFX()
    {
        photonView.RPC("showFx", RpcTarget.AllBuffered, "MuzzleFx", muzzlEffenctPoint.position, transform.rotation);
    }

    public void HittFx(Vector3 pos,Vector3 normal,bool IsEnemy)
    {
        string fx = IsEnemy ? "BloodFx" : "HittFx";
        photonView.RPC("showFx", RpcTarget.AllBuffered, fx, pos, Quaternion.LookRotation(normal));
    }

    public void Trail(Vector3 start,Vector3 end)
    {
        var Trail = ObjectPool.Instance.spawn("TrailFx", start, Quaternion.identity);
        Trail.GetComponent<TrailRenderer>().Clear();
        Trail.transform.position = end;
    }
}
