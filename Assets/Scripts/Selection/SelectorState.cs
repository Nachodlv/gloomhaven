using System;

namespace Selection
{
    public abstract class SelectorState
    {
        public abstract void OnSquareSelected(BoardPainter boardPainter, Square destination, Character character,
            Action onSuccessfulAction);
        public abstract void OnSquareHovered(BoardPainter boardPainter, Square destination, Character character);
        public abstract void OnSquareUnHovered(BoardPainter boardPainter, Square destination, Character character);
        
    }
}