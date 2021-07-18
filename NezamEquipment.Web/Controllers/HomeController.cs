using System;
using System.Web.Mvc;
using NezamEquipment.Web.Models.Home;
using System.Management;
using System.Management.Instrumentation;

using System.Windows.Input;

namespace NezamEquipment.Web.Controllers
{
    public partial class HomeController : Controller
    {
        public virtual ActionResult Index()
        
        {

            //return RedirectToAction(MVC.Statistic.Index());
            var viewModel = new HomeIndexViewModel();

            ManagementClass mc = new ManagementClass("win32_processor");
           
            ManagementObjectCollection moc = mc.GetInstances();
          
            String cpuName = String.Empty;
            String systemName = String.Empty;
           
            foreach (ManagementObject mo in moc)
            {

                cpuName = mo.Properties["name"].Value.ToString();
                systemName = mo.Properties["SystemName"].Value.ToString();
                break;
            }
            ManagementClass diskDrive = new ManagementClass("win32_DiskDrive");
            ManagementObjectCollection mdd = diskDrive.GetInstances();
            String model = String.Empty;
            String size = String.Empty;

            foreach (ManagementObject md in mdd)
            {

                model = md.Properties["model"].Value.ToString();
                size = md.Properties["size"].Value.ToString();

                break;
            }
            ManagementClass networkInfo = new ManagementClass("win32_NetworkAdapterConfiguration");
            ManagementObjectCollection mNet = networkInfo.GetInstances();
            //String iPAddress;
            //String dNSDomainSuffixSearchOrder = String.Empty; 
           // String dhcpServer = String.Empty;

            foreach (ManagementObject mn in mNet)
            {
                //iPAddress = mn.Properties["IPAddress"].Value.ToString();
                //dNSDomainSuffixSearchOrder = mn.Properties["DNSDomainSuffixSearchOrder"].Value.ToString();
                //dhcpServer = mn.Properties["DHCPServer"].Value.ToString();

                break;
            }
            ManagementClass comSys = new ManagementClass("win32_ComputerSystem");
            ManagementObjectCollection mcom = comSys.GetInstances();
           // String username = String.Empty;
            //String domain = String.Empty;
          

            foreach (ManagementObject mco in mcom)
            {
                //iPAddress = mco.Properties["usrname"].Value.ToString();
                //domain = mco.Properties["Domain"].Value.ToString();
              
                break;
            }
            return View(viewModel);
        }

    }
}