using System.Collections.Generic;
using System.Linq;
using Stride.Music.Score;
using Stride.Music.Theory;

namespace Stride.Bootstrapper
{
    public class TestScores
    {
        public readonly ScoreBuilder ScoreBuilder;

        public readonly IReadOnlyDictionary<string, IReadOnlyDictionary<Beat, BeatGroup>> Scores;

        public readonly IReadOnlyDictionary<Beat, BeatGroup> SimpleTestPhraseScore;
        public readonly IReadOnlyDictionary<Beat, BeatGroup> EightNotePhraseScore;
        public readonly IReadOnlyDictionary<Beat, BeatGroup> ChordPhraseScore;

        public TestScores(ScoreBuilder scoreBuilder)
        {
            ScoreBuilder = scoreBuilder;
            SimpleTestPhraseScore = ScoreBuilder.FromMelodicPhrase(SimpleTestPhrase);
            EightNotePhraseScore = ScoreBuilder.FromMelodicPhrase(EighthNotePhrase);
            var chordPhrase = ChordsPhrase.Select(chords => (chords, Duration.Quarter));
            ChordPhraseScore = ScoreBuilder.FromChordsPhrase(chordPhrase);
            Scores = new Dictionary<string, IReadOnlyDictionary<Beat, BeatGroup>>
            {
                {"simple-test-phrase", SimpleTestPhraseScore},
                {"eight-note-phrase", EightNotePhraseScore},
                {"diatonic-chord-phrase", ChordPhraseScore}
            };
        }

        IEnumerable<Note> SimpleTestPhrase
            => new[]
            {
                Note.Half(Pitch.D6), Note.Quarter(Pitch.E6), Note.Eighth(Pitch.C5), Note.Eighth(Pitch.B4),
                Note.Quarter(Pitch.C4), Note.Quarter(Pitch.D4), Note.Quarter(Pitch.F4), Note.Quarter(Pitch.C4),
                Note.Whole(Pitch.B5)
            };

        IEnumerable<Note> EighthNotePhrase
            => new[]
                {
                    Pitch.C4, Pitch.E4, Pitch.B4, Pitch.G4.Sharp, Pitch.A4, Pitch.B4, Pitch.C5, Pitch.D5,
                    Pitch.E5, Pitch.C5, Pitch.B4, Pitch.A4, Pitch.G4.Sharp, Pitch.A4, Pitch.E4, Pitch.C4
                }
                .Select(Note.Eighth);

        IEnumerable<IEnumerable<Pitch>> ChordsPhrase 
            => new[]
            {
                new [] {Pitch.C4, Pitch.E4, Pitch.G4, Pitch.B4},
                new [] {Pitch.D4, Pitch.F4, Pitch.A4, Pitch.C5},
                new [] {Pitch.E4, Pitch.G4, Pitch.B4, Pitch.D5},
                new [] {Pitch.F4, Pitch.A4, Pitch.C5, Pitch.E5},
                new [] {Pitch.C4, Pitch.F4, Pitch.G4, Pitch.B4},
                new [] {Pitch.D4, Pitch.F4, Pitch.A4, Pitch.C5},
                new [] {Pitch.E4, Pitch.G4, Pitch.B4, Pitch.D5},
                new [] {Pitch.F4, Pitch.A4, Pitch.C5, Pitch.E5},
            };

        public IReadOnlyDictionary<Beat, BeatGroup> EightNotePhraseScore1 => EightNotePhraseScore;
    }
}