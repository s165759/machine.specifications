﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Machine.Specifications.Explorers;
using Machine.Specifications.Utility;

namespace Machine.Specifications.Runner.Impl.Listener
{
    internal class AssemblyContextRunListener : RunListenerBase
    {
        Assembly _assembly;

        public AssemblyContextRunListener(Assembly assembly)
        {
            _assembly = assembly;
        }

        readonly IList<IAssemblyContext> _executedAssemblyContexts = new List<IAssemblyContext>();
        readonly AssemblyExplorer _explorer = new AssemblyExplorer();

        public override void OnAssemblyStart(AssemblyInfo assembly)
        {
            var assemblyContexts = _explorer.FindAssemblyContextsIn(_assembly);
            assemblyContexts.Each(assemblyContext =>
            {
                assemblyContext.OnAssemblyStart();
                _executedAssemblyContexts.Add(assemblyContext);
            });
        }

        public override void OnAssemblyEnd(AssemblyInfo assembly)
        {
            _executedAssemblyContexts
              .Reverse()
              .Each(assemblyContext => assemblyContext.OnAssemblyComplete());
        }
    }
}