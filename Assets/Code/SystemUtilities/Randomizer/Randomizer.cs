using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Randomizer
{
    private int _intResult;
    private float _floatResult;

    public int GetRandomIndexFromList(int countListElements)
    {
        int newIndex = 0;

        newIndex = Random.Range(0, countListElements);

        return newIndex;
    }

    public int GetRamdonInt(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public int GetRamdonIntWithException(int min, int max, int exception)
    {
        _intResult = UnityEngine.Random.Range(min, max);

        while (_intResult == exception)
        {
            _intResult = UnityEngine.Random.Range(min, max);
        }

        return _intResult;
    }

    public float GetRamdonFloat(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public float GetRamdonFloatWithException(float min, float max, float exception)
    {
        _floatResult = UnityEngine.Random.Range(min, max);

        while (_floatResult == exception)
        {
            _floatResult = UnityEngine.Random.Range(min, max);
        }

        return _floatResult;
    }

    public List<T> ShuffleList<T>(List<T> inputList)
    {
        int i = 0;
        int t = inputList.Count;
        int r = 0;
        T p;

        List<T> tempList = new List<T>();
        tempList.AddRange(inputList);

        while (i < t)
        {
            r = Random.Range(i, tempList.Count);
            p = tempList[i];
            tempList[i] = tempList[r];
            tempList[r] = p;
            i++;
        }

        return tempList;
    }
}
