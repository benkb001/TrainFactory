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
                Interactable interactable = w.GetComponent<Interactable>(interactableEntity); 

                while (j < interactorEntities.Count && !interacted) {
                    int interactorEntity = interactorEntities[j]; 
                    Frame interactorFrame = w.GetComponent<Frame>(interactorEntity); 
                    bool interact = false; 

                    if (interactableFrame.IsTouching(interactorFrame) || interactableFrame.IntersectsWith(interactorFrame)) {
                        if (interactable.ItemId == "") {
                            interact = true; 
                        } else if (w.ComponentContainsEntity<HeldItem>(interactorEntity)) {
                            HeldItem held = w.GetComponent<HeldItem>(interactorEntity); 
                            if (held.ItemId == interactable.ItemId && held.ItemCount >= interactable.ItemCount) {
                                interact = true; 
                            }
                        }
                    }

                    if (interact) {
                        interactable.Interacted = true; 
                        interactable.InteractorEntity = interactorEntity; 
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
            Interactable i = w.GetComponent<Interactable>(e); 
            i.Interacted = false;
            i.InteractorEntity = -1; 
        }; 
        world.AddSystem(ts, tf); 
    }

    public static void Register<T>(World w, Action<World, int> tf) {
        w.AddSystem([typeof(T), typeof(Interactable), typeof(Active)], (w, e) => {
            if (w.GetComponent<Interactable>(e).Interacted) {
                tf(w, e); 
            }
        });
    }

    public static void Register<T>(World w, Action<World, int, T> tf) {
        w.AddSystem([typeof(T), typeof(Interactable), typeof(Active)], (w, e) => {
            if (w.GetComponent<Interactable>(e).Interacted) {
                tf(w, e, w.GetComponent<T>(e));
            }
        });
    }
}