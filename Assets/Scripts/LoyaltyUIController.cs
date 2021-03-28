using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoyaltyUIController : MonoBehaviour
{
    public GameObject[] loyaltyIcons;
    int lastElementIndex;
    public GameObject loyaltyIconPrefab;
    public Vector3 loyaltyIconOffset;
    private int newLoyaltyIconIndex;
    public int visibleSize;
    // Start is called before the first frame update
    void Start()
    {
        lastElementIndex = visibleSize - 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void removeLoyaltyIcon()
    {
        loyaltyIcons[lastElementIndex].SetActive(false);
        lastElementIndex--;
    }

    public void deleteAllLoyaltyIcons()
    {
        for (int i = 0; i < loyaltyIcons.Length; i++)
        {
            Destroy(loyaltyIcons[i]);
        }
    }
    public void addLoyaltyIcon()
    {
        visibleSize++;
        lastElementIndex = visibleSize - 1;
        for(int i=0; i<visibleSize; i++)
        {
            loyaltyIcons[i].SetActive(true);
        }
    }
}
