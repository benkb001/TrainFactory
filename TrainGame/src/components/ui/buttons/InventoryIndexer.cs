namespace TrainGame.Components; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public class InventoryIndexer<T> where T : IInventorySource {
    public readonly int Delta; 
    public int ContainerEntity; 
    public readonly InventoryContainer<T> Container; 

    public InventoryIndexer(InventoryContainer<T> Container, int ContainerEntity, int Delta = 1) {
        this.Delta = Delta; 
        this.Container = Container; 
        this.ContainerEntity = ContainerEntity; 
    }
}