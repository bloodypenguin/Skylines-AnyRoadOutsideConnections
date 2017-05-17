using System.Collections.Generic;
using PrefabHook;

namespace AnyRoadOutsideConnections
{
    public class Hook
    {
        private static BuildingInfo _connection;
        private static readonly Queue<RoadBaseAI> InfoQueue = new Queue<RoadBaseAI>();

        public static void Deploy()
        {
            Reset();
            if (!IsHooked())
            {
                return;
            }
            NetInfoHook.OnPostInitialization += OnPostInitialization;
            NetInfoHook.Deploy();
        }


        public static void Revert()
        {
            Reset();
            if (!IsHooked())
            {
                return;
            }
            NetInfoHook.Revert();
        }

        private static void Reset()
        {
            _connection = null;
            InfoQueue.Clear();
        }

        private static void OnPostInitialization(NetInfo info)
        {
            var roadAi = (RoadBaseAI)info?.m_netAI;
            if (roadAi == null)
            {
                return;
            }
            if (roadAi.m_outsideConnection == null && _connection == null)
            {
                InfoQueue.Enqueue(roadAi);
            }
            else
            {
                if (roadAi.m_outsideConnection == null)
                {
                    Process(roadAi);
                }
                else
                {
                    if (_connection == null)
                    {
                        _connection = roadAi.m_outsideConnection;
                    }
                }
                while (InfoQueue.Count > 0)
                {
                    Process(InfoQueue.Dequeue());
                }
            }
        }

        private static void Process(RoadBaseAI ai)
        {
            ai.m_outsideConnection = _connection;
        }

        private static bool IsHooked()
        {
            return Util.IsModActive("Prefab Hook");
        }
    }
}