using Assets.Model;
using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : BaseManager  {

    private UserData userData;
    private Dictionary<RoleType, RoleData> roleDataDict = new Dictionary<RoleType, RoleData>();
    private Transform rolePosition;
    private RoleType currentRoleType;
    private GameObject currentGameObject;
    private GameObject remoteGameObject;
    private GameObject PlayerSyncRequest;
    private List<GameObject> remoteGameobject=new List<GameObject>();

    private ShootRequest shootRequest;
    private DamageRequest damageRequest;

    private Dictionary<RoleData,Transform> remotePlayerDict = new Dictionary<RoleData, Transform>();

    public PlayerManager(GameFacade facade) : base(facade) { }

    public UserData UserData
    {
        set{ userData=value;}
        get { return userData; }
    }

    public void UpdateUserData(int totalCount,int winCount)
    {
        userData.totalCount = totalCount;
        userData.winCount = winCount;
    }

    public void SetCurrentRoleType(int rt)
    {
        currentRoleType = (RoleType)rt;
    }

    public override void OnInit()
    {
        rolePosition = GameObject.Find("RolePosition").transform;
        InitRoleData();
    }

    private void InitRoleData()
    {
        roleDataDict.Add(RoleType.Blue, new RoleData(RoleType.Blue,"Prefabs/Hunter_BLUE", "Prefabs/Arrow_BLUE", rolePosition.Find("position2"), "Prefabs/Explosion_Blue"));
        roleDataDict.Add(RoleType.Red, new RoleData(RoleType.Red , "Prefabs/Hunter_RED", "Prefabs/Arrow_RED", rolePosition.Find("position1"), "Prefabs/Explosion_Red"));
    }

    public void SpawnRoles()
    {
        foreach (RoleData roleData in roleDataDict.Values)
        {
            GameObject go= GameObject.Instantiate(roleData.RolePrefab, roleData.SpawnPosition, Quaternion.identity);
            if (roleData.RoleType == currentRoleType)
            {
                currentGameObject = go;
            }
            else
            {
                remoteGameObject = go;
                remotePlayerDict.Add(roleData,go.transform);
                //remoteGameobject.Add(go);
            }
        }
    }

    public GameObject GetCurrentGameObject()
    {
        return currentGameObject;
    }

    private RoleData GetRoleData(RoleType roleType)
    {
        RoleData roledata = null;
        roleDataDict.TryGetValue(roleType, out roledata);
        return roledata;
    }

    public void AddControlScript()
    {
        PlayerAttack playerAttack=currentGameObject.AddComponent<PlayerAttack>();
        currentGameObject.AddComponent<PlayerMove>();
        RoleData rd;
        rd=GetRoleData(currentRoleType);
        playerAttack.arrowPrefab = rd.ArrowPrefab;     
        playerAttack.SetPlayerManager(this);
    }

    public void CreateSyncRequest()
    {
        PlayerSyncRequest = new GameObject("PlayerSyncRequest");
        PlayerSyncRequest.AddComponent<MoveRequest>().
            SetLocalPlayer(currentGameObject.transform,currentGameObject.GetComponent<PlayerMove>()).
            SetRemotePlayer(remotePlayerDict);
        shootRequest=PlayerSyncRequest.AddComponent<ShootRequest>();
        shootRequest.playerManager = this;
        damageRequest = PlayerSyncRequest.AddComponent<DamageRequest>();
        damageRequest.playerManager = this;
    }

    public void Shoot(GameObject arrowPrefab,Vector3 position,Quaternion rotation)
    {
        facade.PlayNormalSound(AudioManager.Sound_ArrowShoot);
        GameObject.Instantiate(arrowPrefab, position, rotation).GetComponent<Arrow>().isLocal = true ;
        shootRequest.SendRequest(arrowPrefab.GetComponent<Arrow>().roleType, position, rotation.eulerAngles);
    }

    public void RemoteShoot(RoleType roleType,Vector3 pos, Vector3 rotation)
    {
        RoleData roleData;
        roleData=GetRoleData(roleType);
        Transform remoteTransform;
        remotePlayerDict.TryGetValue(roleData ,out remoteTransform);
        remoteTransform.GetComponent<Animator>().SetBool("Attack", true);
        Transform transform=GameObject.Instantiate(roleData.ArrowPrefab).GetComponent<Transform>();
        transform.position = pos;
        transform.eulerAngles =rotation;
    }

    public void SendAttack(int damage)
    {
        damageRequest.SendRequest(damage);
    }

    public void GameOver()
    {
        GameObject.Destroy(currentGameObject);
        GameObject.Destroy(PlayerSyncRequest);
        GameObject.Destroy(remoteGameObject);
        remotePlayerDict.Clear();
        //foreach (var item in remoteGameobject)
        //{
        //    GameObject.Destroy(item);
        //}
        //remoteGameobject.Clear();
        shootRequest = null;
        damageRequest = null;
    }
}
