using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class brickProperties : MonoBehaviour
{
    protected bool[,] fieldsOccupied;




    public abstract bool[,] getFieldsOccupied();

    public static bool[,] convert1DsArraysToSingle2D(params bool[][] arrays)
    {
        int arrayDimensionx = 0;
        int arrayDimensiony = 0;
        List<bool[]> boolList = new List<bool[]>();
        foreach (var singleArray in arrays)
        {
            if (arrayDimensionx == 0)
            {
                arrayDimensionx = singleArray.Length;
                arrayDimensiony++;
                boolList.Add(singleArray);
            }
            else if (arrayDimensionx == singleArray.Length)
            {
                arrayDimensiony++;
                boolList.Add(singleArray);
            }
            else
            {
                throw new ArgumentException("Arrays sent as argument have diffrent size,can not be merged");
            }
        }
        if (arrayDimensionx == 0 && arrayDimensiony == 0)
            throw new ArgumentException("Arrays sent as argument are empty");

        bool[,] tempReturnArray = new bool[arrayDimensionx, arrayDimensiony];
        for (int row = 0; row < arrayDimensionx; row++)
        {
            for (int collumn = 0; collumn < arrayDimensiony; collumn++)
            {
                tempReturnArray[row, collumn] = (bool)boolList.ElementAt(row).GetValue(collumn);
            }

        }      
        return tempReturnArray;
    }




        

}

        
      