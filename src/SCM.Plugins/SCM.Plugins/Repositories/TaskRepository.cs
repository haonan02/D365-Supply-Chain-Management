using Microsoft.Xrm.Sdk;
using System;

namespace SCM.Plugins.Repositories
{
    // 仓库层：只负责搬运数据，没有任何业务判断逻辑
    public class TaskRepository
    {
        private IOrganizationService _service;

        // 构造函数：接收“数据库连接对象”
        public TaskRepository(IOrganizationService service)
        {
            _service = service;
        }

        // 方法：创建一个挂在某个实体下的任务
        public void CreateTask(string subject, string description, EntityReference parentRef)
        {
            Entity task = new Entity("task");
            task["subject"] = subject;
            task["description"] = description;

            // 设置外键 (Regarding)
            task["regardingobjectid"] = parentRef;

            // 真正的数据库写入操作在这里
            _service.Create(task);
        }
    }
}