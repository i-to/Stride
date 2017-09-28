﻿using Stride.Gui.Music;

namespace Stride.Gui.Input
{
    public class NoteInput
    {
        readonly DrillViewModel DrillViewModel;

        public NoteInput(DrillViewModel drillViewModel)
        {
            DrillViewModel = drillViewModel;
        }

        public void NoteOn(Pitch pitch) => DrillViewModel.UpdatePlayedPitch(pitch);
        public void NoteOff(Pitch pitch) => DrillViewModel.UpdatePlayedPitch(null);
    }
}