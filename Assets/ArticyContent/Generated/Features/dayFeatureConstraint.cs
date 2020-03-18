//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Articy.Project_Of_Death;
using Articy.Unity;
using Articy.Unity.Constraints;
using Articy.Unity.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Articy.Project_Of_Death.Features
{
    
    
    public class dayFeatureConstraint
    {
        
        private Boolean mLoadedConstraints;
        
        private ReferenceSlotConstraint mDay_task_slot;
        
        private ReferenceSlotConstraint mDay_news_slot;
        
        private ReferenceSlotConstraint mDay_fate_slot;
        
        public ReferenceSlotConstraint day_task_slot
        {
            get
            {
                EnsureConstraints();
                return mDay_task_slot;
            }
        }
        
        public ReferenceSlotConstraint day_news_slot
        {
            get
            {
                EnsureConstraints();
                return mDay_news_slot;
            }
        }
        
        public ReferenceSlotConstraint day_fate_slot
        {
            get
            {
                EnsureConstraints();
                return mDay_fate_slot;
            }
        }
        
        public virtual void EnsureConstraints()
        {
            if ((mLoadedConstraints == true))
            {
                return;
            }
            mLoadedConstraints = true;
            mDay_task_slot = new Articy.Unity.Constraints.ReferenceSlotConstraint("Dialogue;DialogueFragment;FlowFragment;", "", "None;", "");
            mDay_news_slot = new Articy.Unity.Constraints.ReferenceSlotConstraint("FlowFragment;Dialogue;DialogueFragment;", "", "None;", "");
            mDay_fate_slot = new Articy.Unity.Constraints.ReferenceSlotConstraint("FlowFragment;Dialogue;DialogueFragment;", "", "None;", "");
        }
    }
}