using ICities;

namespace AnyRoadOutsideConnections
{
    public class LoadingExtension : LoadingExtensionBase
    {
        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            if (loading.currentMode == AppMode.AssetEditor || loading.currentMode == AppMode.MapEditor)
            {
                return;
            }
            Hook.Deploy();
        }


        public override void OnReleased()
        {
            base.OnReleased();
            Hook.Revert();
        }
    }
}