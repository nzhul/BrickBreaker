using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CollectablesManager : MonoBehaviour {

	public List<Collectable> AvailableBuffs;
	public List<Collectable> AvailableDebuffs;

	[Range(0, 100)]
	public float BuffChance;

	[Range(0, 100)]
	public float DebuffChance;
}
