using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.StaticCheckVisitor.factory
{
    using st2c.StaticCheckVisitor.strategys;
    using System;
    using System.Collections.Generic;

    namespace staticCheckVisitor.factory
    {
        public class Factory
        {
            private readonly Dictionary<int, IStrategy> strategyHashMap;
            private readonly Dictionary<int, Dictionary<int, IStrategy>> branchStrategyMap;

            private static readonly Factory factory = new Factory();

            private Factory()
            {
                strategyHashMap = new Dictionary<int, IStrategy>();
                branchStrategyMap = new Dictionary<int, Dictionary<int, IStrategy>>();
            }

            public static Factory GetFactory()
            {
                return factory;
            }

            public void Register(int ruleIndex, int branch, IStrategy strategy)
            {
                if (branch == 0)
                {
                    RegisterStrategy(ruleIndex, strategy);
                }
                else
                {
                    RegisterStrategy(ruleIndex, branch, strategy);
                }
            }

            /// <summary>
            /// 没有分支/语法的第一个分支的策略注册到strategyHashMap中
            /// </summary>
            private void RegisterStrategy(int ruleIndex, IStrategy strategy)
            {
                this.strategyHashMap[ruleIndex] = strategy;
            }

            /// <summary>
            /// 分支的策略注册到branchStrategyMap中
            /// </summary>
            private void RegisterStrategy(int ruleIndex, int branch, IStrategy strategy)
            {
                Dictionary<int, IStrategy> map;
                if (branchStrategyMap.ContainsKey(ruleIndex))
                {
                    map = branchStrategyMap[ruleIndex];
                }
                else
                {
                    map = new Dictionary<int, IStrategy>();
                    branchStrategyMap[ruleIndex] = map;
                }
                map[branch] = strategy;
            }

            public IStrategy GetStrategy(int ruleIndex)
            {
                if (!strategyHashMap.TryGetValue(ruleIndex, out var strategy))
                {
                    throw new Exception($"can not find default strategy for ruleIndex = {ruleIndex}");
                }
                return strategy;
            }

            public IStrategy GetStrategy(int ruleIndex, int branch)
            {
                IStrategy strategy;
                if (branch == 0)
                {
                    if (!strategyHashMap.TryGetValue(ruleIndex, out strategy))
                    {
                        throw new Exception($"can not find strategy for ruleIndex = {ruleIndex}, branch = {branch}");
                    }
                }
                else
                {
                    if (!branchStrategyMap.TryGetValue(ruleIndex, out var branchMap) ||
                       !branchMap.TryGetValue(branch, out strategy))
                    {
                        throw new Exception($"can not find strategy for ruleIndex = {ruleIndex}, branch = {branch}");
                    }
                }
                return strategy;
            }
        }
    }
}
