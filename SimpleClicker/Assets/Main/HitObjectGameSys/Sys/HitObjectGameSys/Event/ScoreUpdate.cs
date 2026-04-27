using EventSys.Interface;

namespace Main.HitObjectGameSys.Event
{
    public readonly struct ScoreUpdate : IEventData
    {
        private readonly int _score;
        public int Score => _score;
        public ScoreUpdate(int score)
        {
            _score = score;
        }

    }
}