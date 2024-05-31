using Infrastructure.StateMachine;
using UnityEngine;

namespace Infrastructure.Services.Input
{
    public interface IInputService : IControlFromState
    {
        Vector3 GetMovementDirection(float z);
    }
}
