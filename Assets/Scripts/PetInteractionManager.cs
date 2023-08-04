using System.Collections;
using UnityEngine;

enum PetState
{
    IDLE,
    EATING,
    LOVING,
    PLAYING,
}

public class PetInteractionManager : MonoBehaviour
{
    public Pet pet;
    public Animator animator;

    PetState state = PetState.IDLE;

    float stateTimerInterval = 3f;

    int baseMeterAmount = 50;

    public void FeedPet()
    {
        if(state == PetState.IDLE)
        {
            state = PetState.EATING;
            animator.SetBool("Eat", true);
            pet.AddFood(baseMeterAmount);
            StartCoroutine("StateTimer");
        }
    }

    public void PetPet()
    {
        if (state == PetState.IDLE)
        {
            state = PetState.LOVING;
            animator.SetBool("Pet", true);
            pet.AddLove(baseMeterAmount);
            StartCoroutine("StateTimer");
        }
    }

    public void PlayPet()
    {
        if (state == PetState.IDLE)
        {
            state = PetState.PLAYING;
            animator.SetBool("Play", true);
            pet.AddFun(baseMeterAmount);
            StartCoroutine("StateTimer");
        }
    }

    IEnumerator StateTimer()
    {
        yield return new WaitForSeconds(stateTimerInterval);
        ResetAnimationBools();
        state = PetState.IDLE;
    }

    void ResetAnimationBools()
    {
        animator.SetBool("Pet", false);
        animator.SetBool("Play", false);
        animator.SetBool("Eat", false);
    }
}
