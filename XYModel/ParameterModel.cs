
namespace XYModel
{
    public class ParameterModel
    {
        /// <summary>
        /// 托盤存放區域寬度 mm
        /// </summary>
        private double _traySize_X { get; set; }
        public double TraySize_X
        {
            get { return _traySize_X; }
            set { _traySize_X = value; }
        }

        /// <summary>
        /// 托盤存放區域深度 mm
        /// </summary>
        private double _traySize_Y { get; set; }
        public double TraySize_Y
        {
            get { return _traySize_Y; }
            set { _traySize_Y = value; }
        }
    }
}
