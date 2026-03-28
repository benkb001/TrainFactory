namespace TrainGame.Utils;

using System;
using System.Collections.Generic;

public class CallbackRegistry<CONTEXT, INTERFACE, OBJECT> {
    private Dictionary<Type, Action<CONTEXT, INTERFACE, OBJECT>> callbacks = new(); 

    public void Register<IMPLEMENTING>(Action<CONTEXT, IMPLEMENTING, OBJECT> callback) where IMPLEMENTING : INTERFACE {
        Type x = typeof(IMPLEMENTING); 

        callbacks[x] = (CONTEXT w, INTERFACE inter, OBJECT obj) => {
            if (inter is IMPLEMENTING imp) {
                callback(w, imp, obj);
            }
        };
    }

    public void Callback(CONTEXT w, INTERFACE t, OBJECT obj) {
        Type type = t.GetType(); 

        if (callbacks.ContainsKey(type)) {
            callbacks[type](w, t, obj);
        }
    }
}
