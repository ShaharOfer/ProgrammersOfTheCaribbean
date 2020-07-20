using System.Collections.Generic;
using Pirates;

namespace ProgrammersOfTheCaribbean
{
    public class MyBot : IPirateBot
    {
        private OccupiedCenterStrategy _occupiedCenterStrategy;
        private AttackStrategy _attckStrategy;

        public MyBot()
        {
            _occupiedCenterStrategy = new OccupiedCenterStrategy();
            _attckStrategy = new AttackStrategy(_occupiedCenterStrategy);
        }

        public void DoTurn(IPirateGame state)
        {
            Dictionary<Pirate, Location> pirateToLocation = Strategy(state);
            MovePirates(state, pirateToLocation);    
        }


        private void MovePirates(IPirateGame state, Dictionary<Pirate, Location> piratesToLocations)
        {
            foreach(var pair in piratesToLocations)
            {
                var directions = state.GetDirections(pair.Key, pair.Value);
                state.SetSail(pair.Key, directions[0]);
            }
        }

        private Dictionary<Pirate, Location> Strategy(IPirateGame state)
        {
            if (state.MyIslands().Count < 2 || state.MyPirates().Count >= state.EnemyPirates().Count)
            {
                return _occupiedCenterStrategy.DoTurn(state, state.MyPirates(), state.Islands());
            }
            else //if (state.MyIslands().Count >= 2 && state.EnemyPirates().Count <= 3 && state.MyPirates().Count >= 4)
            {
                return _attckStrategy.DoTurn(state, state.MyPirates(), state.Islands());
            }
        }
    }
}
