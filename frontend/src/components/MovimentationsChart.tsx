import * as React from 'react';
import { LineChart } from '@mui/x-charts/LineChart';

export interface ISerieItem {
  value: number,
  atDate: Date,
}

interface IMovimentationsChartProps {
    series: ISerieItem[]
}

const MovimentationsChart = ({series}: IMovimentationsChartProps) => {
    return <LineChart
      xAxis={[
        { 
          data: series.map(item => item.atDate),
          scaleType: 'time',
          valueFormatter: (atDate) => {
            return new Date().toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
          }
        }
      ]}
      series={[
        {
          data: series.map(item => item.value),
        },
      ]}
      height={300}
    />

}

export default MovimentationsChart;