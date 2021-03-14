using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "X-brick", menuName = "brick Template")]
public class brickTemplate : ScriptableObject
{
    public brickPropertiesClass boolBrick0deg;
    public brickPropertiesClass boolBrick90deg;
    public brickPropertiesClass boolBrick180deg;
    public brickPropertiesClass boolBrick270deg;
    
    public bool[,] getBoolean2dMatrix(brickPropertiesClass.angleType incomingAngle)
    {
        
        bool[,] returnBool = new bool[4, 4];
        if (incomingAngle == brickPropertiesClass.angleType.A0deg)
        {
            for (int i = 0; i < boolBrick0deg.rows.Length; i++)
            {
                for (int j = 0; j < boolBrick0deg.rows[i].row.Length; j++)
                {
                    returnBool[i, j] = boolBrick0deg.rows[i].row[j];
                }

            }
        }
        else if (incomingAngle == brickPropertiesClass.angleType.A90deg)
        {
            for (int i = 0; i < boolBrick90deg.rows.Length; i++)
            {
                for (int j = 0; j < boolBrick90deg.rows[i].row.Length; j++)
                {
                    returnBool[i, j] = boolBrick90deg.rows[i].row[j];
                }

            }
        }
        else if (incomingAngle == brickPropertiesClass.angleType.A180deg)
        {
            for (int i = 0; i < boolBrick180deg.rows.Length; i++)
            {
                for (int j = 0; j < boolBrick180deg.rows[i].row.Length; j++)
                {
                    returnBool[i, j] = boolBrick180deg.rows[i].row[j];
                }

            }
        }
        else if (incomingAngle == brickPropertiesClass.angleType.A270deg)
        {
            for (int i = 0; i < boolBrick270deg.rows.Length; i++)
            {
                for (int j = 0; j < boolBrick270deg.rows[i].row.Length; j++)
                {
                    returnBool[i, j] = boolBrick270deg.rows[i].row[j];
                }

            }
        }
        else
            throw new System.Exception("Something went wrong with geting 4x4 brick matrix");
        return returnBool;
    }
}




