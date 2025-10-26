namespace TrainGame.ECS;

using TrainGame.Components; 

public class Test {
    public int t = 0; 
    public bool b = false; 
}

public class Test1 {
    public int t = 0; 
}

public class Test2 {
    public int t = 0; 
}

public class Test3 {
    public int t = 0; 
}

public class Garbage {
    public int g; 
}

public class ECSTest
{
    [Fact]
    public void ECS_ShouldRegisterComponents()
    {
        World w = new World(); 
        w.AddComponentType<Test>();

        Assert.True(w.ComponentTypeIsRegistered<Test>()); 
        Assert.False(w.ComponentTypeIsRegistered<Garbage>()); 
    }

    
    [Fact]
    public void ECS_ShouldRegisterSystems() {
        World w = new World(); 

        Assert.Equal(0, w.SystemCount()); 

        w.AddComponentType<Test>();

        Action<World, int> transform = (w, e) => {
            Test cur = w.GetComponent<Test>(e); 
            cur.t++; 
        }; 
        w.AddSystem([typeof(Test)], transform); 

        Assert.Equal(1, w.SystemCount());
    }
    
    [Fact]
    public void ECS_ShouldRegisterEntities() {
        World w = new World(); 
        Assert.Equal(0, w.EntityCount()); 

        w.AddComponentType<Test>();
        RegisterComponents.All(w);
        Action<World, int> transform = (w, e) => {
            Test cur = w.GetComponent<Test>(e); 
            cur.t++; 
        }; 

        _System s = w.AddSystem([typeof(Test)], transform); 
        int e = w.AddEntity(); 

        Assert.Equal(0, e); 
        Assert.Equal(1, w.EntityCount()); 
        w.SetComponent<Test>(e, new Test()); 

        Assert.True(s.ContainsEntity(e));
        Assert.True(w.ComponentContainsEntity<Test>(e)); 
        Assert.True(w.EntityExists(e));

        Assert.False(s.ContainsEntity(e + 1)); 
        Assert.False(w.ComponentContainsEntity<Test>(e + 1)); 
        Assert.False(w.EntityExists(e + 1)); 
    }

    [Fact]
    public void ECS_ShouldGiveSignatureIndicesToComponentTypes() {
        World w = new World(); 
        w.AddComponentType<Test1>(); 
        w.AddComponentType<Test2>(); 
        w.AddComponentType<Test3>(); 

        Assert.Equal(0, w.GetComponentIndex<Test1>());
        Assert.Equal(1, w.GetComponentIndex<Test2>());
        Assert.Equal(2, w.GetComponentIndex<Test3>());
    }

    [Fact]
    public void ECS_ShouldAssignSystemSignatureCorrespondingToComponentTypesActedOn() {
        World w = new World(); 

        w.AddComponentType<Test1>(); 
        w.AddComponentType<Test2>(); 
        w.AddComponentType<Test3>(); 

        Type[] types = [typeof(Test1), typeof(Test3)]; 

        Action<World, int> transformer = (w, e) => {
            //noop
        }; 

        _System s = w.AddSystem(types, transformer);

        int s1 = w.GetComponentIndex<Test1>(); 
        int s2 = w.GetComponentIndex<Test2>(); 
        int s3 = w.GetComponentIndex<Test3>(); 

        Assert.True(s.ActsOnComponentType(s1)); 
        Assert.True(s.ActsOnComponentType(s3)); 

        Assert.False(s.ActsOnComponentType(s2)); 
    }

    [Fact]
    public void ECS_SystemsShouldUpdateWorld() {
        World w = new World(); 

        w.AddComponentType<Test>(); 
        RegisterComponents.All(w);

        Type[] types = [typeof(Test)]; 

        Action<World, int> transformer = (w, e) => {
            Test t = w.GetComponent<Test>(e); 
            t.t += 1; 
            w.SetComponent<Test>(e, t); 
        };

        w.AddSystem(types, transformer);

        int e = w.AddEntity(); 

        w.SetComponent<Test>(e, new Test()); 

        w.Update(); 

        Test t_updated = w.GetComponent<Test>(e); 
        Assert.Equal(1, t_updated.t); 
    }

    [Fact]
    public void ECS_ShouldBeAbleToRemoveEntitiesThroughTransformations() {
        World w = new World(); 
        w.AddComponentType<Test>(); 
        RegisterComponents.All(w);

        Type[] types = [typeof(Test)]; 
        Action<World, int> transformer = (w, e) => {
            w.RemoveEntity(e); 
        };
        _System s = w.AddSystem(types, transformer); 

        for (int i = 0; i < 999; i++) {
            int e = w.AddEntity();
            w.SetComponent<Test>(e, new Test());
        }

        w.Update(); 

        Assert.Equal(0, w.EntityCount()); 
        Assert.Equal(0, s.EntityCount());
        Assert.Equal(0, w.ComponentEntityCount<Test>()); 
    }

    [Fact]
    public void ECS_ShouldBeAbleToRemoveComponentsThroughTransformations() {
        World w = new World(); 
        w.AddComponentType<Test>();
        w.AddComponentType<Garbage>(); 
        RegisterComponents.All(w);

        Type[] types = [typeof(Garbage)]; 
        Action<World, int> transformer = (w, e) => {
            w.RemoveComponent<Garbage>(e); 
        };

        _System s = w.AddSystem(types, transformer); 

        int numEntities = 999;

        for (int i = 0; i < numEntities; i++) {
            int e = w.AddEntity();
            w.SetComponent<Garbage>(e, new Garbage());
        }

        Assert.Equal(numEntities, w.EntityCount());
        Assert.Equal(numEntities, w.ComponentEntityCount<Garbage>());
        Assert.Equal(numEntities, s.EntityCount()); 

        w.Update(); 

        Assert.Equal(numEntities, w.EntityCount()); 
        Assert.Equal(0, s.EntityCount());
        Assert.Equal(0, w.ComponentEntityCount<Garbage>()); 
    }

    [Fact]
    public void ECS_ShouldBeAbleToAddEntitiesThroughTransformations() {
        World w = new World(); 
        w.AddComponentType<Test1>(); 

        w.AddComponentType<Test2>(); 
        RegisterComponents.All(w);
        Type[] types = [typeof(Test1)]; 

        Action<World, int> transformer = (w, e) => {
            int e_new = w.AddEntity(); 
            w.SetComponent<Test2>(e, new Test2()); 
        };

        w.AddSystem(types, transformer); 

        int e = w.AddEntity(); 
        w.SetComponent<Test1>(e, new Test1()); 

        Assert.Equal(1, w.EntityCount()); 
        Assert.Equal(1, w.ComponentEntityCount<Test1>()); 
        Assert.Equal(0, w.ComponentEntityCount<Test2>()); 

        w.Update(); 

        Assert.Equal(2, w.EntityCount()); 
        Assert.Equal(1, w.ComponentEntityCount<Test1>()); 
        Assert.Equal(1, w.ComponentEntityCount<Test2>()); 
    }

    [Fact]
    public void ECS_ShouldBeAbleToAddComponentsToEntitiesThroughTransformations() {
        World w = new World(); 
        Assert.Equal(0, w.EntityCount()); 

        w.AddComponentType<Test1>();
        w.AddComponentType<Test2>();
        RegisterComponents.All(w);
        Action<World, int> transform = (w, e) => {
            w.SetComponent<Test2>(e, new Test2()); 
        }; 

        _System s = w.AddSystem([typeof(Test1)], transform); 
        int e = w.AddEntity(); 

        w.SetComponent<Test1>(e, new Test1()); 

        Assert.Equal(0, w.ComponentEntityCount<Test2>());

        w.Update(); 

        Assert.Equal(1, w.ComponentEntityCount<Test2>());
    }

    [Fact]
    public void ECS_SystemsShouldNotActOnEntitiesThatShareOneButNotAllRequiredComponents() {
        World w = new World(); 

        w.AddComponentType<Test>(); 
        w.AddComponentType<Garbage>(); 
        RegisterComponents.All(w);
        Type[] types = [typeof(Garbage), typeof(Test)]; 
        Action<World, int> transformer = (w, e) => {
            w.RemoveEntity(e); 
        };

        _System s = w.AddSystem(types, transformer); 

        int e1 = w.AddEntity(); 

        w.SetComponent<Test>(e1, new Test()); 

        int e2 = w.AddEntity(); 
        w.SetComponent<Test>(e2, new Test()); 
        w.SetComponent<Garbage>(e2, new Garbage()); 

        Assert.True(w.EntityExists(e2)); 
        Assert.True(w.EntityExists(e1)); 

        w.Update(); 

        Assert.False(w.EntityExists(e2)); 
        Assert.True(w.EntityExists(e1)); 
    }

    [Fact]
    public void ECS_ShouldApplyTransformationsInTheOrderTheyAreRegistered() {
        World w = new World(); 

        w.AddComponentType<Test>(); 
        RegisterComponents.All(w);
        Type[] types = [typeof(Test)];  
        Action<World, int> tf_check = (w, e) => {
            Test t = w.GetComponent<Test>(e); 
            t.b = true; 
            w.SetComponent<Test>(e, t); 
        };

        Action<World, int> tf_inc = (w, e) => {
            Test t = w.GetComponent<Test>(e); 
            if (t.b) {
                t.t++; 
                w.SetComponent<Test>(e, t); 
            }
        };

        w.AddSystem(types, tf_check); 
        w.AddSystem(types, tf_inc); 

        int e = w.AddEntity(); 
        w.SetComponent<Test>(e, new Test()); 

        w.Update(); 

        Assert.Equal(1, w.GetComponent<Test>(e).t); 
    }

    [Fact]
    public void ECS_ShouldThrowExceptionWhenAddingUnregisteredComponentsToEntities() {
        World w = new World(); 
        w.AddComponentType<Test>(); 
        RegisterComponents.All(w);
        Type[] types = [typeof(Test)]; 
        Action<World, int> transformer = (w, e) => {
            //noop
        };
        w.AddSystem(types, transformer); 
        
        int e = w.AddEntity(); 

        Assert.Throws<InvalidOperationException>(() => {
            w.SetComponent<Garbage>(e, new Garbage()); 
        });
    }

    [Fact]
    public void ECS_ShouldThrowExceptionWhenRegisteringTheSameComponentTypeMoreThanOnce() {
        World w = new World(); 
        Assert.Throws<InvalidOperationException>(() => {
            w.AddComponentType<Test>(); 
            w.AddComponentType<Test>(); 
        });
    }

    [Fact]
    public void ECS_ShouldThrowExceptionWhenRegisteringSystemsThatActOnUnregisteredComponents() {
        World w = new World(); 
        Assert.Throws<InvalidOperationException>(() => {
            Type[] types = [typeof(Test)]; 

            Action<World, int> transformer = (w, e) => {
                //noop
            }; 

            _System s = w.AddSystem(types, transformer);
        });
    }

    [Fact]
    public void ECS_ShouldThrowExceptionWhenAddingComponentsToEntitiesThatDontExist() {
        World w = new World(); 
        w.AddComponentType<Test>(); 
        Assert.Throws<InvalidOperationException>(() => {
            w.SetComponent<Test>(0, new Test()); 
        }); 
    }
}