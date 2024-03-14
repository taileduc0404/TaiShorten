using TaiShorten.Interfaces;

namespace TaiShorten.Implement
{
    public class RandomProvider : IRandomProvider
    {
        public Random GetRandom()
        {
            return new Random();
        }
    }
}
