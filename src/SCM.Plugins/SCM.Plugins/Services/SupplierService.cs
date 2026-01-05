using Microsoft.Xrm.Sdk;
using SCM.Plugins.Repositories; // 引用刚才写的仓库
using System;

namespace SCM.Plugins.Services
{
    // 服务层：负责“思考”，比如标题该怎么写
    public class SupplierService
    {
        private TaskRepository _taskRepo;

        public SupplierService(IOrganizationService service)
        {
            // 初始化仓库，把干活的工具准备好
            _taskRepo = new TaskRepository(service);
        }

        // 业务逻辑：处理供应商创建后的后续工作
        public void HandlePostCreate(Entity supplier)
        {
            // 1. 提取业务数据
            Guid supplierId = supplier.Id;
            // 逻辑：如果名字没填，就叫“新供应商”
            string name = supplier.Contains("scm_name") ? supplier["scm_name"].ToString() : "新供应商";

            // 2. 准备任务内容
            string subject = $"请核查资质：{name}";
            string description = $"系统自动生成于 {DateTime.Now}，请尽快联系供应商。";

            // 3. 指挥仓库层去干活
            EntityReference supplierRef = new EntityReference("scm_supplier", supplierId);
            _taskRepo.CreateTask(subject, description, supplierRef);
        }
    }
}