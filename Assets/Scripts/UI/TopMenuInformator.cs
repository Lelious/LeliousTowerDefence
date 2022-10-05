using UnityEngine;
using UnityEngine.UI;

public class TopMenuInformator : MonoBehaviour
{
	[SerializeField] private Text _money;
	[SerializeField] private Text _waveCount;

	public void SetMoney(int amount)
	{
		_money.text = CachedStringValues.cachedStringValues[amount];
	}

	public void SetWaveCount(int amount)
	{
		_waveCount.text = CachedStringValues.cachedStringValues[amount];
	}
}
