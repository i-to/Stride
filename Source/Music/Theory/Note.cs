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

        public static Pitch GetPitch(Note note) => note.Pitch;

        public static Note Whole(Pitch pitch) => new Note(pitch, Duration.Whole);
        public static Note Half(Pitch pitch) => new Note(pitch, Duration.Half);
        public static Note Quarter(Pitch pitch) => new Note(pitch, Duration.Quarter);
    }
}
