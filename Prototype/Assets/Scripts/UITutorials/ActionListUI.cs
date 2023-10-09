using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionListUI : MonoBehaviour
{
    // The list of available actions the player can choose
    public ActionList actionList;
    // Reference to a prefab UI object which will be cloned as a child of this object
    public ActionUI prefab;

    // Player object
    Player player;

    //private int counter = 0;

    // A list of the cloned prefab ActionUI's we've created 
    List<ActionUI> uis = new List<ActionUI>();

    // Variables for the components of the object to which this ActionListUI is attached
    LayoutGroup layoutGroup;
    ContentSizeFitter contentSizeFitter;


    // Start is called before the first frame update
    void Start()
    {
        // Set the variables for the components
        layoutGroup = GetComponent<LayoutGroup>();
        contentSizeFitter = GetComponent<ContentSizeFitter>();

        // Update once on startup
        StartCoroutine(UpdateUI());

        // Subscribe the ActionList of this ActionListUI to the event that will notify the UI of changes to the underlying ActionList
        actionList.onChanged.AddListener(() => { StartCoroutine(UpdateUI()); });
    }

    // Perform a start function that returns an iterator over the Actions of the Player
    IEnumerator UpdateUI()
    {
        // Turn on the components
        contentSizeFitter.enabled = true;
        layoutGroup.enabled = true;

        // Wait until the end of the frame and then continue
        yield return new WaitForEndOfFrame();

        // Set the Player object equal to the one we've attached to this ActionListUI
        player = actionList.GetComponent<Player>();

        // 1: Step through the List of cloned prefabs and assign Actions to each element and turn off any spare ones
        // For as many times as we have Action components *on the Player* ...
        for (int i = 0; i < actionList.actions.Length; i++)
        {
            // 1.a: Identify any Actions without UIs
            // If there are Actions that don't have UIs...
            if (i >= uis.Count)
            {
                // 1.a.i: Create an extra UI
                uis.Add(Instantiate(prefab, transform));

                // 1.a.ii: Give the UI the reference to the player so it can display the Player's nth Action particulars
                uis[i].Init(player);
            }

            // 1.b: Make each ActionUI active
            uis[i].gameObject.SetActive(true);

            // 1.c: Make the variables of each ActionUI the same as those of the Action in the same numerical position
            uis[i].SetAction(actionList.actions[i]);

            // 1.d: Put the ActionUIs in order (every time we iterate, make the current iteration the last - the parent LayoutGroup will order the transforms)
            uis[i].transform.SetAsLastSibling();
        }

        // 2: Disable any remaining UIs without Actions
        for (int i = actionList.actions.Length; i < uis.Count; i++)
        {
            uis[i].gameObject.SetActive(false);
        }

        // yield return only returns those items required / calculated by a function, so that function will exit early if it would otherwise calculate more than needed.
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        // Turn off the content size fitter component
        contentSizeFitter.enabled = false;

        // Turn off the layout group component
        layoutGroup.enabled = false;
    }
}
