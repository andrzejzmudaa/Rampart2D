using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannon_prefab_script : MonoBehaviour
{
    int healthPoints;
    public cannonBallScript cannonBall;
    cannonBallScript cannonBallSpawned;
    float cannonAngle;
    public bool shootedBallStillExist { get; set; }
    float xBallStartMove;
    float yBallStartMove;

    public int HealthPoints { get => healthPoints; }
    // Start is called before the first frame update
    void Start()
    {
        healthPoints = 100;
        shootedBallStillExist = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void rotateToPlayerAim(Vector3 _vectorTo)
    {
        cannonAngle = Quaternion.FromToRotation(Vector3.up, _vectorTo - this.gameObject.transform.position).eulerAngles.z;
        if (cannonAngle >= 0 && cannonAngle < 10)
        {
            this.transform.eulerAngles = new Vector3(0, 0, 0);
            xBallStartMove = 0f;
            yBallStartMove = 0.8f;
        }
        if (cannonAngle >= 10 && cannonAngle < 160)
        {
            this.transform.eulerAngles = new Vector3(0, 0, 45);
            xBallStartMove = -0.75f;
            yBallStartMove = 0.75f;
        }
        else if (cannonAngle >= 160 && cannonAngle < 165)
        {
            this.transform.eulerAngles = new Vector3(0, 0, 45);
            xBallStartMove = -1f;
            yBallStartMove = 0f;
        }
        else if (cannonAngle >= 165 && cannonAngle < 170)
        {
            this.transform.eulerAngles = new Vector3(0, 0, 45);
            xBallStartMove = -0.75f;
            yBallStartMove = 0.75f;
        }
        else if (cannonAngle >= 170 && cannonAngle < 190)
        {//Srodek
            this.transform.eulerAngles = new Vector3(0, 0, 0);
            xBallStartMove = 0f;
            yBallStartMove = 0.8f;
        }
        else if (cannonAngle >= 190 && cannonAngle < 195)
        {
            this.transform.eulerAngles = new Vector3(0, 0, 135);
            xBallStartMove = 0.75f;
            yBallStartMove = 0.75f;
        }
        else if (cannonAngle >= 195 && cannonAngle < 200)
        {
            this.transform.eulerAngles = new Vector3(0, 0, 135);
            xBallStartMove = 1f;
            yBallStartMove = 0f;
        }
        else if (cannonAngle >= 200 && cannonAngle < 350)
        {
            this.transform.eulerAngles = new Vector3(0, 0, 135);
            xBallStartMove = 0.75f;
            yBallStartMove = 0.75f;
        }
        if (cannonAngle >= 350)
        {
            this.transform.eulerAngles = new Vector3(0, 0, 0);
            xBallStartMove = 0f;
            yBallStartMove = 0.8f;
        }




        //this.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public void shootCanonBall(Vector3 _targetPos)
    {
        if (!shootedBallStillExist)
        {
            cannonBallSpawned = Object.Instantiate<cannonBallScript>(cannonBall, new Vector3(this.transform.position.x + xBallStartMove, this.transform.position.y + yBallStartMove, -2), this.transform.rotation, this.gameObject.transform);
            cannonBallSpawned.setTargetPos(_targetPos);
        }


    }

}
