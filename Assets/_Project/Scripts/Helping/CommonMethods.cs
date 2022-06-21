using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class CommonMethods : MonoBehaviour
{
    public void PlaySound(AudioClip _clip) {

        Toolbox.Soundmanager.PlaySound(_clip);
    }

    public void LoadSceneWithoutLoading(int _index) {

        Toolbox.GameManager.LoadScene(_index,  false, 0);
    }

    public void LoadSceneWithLoading(int _index)
    {
        Toolbox.GameManager.LoadScene(_index, true, 0);
    }

    public void DestroyAfterDelay(float _time)
    {
        Destroy(this.gameObject, _time);
    }
    public void InstantiateObj(GameObject _obj)
    {
        if(_obj)
            Instantiate(_obj, this.transform.position, Quaternion.identity);
    }
    public void EnableAnimator(Animator _anim) {

        _anim.enabled = true;
    }


}
