namespace TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

public class DrawEmbarkMessage {
    private Train train; 
    private City city; 
    public Vector2 Position; 
    public float Width;
    public float Height;
    public float Padding;
    public readonly int TrainEntity;

    public DrawEmbarkMessage(Train t, int TrainEntity, City comingFrom, Vector2 Position, float Width = 100f, float Height = 100f, float Padding = 0f) {
        this.Position = Position; 
        this.train = t; 
        this.city = comingFrom; 
        this.Width = Width; 
        this.Height = Height; 
        this.Padding = Padding; 
        this.TrainEntity = TrainEntity;
    }

    public Train GetTrain() {
        return train; 
    }

    public City GetCity() {
        return city; 
    }
}