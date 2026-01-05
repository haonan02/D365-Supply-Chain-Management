using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace SCM.Functions
{
    public class ReceiveOrder
    {
        private readonly ILogger<ReceiveOrder> _logger;

        public ReceiveOrder(ILogger<ReceiveOrder> logger)
        {
            _logger = logger;
        }

        // [Function("名字")] 这里定义了接口的名字
        [Function("ReceiveOrder")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("收到一个新的订单请求...");

            // 1. 读取请求体 (Body)
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            // 2. 解析 JSON 数据 (假设外部系统传给我们 SKU 和 Quantity)
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string sku = data?.sku;
            int? quantity = data?.quantity;

            // 3. 简单校验
            if (string.IsNullOrEmpty(sku) || quantity == null)
            {
                return new BadRequestObjectResult("❌ 失败：请提供 SKU 和 Quantity！");
            }

            // 4. 模拟业务逻辑 (以后这里要写代码去连 D365)
            string responseMessage = $"✅ 成功：已接收订单！产品：{sku}, 数量：{quantity}。正在写入 Dynamics 365...";

            _logger.LogInformation(responseMessage);

            return new OkObjectResult(responseMessage);
        }
    }
}