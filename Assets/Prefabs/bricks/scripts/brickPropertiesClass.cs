﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class brickPropertiesClass
{
	[System.Serializable]
	public struct rowData
	{
		public bool[] row;
	}

	public rowData[] rows = new rowData[4];

	public enum angleType
	{
		A0deg,
		A90deg,
		A180deg,
		A270deg
	}






}
