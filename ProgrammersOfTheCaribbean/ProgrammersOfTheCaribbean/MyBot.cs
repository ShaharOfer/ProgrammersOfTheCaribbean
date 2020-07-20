using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
                /*
                int col = Math.Abs(pair.Key.Loc.Col - pair.Value.Col);
                int row = Math.Abs(pair.Key.Loc.Row - pair.Value.Row);
                if (col > row && directions.Count >= 2)
                {
                    state.SetSail(pair.Key, directions[1]);
                }
                else
                {
                    state.SetSail(pair.Key, directions[0]);
                }
                */
                
            }
        }

        private Dictionary<Pirate, Location> Strategy(IPirateGame state)
        {
            if (state.MyIslands().Count < 3)// || state.MyPirates().Count >= state.EnemyPirates().Count)
            {
                return _occupiedCenterStrategy.DoTurn(state, state.MyPirates(), state.NotMyIslands());
            }
            else //if (state.MyIslands().Count >= 2 && state.EnemyPirates().Count <= 3 && state.MyPirates().Count >= 4)
            {
                return _attckStrategy.DoTurn(state, state.MyPirates(), state.Islands());
            }
        }
    }
}
