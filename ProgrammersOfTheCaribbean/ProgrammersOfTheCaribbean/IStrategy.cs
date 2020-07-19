using System.Collections.Generic;
using Pirates;


namespace ProgrammersOfTheCaribbean
{
    public interface IStrategy
    {
        Dictionary<Pirate, Location> DoTurn(IPirateGame state, List<Pirate> myPirates, List<Island> islands);
    }
}
