using Microsoft.Xrm.Sdk;
using SCM.Plugins.Services; // 引用服务层
using System;

namespace SCM.Plugins
{
    // 插件层（Controller）：只负责接客（获取Context）和送客（报错）
    public class PostCreateSupplier : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // 1. 获取基础设施
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            // 2. 校验 Target
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity supplierEntity = (Entity)context.InputParameters["Target"];

                // 补全 ID (Post操作 ID 在 Output 也可以，保险起见补全)
                if (supplierEntity.Id == Guid.Empty && context.OutputParameters.Contains("id"))
                {
                    supplierEntity.Id = (Guid)context.OutputParameters["id"];
                }

                try
                {
                    // 3. 【核心变化】代码只剩这两行！
                    // 把脏活累活都交给 Service 去做
                    SupplierService businessLogic = new SupplierService(service);
                    businessLogic.HandlePostCreate(supplierEntity);
                }
                catch (Exception ex)
                {
                    throw new InvalidPluginExecutionException($"业务逻辑执行失败: {ex.Message}");
                }
            }
        }
    }
}