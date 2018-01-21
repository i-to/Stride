using System;
using System.Windows.Media;
using Stride.Drawing.Wpf;
using Stride.Gui.Wpf.Input;
using Stride.Gui.Wpf.Model;
using Stride.Music.Layout;
using Stride.Music.Score;
using Stride.Music.Theory;

namespace Stride.Gui.Wpf
{
    public class DrillViewModel : NoteSink
    {
        readonly MusicDrawingBuilder MusicDrawingBuilder;
        readonly DrillQuiz DrillQuiz;
        readonly ScoreBuilder ScoreBuilder;
        readonly ScoreLayout Layout;

        public DrillViewModel(
            MusicDrawingBuilder musicDrawingBuilder,
            DrillQuiz drillQuiz,
            ScoreBuilder scoreBuilder,
            ScoreLayout layout)
        {
            MusicDrawingBuilder = musicDrawingBuilder;
            DrillQuiz = drillQuiz;
            ScoreBuilder = scoreBuilder;
            Layout = layout;
        }

        public System.Windows.Media.Drawing MusicDrawing { get; private set; }
        public event EventHandler MusicDrawingChanged;
        void RaiseMusicDrawingChanged() => MusicDrawingChanged?.Invoke(this, EventArgs.Empty);

        public void InitializeDrillDrawing() => Update();

        void Update()
        {
            var score = ScoreBuilder.CreateScore(DrillQuiz.LowestTreebleStaffPitch, DrillQuiz.TestPhrase);
            var layout = Layout.CreateLayout(score);
            MusicDrawing = MusicDrawingBuilder.BuildDrawing(layout);
            RaiseMusicDrawingChanged();
        }

        public void NoteOn(Pitch pitch)
        {
            DrillQuiz.PitchOn(pitch);
            Update();
        }

        public void NoteOff(Pitch pitch)
        {
            DrillQuiz.PitchOff(pitch);
            Update();
        }
    }
}
