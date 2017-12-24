using System;
using System.Collections.Generic;
using System.Windows.Input;
using Stride.Music.Theory;

namespace Stride.Gui.Input
{
    public class KeyboardPitchMappings
    {
        public readonly IReadOnlyDictionary<Key, Pitch> Named =
            new Dictionary<Key, Pitch>
            {
                {Key.A, Pitch.A4},
                {Key.B, Pitch.B4},
                {Key.C, Pitch.C5},
                {Key.D, Pitch.D5},
                {Key.E, Pitch.E5},
                {Key.F, Pitch.F5},
                {Key.G, Pitch.G5}
            };

        public readonly IReadOnlyDictionary<Key, Pitch> LinearDiatonic =
            new Dictionary<Key, Pitch>
            {
                {Key.A, Pitch.C5},
                {Key.S, Pitch.D5},
                {Key.D, Pitch.E5},
                {Key.F, Pitch.F5},
                {Key.J, Pitch.G5},
                {Key.K, Pitch.A5},
                {Key.L, Pitch.B5},
                {Key.OemSemicolon, Pitch.C6}
            };

        public IReadOnlyDictionary<Key, Pitch> this[NoteInputMode mode]
        {
            get
            {
                if (mode == NoteInputMode.KeyboardNamed)
                    return Named;
                if (mode == NoteInputMode.KeyboardLinearDiatonic)
                    return LinearDiatonic;
                throw new ArgumentOutOfRangeException($"Keyboard mode expected, but given: {mode}");
            }
        }
    }
}