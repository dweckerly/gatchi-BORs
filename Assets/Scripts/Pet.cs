using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PetAttributes
{
    BULK,
    GAUNT,
    TRUSTING,
    SUSPICIOUS,
    CAREFREE,
    SERIOUS,
    IMPISH,
    POLITE,
    HUMBLE,
    PROUD,
    BOLD,
    ANXIOUS
}

public class Pet : MonoBehaviour
{
    // want some kind of list with max size that can hold attributes where the value is > 10
    // maybe an LRU check to keep up with the most resent ones updated?

    Dictionary<PetAttributes, int> attributes = new()
    {
        { PetAttributes.BULK, 0 },
        { PetAttributes.GAUNT, 0 },
        { PetAttributes.TRUSTING, 0 },
        { PetAttributes.SUSPICIOUS, 0 },
        { PetAttributes.CAREFREE, 0 },
        { PetAttributes.SERIOUS, 0 },
        { PetAttributes.IMPISH, 0 },
        { PetAttributes.POLITE, 0 },
        { PetAttributes.HUMBLE, 0 },
        { PetAttributes.PROUD, 0 },
        { PetAttributes.BOLD, 0 },
        { PetAttributes.ANXIOUS, 0 }
    };
    int attributeMax = 100;

    public PetMeterManager pmm;

    int meterMax = 296;
    float foodCurrent;
    float loveCurrent;
    float funCurrent;

    float hungerLossRate = 0.001f;
    float loveLossRate = 0.001f;
    float funLossRate = 0.001f;

    float meterLossInterval = 0.1f;
    float attributeCheckinterval = 1f;

    int mutations = 3;
    int mutationAmount = 10;

    void Start()
    {
        // randomly enhance some attributes at the start
        int attrSize = attributes.Count;
        for(int i = 0; i < mutations; i++)
        {
            int rand = Random.Range(0, attrSize);
            PetAttributes key = GetAttrKey(rand);
            attributes[key] += Random.Range(1, mutationAmount);
        }

        foodCurrent = meterMax / 2;
        loveCurrent = meterMax / 2;
        funCurrent = meterMax / 2;

        pmm.UpdateFoodDisplay(foodCurrent);
        pmm.UpdateLoveDisplay(loveCurrent);
        pmm.UpdateFunDisplay(funCurrent);

        StartCoroutine("HungerLoss");
        StartCoroutine("LoveLoss");
        StartCoroutine("FunLoss");
        StartCoroutine("AttributeCheck");

    }

    IEnumerator HungerLoss()
    {
        while(foodCurrent > 0)
        {
            foodCurrent -= MeterLoss(hungerLossRate);
            if (foodCurrent < 0)
                foodCurrent = 0;
            // Update UI
            pmm.UpdateFoodDisplay(foodCurrent);
            yield return new WaitForSeconds(meterLossInterval);
        }
    }

    IEnumerator LoveLoss()
    {
        while (loveCurrent > 0)
        {
            loveCurrent -= MeterLoss(loveLossRate);
            if(loveCurrent < 0)
                loveCurrent = 0;
            // Update UI
            pmm.UpdateLoveDisplay(loveCurrent);
            yield return new WaitForSeconds(meterLossInterval);
        }
    }

    IEnumerator FunLoss()
    {
        while (funCurrent > 0)
        {
            funCurrent -= MeterLoss(funLossRate);
            if (funCurrent < 0)
                funCurrent = 0;
            // Update UI
            pmm.UpdateFunDisplay(funCurrent);
            yield return new WaitForSeconds(meterLossInterval);
        }
    }

    public void AddFood(int amt)
    {
        foodCurrent += amt;
        if (foodCurrent > meterMax)
            foodCurrent = meterMax;
        pmm.UpdateFoodDisplay(foodCurrent);
    }

    public void AddLove(int amt)
    {
        loveCurrent += amt;
        if (loveCurrent > meterMax)
            loveCurrent = meterMax;
        pmm.UpdateLoveDisplay(loveCurrent);
    }

    public void AddFun(int amt)
    {
        funCurrent += amt;
        if (funCurrent > meterMax)
            funCurrent = meterMax;
        pmm.UpdateFunDisplay(funCurrent);
    }

    float MeterLoss(float rate)
    {
        return meterMax * rate;
    }

    List<PetAttributes> GetMaxAttrs()
    {
        PetAttributes maxAttr = new PetAttributes();
        int maxAttrAmount = int.MinValue;

        // find max value set to return key
        foreach(PetAttributes key in attributes.Keys)
        {
            if(attributes[key] > maxAttrAmount)
            {
                maxAttr = key;
            }
        }

        // find tied amounts and add them to the list (if max attr is > 0)
        List<PetAttributes> maxAttrList = new List<PetAttributes>();
        if (maxAttr > 0)
        {
            maxAttrList.Add(maxAttr);
            foreach (PetAttributes key in attributes.Keys)
            {
                if (attributes[key] == maxAttrAmount && !maxAttrList.Contains(key))
                {
                    maxAttrList.Add(key);
                }
            }
        }
        return maxAttrList;
    }

    PetAttributes GetAttrKey(int rand)
    {
        return attributes.Keys.ElementAt(rand);
    }

    void ApplyMaxAttrRates(List<PetAttributes> attrs)
    {
        if(attrs.Count > 0)
        {
            foreach (PetAttributes key in attrs)
            {
                if (key == PetAttributes.BULK)
                {

                }
                else if (key == PetAttributes.GAUNT)
                {

                }
            }
        }
    }

    float MeterPercent(float amt)
    {
        return (amt / meterMax) * 100;
    }

    IEnumerator AttributeCheck()
    {
        while (true)
        {
            if (MeterPercent(foodCurrent) >= 75f)
            {
                attributes[PetAttributes.BULK]++;
                attributes[PetAttributes.GAUNT]--;
                if (attributes[PetAttributes.GAUNT] < 0)
                    attributes[PetAttributes.GAUNT] = 0;
            }
            if (MeterPercent(foodCurrent) <= 25f)
            {
                attributes[PetAttributes.GAUNT]++;
                attributes[PetAttributes.BULK]--;
                if (attributes[PetAttributes.BULK] < 0)
                    attributes[PetAttributes.BULK] = 0;
            }
            if (MeterPercent(loveCurrent) >= 75f)
            {
                attributes[PetAttributes.TRUSTING]++;
                attributes[PetAttributes.SUSPICIOUS]--;
                if (attributes[PetAttributes.SUSPICIOUS] < 0)
                    attributes[PetAttributes.SUSPICIOUS] = 0;
            }
            if (MeterPercent(loveCurrent) <= 25f)
            {
                attributes[PetAttributes.SUSPICIOUS]++;
                attributes[PetAttributes.TRUSTING]--;
                if (attributes[PetAttributes.TRUSTING] < 0)
                    attributes[PetAttributes.TRUSTING] = 0;
            }
            if (MeterPercent(funCurrent) <= 75f)
            {
                attributes[PetAttributes.CAREFREE]++;
                attributes[PetAttributes.SERIOUS]--;
                if (attributes[PetAttributes.SERIOUS] < 0)
                    attributes[PetAttributes.SERIOUS] = 0;
            }
            if (MeterPercent(funCurrent) <= 25f)
            {
                attributes[PetAttributes.SERIOUS]++;
                attributes[PetAttributes.CAREFREE]--;
                if (attributes[PetAttributes.CAREFREE] < 0)
                    attributes[PetAttributes.CAREFREE] = 0;
            }
            if (MeterPercent(funCurrent) >= 70f && MeterPercent(loveCurrent) <= 50f)
            {
                attributes[PetAttributes.IMPISH]++;
                attributes[PetAttributes.POLITE]--;
                if (attributes[PetAttributes.POLITE] < 0)
                    attributes[PetAttributes.POLITE] = 0;
            }
            if (MeterPercent(loveCurrent) >= 70f && MeterPercent(funCurrent) <= 50f)
            {
                attributes[PetAttributes.POLITE]++;
                attributes[PetAttributes.IMPISH]--;
                if (attributes[PetAttributes.IMPISH] < 0)
                    attributes[PetAttributes.IMPISH] = 0;
            }
            if (MeterPercent(foodCurrent) >= 70f && MeterPercent(loveCurrent) <= 50f)
            {
                attributes[PetAttributes.PROUD]++;
            }
            if (MeterPercent(loveCurrent) >= 70f && MeterPercent(foodCurrent) <= 50f)
            {
                attributes[PetAttributes.HUMBLE]++;
            }
            if(MeterPercent(loveCurrent) >= 50f && MeterPercent(funCurrent) >= 50f)
            {
                attributes[PetAttributes.PROUD]--;
                if (attributes[PetAttributes.PROUD] < 0)
                    attributes[PetAttributes.PROUD] = 0;
            }
            if (MeterPercent(foodCurrent) >= 50f && MeterPercent(funCurrent) >= 50f)
            {
                attributes[PetAttributes.HUMBLE]--;
                if (attributes[PetAttributes.HUMBLE] < 0)
                    attributes[PetAttributes.HUMBLE] = 0;
            }
            if (MeterPercent(loveCurrent) >= 70f && MeterPercent(funCurrent) >= 70f)
            {
                attributes[PetAttributes.BOLD]++;
                attributes[PetAttributes.ANXIOUS]--;
                if (attributes[PetAttributes.ANXIOUS] < 0)
                    attributes[PetAttributes.ANXIOUS] = 0;
            }
            if (MeterPercent(loveCurrent) <= 50f && MeterPercent(foodCurrent) <= 50f)
            {
                attributes[PetAttributes.BOLD]--;
                if (attributes[PetAttributes.BOLD] < 0)
                    attributes[PetAttributes.BOLD] = 0;
                attributes[PetAttributes.ANXIOUS]++;
            }
            yield return new WaitForSeconds(attributeCheckinterval);
        }
    }
}
