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

        public IReadOnlyDictionary<Beat, BeatGroup> FromMelodicPhrase(IEnumerable<Note> phrase) => 
            MelodicPhraseToBeatGroups(phrase).ToReadOnlyDictionary();

        public IReadOnlyDictionary<Beat, BeatGroup> FromChordsPhrase(
            IEnumerable<(IEnumerable<Pitch> Chord, Duration Duration)> phrase)
            => ChordsPhraseToBeatGroups(phrase).ToReadOnlyDictionary();

        IEnumerable<(Beat, BeatGroup)> MelodicPhraseToBeatGroups(IEnumerable<Note> phrase)
        {
            var pitches = phrase.Select(Note.GetPitch);
            var staffPositions = pitches.Select(p => StaffPositionComputation.ComputeStaffPosition(LowestTreebleStaffPitch, p));
            var beats = BarsComputation.SplitToBars(phrase);
            return phrase.EquiZip(staffPositions, beats,
                (note, staffPosition, beat) => 
                    (beat, BeatGroup.Create(new ScoreNote(note.Duration, staffPosition, note.Pitch.PitchClass.Accidental))));
        }

        IEnumerable<(Beat, BeatGroup)> ChordsPhraseToBeatGroups(IEnumerable<(IEnumerable<Pitch> Chord, Duration Duration)> phrase)
        {
            var lowNotes = phrase.Select(chord => new Note(chord.Chord.First(), chord.Duration));
            var beats = BarsComputation.SplitToBars(lowNotes);
            return phrase.EquiZip(beats, (phraseEntry, beat) =>
                (beat, CreateBeatGroup(phraseEntry.Chord, phraseEntry.Duration)));
        }

        BeatGroup CreateBeatGroup(IEnumerable<Pitch> pitches, Duration duration)
        {
            var staffPositions = StaffPositionComputation.ComputeStaffPositions(LowestTreebleStaffPitch, pitches);
            var accidentals = pitches.Select(Pitch.GetAccidental);
            var scoreNotes = staffPositions.EquiZip(accidentals,
                (staffPosition, accidental) => new ScoreNote(duration, staffPosition, accidental));
            return BeatGroup.Create(scoreNotes);
        }
    }
}
