//Copyright 2020 Placeholder Gameworks
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
//and associated documentation files (the "Software"), to deal in the Software without restriction, 
//including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
//and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
//subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
//PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
//FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
//OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.
using Articy.Project_Of_Death.GlobalVariables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EPaperworkMarkType
{
    Live,
    Die,
    Unmarked
}

public class PaperworkMark : MonoBehaviour, Interactable
{
    [SerializeField]
    Paperwork PaperworkParent;

    [SerializeField]
    EPaperworkMarkType MarkType;

    [SerializeField]
    Image ImageMark;

    [SerializeField]
    Collider2D ColliderMark;

    public void Hover()
    {
        PaperworkParent.HoverMark(MarkType);
    }

    public void UpdateDragGrabPosition(Vector3 position)
    {

    }

    public bool CanDrag()
    {
        return false;
    }

    public bool IsDragging()
    {
        return false;
    }

    public void ToggleDragging(bool drag)
    {

    }

    public void Unhover()
    {
        PaperworkParent.UnHoverMark();
    }

    public void ToggleMarkIcon(bool visible)
    {
        ImageMark.enabled = visible;
    }

    public string GetHoverText()
    {
        if (PaperworkParent.IsMarked())
        {
            if (Eraser.instance.IsPickedUp())
            {
                switch (MarkType)
                {
                    case EPaperworkMarkType.Live:
                        if (ImageMark.enabled)
                        {
                            return PaperworkParent.TextName.text + ", SORRY SORRY SORRY.";
                        }
                        else
                        {
                            return "I could erase the mark and change my mind...";
                        }
                    case EPaperworkMarkType.Die:
                        if (ImageMark.enabled)
                        {
                            return PaperworkParent.TextName.text + ", I'm not done with you yet.";
                        }
                        else
                        {
                            return "I could erase the mark and change my mind...";
                        }
                }
                return "You should not be seeing this hover text! ERROR!";
            }
            else if (Eraser.instance.gameObject.activeSelf)
            {
                if (ImageMark.enabled)
                {
                    return "I could erase this if I wanted to...";
                }
                else
                {
                    return "I could erase the mark and change my mind...";
                }
            }
            else
            {
                return "No way back now. Unless I find something to erase this with...";
            }
        }

        if (MarkerOfDeath.instance.IsPickedUp())
        {
            switch (MarkType)
            {
                case EPaperworkMarkType.Live:
                    if (PaperworkParent.GetProfile() == GameManager.instance.GrimReaperPaperworkProfile)
                    {
                        return "?????";
                    }
                    else if (PaperworkParent.GetProfile() == GameManager.instance.FatePaperworkProfile)
                    {
                        return "After all this time?";
                    }
                    else
                    {
                        return PaperworkParent.TextName.text + ", you will live.";
                    }
                case EPaperworkMarkType.Die:
                    if (PaperworkParent.GetProfile() == GameManager.instance.GrimReaperPaperworkProfile)
                    {
                        return "?!?!?";
                    }
                    else if (PaperworkParent.GetProfile() == GameManager.instance.FatePaperworkProfile)
                    {
                        return "It would take such a little amount of effort...";
                    }
                    else
                    {
                        return PaperworkParent.TextName.text + ", you will die.";
                    }
            }
        }
        else if (Eraser.instance.IsPickedUp())
        {
            return "Nothing to erase here!";
        }
        else
        {
            if (PaperworkParent.Status == EPaperworkStatus.FOCUS)
            {
                return "I need to pick up the Marker of Death first...";
            }
            else
            {
                return PaperworkParent.GetHoverText();
            }
            //switch (MarkType)
            //{
            //    case EPaperworkMarkType.Live:
            //        return PaperworkParent.TextName.text + ", you will live.";
            //    case EPaperworkMarkType.Die:
            //        return PaperworkParent.TextName.text + ", you will die.";
            //}
        }

        return "";
    }

    public void Interact()
    {
        if (PaperworkParent.Status != EPaperworkStatus.FOCUS || PaperworkParent.IsMarked() && !Eraser.instance.IsPickedUp())
        {
            PaperworkParent.Interact();
            return;
        }

        if (MarkerOfDeath.instance.IsPickedUp())
        {
            switch (MarkType)
            {
                case EPaperworkMarkType.Live:
                {
                    if (SaveManager.instance.CurrentOptions.SkipMarkPopUp)
                    {
                        PaperworkParent.MarkPaperworkLive();
                        ConfirmMark();
                    }
                    else
                    {
                        MarkConfirm.instance.Show(PaperworkParent, EPaperworkMarkType.Live, this);

                    }

                    break;
                }
                case EPaperworkMarkType.Die:
                {
                    if (SaveManager.instance.CurrentOptions.SkipMarkPopUp)
                    {
                        PaperworkParent.MarkPaperworkDie();
                        ConfirmMark();
                    }
                    else
                    {
                        MarkConfirm.instance.Show(PaperworkParent, EPaperworkMarkType.Die, this);

                    }

                    break;
                }
            }
        }
        else if (Eraser.instance.IsPickedUp() && MarkType == PaperworkParent.MarkStatus)
        {
            EraseMark();
        }
        else
        {
            PaperworkParent.Interact();
        }

    }

    public void ConfirmMark()
    {
        switch (MarkType)
        {
            case EPaperworkMarkType.Live:
                FaxMachine.instance.NotifySpare();
                AudioManager.instance.PlayOneShotEffect(AudioManager.instance.MarkSpare);
                DesktopManager.instance.ApplyParameterConsequence(PaperworkParent.GetProfile().Template.profile_spare_data);
                if (PaperworkParent.bIsCreditsProfile)
                {
                    CreditsController.instance.AddSparedDev(PaperworkParent.GetProfile());
                }
                SaveManager.instance.GetCurrentCarryoverPlayerState().AddSparedProfile(PaperworkParent.GetProfile().Id);
                break;
            case EPaperworkMarkType.Die:
                FaxMachine.instance.NotifyDeath();
                AudioManager.instance.PlayOneShotEffect(AudioManager.instance.MarkDeath);
                DesktopManager.instance.ApplyParameterConsequence(PaperworkParent.GetProfile().Template.profile_death_data);
                if (PaperworkParent.bIsCreditsProfile)
                {
                    CreditsController.instance.AddDoomedDev(PaperworkParent.GetProfile());
                }
                if(PaperworkParent.GetProfile() == GameManager.instance.FatePaperworkProfile)
                {
                    AudioManager.instance.PlayOneShotEffect(FaxMachine.instance.ClipKilledFate);
                }
                SaveManager.instance.GetCurrentCarryoverPlayerState().AddDoomedProfile(PaperworkParent.GetProfile().Id);
                break;
        }
        StartCoroutine(FadeInMark());
        

        if (PaperworkParent.bIsCreditsProfile)
        {
            PaperworkParent.StartFadeOutParticle();
            CreditsController.instance.SpawnCreditsPaperwork();
            return;
        }


        ElevatorManager.instance.ResetCurrentSceneTime();
        SaveManager.instance.GetCurrentPlayerState().SetProfileMarkStatusByID(PaperworkParent.GetProfile().Id, MarkType);
        SaveManager.instance.MarkSavegameDirty();
    }

    public IEnumerator FadeInMark()
    {
        ImageMark.enabled = true;
        float elapsedTime = 0;
        float dur = 1.5f;
        float alpha = 0.0f;
        Color whiteNope = Color.white;
        whiteNope.a = 0.0f;

        while (elapsedTime < dur)
        {
            elapsedTime += Time.deltaTime;

            alpha = elapsedTime / dur;
            ImageMark.material.SetFloat("_Level", 1.0f - alpha);

            yield return null;
        }
    }

    public IEnumerator FadeOutMark()
    {
        float elapsedTime = 0;
        float dur = 1.5f;
        float alpha = 0.0f;
        Color whiteNope = Color.white;
        whiteNope.a = 0.0f;

        while (elapsedTime < dur)
        {
            elapsedTime += Time.deltaTime;

            alpha = elapsedTime / dur;
            ImageMark.material.SetFloat("_Level", alpha);

            yield return null;
        }

        ImageMark.enabled = false;
    }

    public void EraseMark()
    {
        if (PaperworkParent.MarkStatus == EPaperworkMarkType.Unmarked)
        {
            return;
        }
        if (PaperworkParent.GetProfile() == GameManager.instance.FatePaperworkProfile)
        {
            ArticyGlobalVariables.Default.profile.fate_spared = false;
        }

        switch (MarkType)
        {
            case EPaperworkMarkType.Live:
                FaxMachine.instance.NotifyEraseSpare();
                DesktopManager.instance.ReverseParameterConsequence(PaperworkParent.GetProfile().Template.profile_spare_data);
                break;
            case EPaperworkMarkType.Die:
                FaxMachine.instance.NotifyEraseDeath();
                DesktopManager.instance.ReverseParameterConsequence(PaperworkParent.GetProfile().Template.profile_death_data);
                break;
        }

        Eraser.instance.HandleUsed();

        SaveManager.instance.GetCurrentCarryoverPlayerState().AmountOfEraserUses++;

        StartCoroutine(FadeOutMark());

        PaperworkParent.MarkStatus = EPaperworkMarkType.Unmarked;
        SaveManager.instance.GetCurrentPlayerState().SetProfileMarkStatusByID(PaperworkParent.GetProfile().Id, EPaperworkMarkType.Unmarked);
        SaveManager.instance.MarkSavegameDirty();
    }

    public void NotifyFocused()
    {
        //ColliderMark.enabled = true;
    }

    public void NotifyUnFocused()
    {
        //ColliderMark.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PaperworkParent.MarkStatus == MarkType)
        {
            ImageMark.enabled = true;
        }
        else
        {
            ImageMark.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
