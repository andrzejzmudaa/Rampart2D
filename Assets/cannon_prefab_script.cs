using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannon_prefab_script : MonoBehaviour
{
    int healthPoints;
    public cannonBallScript cannonBall;
    cannonBallScript cannonBallSpawned;

    public int HealthPoints { get => healthPoints; }
    // Start is called before the first frame update
    void Start()
    {
        healthPoints = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void rotateToPlayerAim(Vector3 _vectorTo)
    {
        float angle = Quaternion.FromToRotation(Vector3.up, _vectorTo - this.gameObject.transform.position).eulerAngles.z;
        this.transform.eulerAngles = new Vector3(0,0, angle);
    }

    public void shootCanonBall(Vector3 _targetPos)
    {
        cannonBallSpawned = Object.Instantiate<cannonBallScript>(cannonBall, this.transform.position, this.transform.rotation , this.gameObject.transform.parent);
        cannonBallSpawned.setTargetPos(_targetPos);


    }
}
