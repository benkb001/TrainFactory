namespace TrainGame; 

using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.Constants; 
using TrainGame.ECS; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class Game1 : Game {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private World w; 
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        w = new World(_graphics, GraphicsDevice, _spriteBatch); 
        Texture2D button = Content.Load<Texture2D>("Images/button");
        w.SetTexture(Textures.Button, button); 

        Texture2D _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
        w.SetTexture(Textures.Pixel, _pixel); 
        w.SetPixelTexture(_pixel); 

        SpriteFont font = Content.Load<SpriteFont>("AnonymousPro");
        w.SetFont(font); 
        
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int nextTestButton = EntityFactory.Add(w); 

        w.SetComponent<Frame>(nextTestButton, new Frame(200, 0, 100, 50));
        w.SetComponent<Button>(nextTestButton, new Button());
        w.SetComponent<Sprite>(nextTestButton, new Sprite(w.GetTexture(Textures.Button), Depth.NextTestButton));
        w.SetComponent<NextDrawTestButton>(nextTestButton, new NextDrawTestButton()); 

        MovementSystem.SetCollisionSpace(SceneSystem.CameraPositions[SceneType.RPG]);
        VirtualMouse.UsePhysicalMouse(); 
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        w.Update(); 
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {

        w.Draw(); 

        //base.Draw(gameTime);
    }
}