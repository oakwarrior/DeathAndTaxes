using System.Collections.Generic;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Unity.Utils;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ArticyFlowPlayer))]
public class ArticyDebugFlowPlayer : MonoBehaviour, IArticyFlowPlayerCallbacks
{
    [Header("UI")]
    // a prefab used to instantiate a branch
    public GameObject branchPrefab;
    // the display name label, used to show the name of the current paused on node
    public Text displayNameLabel;
    // the main text label, used to show the text of the current paused on node
    public Text textLabel;

    // our 3 info labels in the middle of the panel, displaying basic information about the current pause.
    public Text typeLabel;
    public Text technicalNameLabel;
    public Text idLabel;

    // the ui target for our vertical list of branch buttons
    public RectTransform branchLayoutPanel;

    // the preview image ui element. A simple 64x64 image that will show the articy preview image or speaker, depending on the current pause.
    public Image previewImagePanel;

    [Header("Options")]
    // you can set this to true to see false branches in red, very helpful for debugging.
    public bool showFalseBranches = false;

    // the flow player component found on this game object
    private ArticyFlowPlayer flowPlayer;

    // Used to initializes our debug flow player handler.
    void Start()
    {
        // you could assign this via the inspector but this works equally well for our purpose.
        flowPlayer = GetComponent<ArticyFlowPlayer>();
        Debug.Assert(flowPlayer != null, "ArticyDebugFlowPlayer needs the ArticyFlowPlayer component!.");

        // by clearing at start we can safely have a prefab instantiated in the editor for our convenience and automatically get rid of it when we play.
        ClearAllBranches();

        // just a little reminder text to actually set a start on object, otherwise the Flowplayer won't do anything and just idles.
        if (flowPlayer != null && flowPlayer.StartOn == null)
            textLabel.text = "<color=green>No object selected in the flow player. Navigate to the ArticyflowPlayer and choose a StartOn node.</color>";
    }

    // This is called everytime the flow player reaches and object of interest.
    public void OnFlowPlayerPaused(IFlowObject aObject)
    {
        if (aObject != null)
        {
            typeLabel.text = aObject.GetType().Name;

            // reset, just in case we don't have any
            idLabel.text = string.Empty;
            technicalNameLabel.text = string.Empty;

            var articyObj = aObject as IArticyObject;
            if (articyObj != null)
            {
                idLabel.text = articyObj.Id.ToHex();
                technicalNameLabel.text = articyObj.TechnicalName;
            }
        }
        // To show the displayname in the ui of the current node
        var modelWithDisplayName = aObject as IObjectWithDisplayName;
        if (modelWithDisplayName != null)
            displayNameLabel.text = modelWithDisplayName.DisplayName;
        else
            displayNameLabel.text = string.Empty;

        // To show text in the ui of the current node
        // we just check if it has a text property by using the object property interfaces, if it has the property we use it to show the text in our main text label.
        var modelWithText = aObject as IObjectWithText;
        if (modelWithText != null)
            textLabel.text = modelWithText.Text;
        else
            textLabel.text = string.Empty;

        // this will make sure that we find a proper preview image to show in our ui.
        ExtractCurrentPausePreviewImage(aObject);
    }

    // called everytime the flow player encounteres multiple branches, or paused on a node and want to tell us how to continue.
    public void OnBranchesUpdated(IList<Branch> aBranches)
    {
        // we clear all old branch buttons
        ClearAllBranches();

        // for every branch provided by the flow player, we will create a button in our vertical list
        foreach (var branch in aBranches)
        {
            // if the branch is invalid, because a script evaluated to false, we don't create a button, unless we want to see false branches.
            if (!branch.IsValid && !showFalseBranches) continue;

            // we create a our button prefab and parent it to our vertical list
            var btn = Instantiate(branchPrefab);
            var rect = btn.GetComponent<RectTransform>();
            rect.SetParent(branchLayoutPanel, false);

            // here we make sure to get the Branch component from our button, either by referencing an already existing one, or by adding it.
            var branchBtn = btn.GetComponent<ArticyDebugBranch>();
            if (branchBtn == null)
                branchBtn = btn.AddComponent<ArticyDebugBranch>();

            // this will assign the flowplayer and branch and will create a proper label for the button.
            branchBtn.AssignBranch(flowPlayer, branch);
        }
    }

    // convenience method to clear everything underneath our branch layout panel, this should only be our dynamically created branch buttons.
    private void ClearAllBranches()
    {
        foreach (Transform child in branchLayoutPanel)
            Destroy(child.gameObject);
    }

    // method to find a preview image to show in the ui.
    private void ExtractCurrentPausePreviewImage(IFlowObject aObject)
    {
        IAsset articyAsset = null;

        previewImagePanel.sprite = null;

        // to figure out which asset we could show in our preview, we first try to see if it is an object with a speaker
        var dlgSpeaker = aObject as IObjectWithSpeaker;
        if (dlgSpeaker != null)
        {
            // if we have a speaker, we extract it, because now we have to check if it has a preview image.
            ArticyObject speaker = dlgSpeaker.Speaker;
            if (speaker != null)
            {
                var speakerWithPreviewImage = speaker as IObjectWithPreviewImage;
                if (speakerWithPreviewImage != null)
                {
                    // our speaker has the property for preview image and we assign it to our asset.
                    articyAsset = speakerWithPreviewImage.PreviewImage.Asset;
                }
            }
        }

        // if we have no asset until now, we could try to check if the target itself has a preview image.
        if (articyAsset == null)
        {
            var objectWithPreviewImage = aObject as IObjectWithPreviewImage;
            if (objectWithPreviewImage != null)
            {
                articyAsset = objectWithPreviewImage.PreviewImage.Asset;
            }
        }

        // and if we have an asset at this point, we load it as a sprite and show it in our ui image.
        if (articyAsset != null)
        {
            previewImagePanel.sprite = articyAsset.LoadAssetAsSprite();
        }
    }

    public void CopyTargetLabel(BaseEventData aData)
    {
        var pointerData = aData as PointerEventData;
        if (pointerData != null)
            GUIUtility.systemCopyBuffer = pointerData.pointerPress.GetComponent<Text>().text;
        Debug.LogFormat("Copied text \"{0}\" into clipboard!", GUIUtility.systemCopyBuffer);
    }
}
