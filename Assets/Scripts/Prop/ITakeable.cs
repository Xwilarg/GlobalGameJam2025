using UnityEngine;

namespace GGJ.Prop
{
    public interface ITakeable
    {
        public GameObject GameObject { get; }

        public bool CanBeSold { get; }
    }
}