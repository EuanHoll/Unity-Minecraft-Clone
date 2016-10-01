using UnityEngine;
using System.Collections;
using System;

public class BlockCulling : MonoBehaviour {

    int secondCounter = 0;
    bool isVanished;
    bool hasBeenUpdated = false;
    bool timePassed = false;

	// Use this for initialization
	void Start () {
        isVanished = false;
        StartCoroutine(updateStatus());
        InvokeRepeating("counter", 1f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
        hasBeenUpdated = false;
    }

    public void updateBlock(bool isVanish)
    {
        bool isVisible = isVanish;
        if (isVisible == false && isVanished == false)
        {
            vanishBlock();
            isVanished = true;
        }
        else if (isVisible == true && isVanished == true)
        {
            unVanishBlock();
            isVanished = false;
            setCounterZero();
            timePassed = true;
        } else if( isVisible == true && isVanished == false)
        {
            setCounterZero();
        }
        hasBeenUpdated = true;
    }

    private void vanishBlock()
    {
        gameObject.transform.GetComponent<MeshRenderer>().enabled = false;
    }

    private void unVanishBlock()
    {
        gameObject.transform.GetComponent<MeshRenderer>().enabled = true;
    }

    IEnumerator updateStatus()
    {
        while (true)
        {
            if(!timePassed)
            updateBlockFalse();
            yield return new WaitForSeconds(5f);
        }
    }

    void updateBlockFalse()
    {
        updateBlock(false);
    }

    void counter()
    {
        if(secondCounter > 5)
        {
            timePassed = false;
            secondCounter = 0;
        }
        secondCounter += 1;
    }

    void setCounterZero()
    {
        secondCounter = 0;
    }
}
