import {IInputs, IOutputs} from "./generated/ManifestTypes";
import * as React from "react";
import * as ReactDOM from "react-dom";
import { SliderComponent, IProps } from "./SliderComponent";

export class StockSlider implements ComponentFramework.StandardControl<IInputs, IOutputs> {

    private _notifyOutputChanged: () => void;
    private _container: HTMLDivElement;
    private _currentValue: number;

    public init(context: ComponentFramework.Context<IInputs>, notifyOutputChanged: () => void, state: ComponentFramework.Dictionary, container:HTMLDivElement): void
    {
        this._notifyOutputChanged = notifyOutputChanged;
        this._container = container;
        // 初始化时获取当前值
        this._currentValue = context.parameters.sliderValue.raw || 0;
    }

    public updateView(context: ComponentFramework.Context<IInputs>): void
    {
        // 1. 获取最新值
        this._currentValue = context.parameters.sliderValue.raw || 0;

        // 2. 准备传给 React 组件的参数 (Props)
        const props: IProps = {
            value: this._currentValue,
            onChange: this.onChange.bind(this)
        };

        // 3. 【核心】使用 React 渲染组件到容器里
        ReactDOM.render(
            React.createElement(SliderComponent, props),
            this._container
        );
    }

    // 当 React 组件里的滑块变动时，会调用这个函数
    private onChange(newValue: number) {
        this._currentValue = newValue;
        this._notifyOutputChanged(); // 通知 Dynamics 系统保存数据
    }

    public getOutputs(): IOutputs
    {
        return {
            sliderValue: this._currentValue
        };
    }

    public destroy(): void
    {
        // 清理 React 组件，防止内存泄漏
        ReactDOM.unmountComponentAtNode(this._container);
    }
}