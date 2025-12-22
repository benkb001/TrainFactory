namespace TrainGame.Components; 

public interface IAssembler<T> {
    Machine GetMachine(); 
    T Assemble(); 
}