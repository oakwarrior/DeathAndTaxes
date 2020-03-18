using Articy.Unity;
using Articy.Unity.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class ArticyDebugBranch : MonoBehaviour
{
    // the unity ui button text, so we can assign in code different labels.
    private Text dialogText;
    // the branch identifier, so we can tell the processor which way it should continue to traverse our flow when the user clicked this button
    private Branch branch;
    // the processor itself.
    private ArticyFlowPlayer processor;

    /// Called when the button is created to represent a single branch out of possible many. This is important to give the ui button the branch that is used to follow along if the user pressed the button in the ui
    public void AssignBranch(ArticyFlowPlayer aProcessor, Branch aBranch)
    {
        // You would usually do this in the inspector in the button itself, but its so important for the correct functionalty
        // we placed it here to show you what happened when the button is pressed by the user.
        GetComponentInChildren<Button>().onClick.AddListener(OnBranchSelected);

        // we find the text component in our children, this should be the label of the button, unless you changed the button somewhat, then you need to take care of selecting the proper text.
        dialogText = GetComponentInChildren<Text>();

        // store for later use
        branch = aBranch;
        processor = aProcessor;

        // a nice debug aid, if we show all branches (valid or invalid) we can identify branches that shouldn't be allowed because of our scripts
        dialogText.color = aBranch.IsValid ? Color.black : Color.red;

        var target = aBranch.Target;
        dialogText.text = "";

        // now we figure out which text our button should have, and we just try to cast our target into different types, 
        // creating some sort of priority naming  MenuText -> DisplayName -> TechnicalName -> ClassName/Null

        var obj = target as IObjectWithMenuText;

        if (obj != null)
        {
            dialogText.text = obj.MenuText;

            // Empty? Usually it would have a menu text, but it was deliberately left empty, in a normal game this could mean a single branch to just continue the dialog, if the protagonist is talking for
            // example, how you handle this is up to you, for this we just use the text normal text to show.
            if (dialogText.text == "")
            {
                var txtObj = obj as IObjectWithText;
                if (txtObj != null)
                    dialogText.text = txtObj.Text;
                else
                    dialogText.text = "...";
            }
        }

        // if the text is still empty, we can show the displayname of the target
        if (dialogText.text == "")
        {
            var dspObj = target as IObjectWithDisplayName;
            if (dspObj != null)
                dialogText.text = dspObj.DisplayName;
            else
            {
                // if it is still empty, we just show the technical name
                var articyObject = target as IArticyObject;
                if (articyObject != null)
                    dialogText.text = articyObject.TechnicalName;
                else
                {
                    // if for some reason the object cannot be cast to a basic articy type, we show its class name or null.
                    dialogText.text = target == null ? "null" : target.GetType().Name;
                }
            }
        }
    }

    // the method used when the button is clicked
    public void OnBranchSelected()
    {
        // by giving the processor the branch assigned to the button on creation, the processor knows where to continue the flow
        processor.Play(branch);
    }
}
