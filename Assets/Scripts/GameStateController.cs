using UnityEngine;
using System.Collections;

public class GameStateController {

    private int startTimeout = 5;
    private int nextWaveTimeout = 10;
    private int currentWaveTimer = 0;
    private int waveCounter = 5;
    private Spawn spawn;
    private EntityController entityController;
    private BuildView[] buildView;
    private InputController.UpdateWaveTimer updateWaveTimer;

    public GameStateController ( EntityController entityController, InputController.UpdateWaveTimer updateWaveTimer ) {
        this.updateWaveTimer = updateWaveTimer;
        StartWaveTimer( startTimeout );
        this.entityController = entityController;
        spawn = new Spawn( entityController );
        buildView = GameObject.FindObjectsOfType<BuildView>();
    }

    private bool WaveCounter () {
        --currentWaveTimer;
        updateWaveTimer( currentWaveTimer );
        if ( currentWaveTimer == 0 ) {
            NextWave();
            return false;
        }
        return true;
    }

    private void StartWaveTimer ( int waveTimer) {
        SceneManager.Instance.CoroutineManager.InvokeRepeatingBool( WaveCounter, 1 );
        currentWaveTimer = waveTimer;
        updateWaveTimer( currentWaveTimer );
    }

    private void NextWave () {
        Debug.Log( "NextWave" );
        --waveCounter;

        if ( waveCounter > 0 ) {
            StartWaveTimer( nextWaveTimeout );
        }

        if ( waveCounter == 4 ) {
            AddHero();
        }

        if ( waveCounter == 0) {
            AddBigMob();
        }

        AddCreeps();
    }

    private void AddCreeps () {
        foreach ( var key in buildView ) {
            if ( key.spawnUnitType != BaseUnit.UnitType.hero && key.spawnUnitType != BaseUnit.UnitType.bigMob ) {
                spawn.CreateUnitByType( key.spawnUnitType, key.spawnPosition.transform.position, key.GetUnit(), key.faction );
            }
        }
    }

    private void AddHero () {
        foreach ( var key in buildView ) {
            if ( key.spawnUnitType == BaseUnit.UnitType.hero ) {
                spawn.CreateUnitByType( key.spawnUnitType, key.spawnPosition.transform.position, key.GetUnit(), key.faction );
            }
        }
    }

    private void AddBigMob () {
        foreach ( var key in buildView ) {
            if ( key.spawnUnitType == BaseUnit.UnitType.bigMob ) {
                spawn.CreateUnitByType( key.spawnUnitType, key.spawnPosition.transform.position, key.GetUnit(), key.faction );
            }
        }
    }

}