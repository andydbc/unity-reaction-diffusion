using UnityEngine;

namespace ReactionDiffusion
{
    public class RDSettings : ScriptableObject
    {
        public float feed;
        public float kill;

        public float du;
        public float dv;
    }
}
