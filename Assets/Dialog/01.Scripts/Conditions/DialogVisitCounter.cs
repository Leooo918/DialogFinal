using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    public static class DialogVisitCounter
    {
        public static Dictionary<string, int> visit = new Dictionary<string, int>();

        public static int GetVisit(string nodeGuid)
        {
            if (visit.TryGetValue(nodeGuid, out int visitCnt))
                return visitCnt;

            return 0;
        }

        public static void CountVisit(string nodeGuid)
        {
            if (visit.TryGetValue(nodeGuid, out int cnt))
            {
                visit[nodeGuid] = cnt + 1;
                return;
            }

            visit.Add(nodeGuid, 1);
        }
    }
}
