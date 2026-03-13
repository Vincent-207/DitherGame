using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableHandler : MonoBehaviour
{
    [SerializeField] float interactionRange;
    bool isInteracting = false;
    [SerializeField] InputActionReference interact;
    [SerializeField] AmountBar amountBar;
    void Start()
    {
        amountBar.Hide();
    }
    void Update()
    {
        Interactable interactable;
        if(isPlayerLookingAtInteractable(out interactable))
        {
            HandleInteractable(interactable);
        }
    }

    void HandleInteractable(Interactable interactable)
    {
        if(!isInteracting && interact.action.IsPressed() && interactable.enabled)
        {
            StartInteraction(interactable);
        }
    }
    void StartInteraction(Interactable interactable)
    {
        StartCoroutine(Interaction(interactable.Duration, interactable));
    }
    bool isPlayerLookingAtInteractable()
    {
        Interactable interactable;
        return isPlayerLookingAtInteractable(out interactable);
    }
    bool isPlayerLookingAtInteractable(out Interactable interactable)
    {
        RaycastHit hit;
        interactable = null;
        if(Physics.Raycast(transform.position, transform.forward, out hit, interactionRange))
        {
            interactable = hit.collider.GetComponent<Interactable>();
            if(interactable != null)
            {
                return true;
            }
        }

        return false;
    }

    IEnumerator Interaction(float duration, Interactable interactable)
    {
        float elapsedTime = 0;
        SetInteractionState(true);

        while(elapsedTime < duration)
        {
            if(interact.action.IsPressed() && isInteracting && interactable.enabled)
            {
                elapsedTime += Time.deltaTime;
                amountBar.SetProportion(elapsedTime / duration);
                yield return null;
            }
            else
            {
                SetInteractionState(false);
                yield break;
            }
        }

        interactable.DoInteract();
        SetInteractionState(false);
    }


    void SetInteractionState(bool input)
    {
        isInteracting = input;
        if(isInteracting == false) amountBar.Hide();
        else amountBar.Show();
    }
}
