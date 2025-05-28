import * as React from 'react';
import { LineChart } from '@mui/x-charts/LineChart';

export interface ISerieItem {
  yData: number,
  xData: Date,
}

interface IMovimentationsChartProps {
    series: ISerieItem[]
}

const MovimentationsChart = ({series}: IMovimentationsChartProps) => {
    return <LineChart
      xAxis={[
        { 
          data: series.map(item => item.xData),
          scaleType: 'time',
          valueFormatter: (value) => {
            return value.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
          }
        }
      ]}
      series={[
        {
          data: series.map(item => item.yData),
        },
      ]}
      height={300}
    />

}

export default MovimentationsChart;