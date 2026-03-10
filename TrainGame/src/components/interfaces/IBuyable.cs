namespace TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 

public interface IBuyable {
    Dictionary<string, int> GetCost(); 
}