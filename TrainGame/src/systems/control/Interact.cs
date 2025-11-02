namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public class InteractSystem {
    public static void RegisterInteract(World world) {
        Action<World> update = (w) => {
            if (!VirtualKeyboard.IsClicked(KeyBinds.Interact)) {
                return; 
            }
            
            bool interacted = false; 
            List<int> interactableEntities = w.GetMatchingEntities([typeof(Interactable), typeof(Frame), typeof(Active)]); 
            List<int> interactorEntities = w.GetMatchingEntities([typeof(Interactor), typeof(Frame), typeof(Active)]); 
            
            int i = 0; 
            while (i < interactableEntities.Count && !interacted) {
                int j = 0; 
                int interactableEntity = interactableEntities[i]; 
                Frame interactableFrame = w.GetComponent<Frame>(interactableEntity);
                while (j < interactorEntities.Count && !interacted) {
                    int interactorEntity = interactorEntities[j]; 
                    Frame interactorFrame = w.GetComponent<Frame>(interactorEntity); 
                    if (interactableFrame.IsTouching(interactorFrame)) {
                        w.GetComponent<Interactable>(interactableEntity).Interacted = true; 
                        interacted = true; 
                    }
                    j++; 
                }
                i++; 
            }
        }; 
        world.AddSystem(update); 
    }

    public static void RegisterUninteract(World world) {
        Type[] ts = [typeof(Interactable), typeof(Active)]; 
        Action<World, int> tf = (w, e) => {
            w.GetComponent<Interactable>(e).Interacted = false;
        }; 
        world.AddSystem(ts, tf); 
    }
}