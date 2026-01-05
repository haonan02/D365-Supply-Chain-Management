# Dynamics 365 Supply Chain Management System

## 🌟 Project Overview
An enterprise-grade SCM system developed on **Microsoft Power Platform**.
It integrates **Dataverse**, **Model-Driven Apps**, **C# Plugins**, and **PCF Controls** to manage suppliers and products with automated logic.

---

## 🚀 Key Technical Features (Day 1 - 30)

### 1. Backend Architecture (C# Plugins)
*   **Service-Repository Pattern:** Refactored plugins to decouple business logic from data access, ensuring maintainability.
*   **Custom API (Action):** Developed `scm_ApproveSupplier` Custom Action to encapsulate complex approval logic.
*   **Pre-Validation & Post-Operation:**
    *   Prevented deletion of VIP suppliers using synchronous pre-validation.
    *   Automated task creation upon new supplier onboarding.
*   **Advanced Data Operations:** Implemented `QueryExpression` for related record checks and `Pre-Image` for price change auditing.

### 2. Frontend Customization (TypeScript & PCF)
*   **Custom PCF Control:** Developed a React-based **Stock Slider** component to replace standard integer inputs using Fluent UI.
*   **Client-Side Logic:** Used JavaScript to implement dynamic form behavior (Show/Hide fields based on toggle).
*   **Ribbon Customization:** Integrated custom buttons with JavaScript execution context for seamless user interaction.

### 3. System Integration
*   **Webhooks:** Configured real-time data push to external systems upon record creation.
*   **Virtual Tables:** Integrated external OData services to view data without replication.

---

## 🛠️ Tech Stack
*   **Languages:** C#, TypeScript, JavaScript, SQL (Concept)
*   **Tools:** Visual Studio 2022, VS Code, Plugin Registration Tool (PRT), XrmToolBox
*   **DevOps:** Git, Solution Management (ALM)

---
*Created by [Your Name]*