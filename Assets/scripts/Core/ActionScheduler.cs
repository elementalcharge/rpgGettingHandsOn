using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;
        public void StartAction(IAction action)
        {
            if (currentAction != null && currentAction.Equals( action) ) return;
            else if (currentAction != null)
            {
                currentAction.Cancel();
                print("Cancelling" + currentAction);
                
            }
            currentAction = action;
            print("current action is" + currentAction);
            /*else
            {
                //currentAction.cancel();
            }*/

        }

        public void CancelCurrentAction() {
            StartAction(null);
        }

    }
}
