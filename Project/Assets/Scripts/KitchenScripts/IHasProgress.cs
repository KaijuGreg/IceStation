using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress {

    public event EventHandler<OnProgressChangedEventsArgs> OnProgressChanged; // this is to notify the cutting progress bar
    public class OnProgressChangedEventsArgs : EventArgs {
        public float progressNormalized;
    }


}
