using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Stride.Music.Theory;
using Stride.Utility;

namespace Stride.Music.Score
{
    public class ScoreBuilder
    {
        readonly StaffPositionComputation StaffPositionComputation;
        readonly BarsComputation BarsComputation;

        readonly Pitch LowestTreebleStaffPitch;

        public ScoreBuilder(
            StaffPositionComputation staffPositionComputation,
            BarsComputation barsComputation,
            Pitch lowestTreebleStaffPitch)
        {
            StaffPositionComputation = staffPositionComputation;
            BarsComputation = barsComputation;
            LowestTreebleStaffPitch = lowestTreebleStaffPitch;
        }

        public IEnumerable<BeatGroup> FromMelodicPhrase(IEnumerable<Note> phrase)
        {
            var pitches = phrase.Select(Note.GetPitch);
            var staffPositions = pitches.Select(p => StaffPositionComputation.ComputeStaffPosition(LowestTreebleStaffPitch, p));
            var beats = BarsComputation.SplitToBars(phrase);
            return phrase.EquiZip(staffPositions, beats,
                (note, staffPosition, beat) => 
                    new BeatGroup(beat, new ScoreNote(note.Duration, staffPosition, note.Pitch.PitchClass.Accidental).YieldReadOnlyList()));
        }

        public IEnumerable<BeatGroup> FromChordsPhrase(IEnumerable<(IEnumerable<Pitch> Chord, Duration Duration)> phrase)
        {
            var lowNotes = phrase.Select(chord => new Note(chord.Chord.First(), chord.Duration));
            var beats = BarsComputation.SplitToBars(lowNotes);
            return phrase.EquiZip(beats, (phraseEntry, beat) =>
                CreateBeatGroup(phraseEntry.Chord, phraseEntry.Duration, beat));
        }

        BeatGroup CreateBeatGroup(IEnumerable<Pitch> pitches, Duration duration, Beat beat)
        {
            var staffPositions = StaffPositionComputation.ComputeStaffPositionsHarmonic(LowestTreebleStaffPitch, pitches);
            var accidentals = pitches.Select(Pitch.GetAccidental);
            var scoreNotes = staffPositions
                .EquiZip(accidentals,
                    (staffPosition, accidental) => new ScoreNote(duration, staffPosition, accidental))
                .ToReadOnlyList();
            return new BeatGroup(beat, scoreNotes);
        }
    }
}
