using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TripPlanning.GisAnalysis
{
    public class GeoAnalysisConfig
    {
        public static void Build(IConfiguration configuration)
        {
            Tolerance = double.Parse(configuration.GetSection(nameof(Tolerance)).Value);
            FlightPreferDist = double.Parse(configuration.GetSection(nameof(FlightPreferDist)).Value);
            TransitWeigth = double.Parse(configuration.GetSection(nameof(TransitWeigth)).Value);
            MaxTransit = int.Parse(configuration.GetSection(nameof(MaxTransit)).Value);
        }

        /// <summary>
        /// 偏好飞机的行程阈值,单位为经纬度,1°约100km
        /// </summary>
        public static double FlightPreferDist { get; set; }

        /// <summary>
        /// 城市间距的容差
        /// </summary>
        public static double Tolerance { get; set; }

        /// <summary>
        /// 同城换乘权重
        /// </summary>
        public static double TransitWeigth { get; set; }

        /// <summary>
        /// 允许最大换乘次数
        /// </summary>
        public static int MaxTransit { get; set; }
    }
}
