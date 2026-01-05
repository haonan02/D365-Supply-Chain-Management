using Microsoft.Xrm.Sdk;
using System;

namespace SCM.Plugins
{
    public class PreUpdateProduct : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // 1. 确保是 Update 操作
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity targetEntity = (Entity)context.InputParameters["Target"];

                // 2. 检查：这次是否修改了价格？(逻辑名必须全小写: scm_price)
                if (!targetEntity.Contains("scm_price")) return;

                // 3. 【核心】获取 "PreImage" (修改前的快照)
                // 这里的 "PreImage" 必须和一会我们在 PRT 里注册的名字一模一样！
                if (!context.PreEntityImages.Contains("PreImage"))
                {   
                    throw new InvalidPluginExecutionException("未找到 PreImage，请检查插件注册配置！");
                }
                Entity preImage = context.PreEntityImages["PreImage"];

                try
                {
                    // 4. 获取新旧价格 (Money 类型)
                    // targetEntity 里是新值
                    decimal newPrice = ((Money)targetEntity["scm_price"]).Value;

                    // preImage 里是旧值 (注意判空，防止旧数据没价格)
                    decimal oldPrice = 0;
                    if (preImage.Contains("scm_price"))
                    {
                        oldPrice = ((Money)preImage["scm_price"]).Value;
                    }

                    // 5. 业务逻辑：如果降价了
                    if (newPrice < oldPrice)
                    {
                        string logMessage = $"\n[系统日志] {DateTime.Now}: 价格从 {oldPrice} 降到了 {newPrice}。";

                        // 6. 追加日志到 Description
                        // 先获取现有的描述
                        string currentDesc = "";
                        if (targetEntity.Contains("scm_description"))
                        {
                            currentDesc = targetEntity["scm_description"].ToString();
                        }
                        else if (preImage.Contains("scm_description"))
                        {
                            currentDesc = preImage["scm_description"].ToString();
                        }

                        // 赋值回 Target (因为是 Pre-Operation，这里赋值会自动保存)
                        targetEntity["scm_description"] = currentDesc + logMessage;
                    }
                }
                catch (Exception ex)
                {


                    throw new InvalidPluginExecutionException($"插件出错: {ex.Message}");
                }
            }
        }
    }
}