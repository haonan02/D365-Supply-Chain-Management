// Namespace: SCM (Supply Chain Management)
var SCM = SCM || {};

SCM.SupplierLogic = {
    // Event Handler
    handleKeyPartnerChange: function (executionContext) {
        "use strict"; // 启用严格模式，更规范

        // ------------------------------------------------
        // 测试通了之后，这一行要注释掉，不然用户会烦
        // alert("代码逻辑已触发"); 
        // ------------------------------------------------

        var formContext = executionContext.getFormContext();

        // 1. 获取字段属性 (Attribute) - 用于读写数据
        // 逻辑名必须全小写
        var isKeyPartnerAttr = formContext.getAttribute("scm_iskeypartner");
        var creditLimitAttr = formContext.getAttribute("scm_creditlimit");

        // 2. 获取界面控件 (Control) - 用于显示隐藏
        var creditLimitControl = formContext.getControl("scm_creditlimit");

        // 安全检查：如果字段不存在，直接停止，防止报错
        if (!isKeyPartnerAttr || !creditLimitAttr || !creditLimitControl) {
            console.log("Error: 找不到字段，请检查逻辑名称。");
            return;
        }

        // 3. 获取当前开关的值 (true/false)
        var isKeyPartner = isKeyPartnerAttr.getValue();

        // 4. 业务逻辑
        if (isKeyPartner === true) {
            // === 情况 A: 是核心伙伴 ===
            
            // 显示控件
            creditLimitControl.setVisible(true);
            // 设为必填 (红星号)
            creditLimitAttr.setRequiredLevel("required");

        } else {
            // === 情况 B: 不是核心伙伴 ===
            
            // 隐藏控件
            creditLimitControl.setVisible(false);
            // 取消必填
            creditLimitAttr.setRequiredLevel("none");
            
            // 【重要】隐藏时清空数值，防止脏数据
            creditLimitAttr.setValue(null);
        }
    }
}