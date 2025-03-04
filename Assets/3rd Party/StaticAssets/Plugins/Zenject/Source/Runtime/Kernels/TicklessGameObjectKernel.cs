#if !NOT_UNITY3D

namespace Zenject
{
    public class TicklessGameObjectKernel : MonoKernel
    {
        public override void Update()
        {
        }

        public override void FixedUpdate()
        {
        }

        public override void LateUpdate()
        {
        }
    }
}

#endif