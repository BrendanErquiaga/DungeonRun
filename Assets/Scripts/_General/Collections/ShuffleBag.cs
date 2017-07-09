using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShuffleBag<T>
{
	List<T> currentBag;
	List<T> originalBag;
	bool autoRefill;

	public bool BagEmpty
	{
		get
		{
			return (currentBag.Count == 0);
		}
	}
	
	public int CurrentCount
	{
		get
		{
			return currentBag.Count;
		}
	}

	public ShuffleBag(List<T> bag, bool autoRefill = true)
	{
        originalBag = new List<T>();
        currentBag = new List<T>();
        for (int i = 0; i < bag.Count; i++)
        {
            originalBag.Add(bag[i]);
        }
		Refill();
		this.autoRefill = autoRefill;
	}

	public T GetNextItemInBag()
	{
		if (currentBag.Count <= 0 && autoRefill)
			Refill();
		if (currentBag.Count > 0)
		{
			int index = Random.Range(0, currentBag.Count);
			T result = currentBag[index];
			currentBag.RemoveAt(index);
			return result;
		}
		else
            throw new System.ArgumentOutOfRangeException("currentBag", "Current bag is empty!");
			
	}

	/// <summary>
	/// Refills the bag with its original contents
	/// </summary>
	public void Refill()
	{
        for (int i = 0; i < originalBag.Count; i++)
        {
            currentBag.Add(originalBag[i]);
        }
    }

	/// <summary>
	/// Saves the current bag.
	/// </summary>
	public void SaveCurrentBag()
	{
		originalBag = currentBag;
	}
}
