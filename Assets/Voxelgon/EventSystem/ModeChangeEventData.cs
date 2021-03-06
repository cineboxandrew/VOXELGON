﻿using UnityEngine.EventSystems;
using Voxelgon;

namespace Voxelgon.EventSystems{
    public class ModeChangeEventData : BaseEventData {

        public GameMode.Mode newMode;
        public GameMode.Mode oldMode;
        public ModeChangeEventSource source;

        public ModeChangeEventData(EventSystem eSystem) : base(eSystem) {}

    }
}