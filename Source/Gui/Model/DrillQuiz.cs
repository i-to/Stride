using System.Collections.Generic;
using Stride.Music.Theory;

namespace Stride.Gui.Model
{
    public class DrillQuiz
    {
        public DrillQuiz()
        {
            PlayingPitches = new List<Pitch>();
        }

        public int CurrentPosition { get; private set; }
        Drill Drill;
        readonly List<Pitch> PlayingPitches;

        Pitch TestPitch => TestPhrase[CurrentPosition].Pitch;

        public IReadOnlyList<Note> TestPhrase => Drill.Notes;
        public IReadOnlyList<Pitch> SoundingPitches => PlayingPitches;
        public Pitch LowestTreebleStaffPitch => Drill.LowestTreebleSaffPitch;

        public void Start(Drill session)
        {
            Drill = session;
            CurrentPosition = -1;
            SwitchToNextQuestion();
        }

        public void SwitchToNextQuestion()
        {
            CurrentPosition = (CurrentPosition + 1) % Drill.Notes.Count;
        }

        void CheckAnswer(Pitch testPitch, Pitch playedPitch)
        {
            if (PlayingPitches.Count == 1 && testPitch == playedPitch)
                SwitchToNextQuestion();
        }

        public void PitchOn(Pitch pitch)
        {
            PlayingPitches.Add(pitch);
        }

        public void PitchOff(Pitch pitch)
        {
            CheckAnswer(TestPitch, pitch);
            PlayingPitches.Remove(pitch);
        }
    }
}
