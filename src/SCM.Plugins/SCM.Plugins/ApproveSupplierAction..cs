using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace SCM.Plugins
{
    public class ApproveSupplierAction : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // ▼▼▼ 修正点：把 IPluginContext 改成了 IPluginExecutionContext ▼▼▼
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            // 1. 检查 API 名字
            if (context.MessageName != "scm_ApproveSupplier") return;

            try
            {
                // 2. 获取目标 (Target)
                EntityReference targetRef = (EntityReference)context.InputParameters["Target"];

                // 3. 查信用额度
                Entity supplier = service.Retrieve(targetRef.LogicalName, targetRef.Id, new ColumnSet("scm_creditlimit", "scm_name"));

                decimal creditLimit = 0;
                if (supplier.Contains("scm_creditlimit") && supplier["scm_creditlimit"] != null)
                {
                    creditLimit = ((Money)supplier["scm_creditlimit"]).Value;
                }

                // 4. 审核逻辑判断
                string resultMessage = "";

                if (creditLimit > 0)
                {
                    Entity updateSupplier = new Entity(targetRef.LogicalName, targetRef.Id);
                    updateSupplier["scm_description"] = $"[系统自动审核] 于 {DateTime.Now} 通过。信用额度: {creditLimit}";
                    service.Update(updateSupplier);

                    resultMessage = "✅ 审核成功！供应商资质有效，已自动批准。";
                }
                else
                {
                    resultMessage = "❌ 审核失败：该供应商信用额度为 0，请先完善财务信息。";
                }

                // 5. 设置返回值
                context.OutputParameters["Result"] = resultMessage;

            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException($"API 执行错误: {ex.Message}");
            }
        }
    }
}