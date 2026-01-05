var SCM = SCM || {};

SCM.RibbonFunctions = {
    // === 功能 1: 风险评估 (Day 24 做过的) ===
    checkRisk: function (primaryControl) {
        var formContext = primaryControl;
        var creditLimitAttr = formContext.getAttribute("scm_creditlimit");
        if (!creditLimitAttr || creditLimitAttr.getValue() == null) {
            Xrm.Navigation.openAlertDialog({ text: "⚠️ 请先填写信用额度！" });
            return;
        }
        var amount = creditLimitAttr.getValue();
        if (amount > 50000) {
            Xrm.Navigation.openAlertDialog({ text: "🔴 高风险警告：信用额度超过 5万！" });
        } else {
            Xrm.Navigation.openAlertDialog({ text: "🟢 低风险：系统已自动通过初步审核。" });
        }
    },

    // === 功能 2: 一键审批 (Day 26 新增核心) ===
    approveSupplier: function (primaryControl) {
        // 1. 获取上下文
        var formContext = primaryControl;
        var recordId = formContext.data.entity.getId().replace(/[{}]/g, ""); // 获取当前ID

        // 2. 显示“正在处理”遮罩层 (用户体验加分项！)
        // 这样用户就不会乱点，知道系统在干活
        Xrm.Utility.showProgressIndicator("正在连接总行系统进行审核，请稍候...");

        // 3. 构造 API 请求 (Day 25 控制台里跑过的那段)
        var request = {
            entity: {
                id: recordId,
                entityType: "scm_supplier"
            },
            getMetadata: function () {
                return {
                    boundParameter: "entity",
                    operationType: 0,
                    operationName: "scm_ApproveSupplier", // 你的 API 名字
                    parameterTypes: {
                        "entity": {
                            "typeName": "mscrm.scm_supplier",
                            "structuralProperty": 5
                        }
                    }
                };
            }
        };

        // 4. 发送请求
        Xrm.WebApi.online.execute(request).then(
            function (result) {
                // 关闭遮罩层
                Xrm.Utility.closeProgressIndicator();

                if (result.ok) {
                    result.json().then(function (response) {
                        // 5. 成功后：弹窗 + 刷新
                        var alertStrings = { confirmButtonLabel: "好", text: response.Result, title: "审批结果" };
                        Xrm.Navigation.openAlertDialog(alertStrings).then(function () {
                            // 用户点“好”之后，刷新页面，显示最新的 Description
                            formContext.data.refresh(false);
                        });
                    });
                }
            },
            function (error) {
                // 关闭遮罩层
                Xrm.Utility.closeProgressIndicator();
                
                // 报错处理
                var alertStrings = { confirmButtonLabel: "关闭", text: "❌ 调用失败: " + error.message, title: "错误" };
                Xrm.Navigation.openAlertDialog(alertStrings);
            }
        );
    }
}