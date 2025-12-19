namespace TrainGame.Utils; 

using System.Collections.Generic;
using System; 
using System.Linq;
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public static class ID {
    private static HashSet<string> usedIDs = new(); 

    public static string GetNext(string s) {
        string res; 
        int i = usedIDs.Count; 

        do {
            res = $"{s}{i}"; 
            i++; 
        } while (usedIDs.Contains(res)); 

        usedIDs.Add(res); 
        
        return res;
    }

    public static bool Used(string s) {
        return usedIDs.Contains(s); 
    }
}