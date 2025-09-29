using BezierSolution;
using UnityEngine;

public class WizardPath : MonoBehaviour
{
    [SerializeField] private BezierSpline   _leftPathIn, 
                                            _leftPathOut, 
                                            _rightPathIn, 
                                            _rightPathOut;

    private CurrentWizardPath _path = CurrentWizardPath.None;

    public BezierSpline GetNextPath()
    {
        switch(_path)
        {
            case CurrentWizardPath.LeftIn:
                _path = CurrentWizardPath.LeftOut;
                return _leftPathOut;
            case CurrentWizardPath.RightIn:
                _path = CurrentWizardPath.RightOut;
                return _rightPathOut;
            default:
                return GetRandomInPath();
        }
    }

    public CurrentWizardPath GetCurrentPath() => _path;

    private BezierSpline GetRandomInPath()
    {
        var rnd = Random.Range(0, 2);
        switch (rnd)
        {
            case 0:
                _path = CurrentWizardPath.LeftIn;
                return _leftPathIn;
            case 1:
                _path = CurrentWizardPath.RightIn;
                return _rightPathIn;
            default:
                _path = CurrentWizardPath.RightIn;
                return _rightPathIn;
        }
    }
}