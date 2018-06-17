using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MoreLinq;
using Stride.Music.Score;
using Stride.Music.Theory;

namespace Stride.Music.Layout
{
    public class StemsLayoutAlgorithm
    {
        readonly StavesMetrics Metrics;
        readonly VerticalLayoutAlgorithm VerticalLayout;

        public StemsLayoutAlgorithm(StavesMetrics metrics, VerticalLayoutAlgorithm verticalLayout)
        {
            Metrics = metrics;
            VerticalLayout = verticalLayout;
        }

        public IEnumerable<LayoutObject> Create(
            IEnumerable<IEnumerable<Beat>> barsActiveBeats,
            IReadOnlyDictionary<Beat, BeatGroup> beatGroups,
            IReadOnlyDictionary<Beat, double> tickPositions)
            => barsActiveBeats.SelectMany(
                barActiveBeats => ComputeStemsAndFlagsForBar(barActiveBeats, beatGroups, tickPositions));

        IEnumerable<LayoutObject> ComputeStemsAndFlagsForBar(
            IEnumerable<Beat> barActiveBeats,
            IReadOnlyDictionary<Beat, BeatGroup> beatGroups,
            IReadOnlyDictionary<Beat, double> tickPositions) 
            => barActiveBeats.SelectMany(
                    beat => CreateStemAndFlagObjects(tickPositions[beat], beatGroups[beat]));

        IEnumerable<LayoutObject> CreateStemAndFlagObjects(
            double tickPosition,
            BeatGroup beatGroup)
        {
            var (stem, flag) = CreateStemAndFlagForIndividualBeat(tickPosition, beatGroup);
            if (stem != null)
                yield return stem;
            if (flag != null)
                yield return flag;
        }

        (LineObject stem, SymbolObject flag) CreateStemAndFlagForIndividualBeat(
            double tickPosition,
            BeatGroup beatGroup)
        {
            var result = (stem: (LineObject) null, flag: (SymbolObject) null);

            var notes = beatGroup.ScoreNotes;
            var duration = notes.First().Duration;
            var clef = notes.First().StaffPosition.Clef;
            if (notes.Skip(1).Any(note => note.Duration != duration))
                throw new NotImplementedException("Stemming of beat groups of non-uniform direction is not implemented yet.");
            if (notes.Skip(1).Any(note => note.StaffPosition.Clef != clef))
                throw new NotImplementedException("Stemming of beat groups spanning different clefs is not implemented yet.");

            if (duration.IsWhole())
                return result;

            // todo: handle groups correctly
            var stemmedNotes = notes.Where(note => !note.Duration.IsWhole());
            if (!stemmedNotes.Any())
                return result;

            // todo: this assumes single clef
            var offsetDistance = notes.Last().StaffPosition.VerticalOffset - notes.First().StaffPosition.VerticalOffset;
            var groupHeight = offsetDistance * Metrics.HalfSpace;

            var stemLength = Metrics.RegularStemLength + groupHeight;
            var position = notes.First().StaffPosition;
            bool stemUp = position.VerticalOffset < 0;
            var (xOffset, yOffset, length) = stemUp
                ? (Metrics.OtherNoteheadWidth - Metrics.StemLineThickness, 0, -stemLength)
                : (0, 2, stemLength);
            var x = xOffset + tickPosition;
            var y = yOffset + VerticalLayout.StaffPositionToYOffset(position);
            var origin = new Point(x, y);
            var end = new Point(x, y + length);
            result.stem = new LineObject(origin, end, Metrics.StemLineThickness);

            if (duration.IsHalf() || duration.IsQuarter())
                return result;
            var (flagY, symbol) = stemUp
                ? (y - stemLength, Symbol.FlagEighthUp)
                : (y + stemLength, Symbol.FlagEightsDown);
            var flagOrigin = new Point(x, flagY);
            result.flag = new SymbolObject(flagOrigin, symbol, Metrics.GlyphSize, false);
            return result;
        }
    }
}