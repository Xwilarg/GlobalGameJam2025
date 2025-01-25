using UnityEngine;

namespace GGJ.Prop
{
    public interface ITakeable
    {
        public GameObject GameObject { get; }

        public bool CanBeSold { get; }
        public bool CanCut { get; }
        public bool CanWater { get; }
        public bool CanPlant { get; }
        public bool IsSwitchAllowed { get; }

        public bool IsReserved { set; get; }
    }
}