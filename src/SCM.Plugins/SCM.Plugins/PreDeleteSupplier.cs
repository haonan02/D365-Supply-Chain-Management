using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query; // 必须引入这个命名空间！
using System;

namespace SCM.Plugins
{
    public class PreDeleteSupplier : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is EntityReference)
            {
                EntityReference targetEntity = (EntityReference)context.InputParameters["Target"];

                try
                {
                    // === 第一关：VIP 校验 (之前的逻辑) ===
                    string keyPartnerField = "scm_iskeypartner";
                    Entity currentSupplier = service.Retrieve(targetEntity.LogicalName, targetEntity.Id, new ColumnSet(keyPartnerField));
                    bool isKeyPartner = currentSupplier.GetAttributeValue<bool?>(keyPartnerField) ?? false;

                    if (isKeyPartner)
                    {
                        throw new InvalidPluginExecutionException("🛑 拦截：VIP 供应商禁止删除！");
                    }

                    // === 第二关：关联产品校验 (Day 13 新增核心) ===

                    // 1. 定义我们要查哪张表？ -> Product 表
                    QueryExpression query = new QueryExpression("scm_product"); // 确保这是 Product 表的逻辑名

                    // 2. 我们只关心数量，不关心具体列，设为 false 提高性能
                    query.ColumnSet = new ColumnSet(false);

                    // 3. 添加过滤条件：WHERE scm_supplier == 当前要删的供应商ID
                    // "scm_supplier" 是 Product 表里那个 Lookup 字段的逻辑名，请务必核对！
                    query.Criteria.AddCondition("scm_supplier", ConditionOperator.Equal, targetEntity.Id);

                    // 4. 执行查询 (RetrieveMultiple)
                    EntityCollection results = service.RetrieveMultiple(query);

                    // 5. 判断结果
                    if (results.Entities.Count > 0)
                    {
                        // 如果查到了产品，说明有关联数据，禁止删除！
                        throw new InvalidPluginExecutionException($"🛑 拦截：该供应商名下仍有 {results.Entities.Count} 个关联产品，无法删除！请先清空产品。");
                    }

                }
                catch (InvalidPluginExecutionException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new InvalidPluginExecutionException($"检查出错: {ex.Message}");
                }
            }
        }
    }
}