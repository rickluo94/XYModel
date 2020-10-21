using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XYModel
{
    public class ActionParameter
    {
        private ParameterModel _parameter = new ParameterModel();

        /// <summary>
        /// 托盤寬有幾個分隔
        /// </summary>
        private int _trayAreaCount_X { get; set; }
        public int TrayAreaCount_X
        {
            get { return _trayAreaCount_X; }
            set { _trayAreaCount_X = value; }
        }

        /// <summary>
        /// 托盤深有幾個分格
        /// </summary>
        private int _trayAreaCount_Y { get; set; }
        public int TrayAreaCount_Y
        {
            get { return _trayAreaCount_Y; }
            set { _trayAreaCount_Y = value; }
        }

        public bool IndicatorInitial()
        {
            string path = Directory.GetCurrentDirectory();
            path = $"{path}\\XYParameter.xml";
            if (File.Exists(path) == false)
            {
                return false;
            }
            else
            {
                XmlDocument _xd = new XmlDocument();
                try
                {
                    _xd.Load(path);

                    XmlNodeList DataList;

                    SteLib.SteLibXMLTransport.XmlDecodeNode(_xd.InnerXml, "TrayLength_X", out DataList);
                    _parameter.TraySize_X = Convert.ToDouble(DataList[0].InnerText);
                    SteLib.SteLibXMLTransport.XmlDecodeNode(_xd.InnerXml, "TrayLength_Y", out DataList);
                    _parameter.TraySize_Y = Convert.ToDouble(DataList[0].InnerText);
                }
                catch (Exception ex)
                { return false; }
                return true;
            }
        }

        /// <summary>
        /// XY將托盤區域轉換為座標單位mm將雙精確度浮點數值四捨五入到最接近的整數值，並將中間點值四捨五入到最接近的偶數。
        /// </summary>
        /// <param name="AreaName"></param>
        /// <returns></returns>
        public Dictionary<string, double> TrayAreaNameConvertToCoordinate(string AreaName)
        {
            Dictionary<string, double> Coordinate = new Dictionary<string, double>();
            double _areaSize_X;
            double _areaSize_Y;
            byte[] _areaName_Ascll;
            int _AreaNameInt_X = 0;
            int _AreaNameInt_Y = 0;

            try
            {

                //計算區域的固定長跟寬
                _areaSize_X = _parameter.TraySize_X / _trayAreaCount_X;
                _areaSize_Y = _parameter.TraySize_Y / _trayAreaCount_Y;

                //托盤區域名稱轉為數字
                _areaName_Ascll = Encoding.Default.GetBytes(AreaName);
                int MultiplyBy_X = 1;
                int MultiplyBy_Y = 1;

                for (int i = _areaName_Ascll.Length - 1; i >= 0; i--)
                {
                    //Y座標轉換
                    if ((_areaName_Ascll[i] >= 'A') && (_areaName_Ascll[i] <= 'Z'))
                    {
                        _AreaNameInt_Y = _AreaNameInt_Y + MultiplyBy_Y * (_areaName_Ascll[i] - 'A');
                        MultiplyBy_Y = MultiplyBy_Y * 26;
                    }

                    //X座標轉換
                    if ((_areaName_Ascll[i] >= '0') && (_areaName_Ascll[i] <= '9'))
                    {
                        _AreaNameInt_X = _AreaNameInt_X + MultiplyBy_X * (Convert.ToInt16(_areaName_Ascll[i].ToString()) - '0');
                        MultiplyBy_X = MultiplyBy_X * 10;
                    }
                }

                //計算實際要標示的位置(mm)
                Coordinate.Add("X", Math.Round((_areaSize_X * (_AreaNameInt_X - 1) + (_areaSize_X / 2)) * 10));
                Coordinate.Add("Y", Math.Round((_areaSize_Y * _AreaNameInt_Y + (_areaSize_Y / 2)) * 10));
            }
            catch (Exception ex)
            { }

            return Coordinate;
        }
    }
}
