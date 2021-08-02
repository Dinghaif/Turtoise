using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;
using DotRas;


namespace ConnectVPN
{
    public class VPNHelper
    {
        private static string Windir = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\";
        private static string RasDialFileName = "rasdial.exe";

        private static string VPNPROCESS = Windir + RasDialFileName;
        public string IPTOPing { get; set; }
        public string VPNName { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public VPNHelper()
        {

        }
        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="_vpnIP"></param>
        /// <param name="_vpnName"></param>
        /// <param name="_userName"></param>
        /// <param name="_password"></param>
        public VPNHelper(string _vpnIP, string _vpnName, string _userName, string _password)
        {
            this.IPTOPing = _vpnIP;
            this.VPNName = _vpnName;
            this.UserName = _userName;
            this.PassWord = _password;
            Console.WriteLine("初始化成功");
        }
        /// <summary>
        /// 尝试连接VPN
        /// </summary>
        public void TryConnectVPN()
        {
            this.TryConnectVPN(this.VPNName, this.UserName, this.PassWord);
        }
        /// <summary>
        /// 尝试断开连接
        /// </summary>
        public void TryDisConnectVPN()
        {
            this.TryDisConnectVPN(this.VPNName);
        }
        /// <summary>
        /// 创建或更新VPN
        /// </summary>
        public void CreateOrUpdateVPN()
        {
            this.CreateOrUpdateVPN(this.VPNName, this.IPTOPing);
        }
        /// <summary>
        /// 尝试删除连接
        /// </summary>
        public void TryDeleteVPN()
        {
            this.TryDeleteVPN(this.VPNName);
        }
        /// <summary>
        /// 尝试连接VPN（指定VPN名称、用户名、密码）
        /// </summary>
        /// <param name="connVpnName"></param>
        /// <param name="connUserName"></param>
        /// <param name="connPassWord"></param>
        public void TryConnectVPN(string connVpnName, string connUserName, string connPassWord)
        {
            try
            {
                string args = string.Format("{0} {1} {2}", connVpnName, connUserName, connPassWord);
                ProcessStartInfo myProcess = new ProcessStartInfo(VPNPROCESS, args);
                myProcess.CreateNoWindow = false;
                myProcess.UseShellExecute = false;
                Process.Start(myProcess);
                Console.WriteLine("连接成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        /// <summary>
        /// 尝试断开VPN（指定VPN名称）
        /// </summary>
        /// <param name="disConnVpnName"></param>
        public void TryDisConnectVPN(string disConnVpnName)
        {
            try
            {
                string args = string.Format(@"{0}/d", disConnVpnName);
                ProcessStartInfo myProcess = new ProcessStartInfo(VPNPROCESS, args);
                myProcess.CreateNoWindow = true;
                myProcess.UseShellExecute = false;
                Process.Start(myProcess);
                Console.WriteLine("VPN连接已断开");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }
        /// <summary>
        /// 创建或更新VPN（指定名称及IP）
        /// </summary>
        /// <param name="updateVPNname"></param>
        /// <param name="updateVPNip"></param>
        public void CreateOrUpdateVPN(string updateVPNname, string updateVPNip)
        {
            RasDialer dialer = new RasDialer();
            RasPhoneBook allUserPhoneBook = new RasPhoneBook();
            allUserPhoneBook.Open();

            if (allUserPhoneBook.Entries.Contains(updateVPNname))
            {
                allUserPhoneBook.Entries[updateVPNname].PhoneNumber = updateVPNip;
                //不管当前 VPN是否连接，服务器地址的更新总能成功， 如果正在连接， 则需要 VPN重启
                //后才能起作用
                allUserPhoneBook.Entries[updateVPNname].Update();
            }
            //创建一个新VPN
            else
            {
                try
                {
                    RasEntry entry = RasEntry.CreateVpnEntry(updateVPNname, updateVPNip, RasVpnStrategy.PptpFirst, RasDevice.GetDeviceByName("(PPTP)", RasDeviceType.Vpn));
                    allUserPhoneBook.Entries.Add(entry);
                    dialer.EntryName = updateVPNname;
                    dialer.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.AllUsers);
                    
                    Console.WriteLine("创建新VPN成功");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }
        }

        /// <summary>
        /// 删除指定名称的VPN
        ///如果 VPN正在运行，一样会在电话本里删除，但是不会断开连接，所以，最好是先断开连接，
        ///再进行删除操作
        /// </summary>
        /// <param name="delVpnName"></param>
        public void TryDeleteVPN(string delVpnName)
        {
            RasDialer dialer = new RasDialer();
            RasPhoneBook allUsersPhoneBook = new RasPhoneBook();
            allUsersPhoneBook.Open();
            if (allUsersPhoneBook.Entries.Contains(delVpnName))
            {
                allUsersPhoneBook.Entries.Remove(delVpnName);
            }
            Console.WriteLine("已成功删除VPN");
        }
        /// <summary>
        /// 获取当前正在连接中的VPN名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetCurrentConnectingVPNNames()
        {
            List<string> ConnectingVPNList = new List<string>();
            Process proIP = new Process();
            proIP.StartInfo.FileName = "cmd.exe ";
            proIP.StartInfo.UseShellExecute = false;
            proIP.StartInfo.RedirectStandardInput = true;
            proIP.StartInfo.RedirectStandardOutput = true;
            proIP.StartInfo.RedirectStandardError = true;
            proIP.StartInfo.CreateNoWindow = true; // 不显示 cmd窗口
            proIP.Start();
            proIP.StandardInput.WriteLine(RasDialFileName);
            proIP.StandardInput.WriteLine("exit");
            // 命令行运行结果
            string strResult = proIP.StandardOutput.ReadToEnd();
            Console.WriteLine("output: "+ strResult);
            proIP.Close();
            Regex regger = new Regex("(?<= 已连接 \r\n)(.*\n)*(?= 命令已完成 )",
            RegexOptions.Multiline);
            // 如果匹配，则说明有正在连接的 VPN 
            if (regger.IsMatch(strResult))
            {
                string[] list = regger.Match(strResult).Value.ToString().Split('\n');
                for (int index = 0; index < list.Length; index++)
                {
                    if (list[index] != string.Empty)
                        ConnectingVPNList.Add(list[index].Replace("\r", ""));
                }
            }
            // 没有正在连接的 VPN，则直接返回一个空 List<string> 
            return ConnectingVPNList;
        }
    }
    class programs
    {
        public static void Main()
        {
            VPNHelper vpnhelp = new VPNHelper("211.141.114.44", "vpn142", "ldjjx142@fzrb.jx", "ldjjx142");//VPN主机IP地址：211.141.114.44    ldjjx142@fzrb.jx     VPN密码：ldjjx142
            var list = vpnhelp.GetCurrentConnectingVPNNames();
            Console.WriteLine("连接列表:");
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("End");
            //vpnhelp.CreateOrUpdateVPN("vpn142", "211.141.114.44");
            vpnhelp.TryConnectVPN("vpn142", "ldjjx142@fzrb.jx", "ldjjx142");
            //vpnhelp.TryDisConnectVPN("vpn142");
            //vpnhelp.TryDeleteVPN("vpn142");
            Console.ReadKey();

        }
    }
}


