namespace Stride.Music.Theory
{
    public class Note
    {
        public readonly Pitch Pitch;
        public readonly Duration Duration;

        public Note(Pitch pitch, Duration duration)
        {
            Pitch = pitch;
            Duration = duration;
        }

        public static Note Whole(Pitch pitch) =>
            new Note(pitch, Duration.Whole);
    }
}
