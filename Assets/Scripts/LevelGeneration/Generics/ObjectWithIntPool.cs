using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedObjectPool<T1,T2> : MonoBehaviour where T1 : ObjectWithInt<T2>
{
    [SerializeField]
    protected List<T1> objectPool;

    protected bool poolIsPrepared = false;
    protected int sumOfChances;

    public bool PoolIsPrepared
    {
        get
        {
            return poolIsPrepared;
        }

        protected set
        {
            poolIsPrepared = value;
        }
    }

    public List<T1> ObjectPool
    {
        get
        {
            return objectPool;
        }

        set
        {
            objectPool = value;
        }
    }

    public void Awake()
    {
        ObjectPoolAwake();
    }

    protected virtual void ObjectPoolAwake()
    {
        ObjectPool = new List<T1>();
        CalculatePoolSize();        
    }

    public virtual void CalculatePoolSize()
    {
        sumOfChances = GetSumOfChances(this.ObjectPool);
        PoolIsPrepared = true;
    }

    protected int GetSumOfChances(List<T1> objectPool)
    {
        int tempSum = 0;

        foreach (T1 objectToCheck in objectPool)
        {
            tempSum += objectToCheck.value;
        }

        return tempSum;
    }

    protected virtual T2 GetRandomObjectFromPool(List<T1> poolToCheck, int sumOfPoolChances)
    {
        if (poolToCheck.Count == 1)
        {
            return poolToCheck[0].objectToUse;
        }

        T2 objectToReturn = default(T2);

        int r = Random.Range(1, sumOfPoolChances + 1);
        int previousFloor = 0;

        foreach (T1 poolObject in poolToCheck)
        {
            if (r <= poolObject.value + previousFloor)
            {
                objectToReturn = poolObject.objectToUse;
                break;
            }
            else
            {
                previousFloor += poolObject.value;
            }
        }

        return objectToReturn;
    }
    
    public virtual T2 GetRandomObjectFromPool()
    {
        return GetRandomObjectFromPool(this.ObjectPool, this.sumOfChances);
    }

    public virtual T2 GetDifferentRandomObjectFromPool(T2 previousObject)
    {
        if(this.ObjectPool.Count == 1)
        {
            return this.ObjectPool[0].objectToUse;
        }

        T2 differentObject = GetRandomObjectFromPool();
        int count = 0;
        while(differentObject.Equals(previousObject) && count < 100) 
        {
            differentObject = GetRandomObjectFromPool();
            count++;
        }

        if(count >= 100)
        {
            Debug.LogWarning("Object pool attempted to get a different object 100 times, think about improving performance.");
        }

        return differentObject;
    }
}
