using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class budzyn
{

    public static int[,] processPixels2Dimensional(int[,] pixels, int outWidth, int outHeight)
    {

        bool[,] maskData = new bool[outWidth,outHeight];



        int pixelValue;
        for (int i = 0; i < outWidth; i++)
        {
            for (int j = 0; j < outHeight; j++)
            {

                // if on edge
                if ((i == 0) || (i == outWidth - 1) || (j == 0) || (j == outHeight - 1))
                {
                    pixelValue = pixels[i,j];
                    if (pixelValue == 0)
                    {
                        maskData[i,j] = true; // marking all background pixels on edge
                    }
                    else
                    {
                        maskData[i,j] = false;
                    }
                }
                else
                {
                    maskData[i,j] = false; // mark 0 for now
                }
            }
        }


        int numberOfHolesFilled = 0;

        // iterator
        int it = 0;

        // extremum for every iteration
        int exminX = 0;
        int exmaxX = outWidth;
        int exminY = 0;
        int exmaxY = outHeight;

        do
        {        
            numberOfHolesFilled = 0;
            int sizeOfArray = 8;
            int[] indexesX = new int[sizeOfArray];
            int[] indexesY = new int[sizeOfArray];
            int xVal, yVal;
            for (int x = exminX; x < exmaxX; x++)
            {
                for (int y = exminY; y < exmaxY; y++)
                {
                    // we start from the edge and then in every iteration of "do while" loop we add one pixel layer inside from the border - this "if" checks if we don't go too much to the center at this step. If we do - then we will only lose time because on this step there is no information in the center
                    
                    

                        // if our border pixel is 0 - still belong to the edge
                        if ((maskData[x ,y]))
                        {

                            indexesX[0] = x + 1;
                            indexesX[1] = x ;
                            indexesX[2] = x - 1;
                            indexesX[3] = x;
                            indexesX[4] = x - 1;
                            indexesX[5] = x - 1;
                            indexesX[6] = x + 1;
                            indexesX[7] = x + 1;


                            indexesY[0] = y;
                            indexesY[1] = y + 1;
                            indexesY[2] = y;
                            indexesY[3] = y - 1;
                            indexesY[4] = y - 1;
                            indexesY[5] = y + 1;
                            indexesY[6] = y - 1;
                            indexesY[7] = y + 1;


                            for (int i = 0; i < sizeOfArray; i++)
                            {
                                xVal = indexesX[i];
                                yVal = indexesY[i];

                                if (xVal >= 0 && xVal < outWidth &&
                                    yVal >= 0 && yVal < outHeight)
                                {

                                    pixelValue = pixels[xVal,yVal];
                                    if (pixelValue == 0)
                                    {
                                        maskData[xVal,yVal] = true; // marking all background pixels on edge
                                    }
                                    else
                                    {
                                        maskData[xVal,yVal] = false;
                                    }

                                    if (!maskData[xVal,yVal] && pixelValue == 0)
                                    {
                                        maskData[xVal,yVal] = true;
                                        numberOfHolesFilled++;
                                    }
                                }
                            }

                        }
                    
                }
            }
            it++;
        }
        while (numberOfHolesFilled > 0);

        // at this step maskData contains value 1 in all background external pixels (not on edge, not inside to be filled)

        /// 11111               11111
        /// 10001              10001
        /// 10101  ==>      10001
        /// 10001              10001
        /// 11111               11111


        // we need to fill 2 (or whatever) for each pixel not outside (0 in maskData), but which is not a wall already (pixel = 0)
        //    int **resArray  = (int**)malloc(sizeof(int) * outWidth * outHeight);
        //    const int size = outWidth;
        //    enum N { N = outHeight };
        //    auto resArray = new int [outWidth][size];
        //    auto array = new double[outWidth][outHeight]();

        //    int resArray[outWidth][outHeight];

        int[,] resArray = new int[outWidth,outWidth];

        for (int i = 0; i < outWidth; i++)
        {
            for (int j = 0; j < outHeight; j++)
            {


                int pixel = pixels[i,j];
                bool maskValue = maskData[i,j];
                int newPixelValue;
                if (pixel == 0 && !maskValue)
                {
                    newPixelValue = 2;
                }
                else
                {
                    newPixelValue = pixel;
                }
                resArray[i,j] = newPixelValue;
            }
        }
        // and now all holes were filled

        return resArray;
    }





    public static void processMap2D(Dictionary<Vector2Int, rampartTile> _playerTiles, BoundsInt _mapBounds ,Tilemap _incomingPlayerMap ,Color _playerColor)
    {
        rampartTile tempRampartTile;
        Tilemap plyerMapCloneForCastleBackground = _incomingPlayerMap.transform.GetChild(1).gameObject.GetComponent<Tilemap>();


        for (int i = _mapBounds.xMin; i <= _mapBounds.xMax; i++)
        {
            for (int j = _mapBounds.yMin; j <= _mapBounds.yMax; j++)
            {
                _playerTiles.TryGetValue(new Vector2Int(i, j), out tempRampartTile);
                if (tempRampartTile.isOccupied && tempRampartTile.isPlayable)
                    tempRampartTile.cellStatus = rampartTile.tileAlgorithmStatus.Wall;
                // if on edge
                if ((i == _mapBounds.xMin) || (i == _mapBounds.xMax) || (j == _mapBounds.yMin) || (j == _mapBounds.yMax))
                {
                 
                    if (!tempRampartTile.isOccupied)
                    {
                        tempRampartTile.cellStatus = rampartTile.tileAlgorithmStatus.NotInternal;
                    }
                    else
                    {
                        tempRampartTile.cellStatus = rampartTile.tileAlgorithmStatus.Wall;
                    }
                }
                else if (!tempRampartTile.isOccupied)
                {
                    tempRampartTile.cellStatus = rampartTile.tileAlgorithmStatus.NotDefinedYet;
                }
            }
        }


        // extremum for every iteration
        int exminX = _mapBounds.xMin;
        int exmaxX = _mapBounds.xMax;
        int exminY = _mapBounds.yMin;
        int exmaxY = _mapBounds.yMax;
        List<rampartTile> tilesNotCheckedYet = new List<rampartTile>();
        int previousCounterValue = 0;

        do
        {
            previousCounterValue = tilesNotCheckedYet.Count;
            tilesNotCheckedYet.Clear();
            int sizeOfArray = 8;
            int[] indexesX = new int[sizeOfArray];
            int[] indexesY = new int[sizeOfArray];
            int xVal, yVal;
            for (int x = exminX; x <= exmaxX; x++)
            {
                for (int y = exminY; y <= exmaxY; y++)
                {
                    // we start from the edge and then in every iteration of "do while" loop we add one pixel layer inside from the border - this "if" checks if we don't go too much to the center at this step. If we do - then we will only lose time because on this step there is no information in the center

                    _playerTiles.TryGetValue(new Vector2Int(x, y), out tempRampartTile);

                    if (tempRampartTile.cellStatus == rampartTile.tileAlgorithmStatus.NotInternal)
                    {

                        indexesX[0] = x + 1;
                        indexesX[1] = x;
                        indexesX[2] = x - 1;
                        indexesX[3] = x;
                        indexesX[4] = x - 1;
                        indexesX[5] = x - 1;
                        indexesX[6] = x + 1;
                        indexesX[7] = x + 1;


                        indexesY[0] = y;
                        indexesY[1] = y + 1;
                        indexesY[2] = y;
                        indexesY[3] = y - 1;
                        indexesY[4] = y - 1;
                        indexesY[5] = y + 1;
                        indexesY[6] = y - 1;
                        indexesY[7] = y + 1;


                        for (int i = 0; i < sizeOfArray; i++)
                        {
                            xVal = indexesX[i];
                            yVal = indexesY[i];

                            if (xVal >= exminX && xVal <= exmaxX &&
                                yVal >= exminY && yVal <= exmaxY)
                            {
                                _playerTiles.TryGetValue(new Vector2Int(xVal, yVal), out tempRampartTile);
                                //If already status is known,skip all calculations and continue with next
                                if (tempRampartTile.cellStatus == rampartTile.tileAlgorithmStatus.NotInternal || tempRampartTile.cellStatus == rampartTile.tileAlgorithmStatus.Wall)
                                    continue;
                                if (tempRampartTile.isOccupied && tempRampartTile.isPlayable)
                                {
                                    tempRampartTile.cellStatus = rampartTile.tileAlgorithmStatus.Wall;

                                }
                                else
                                {
                                    tempRampartTile.cellStatus = rampartTile.tileAlgorithmStatus.NotInternal;
                                }
                            }
                        }

                    }
                    else if (tempRampartTile.cellStatus != rampartTile.tileAlgorithmStatus.Wall)
                    {
                        tilesNotCheckedYet.Add(tempRampartTile);
                    }

                }
            }
        }
        while (tilesNotCheckedYet.Count > 0 && tilesNotCheckedYet.Count != previousCounterValue);

        for (int i = _mapBounds.xMin; i <= _mapBounds.xMax; i++)
        {
            for (int j = _mapBounds.yMin; j <= _mapBounds.yMax; j++)
            {
                _playerTiles.TryGetValue(new Vector2Int(i, j), out tempRampartTile);
                if (tempRampartTile.cellStatus == rampartTile.tileAlgorithmStatus.NotDefinedYet)
                {
                    tempRampartTile.cellStatus = rampartTile.tileAlgorithmStatus.Internal;
                    //_incomingPlayerMap.SetColor(tempRampartTile.TilePos, new Color(_playerColor.r, _playerColor.g, _playerColor.b));
                    if ((Mathf.Abs(tempRampartTile.TilePos.x) % 2) == (Mathf.Abs(tempRampartTile.TilePos.y) % 2))
                    {
                        _incomingPlayerMap.SetColor(tempRampartTile.TilePos, new Color(_playerColor.r, _playerColor.g, _playerColor.b, 0.1f));
                        if (tempRampartTile.isCastle)
                        {
                            plyerMapCloneForCastleBackground.SetColor(tempRampartTile.TilePos, new Color(_playerColor.r, _playerColor.g, _playerColor.b, 0.1f));
                        }
                    }
                    else
                    {
                        _incomingPlayerMap.SetColor(tempRampartTile.TilePos, new Color(_playerColor.r, _playerColor.g, _playerColor.b, 0.7f));
                        if (tempRampartTile.isCastle)
                        {
                            plyerMapCloneForCastleBackground.SetColor(tempRampartTile.TilePos, new Color(_playerColor.r, _playerColor.g, _playerColor.b, 0.7f));
                        }
                    }
                }
                if (tempRampartTile.cellStatus == rampartTile.tileAlgorithmStatus.NotInternal)
                setBackgroundColorAsChess(new Vector2Int(i,j),_incomingPlayerMap);



            }
        }
        // and now all holes were filled

    }


    public static void setBackgroundColorAsChess(Vector2Int _vectorXY, Tilemap _playerMap)
    {
        if ((Mathf.Abs(_vectorXY.x) % 2) == (Mathf.Abs(_vectorXY.y) % 2))
        {
            _playerMap.SetColor(new Vector3Int(_vectorXY.x, _vectorXY.y, 0), new Color(1.0f, 1.0f, 1.0f, 0.8f));
        }
        else
        {
            _playerMap.SetColor(new Vector3Int(_vectorXY.x, _vectorXY.y, 0), new Color(1.0f, 1.0f, 1.0f, 1.0f));
        }
    }
}


  





