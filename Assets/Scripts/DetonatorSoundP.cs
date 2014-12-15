using UnityEngine;
using System.Collections;

public class DetonatorSoundP : DetonatorSound {

    // need to call Init from Awake to use this component without Explode On Start setting.
    void Awake()
    {
        Init();
    }

}
