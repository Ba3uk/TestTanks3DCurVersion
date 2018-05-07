using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TanksAI : TankController {

    private static int tankCount;


    [SerializeField] [Range(1, 5)]
    private float timeToRandomTurn = 2f;
    private float timeToLastRandomTurn;

    private TankShoot ts;
    private TankAIRespawn tankAIRespawn;

    void Start () {
        timeToLastRandomTurn = 0;
        InitializationTank();
        ts = GetComponent<TankShoot>();
    }

    private void FixedUpdate()
    {
        if (isServer)
        {
            ControllerAI();
        }
        else
            Move();
    }

    // Проверяет есть ли препядствие перед объектом, Если есть, то возвращает false, в противном случае true;
    private bool ForwardScan()
    {
        Ray leftRay, rightRay;
        RaycastHit[] leftHit, rightHit;
        Bounds bounds = col.bounds;
        bounds.Expand(-.03f);
        float hight = bounds.center.y-0.55f;

        switch (CurrentTurn)
        {
           

            case (TurnPosition.Left):
                {
                    
                    leftRay = new Ray(new Vector3(bounds.max.x, hight, bounds.max.z), transform.forward);
                    rightRay = new Ray(new Vector3(bounds.max.x, hight, bounds.min.z), transform.forward);

                    //Debug.DrawRay(new Vector3(bounds.max.x, hight, bounds.max.z), transform.forward * 0.1f, Color.green);
                    //Debug.DrawRay(new Vector3(bounds.max.x, hight, bounds.min.z), transform.forward * 0.1f, Color.red);
                    break;
                }

            case (TurnPosition.Right):
                {
                    leftRay = new Ray(new Vector3(bounds.min.x, hight, bounds.min.z), transform.forward);
                    rightRay = new Ray(new Vector3(bounds.min.x, hight, bounds.max.z), transform.forward);

                    //Debug.DrawRay(new Vector3(bounds.min.x, hight, bounds.max.z), transform.forward * 0.1f, Color.green);
                    //Debug.DrawRay(new Vector3(bounds.min.x, hight, bounds.min.z), transform.forward * 0.1f, Color.red);
                    break;
                }

            case (TurnPosition.Up):
                {
                    leftRay = new Ray(new Vector3(bounds.min.x, hight, bounds.max.z), transform.forward);
                    rightRay = new Ray(new Vector3(bounds.max.x, hight, bounds.max.z), transform.forward);

                    //Debug.DrawRay(new Vector3(bounds.min.x, hight, bounds.max.z), transform.forward * 0.1f, Color.green);
                    //Debug.DrawRay(new Vector3(bounds.max.x, hight, bounds.max.z), transform.forward * 0.1f, Color.red);
                    break;
                }

            case (TurnPosition.Down):
                {
                    leftRay = new Ray(new Vector3(bounds.max.x, hight, bounds.min.z), transform.forward);
                    rightRay = new Ray(new Vector3(bounds.min.x, hight, bounds.min.z), transform.forward);

                    //Debug.DrawRay(new Vector3(bounds.min.x, hight, bounds.min.z), transform.forward * 0.1f, Color.green);
                    //Debug.DrawRay(new Vector3(bounds.max.x, hight, bounds.min.z), transform.forward * 0.1f, Color.red);
                    break;
                }

            default:
                {
                    leftRay = new Ray();
                    rightRay = new Ray();
                    break;
                }
        }


        leftHit = Physics.RaycastAll(leftRay, 0.1f);
        rightHit = Physics.RaycastAll(rightRay, 0.1f);

        if(leftHit.Length == 0 && rightHit.Length == 0)
        {
            return true;
        }

        return false;
    }

    private void ControllerAI()
    {
        if (timeToLastRandomTurn < Time.fixedTime - timeToRandomTurn)
        {

            CurrentTurn = RandomDir(CurrentTurn);
            timeToLastRandomTurn = Time.fixedTime;
            CmdTurnPos(CurrentTurn);
        }


        if (ForwardScan())
        {
            ScanObject();
            Move();
            SoundConroller();
        }
        else
        {
            CurrentTurn = RandomDir(CurrentTurn);
            CmdTurnPos(CurrentTurn);
        }
    }

    private void ScanObject()
    {
        Bounds bounds = col.bounds;
        bounds.Expand(-.03f);
        Vector3 orgignV;
        if (CurrentTurn == TurnPosition.Left || CurrentTurn == TurnPosition.Right)
        {
            orgignV = new Vector3(bounds.max.x, bounds.center.y, bounds.center.z);
        }
        else
        {
             orgignV = new Vector3(bounds.center.x, bounds.center.y, bounds.max.z);
        }

        Ray ray = new Ray(orgignV, transform.forward);
        RaycastHit[] hitArr = Physics.RaycastAll(ray, 20f);
        
        foreach(RaycastHit hit in hitArr)
        {
            switch (hit.collider.tag)
            {
                case ("Wall"):
                    {
                       
                            if (Vector3.Distance(hit.collider.transform.position, transform.position) < .5f) 
                        {
                            CurrentTurn = RandomDir(CurrentTurn);
                            CmdTurnPos(CurrentTurn);
                        }

                        return;
                        
                    }
                case ("Bullet"):
                    {

                        if (hit.collider.GetComponent<BulletController>().tagTarget == "Empty")
                        {
                            if (ts.BulletIsReady()) ts.ClientFire();

                            if (Vector3.Distance(hit.collider.transform.position, transform.position) < 3f)
                            {
                                CurrentTurn = RandomDir(CurrentTurn);
                                CmdTurnPos(CurrentTurn);
                            }
                        }
                        break;

                    }
                case ("BrickWall"):
                case ("Player"):
                    {
                        
                        if (ts.BulletIsReady())
                        {
                            ts.ClientFire();
                        }
                        break;

                    }

            }
        }

     
    }

    private TurnPosition RandomDir(TurnPosition curTurn)
    {
        int i = (int)curTurn;
        int result;
        while (true)
        {
            
           result = (int)Random.Range(0, 4);
            if (i != result)
                return (TurnPosition)result;
        }

    }

    public void OnEnable()
    {
        tankCount++;
    }
    public void OnDisable()
    {
        tankCount--;
    }

    [Command]
    private void CmdTurnPos(TurnPosition turn)
    {
        RpcTurnTank(turn);
    }

    [ClientRpc]
    private void RpcTurnTank(TurnPosition turn)
    {
        CurrentTurn = turn;
    }




    public override void Died()
    {
        CmdDied();
    }

    [Command]
    public  void CmdDied()
    {

        RpcDied();
    }
    [ClientRpc]
    public void RpcDied()
    {
        InterfaceManager im = GameObject.Find("InterfaceConvas").GetComponent<InterfaceManager>();
        im.AddScore();
        Destroy(gameObject);
        base.Died();
    }




    public static int CountAI
    {
        get { return tankCount; }
    }
}
