using UnityEngine;
using System.Collections.Generic;

public class EffectsConstructor {

    private List<Effect> effects = new List<Effect>();
    private int iterator = 0;

    public List<Effect> AddEffect ( Effect effect) {

        iterator++;

        effect.id = iterator;

        effects.Add( effect );

        for ( int i = 0; i < effects.Count; ++i ) {
            effects[i].stackRelationship.Add( effect, false );
        }

        return effects;
    }

}
