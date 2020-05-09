
namespace Fuel123.Models
{
    public struct TransferData
    {
        public int TankPage; 
        public int FuelPage; 
        public int OperationPage; 
        public string strTankTypeFind; 
        public string strFuelTypeFind;

        public TransferData(int tankpage=1, int fuelpage=1, int operationkpage = 1, string strtanktypefind="", string strfueltypefind = "")
        {
            TankPage = tankpage;
            FuelPage = fuelpage;
            OperationPage = operationkpage;
            strTankTypeFind = strtanktypefind;
            strFuelTypeFind = strfueltypefind;
        }
        
    }
}