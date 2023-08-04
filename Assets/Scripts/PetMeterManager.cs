using UnityEngine;
using UnityEngine.UI;

public class PetMeterManager : MonoBehaviour
{
    public Image foodBar;
    public Image loveBar;
    public Image funBar;
    
    public void UpdateFoodDisplay(float amt)
    {
        foodBar.rectTransform.sizeDelta = new Vector2(amt, foodBar.rectTransform.sizeDelta.y);
    }

    public void UpdateLoveDisplay(float amt)
    {
        loveBar.rectTransform.sizeDelta = new Vector2(amt, loveBar.rectTransform.sizeDelta.y);
    }

    public void UpdateFunDisplay(float amt)
    {
        funBar.rectTransform.sizeDelta = new Vector2(amt, funBar.rectTransform.sizeDelta.y);
    }
}
