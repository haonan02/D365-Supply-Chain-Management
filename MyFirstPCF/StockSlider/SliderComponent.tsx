import * as React from 'react';
import { Slider, ISliderStyles } from '@fluentui/react';

// 定义传进来的参数类型
export interface IProps {
    value: number;
    onChange: (newValue: number) => void;
}

export class SliderComponent extends React.Component<IProps> {
    
    // 滑块滑动时的回调
    private _onChange = (value: number) => {
        // 调用父组件传进来的 onChange 方法，把新值传出去
        this.props.onChange(value);
    }

    public render(): JSX.Element {
        return (
            <div style={{ padding: '10px', backgroundColor: 'white', borderRadius: '4px', border: '1px solid #edebe9' }}>
                {/* 使用微软官方的 Fluent UI 滑块 */}
                <Slider
                    label="库存调整 (Fluent UI)"
                    min={0}
                    max={1000}
                    step={1}
                    defaultValue={this.props.value}
                    showValue={true}
                    onChange={this._onChange}
                    // 加一点样式，让它变成 Dynamics 的主题色
                    styles={{ root: { maxWidth: 300 } }} 
                />
            </div>
        );
    }
}