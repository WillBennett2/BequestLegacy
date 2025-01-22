using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static MapData;

public class LoadMapData 
{
    public List<int> directionMap = new List<int>();
    public List<int> destinationMap = new List<int>();
    public List<int> mapTypes = new List<int>();
    public List<int> mapVariation = new List<int>();

    public void LoadMapLayout(List<int> mapData)
    {
        directionMap.Clear();
        destinationMap.Clear();
        mapTypes.Clear();
        mapVariation.Clear();

        foreach (int data in mapData)
        {

            int num = data;
            List<int> listOfInts = new List<int>();
            while (num > 0)
            {
                listOfInts.Add(num % 10);
                num = num / 10;
            }
            listOfInts.Reverse();
            listOfInts.ToArray();

            directionMap.Add(listOfInts[0]);
            destinationMap.Add(listOfInts[1]);
            mapTypes.Add(listOfInts[2]);
            mapVariation.Add(listOfInts[3]);
            //get type and assign variation based off that
        }
    }

}
